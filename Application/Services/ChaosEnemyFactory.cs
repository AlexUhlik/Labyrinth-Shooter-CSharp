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
    public class ChaosEnemyFactory : EnemyFactory
    {
        public ChaosEnemyFactory(LabyrinthMap map, List<Player> players)
            : base(map, players) { }

        public override Enemy CreateEnemy(float x, float y)
        {
            return new Enemy(x, y)
            {
                Health = 250,
                Speed = 3.0f,
                DisplayColor = System.Drawing.Color.Red
            };
        }
    }
}
