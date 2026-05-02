using GameCore.Items;
using GameCore.Map;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    /// <summary>
    /// Статический класс-фабрика для создания и случайного размещения призов на карте.
    /// Централизует логику выбора типа бонуса и его координат.
    /// </summary>
    public static class PrizeFactory
    {
        private static readonly Random rnd = new Random();

        /// <summary> Хранит тип последнего созданного приза для предотвращения повторов. </summary>
        private static int _lastPrizeType = -1;

        /// <summary>
        /// Создает конкретный экземпляр приза на основе переданного индекса.
        /// </summary>
        public static Prize CreatePrize(int prizeType, float x, float y)
        {
            switch (prizeType)
            {
                case 0:
                    return new HealthPrize(x, y);
                case 1:
                    return new AmmunitionPrize(x, y);
                case 2:
                    return new ExplosivePrize(x, y);
                case 3:
                    return new FastPrize(x, y);
                default:
                    return new HealthPrize(x, y);
            }
        }

        /// <summary>
        /// Создает пару одинаковых призов в симметричных точках карты.
        /// Используется для поддержания баланса (например, в мультиплеере или для равномерного распределения).
        /// </summary>
        public static List<Prize> SpawnRandomPair(LabyrinthMap map, int? forceType = null)
        {
            int width = map.Width();
            int height = map.Height();

            int x, y;
            // Выбор случайной точки для первого приза
            do
            {
                x = rnd.Next(0, width);
                y = rnd.Next(0, height);
            } while (map.IsWall(x, y));

            // Вычисление центрально-симметричных координат для второго приза
            int mx = width - 1 - x;
            int my = height - 1 - y;

            var pos1 = map.ConvertToWorldCoordinates(x, y);
            var pos2 = map.ConvertToWorldCoordinates(mx, my);

            int type;
            if (forceType.HasValue)
            {
                type = forceType.Value;
            }
            else
            {
                // Пул весов для генерации для более частого выпадания патронов
                int[] pool = { 0, 1, 1, 1, 2, 3 };
                do
                {
                    type = pool[rnd.Next(pool.Length)];
                } while (type == _lastPrizeType && type != 1); 
            }

            _lastPrizeType = type;

            return new List<Prize>
            {
                CreatePrize(type, pos1.X, pos1.Y),
                CreatePrize(type, pos2.X, pos2.Y)
            };
        }
    }
}