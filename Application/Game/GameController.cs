/*using Application.Services;
using GameCore.Bullets;
using GameCore.Characters;
using GameCore.Factories;
using GameCore.Items;
using GameCore.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Windows.Forms;

namespace Application.Game
{
    public class GameController
    {
        private LabyrinthMap _map;
        private EnemyFactory _enemyFactory;
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public List<Bullet> ActiveBullets { get; } = new List<Bullet>();
        public List<Enemy> Enemies { get; } = new List<Enemy>();
        public GameController(LabyrinthMap map)
        {
            _map = map;

            var p1Pos = _map.ConvertToWorldCoordinates(1, 1);
            Player1 = new Player(1, p1Pos.X, p1Pos.Y) { Size = 50 };

            var p2Pos = _map.ConvertToWorldCoordinates(_map.Width() - 2, _map.Height() - 2);
            Player2 = new Player(2, p2Pos.X, p2Pos.Y) { Size = 50 };

            var playersList = new List<Player> { Player1, Player2 };

            _enemyFactory = new Type1EnemyFactory(_map, playersList);
            InitializeLevel();
        }

        public void InitializeLevel()
        {
            for (int i = 0; i < 15; i++)
            {
                var enemy = _enemyFactory.SpawnRandom(25f);
                Enemies.Add(enemy);
                System.Diagnostics.Debug.WriteLine($"Враг #{i} создан: X={enemy.Position.X}, Y={enemy.Position.Y}");
            }
        }

            //for (int i = 0; i < 3; i++)
            //{
            //    Prizes.Add(_prizeFactory.SpawnRandom(150f));
            //}
        

        public void HandleInput(GameInput keyCode)
        {
            Console.WriteLine($"HandleInput вызван с клавишей: {keyCode}, Время: {DateTime.Now:HH:mm:ss.fff}");

            UpdatePlayerPosition(Player1, keyCode, GameInput.W, GameInput.S, GameInput.A, GameInput.D);
            UpdatePlayerPosition(Player2, keyCode, GameInput.Up, GameInput.Down, GameInput.Left, GameInput.Right);

            if (keyCode == GameInput.Space)
            {
                ExecuteShoot(Player1);
            }
            else if (keyCode == GameInput.Enter)
            {
                ExecuteShoot(Player2);
            }
        }

        //private void UpdatePlayerPosition(Player player, GameInput pressedKey, GameInput up, GameInput down, GameInput left, GameInput right)
        //{
        //    float dx = 0, dy = 0;
        //    if (pressedKey == up) dy = LabyrinthMap.TileSize;
        //    else if (pressedKey == down) dy = -LabyrinthMap.TileSize;
        //    else if (pressedKey == left) dx = -LabyrinthMap.TileSize;
        //    else if (pressedKey == right) dx = LabyrinthMap.TileSize;

        //    if (dx != 0 || dy != 0)
        //    {
        //        var nextPos = new GameCore.Point(player.Position.X + dx, player.Position.Y + dy);
        //        var gridCoords = _map.ConvertToTileCoordinates(nextPos);

        //        if (!_map.IsWall(gridCoords.X, gridCoords.Y))
        //        {
        //            player.SetDirection(dx, dy);
        //            player.Move(dx, dy);
        //        }
        //    }
        //}

        private void UpdatePlayerPosition(Player player, GameInput pressedKey, GameInput up, GameInput down, GameInput left, GameInput right)
        {
            float dx = 0, dy = 0;

            if (pressedKey == up) dy = LabyrinthMap.TileSize;
            else if (pressedKey == down) dy = -LabyrinthMap.TileSize;
            else if (pressedKey == left) dx = -LabyrinthMap.TileSize;
            else if (pressedKey == right) dx = LabyrinthMap.TileSize;

            if (dx != 0 || dy != 0)
            {
                int desiredX = Math.Sign(dx);
                int desiredY = Math.Sign(dy);

                bool isAlreadyFacing = (desiredX == (int)player.DirectionX && desiredY == (int)player.DirectionY);

                if (!isAlreadyFacing)
                {
                    player.SetDirection(dx, dy);
                }
                else
                {
                    var nextPos = new GameCore.Point(player.Position.X + dx, player.Position.Y + dy);
                    var gridCoords = _map.ConvertToTileCoordinates(nextPos);

                    if (!_map.IsWall(gridCoords.X, gridCoords.Y))
                    {
                        player.Move(dx, dy);
                    }
                }
            }
        }

        private void ExecuteShoot(Player player)
        {
            if (player.Ammunition <= 0) return;
            player.Ammunition--;

            //float centerX = player.Position.X + player.Size / 2f;
            //float centerY = player.Position.Y + player.Size / 2f;

            float centerX = player.Position.X;
            float centerY = player.Position.Y;

            var bullet = new Bullet(
                centerX,
                centerY,
                player.DirectionX,
                player.DirectionY,
                player.CurrentBullet,
                player.Id
            );

            ActiveBullets.Add(bullet);
        }

        //public void UpdatePhysics()
        //{
        //    for (int i = ActiveBullets.Count - 1; i >= 0; i--)
        //    {
        //        var b = ActiveBullets[i];
        //        b.Update();

        //        var grid = _map.ConvertToTileCoordinates(b.Position);
        //        if (_map.IsWall(grid.X, grid.Y))
        //        {
        //            ActiveBullets.RemoveAt(i);
        //            continue;
        //        }

        //        Player enemy = (b.OwnerId == 1) ? Player2 : Player1;
        //        if (b.GetBounds().IntersectsWith(enemy.GetBounds()))
        //        {
        //            enemy.TakeDamage(b.GetDamage());
        //            ActiveBullets.RemoveAt(i);
        //        }
        //    }
        //    for (int i = Enemies.Count - 1; i >= 0; i--)
        //    {
        //        var enemy = Enemies[i];

        //        // Если HP кончилось — удаляем
        //        if (enemy.Health <= 0)
        //        {
        //            Enemies.RemoveAt(i);
        //            continue;
        //        }

        //        // Вызываем ИИ (передаем игроков для проверки видимости)
        //        bool shootRequested = enemy.UpdateAI(_map, Player1, Player2);

        //        if (shootRequested)
        //        {
        //            ExecuteEnemyShoot(enemy);
        //        }
        //    }
        //}

        public void UpdatePhysics()
        {
            for (int i = ActiveBullets.Count - 1; i >= 0; i--)
            {
                var b = ActiveBullets[i];
                b.Update();

                var grid = _map.ConvertToTileCoordinates(b.Position);
                if (_map.IsWall(grid.X, grid.Y))
                {
                    ActiveBullets.RemoveAt(i);
                    continue;
                }

                bool bulletDestroyed = false;

                if (b.OwnerId == 1 || b.OwnerId == 2)
                {
                    Player opponent = (b.OwnerId == 1) ? Player2 : Player1;
                    if (b.GetBounds().IntersectsWith(opponent.GetBounds()))
                    {
                        opponent.TakeDamage(b.GetDamage());
                        bulletDestroyed = true;
                    }

                    if (!bulletDestroyed)
                    {
                        for (int j = Enemies.Count - 1; j >= 0; j--)
                        {
                            if (b.GetBounds().IntersectsWith(Enemies[j].GetBounds()))
                            {
                                Enemies[j].Health -= b.GetDamage();
                                bulletDestroyed = true;
                                break;
                            }
                        }
                    }
                }
                else if (b.OwnerId == 3)
                {
                    foreach (var p in new[] { Player1, Player2 })
                    {
                        if (b.GetBounds().IntersectsWith(p.GetBounds()))
                        {
                            p.TakeDamage(b.GetDamage());
                            bulletDestroyed = true;
                            break;
                        }
                    }
                }

                if (bulletDestroyed)
                {
                    ActiveBullets.RemoveAt(i);
                }
            }

            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                var enemy = Enemies[i];

                if (enemy.Health <= 0)
                {
                    Enemies.RemoveAt(i);
                    continue;
                }

                bool shootRequested = enemy.UpdatePosition(_map, Player1, Player2);
                if (shootRequested)
                {
                    ExecuteEnemyShoot(enemy);
                }

                if (Player1.Health <= 0) Player1.Respawn();
                if (Player2.Health <= 0) Player2.Respawn();
            }
        }


        private void ExecuteEnemyShoot(Enemy enemy)
        {
            float centerX = enemy.Position.X;
            float centerY = enemy.Position.Y;

            var bullet = new Bullet(
                centerX, centerY,
                enemy.DirectionX, enemy.DirectionY,
                enemy.CurrentBullet,
                3
            );

            ActiveBullets.Add(bullet);
        }

    }
}*/


