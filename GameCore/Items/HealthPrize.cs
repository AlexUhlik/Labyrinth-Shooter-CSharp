using GameCore.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Items
{
    public class HealthPrize : Prize
    {
        public HealthPrize(float x, float y) : base(x, y)
        {
        }

        public override void ApplyEffect(Player player)
        {
            int restoredHealth = player.Health + 25;
            player.Health = Math.Min(restoredHealth, Player.MaxHealth);
        }
    }
}
