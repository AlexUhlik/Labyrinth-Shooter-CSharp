using GameCore.Bullets;
using GameCore.Characters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Items
{
    public class ExplosivePrize : Prize
    {
        public override Color DisplayColor { get; set; } = Color.DarkRed;
        public ExplosivePrize(float x, float y) : base(x, y)
        {
        }

        public override void ApplyEffect(Player player)
        {
            //IBullet baseBullet = player.CurrentBullet.GetBase();
            player.CurrentBullet = new ExplosiveAmmo(player.CurrentBullet, 10f);
        }
    }
}
