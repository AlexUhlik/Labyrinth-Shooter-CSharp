using GameCore.Items;
using GameCore.Map;
using System;
using System.Collections.Generic;

namespace Application.Services.PrizeFactory
{
    public abstract class PrizeSpawner
    {
        private static readonly Random _random = new Random();

        public abstract Prize CreatePrize(float x, float y);

        public List<Prize> SpawnPair(LabyrinthMap map)
        {
            int width = map.Width();
            int height = map.Height();

            int x, y;
            do
            {
                x = _random.Next(0, width);
                y = _random.Next(0, height);
            } while (map.IsWall(x, y));

            int mx = width - 1 - x;
            int my = height - 1 - y;

            var pos1 = map.ConvertToWorldCoordinates(x, y);
            var pos2 = map.ConvertToWorldCoordinates(mx, my);

            return new List<Prize>
            {
                CreatePrize(pos1.X, pos1.Y),
                CreatePrize(pos2.X, pos2.Y)
            };
        }
    }
}