using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Bullets
{
    public class ExplosiveAmmo : BulletDecorator
    {
        public ExplosiveAmmo(IBullet bullet) : base(bullet)
        {
        }

        public override int GetDamage()
        {
            int baseDamage = base.GetDamage();
            return baseDamage + 15;
        }
    }
}
