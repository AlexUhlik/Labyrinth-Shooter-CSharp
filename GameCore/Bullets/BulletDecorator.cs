using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Bullets
{
    public abstract class BulletDecorator : IBullet
    {
        protected IBullet _bullet;
        public BulletDecorator(IBullet bullet) 
        {
            _bullet = bullet;
        }

        public virtual int GetDamage()
        {
            return _bullet.GetDamage();
        }

        public virtual float GetSpeed()
        {
            return _bullet.GetSpeed();
        }

        public virtual IBullet GetBase()
        {
            return _bullet.GetBase();
        }
    }
}
