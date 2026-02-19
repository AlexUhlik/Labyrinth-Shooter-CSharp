using GameCore.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Items
{
    public class AmmunitionPrize : Prize
    {
        public AmmunitionPrize(float x, float y) : base(x, y) 
        { 
        }

        public override void ApplyEffect(Player player)
        {
            player.Ammunition += 15;
        }
    }
}