using Application.Services;
using GameCore.Bullets;
using GameCore.Characters;
using GameCore.Factories;
using GameCore.Map;
using System;
using System.Collections.Generic;

namespace Application.Game
{
    public class GameController
    {
        private readonly LabyrinthMap _map;
        private readonly EnemyFactory _enemyFactory;

        private float _sessionTime = 0;
        private const int TargetEnemyCount = 10;

        public Player Player1 { get; }
        public Player Player2 { get; }
        public List<Bullet> ActiveBullets { get; } = new List<Bullet>();
        public List<Enemy> Enemies { get; } = new List<Enemy>();

        public GameController(LabyrinthMap map)
        {
            _map = map;

            // Инициализация игроков
            var p1Pos = _map.ConvertToWorldCoordinates(1, 1);
            Player1 = new Player(1, p1Pos.X, p1Pos.Y) { Size = 50 };

            var p2Pos = _map.ConvertToWorldCoordinates(_map.Width() - 2, _map.Height() - 2);
            Player2 = new Player(2, p2Pos.X, p2Pos.Y) { Size = 50 };

            _enemyFactory = new Type1EnemyFactory(_map, new List<Player> { Player1, Player2 });

            FillLevelWithEnemies(1.0f);
        }

        /// <summary>
        /// Главный цикл обновления мира
        /// </summary>
        public void UpdateWorld(float deltaTime)
        {
            _sessionTime += deltaTime;
            float currentDifficulty = CalculateDifficulty();

            ProcessBullets();
            ProcessEnemies(currentDifficulty);
            ProcessPlayers();

            // Поддерживаем популяцию врагов
            if (Enemies.Count < TargetEnemyCount)
            {
                Enemies.Add(_enemyFactory.SpawnRandom(100f, currentDifficulty));
            }
        }

