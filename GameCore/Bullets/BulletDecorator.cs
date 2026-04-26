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
        public float TimeLeft { get; set; } = 0;
        public bool IsExpired => TimeLeft <= 0;

        public IBullet Inner
        {
            get => _bullet;
            set => _bullet = value;
        }

        public BulletDecorator(IBullet bullet, float duration) 
        {
            _bullet = bullet;
            TimeLeft = duration;
        }

        public void UpdateTime(float deltaTime)
        {
            TimeLeft -= deltaTime;
            if (_bullet is BulletDecorator innerDecorator)
            {
                innerDecorator.UpdateTime(deltaTime);
            }
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
