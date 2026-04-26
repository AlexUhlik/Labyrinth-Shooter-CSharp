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
    public class FastPrize : Prize
    {
        public override Color DisplayColor { get; set; } = Color.LightYellow;
        public FastPrize(float x, float y) : base(x, y)
        {
        }

        public override void ApplyEffect(Player player)
        {
            //IBullet baseBullet = player.CurrentBullet.GetBase();
            player.CurrentBullet = new FastAmmo(player.CurrentBullet, 10f);
        }
    }
}
