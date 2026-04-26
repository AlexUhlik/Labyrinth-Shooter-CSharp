using GameCore.Items;
using GameCore.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class PrizeFactory
    {
        private static Random rnd = new Random();

        private static int _lastPrizeType = -1;

        public static Prize CreatePrize(int prizeType, float x, float y)
        {
            //int prizeType = rnd.Next(0, 4);

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
        public static Prize SpawnRandom(LabyrinthMap _map, int? forceType = null)
        {
            int x, y;
            do
            {
                x = rnd.Next(0, _map.Width());
                y = rnd.Next(0, _map.Height());
            } while (_map.IsWall(x, y));

            var pos = _map.ConvertToWorldCoordinates(x, y);
            return CreatePrize(forceType ?? rnd.Next(0, 4), pos.X, pos.Y);
        }

        public static List<Prize> SpawnRandomPair(LabyrinthMap map, int? forceType = null)
        {
            int width = map.Width();
            int height = map.Height();

            int x, y;
            do
            {
                x = rnd.Next(0, width);
                y = rnd.Next(0, height);
            } while (map.IsWall(x, y));

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
                do
                {
                    type = rnd.Next(0, 4);
                } while (type == _lastPrizeType);
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
