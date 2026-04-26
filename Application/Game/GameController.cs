using Application.Services;
using GameCore;
using GameCore.Bullets;
using GameCore.Characters;
using GameCore.Factories;
using GameCore.Items;
using GameCore.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Application.Game
{
    public class GameController
    {
        private readonly LabyrinthMap _map;
        private EnemyFactory _currentEnemyFactory;
        private readonly List<Player> _allPlayers;

        private float _sessionTime = 0;
        private float _prizeSpawnTimer = 0;

        private const int TargetEnemyCount = 3;
        private int _totalSpawnedCount = 0;
        private const int MaxEnemiesPerSession = 3;
        private bool _canSpawnNewEnemies = true;

        public bool IsGameOver { get; private set; } = false;

        private int EliminationPoints = 100;

        private const float PrizeInitialDelay = 3f;
        private const float PrizeSpawnInterval = 10f;
        private const float EnemySpawnDistance = 100f;

        private const float Stage2Time = 60f;
        private const float Stage3Time = 150f;

        public Player Player1 { get; }
        public Player Player2 { get; }
        public List<Bullet> ActiveBullets { get; } = new List<Bullet>();
        public List<Prize> ActivePrizes { get; } = new List<Prize>();
        public List<Enemy> Enemies { get; } = new List<Enemy>();

        public List<GameObject> GameObjects { get; } = new List<GameObject>();

        public GameController(LabyrinthMap map)
        {
            _map = map;

            var p1Pos = _map.ConvertToWorldCoordinates(1, 1);
            var p2Pos = _map.ConvertToWorldCoordinates(_map.Width() - 2, _map.Height() - 2);

            Player1 = new Player(1, p1Pos.X, p1Pos.Y) { Size = 50 };
            Player2 = new Player(2, p2Pos.X, p2Pos.Y) { Size = 50 };

            Player1.OnDied += OnPlayerDied;
            Player2.OnDied += OnPlayerDied;

            AddEntity(Player1);
            AddEntity(Player2);

            _allPlayers = new List<Player> { Player1, Player2 };


            UpdateFactory();
            //_currentPrizeFactory = new PrizeFactory()

            FillLevelWithEnemies();
        }

        public void UpdateWorld(float deltaTime)
        {
            if (IsGameOver) return;

            _sessionTime += deltaTime;
            _prizeSpawnTimer += deltaTime;

            UpdatePlayers(deltaTime);
            UpdateFactory();
            UpdateSpawners();

            ProcessBullets();
            ProcessEnemies();
            ProcessPrizePickups(deltaTime);

            //MaintainEnemyCount();
            if (_canSpawnNewEnemies)
            {
                MaintainEnemyCount();
            }
        }

        private void UpdatePlayers(float deltaTime)
        {
            Player1.UpdatePowerUps(deltaTime);
            Player2.UpdatePowerUps(deltaTime);
        }

        private void UpdateSpawners()
        {
            if (_sessionTime > PrizeInitialDelay && _prizeSpawnTimer >= PrizeSpawnInterval)
            {
                SpawnPrizePair();
                _prizeSpawnTimer = 0;
            }
        }

        private void MaintainEnemyCount()
        {
            //if (Enemies.Count < TargetEnemyCount)
            //{
            //    AddEntity(_currentEnemyFactory.SpawnRandom(EnemySpawnDistance));
            //}

            if (_totalSpawnedCount >= MaxEnemiesPerSession)
            {
                _canSpawnNewEnemies = false;
                return;
            }

            if (Enemies.Count < TargetEnemyCount)
            {
                AddEntity(_currentEnemyFactory.SpawnRandom(EnemySpawnDistance));
                _totalSpawnedCount++;
            }
        }

        private void FillLevelWithEnemies()
        {
            for (int i = 0; i < TargetEnemyCount; i++)
            {
                AddEntity(_currentEnemyFactory.SpawnRandom(EnemySpawnDistance));
            }
        }

        private void UpdateFactory()
        {
            if (_sessionTime >= Stage3Time)
            {
                if (!(_currentEnemyFactory is ChaosEnemyFactory))
                    _currentEnemyFactory = new ChaosEnemyFactory(_map, _allPlayers);
            }
            else if (_sessionTime >= Stage2Time)
            {
                if (!(_currentEnemyFactory is EliteEnemyFactory))
                    _currentEnemyFactory = new EliteEnemyFactory(_map, _allPlayers);
            }
            else if (_currentEnemyFactory == null)
            {
                _currentEnemyFactory = new LightEnemyFactory(_map, _allPlayers);
            }
        }

        private void SpawnPrizePair()
        {
            var pair = PrizeFactory.SpawnRandomPair(_map);

            foreach (var prize in pair)
            {
                AddEntity(prize); 
            }
        }

        private void ProcessPrizePickups(float deltaTime)
        {
            foreach (var prize in ActivePrizes.ToList())
            {
                prize.Update(deltaTime); 

                if (prize.IsExpired)
                {
                    RemoveEntity(prize);
                }
                else
                {
                    CheckPickupForPlayer(Player1, prize);
                    CheckPickupForPlayer(Player2, prize);
                }
            }
        }

        private void CheckPickupForPlayer(Player player, Prize prize)
        {
            if (player.GetBounds().IntersectsWith(prize.GetBounds()))
            {
                bool alreadyHasDecorator = false;

                if (prize is ExplosivePrize)
                    alreadyHasDecorator = BulletTools.IsDecoratorActive<ExplosiveAmmo>(player.CurrentBullet);
                else if (prize is FastPrize)
                    alreadyHasDecorator = BulletTools.IsDecoratorActive<FastAmmo>(player.CurrentBullet);

                if (!alreadyHasDecorator)
                {
                    prize.ApplyEffect(player);
                    RemoveEntity(prize);
                }
            }
        }


        public void AddEntity(GameObject entity)
        {
            if (entity == null) return;

            GameObjects.Add(entity);

            if (entity is Enemy enemy)
            {
                enemy.OnDied += OnEnemyDied;
                Enemies.Add(enemy);
            }
            else if (entity is Bullet bullet)
            {
                ActiveBullets.Add(bullet);
            }
            else if (entity is Prize prize)
            {
                ActivePrizes.Add(prize);
            }
        }

        public void RemoveEntity(GameObject entity)
        {
            if (entity == null) return;

            GameObjects.Remove(entity);

            if (entity is Enemy enemy)
            {
                enemy.OnDied -= OnEnemyDied;
                Enemies.Remove(enemy);
            }
            else if (entity is Bullet bullet)
            {
                ActiveBullets.Remove(bullet);
            }
            else if (entity is Prize prize)
            {
                ActivePrizes.Remove(prize);
            }
        }

        private void ProcessBullets()
        {
            foreach (var bullet in ActiveBullets.ToArray())
            {
                bullet.Update();

                var grid = _map.ConvertToTileCoordinates(bullet.Position);

                if (_map.IsWall(grid.X, grid.Y) || CheckBulletCollisions(bullet))
                {
                    RemoveEntity(bullet);
                }
            }
        }

        private bool CheckBulletCollisions(Bullet bullet)
        {
            foreach (var player in _allPlayers)
            {
                if (bullet.OwnerId != player.Id && bullet.GetBounds().IntersectsWith(player.GetBounds()))
                {
                    player.TakeDamage(bullet);
                    return true;
                }
            }

            if (bullet.OwnerId != 3)
            {
                foreach(var enemy in Enemies.ToArray())
                {
                    if (bullet.GetBounds().IntersectsWith(enemy.GetBounds()))
                    {
                        enemy.TakeDamage(bullet);
                        return true;
                    }
                }
            }

            return false;
        }

        private void ProcessEnemies()
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                var enemy = Enemies[i];

                if (enemy.UpdatePosition(_map, Player1, Player2))
                {
                    CreateBullet(enemy);
                }
            }
        }

        private void OnEnemyDied(Unit victim, int killerId)
        {
            if (killerId == 1)
                Player1.Score += EliminationPoints;
            else if (killerId == 2)
                Player2.Score += EliminationPoints;

            if (victim is Enemy enemy)
            {
                RemoveEntity(enemy);
            }
            if (!_canSpawnNewEnemies && Enemies.Count == 0)
            {
                IsGameOver = true;
            }
        }

        private void OnPlayerDied(Unit victim, int killerId)
        {
            var deadPlayer = (Player)victim;
            deadPlayer.Respawn();

        }

        public void HandleInput(GameInput input)
        {
            ProcessPlayerInput(Player1, input, GameInput.W, GameInput.S, GameInput.A, GameInput.D, GameInput.Space);
            ProcessPlayerInput(Player2, input, GameInput.Up, GameInput.Down, GameInput.Left, GameInput.Right, GameInput.Enter);
        }

        private void ProcessPlayerInput(Player player, GameInput input, GameInput up, GameInput down, GameInput left, GameInput right, GameInput fire)
        {
            if (input == fire)
            {
                CreateBullet(player);
                return;
            }

            float dx = 0, dy = 0;
            if (input == up) dy = LabyrinthMap.TileSize;
            else if (input == down) dy = -LabyrinthMap.TileSize;
            else if (input == left) dx = -LabyrinthMap.TileSize;
            else if (input == right) dx = LabyrinthMap.TileSize;

            if (dx != 0 || dy != 0)
            {
                bool isTurning = (Math.Sign(dx) != (int)player.DirectionX || Math.Sign(dy) != (int)player.DirectionY);

                if (isTurning)
                {
                    player.SetDirection(dx, dy);
                }
                else
                {
                    var nextPos = new GameCore.Point(player.Position.X + dx, player.Position.Y + dy);
                    var grid = _map.ConvertToTileCoordinates(nextPos);
                    if (!_map.IsWall(grid.X, grid.Y)) player.Move(dx, dy);
                }
            }
        }

        private void CreateBullet(Unit unit)
        {
            var bullet = unit.Shoot();
            if (bullet != null)
            {
                AddEntity(bullet);
            }
        }

        public void Reset()
        {
            _sessionTime = 0;
            _totalSpawnedCount = 0;
            _canSpawnNewEnemies = true;
            IsGameOver = false;
            // Тут также стоит очистить списки Enemies, Bullets и сбросить очки игроков
        }
    }
}