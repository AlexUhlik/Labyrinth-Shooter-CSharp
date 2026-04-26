using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Bullets
{
    public class ExplosiveAmmo : BulletDecorator
    {
        public ExplosiveAmmo(IBullet bullet, float duration) : base(bullet, duration)
        {
        }

        public override int GetDamage()
        {
            int baseDamage = base.GetDamage();
            return baseDamage + 15;
        }
    }
}
