using GameCore.Bullets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public abstract class Unit : GameObject
    {
        public int Health { get; set; }
        public int Armor { get; set; }
        public float Speed { get; set; }
        public float Rotation { get; set; } = 0f;
        public float DirectionX => (float)Math.Cos(Rotation);
        public float DirectionY => (float)Math.Sin(Rotation);

        public event Action<Unit, int> OnDied;

        public Unit(float x, float y, float size) : base(x, y, size)
        {
        }

        public void Move(float dx, float dy)
        {
            Position += new Point(dx, dy);
        }

        public void SetDirection(float dx, float dy)
        {
            Rotation = (float)Math.Atan2(dy, dx);
        }

        //public (float X, float Y) GetIndicatorPosition(float indicatorSize)
        //{
        //    float offset = Size / 2f - indicatorSize / 2f;
        //    float indicatorX = Position.X + DirectionX * offset;
        //    float indicatorY = Position.Y + DirectionY * offset;

        //    return (indicatorX, indicatorY);
        //}

        public virtual void TakeDamage(int damage, int attackerId)
        {
            int armorDamage = Math.Min(damage, Armor);

            int healtDamage = damage - armorDamage;

            Armor -= armorDamage;
            Health -= healtDamage;

            if (Health <= 0)
            {
                Health = 0;
                IsActive = false;
                OnDied?.Invoke(this, attackerId);
            }
        }

        public void TakeDamage(Bullet bullet)
        {
            TakeDamage(bullet.GetDamage(), bullet.OwnerId);
        }

        public abstract Bullet Shoot(); 
    }


}
