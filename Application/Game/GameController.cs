using Application.Services;
using GameCore;
using GameCore.Bullets;
using GameCore.Characters;
using GameCore.Items;
using GameCore.Map;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Game
{
    /// <summary>
    /// Основной игровой контроллер, управляющий жизненным циклом игрового сеанса.
    /// Отвечает за спавн сущностей, обработку коллизий, обновление состояний и соблюдение игровых правил.
    /// </summary>
    public class GameController
    {
        // Константы игрового баланса
        private const int TargetEnemyCount = 15;        // Одновременное кол-во врагов на карте
        private const int EnemiesPerStage = 10;         // Сколько врагов нужно создать для перехода к след. фабрике
        private const float PrizeInitialDelay = 7f;      // Задержка перед появлением первых призов
        private const float PrizeSpawnInterval = 7f;     // Интервал появления новых призов
        private const float EnemySpawnDistance = 200f;   // Минимальная дистанция спавна врага от игрока

        // Состояние прогрессии
        private int _stage1Spawned = 0;
        private int _stage2Spawned = 0;
        private int _stage3Spawned = 0;

        private readonly LabyrinthMap _map;
        private readonly List<Player> _allPlayers;
        private EnemyFactory _currentEnemyFactory;

        private float _sessionTime;
        private float _prizeSpawnTimer;
        private bool _canSpawnNewEnemies;

        /// <summary> Флаг завершения игры. </summary>
        public bool IsGameOver { get; private set; }

        /// <summary> Первый игрок (управление WASD). </summary>
        public Player Player1 { get; }

        /// <summary> Второй игрок (управление стрелками). </summary>
        public Player Player2 { get; }

        // Списки активных объектов
        public List<Bullet> ActiveBullets { get; } = new List<Bullet>();
        public List<Prize> ActivePrizes { get; } = new List<Prize>();
        public List<Enemy> Enemies { get; } = new List<Enemy>();

        /// <summary> Общий список всех игровых объектов для отрисовки. </summary>
        public List<GameObject> GameObjects { get; } = new List<GameObject>();

        /// <summary>
        /// Конструктор контроллера. Инициализирует игроков и устанавливает начальное состояние мира.
        /// </summary>
        /// <param name="map">Карта лабиринта.</param>
        public GameController(LabyrinthMap map)
        {
            _map = map;
            var p1Pos = _map.ConvertToWorldCoordinates(1, 1);
            var p2Pos = _map.ConvertToWorldCoordinates(_map.Width() - 2, _map.Height() - 2);

            Player1 = new Player(1, p1Pos.X, p1Pos.Y);
            Player2 = new Player(2, p2Pos.X, p2Pos.Y);

            _allPlayers = new List<Player> { Player1, Player2 };
            StartGame();
        }

        /// <summary>
        /// Сбрасывает все игровые параметры и подготавливает сущности к началу новой игры.
        /// </summary>
        private void StartGame()
        {
            _sessionTime = 0;
            _prizeSpawnTimer = 0;
            _stage1Spawned = 0;
            _stage2Spawned = 0;
            _stage3Spawned = 0;
            _canSpawnNewEnemies = true;
            IsGameOver = false;

            foreach (var e in GameObjects.ToList())
            {
                RemoveEntity(e);
            }

            foreach (var player in _allPlayers)
            {
                player.Respawn();
                player.Armor = Player.DefaultArmor;
                player.Score = 0;
                player.OnDied -= OnPlayerDied;
                player.OnDied += OnPlayerDied;
                AddEntity(player);
            }

            UpdateFactory();
            FillLevelWithEnemies();
        }

        /// <summary>
        /// Основной метод обновления игрового мира. Вызывается каждый кадр.
        /// </summary>
        /// <param name="deltaTime">Время, прошедшее с последнего кадра.</param>
        public void UpdateWorld(float deltaTime)
        {
            if (IsGameOver) return;

            _sessionTime += deltaTime;
            _prizeSpawnTimer += deltaTime;

            UpdatePlayers(deltaTime);
            UpdateFactory();
            UpdateSpawners();
            ProcessBullets();
            ProcessEnemies(deltaTime);
            ProcessPrizePickups(deltaTime);

            if (_canSpawnNewEnemies)
            {
                MaintainEnemyCount();
            }
        }

        /// <summary>
        /// Обновляет логику состояния игроков.
        /// </summary>
        private void UpdatePlayers(float deltaTime)
        {
            Player1.Update(deltaTime);
            Player2.Update(deltaTime);
        }

        /// <summary>
        /// Управляет временем появления бонусов в мире.
        /// </summary>
        private void UpdateSpawners()
        {
            if (_sessionTime > PrizeInitialDelay && _prizeSpawnTimer >= PrizeSpawnInterval)
            {
                SpawnPrizePair();
                _prizeSpawnTimer = 0;
            }
        }

        /// <summary>
        /// Поддерживает необходимое количество врагов на карте, учитывая текущую сложность.
        /// </summary>
        private void MaintainEnemyCount()
        {
            if (!_canSpawnNewEnemies) return;
            if (Enemies.Count < TargetEnemyCount)
            {
                AddEntity(_currentEnemyFactory.SpawnRandom(EnemySpawnDistance));
                if (_currentEnemyFactory is LightEnemyFactory) _stage1Spawned++;
                else if (_currentEnemyFactory is EliteEnemyFactory) _stage2Spawned++;
                else if (_currentEnemyFactory is ChaosEnemyFactory) _stage3Spawned++;
            }
        }

        /// <summary>
        /// Первичное заполнение уровня врагами до целевого показателя.
        /// </summary>
        private void FillLevelWithEnemies()
        {
            while (Enemies.Count < TargetEnemyCount && _canSpawnNewEnemies)
            {
                MaintainEnemyCount();
            }
        }

        /// <summary>
        /// Выбирает подходящую фабрику врагов в зависимости от прогресса игрока по этапам.
        /// </summary>
        private void UpdateFactory()
        {
            if (_stage1Spawned < EnemiesPerStage)
            {
                if (!(_currentEnemyFactory is LightEnemyFactory))
                    _currentEnemyFactory = new LightEnemyFactory(_map, _allPlayers);
            }
            else if (_stage2Spawned < EnemiesPerStage)
            {
                if (!(_currentEnemyFactory is EliteEnemyFactory))
                    _currentEnemyFactory = new EliteEnemyFactory(_map, _allPlayers);
            }
            else if (_stage3Spawned < EnemiesPerStage)
            {
                if (!(_currentEnemyFactory is ChaosEnemyFactory))
                    _currentEnemyFactory = new ChaosEnemyFactory(_map, _allPlayers);
            }
            else
            {
                _canSpawnNewEnemies = false;
            }
        }

        /// <summary>
        /// Генерирует пару случайных призов на карте.
        /// </summary>
        private void SpawnPrizePair()
        {
            var pair = PrizeFactory.SpawnRandomPair(_map);
            foreach (var prize in pair)
            {
                AddEntity(prize);
            }
        }

        /// <summary>
        /// Обрабатывает время жизни призов и их проверку на столкновение с игроками.
        /// </summary>
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

        /// <summary>
        /// Проверяет, подобрал ли конкретный игрок конкретный приз.
        /// </summary>
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

        /// <summary>
        /// Регистрирует новую сущность в соответствующих списках игрового мира.
        /// </summary>
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

        /// <summary>
        /// Удаляет сущность из игрового мира.
        /// </summary>
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

        /// <summary>
        /// Обновляет позиции пуль и обрабатывает столкновения со стенами и юнитами.
        /// </summary>

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

        //private void ProcessBullets()
        //{
        //    foreach (var bullet in ActiveBullets.ToArray())
        //    {
        //        // Сначала смотрим, нет ли коллизии там, где пуля УЖЕ стоит
        //        var grid = _map.ConvertToTileCoordinates(bullet.Position);
        //        if (_map.IsWall(grid.X, grid.Y) || CheckBulletCollisions(bullet))
        //        {
        //            RemoveEntity(bullet);
        //            continue; // Пуля уничтожена, не обновляем её
        //        }

        //        bullet.Update(); // И только если преград нет — двигаем
        //    }
        //}

        /// <summary>
        /// Выполняет детальную проверку столкновения пули с игроками или врагами.
        /// </summary>
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
                foreach (var enemy in Enemies.ToArray())
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

        /// <summary>
        /// Обновляет позицию врагов и обрабатывает их стрельбу.
        /// </summary>
        private void ProcessEnemies(float deltaTime)
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                var enemy = Enemies[i];
                if (enemy.UpdatePosition(_map, Player1, Player2, deltaTime))
                {
                    CreateBullet(enemy);
                }
            }
        }

        /// <summary>
        /// Обработчик события смерти врага. Начисляет очки и проверяет условие победы.
        /// </summary>
        private void OnEnemyDied(Unit victim, int killerId)
        {
            if (killerId == 1) Player1.Score += ((Enemy)victim).Score;
            else if (killerId == 2) Player2.Score += ((Enemy)victim).Score;

            if (victim is Enemy enemy)
            {
                RemoveEntity(enemy);
            }

            if (!_canSpawnNewEnemies && Enemies.Count == 0)
            {
                IsGameOver = true;
            }
        }

        /// <summary>
        /// Обработчик события смерти игрока. Вызывает немедленный респавн.
        /// </summary>
        private void OnPlayerDied(Unit victim, int killerId)
        {
            ((Player)victim).Respawn();
        }

        /// <summary>
        /// Распределяет входящий ввод между первым и вторым игроком.
        /// </summary>
        public void HandleInput(GameInput input)
        {
            ProcessPlayerInput(Player1, input, GameInput.W, GameInput.S, GameInput.A, GameInput.D, GameInput.Space);
            ProcessPlayerInput(Player2, input, GameInput.Up, GameInput.Down, GameInput.Left, GameInput.Right, GameInput.Enter);
        }

        /// <summary>
        /// Обрабатывает конкретную клавишу для указанного игрока (движение или стрельба).
        /// </summary>
        private void ProcessPlayerInput(Player player, GameInput input, GameInput up, GameInput down, GameInput left, GameInput right, GameInput fire)
        {
            if (input == fire)
            {
                if (player.CanShoot)
                {
                    CreateBullet(player);
                    player.ResetShootTimer();
                }
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

        /// <summary>
        /// Создает пулю от указанного юнита и добавляет её в мир.
        /// </summary>
        private void CreateBullet(Unit unit)
        {
            var bullet = unit.Shoot();
            if (bullet != null) AddEntity(bullet);
        }

        /// <summary>
        /// Принудительно завершает текущую игру.
        /// </summary>
        public void FinishGameManually() => IsGameOver = true;

        /// <summary>
        /// Публичный метод для перезапуска игры.
        /// </summary>
        public void Reset() => StartGame();
    }
}