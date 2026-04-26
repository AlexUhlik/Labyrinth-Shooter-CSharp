using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Bullets
{
    public class FastAmmo : BulletDecorator
    {
        public FastAmmo(IBullet bullet, float duration) : base(bullet, duration)
        {
        }

        public override float GetSpeed()
        {
            float baseSpeed = base.GetSpeed();

            return baseSpeed * 1.5f;
        }
    }
}
