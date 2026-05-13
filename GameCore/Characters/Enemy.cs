using GameCore.Bullets;
using GameCore.Map;
using System;
using System.Drawing;

namespace GameCore.Characters
{
    /// <summary>
    /// Представляет враждебную сущность.
    /// Реализует алгоритмы патрулирования лабиринта и обнаружения игрока.
    /// </summary>
    public class Enemy : Unit
    {
        private static readonly Random _rnd = new Random();

        // Константы поведения 
        private const int MoveCooldownValue = 35;   // Задержка между сменой направления движения
        private const int ShootCooldownValue = 30;  // Задержка между выстрелами
        private const int VisionRange = 3;          // Дальность зрения в тайлах

        private int _moveTimer = 0;
        private int _shootTimer = 0;

        /// <summary> Цвет для визуализации врага в зависимости от его типа. </summary>
        public Color DisplayColor { get; set; } = Color.White;

        /// <summary> Количество очков, начисляемых игроку за уничтожение данного врага. </summary>
        public int Score { get; set; }

        /// <summary>
        /// Создает экземпляр врага в указанных координатах.
        /// </summary>
        public Enemy(float x, float y) : base(x, y, 60)
        {
            Health = 80;
            Armor = 50;
            Speed = 2.0f;
            CurrentBullet = new StandartBullet();
        }

        /// <summary>
        /// Создает объект пули.
        /// </summary>
        public override Bullet Shoot()
        {
            return new Bullet(Position.X, Position.Y, DirectionX, DirectionY, CurrentBullet, 3);
        }

        /// <summary>
        /// Основной цикл обновления логики врага.
        /// </summary>
        /// <returns>True, если враг готов произвести выстрел.</returns>
        public bool UpdatePosition(LabyrinthMap map, Player p1, Player p2, float deltaTime)
        {
            UpdateDamageFlash(deltaTime);

            if (CanSeeTarget(p1, map) || CanSeeTarget(p2, map))
            {
                _moveTimer = 0; 
                return UpdateShooting();
            }

            _shootTimer = 0;
            UpdatePatrolling(map);
            return false;
        }

        /// <summary>
        /// Логика накопления заряда для выстрела.
        /// </summary>
        private bool UpdateShooting()
        {
            _shootTimer++;
            if (_shootTimer >= ShootCooldownValue)
            {
                _shootTimer = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Алгоритм случайного перемещения по лабиринту.
        /// </summary>
        private void UpdatePatrolling(LabyrinthMap map)
        {
            _moveTimer++;
            if (_moveTimer < MoveCooldownValue) return;

            _moveTimer = 0;

            var (dx, dy) = GetRandomDirection();
            var nextPos = new Point(Position.X + dx, Position.Y + dy);
            var grid = map.ConvertToTileCoordinates(nextPos);

            if (!map.IsWall(grid.X, grid.Y))
            {
                SetDirection(dx, dy);
                Move(dx, dy);
            }
        }

        /// <summary>
        /// Генерирует случайный вектор направления, кратный размеру тайла.
        /// </summary>
        private (int dx, int dy) GetRandomDirection()
        {
            int dir = _rnd.Next(4);
            int dx = 0;
            int dy = 0;

            switch (dir)
            {
                case 0: dy = LabyrinthMap.TileSize; break;  // Вниз
                case 1: dy = -LabyrinthMap.TileSize; break; // Вверх
                case 2: dx = -LabyrinthMap.TileSize; break; // Влево
                case 3: dx = LabyrinthMap.TileSize; break;  // Вправо
            }
            return (dx, dy);
        }

        /// <summary>
        /// Алгоритм проверки прямой видимости.
        /// Проверяет отсутствие стен между врагом и игроком по осям X или Y.
        /// </summary>
        private bool CanSeeTarget(Player target, LabyrinthMap map)
        {
            if (target == null || target.Health <= 0) return false;

            var start = map.ConvertToTileCoordinates(Position);
            var end = map.ConvertToTileCoordinates(target.Position);

            // Если стоим на одной клетке
            if (start.X == end.X && start.Y == end.Y) return true;

            // Проверка дистанции зрения
            if (Math.Abs(start.X - end.X) > VisionRange || Math.Abs(start.Y - end.Y) > VisionRange)
                return false;

            bool onSameX = start.X == end.X;
            bool onSameY = start.Y == end.Y;

            // Проверка по горизонтали
            if (onSameY && Math.Abs(DirectionX) > 0.01f)
            {
                // Игрок должен быть в той же стороне, куда смотрит враг
                if (Math.Sign(end.X - start.X) == Math.Sign(DirectionX))
                {
                    return !IsWallBetween(start.X, end.X, start.Y, true, map);
                }
            }

            // Проверка по вертикали
            if (onSameX && Math.Abs(DirectionY) > 0.01f)
            {
                if (Math.Sign(end.Y - start.Y) == Math.Sign(DirectionY))
                {
                    return !IsWallBetween(start.Y, end.Y, start.X, false, map);
                }
            }

            return false;
        }

        /// <summary>
        /// Итерирует по линии между двумя точками для обнаружения препятствий.
        /// </summary>
        private bool IsWallBetween(int start, int end, int constant, bool isHorizontal, LabyrinthMap map)
        {
            int min = Math.Min(start, end);
            int max = Math.Max(start, end);

            for (int i = min; i <= max; i++)
            {
                bool hitWall = isHorizontal ? map.IsWall(i, constant) : map.IsWall(constant, i);
                if (hitWall) return true;
            }
            return false;
        }
    }
}