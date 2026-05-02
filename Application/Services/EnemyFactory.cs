using System;
using System.Collections.Generic;
using GameCore.Characters;
using GameCore.Map;

namespace Application.Services
{
    /// <summary>
    /// Базовый абстрактный класс для фабрик, создающих противников.
    /// Инкапсулирует логику случайного спавна с учетом проходимости карты и дистанции до игроков.
    /// </summary>
    public abstract class EnemyFactory
    {
        protected static readonly Random _rnd = new Random();

        /// <summary> Ссылка на карту лабиринта для проверки препятствий. </summary>
        protected readonly LabyrinthMap _map;

        /// <summary> Список текущих игроков для расчета безопасной зоны спавна. </summary>
        protected readonly List<Player> _players;

        /// <summary>
        /// Инициализирует новый экземпляр фабрики противников.
        /// </summary>
        /// <param name="map">Текущая карта лабиринта.</param>
        /// <param name="players">Список активных игроков.</param>
        protected EnemyFactory(LabyrinthMap map, List<Player> players)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _players = players ?? throw new ArgumentNullException(nameof(players));
        }

        /// <summary>
        /// Абстрактный метод для создания конкретного экземпляра противника.
        /// </summary>
        /// <param name="x">Координата по оси X в игровом мире.</param>
        /// <param name="y">Координата по оси Y в игровом мире.</param>
        /// <returns>Экземпляр созданного противника.</returns>
        public abstract Enemy CreateEnemy(float x, float y);

        /// <summary>
        /// Выполняет поиск случайной свободной точки на карте и создает там противника.
        /// Гарантирует, что враг не появится в стене или ближе, чем <paramref name="minDistance"/> к игроку.
        /// </summary>
        /// <param name="minDistance">Минимально допустимое расстояние до любого игрока.</param>
        /// <returns>Экземпляр созданного противника.</returns>
        public Enemy SpawnRandom(float minDistance)
        {
            float minDistanceSq = minDistance * minDistance;

            while (true)
            {
                // Выбор случайных координат внутри границ
                int rx = _rnd.Next(1, _map.Width() - 1);
                int ry = _rnd.Next(1, _map.Height() - 1);

                if (_map.IsWall(rx, ry)) continue;

                var pos = _map.ConvertToWorldCoordinates(rx, ry);
                bool isSafe = true;

                foreach (var player in _players)
                {
                    float dx = pos.X - player.Position.X;
                    float dy = pos.Y - player.Position.Y;
                    float distSq = dx * dx + dy * dy;

                    if (distSq < minDistanceSq)
                    {
                        isSafe = false;
                        break;
                    }
                }

                // Если точка прошла проверку на безопасность, создаем врага
                if (isSafe)
                {
                    return CreateEnemy(pos.X, pos.Y);
                }
            }
        }
    }
}