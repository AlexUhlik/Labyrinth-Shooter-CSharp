using GameCore.Characters;
using GameCore.Factories;
using GameCore.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class Type1EnemyFactory : EnemyFactory
    {
        public Type1EnemyFactory(LabyrinthMap map, List<Player> players)
            : base(map, players) { }

        public override Enemy CreateEnemy(float x, float y, float multiplier)
        {
            var enemy = new Enemy(x, y);
            enemy.Health = (int)(enemy.Health * multiplier);
            enemy.Armor = (int)(enemy.Armor * multiplier);

            enemy.Speed *= (1.0f + (multiplier - 1.0f) * 0.2f);
            return enemy;
        }
    }
}
