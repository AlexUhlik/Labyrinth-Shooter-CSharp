using System;
using System.Collections.Generic;
using GameCore.Characters;
using GameCore.Map;

namespace GameCore.Factories
{
    public abstract class EnemyFactory
    {
        protected static Random _rnd = new Random();
        protected LabyrinthMap _map;
        protected List<Player> _players;

        public EnemyFactory(LabyrinthMap map, List<Player> players)
        {
            _map = map;
            _players = players;
        }

        public abstract Enemy CreateEnemy(float x, float y, float multiplier);

        public Enemy SpawnRandom(float minDistance, float multiplier)
        {
            while (true)
            {
                int rx = _rnd.Next(1, _map.Width() - 1);
                int ry = _rnd.Next(1, _map.Height() - 1);

                if (_map.IsWall(rx, ry)) continue;

                var pos = _map.ConvertToWorldCoordinates(rx, ry);
                bool isSafe = true;

                foreach (var player in _players)
                {
                    double dist = Math.Sqrt(Math.Pow(pos.X - player.Position.X, 2) +
                                            Math.Pow(pos.Y - player.Position.Y, 2));
                    if (dist < minDistance)
                    {
                        isSafe = false;
                        break;
                    }
                }

                if (isSafe)
                {
                    return CreateEnemy(pos.X, pos.Y, multiplier);
                }
            }
        }
    }
}