        private float CalculateDifficulty()
        {
            // Каждые 2 минуты (120 сек) сложность растет на 1.0
            return 1.0f + (_sessionTime / 120f);
        }

        private void ProcessBullets()
        {
            for (int i = ActiveBullets.Count - 1; i >= 0; i--)
            {
                var bullet = ActiveBullets[i];
                bullet.Update();

                // 1. Стены
                var grid = _map.ConvertToTileCoordinates(bullet.Position);
                if (_map.IsWall(grid.X, grid.Y))
                {
                    ActiveBullets.RemoveAt(i);
                    continue;
                }

                // 2. Попадания
                if (CheckBulletCollisions(bullet))
                {
                    ActiveBullets.RemoveAt(i);
                }
            }
        }

        private bool CheckBulletCollisions(Bullet bullet)
        {
            // Пули игроков (ID 1 и 2)
            if (bullet.OwnerId == 1 || bullet.OwnerId == 2)
            {
                Player opponent = (bullet.OwnerId == 1) ? Player2 : Player1;
                Player owner = (bullet.OwnerId == 1) ? Player1 : Player2;

                // В оппонента
                if (bullet.GetBounds().IntersectsWith(opponent.GetBounds()))
                {
                    opponent.TakeDamage(bullet.GetDamage());
                    return true;
                }

                // Во врагов
                foreach (var enemy in Enemies)
                {
                    if (bullet.GetBounds().IntersectsWith(enemy.GetBounds()))
                    {
                        enemy.Health -= bullet.GetDamage();
                        if (enemy.Health <= 0) owner.Score += 100; // Начисляем очки
                        return true;
                    }
                }
            }
            // Пули врагов (ID 3)
            else if (bullet.OwnerId == 3)
            {
                foreach (var player in new[] { Player1, Player2 })
                {
                    if (bullet.GetBounds().IntersectsWith(player.GetBounds()))
                    {
                        player.TakeDamage(bullet.GetDamage());
                        return true;
                    }
                }
            }
            return false;
        }

        private void ProcessEnemies(float difficulty)
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                var enemy = Enemies[i];

                if (enemy.Health <= 0)
                {
                    Enemies.RemoveAt(i);
                    continue;
                }

                if (enemy.UpdatePosition(_map, Player1, Player2))
                {
                    CreateEnemyBullet(enemy);
                }
            }
        }

        private void ProcessPlayers()
        {
            if (Player1.Health <= 0) Player1.Respawn();
            if (Player2.Health <= 0) Player2.Respawn();
        }

        private void FillLevelWithEnemies(float difficulty)
        {
            for (int i = 0; i < TargetEnemyCount; i++)
            {
                Enemies.Add(_enemyFactory.SpawnRandom(25f, difficulty));
            }
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
                CreatePlayerBullet(player);
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

        private void CreatePlayerBullet(Player player)
        {
            if (player.Ammunition <= 0) return;
            player.Ammunition--;

            ActiveBullets.Add(new Bullet(
                player.Position.X, player.Position.Y,
                player.DirectionX, player.DirectionY,
                player.CurrentBullet, player.Id));
        }

        private void CreateEnemyBullet(Enemy enemy)
        {
            ActiveBullets.Add(new Bullet(
                enemy.Position.X, enemy.Position.Y,
                enemy.DirectionX, enemy.DirectionY,
                enemy.CurrentBullet, 3));
        }
    }
}