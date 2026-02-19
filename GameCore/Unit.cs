using System;
using System.Collections.Generic;
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

        public Unit(float x, float y, float size) : base(x, y, size)
        {
        }

        public void Move(float dx, float dy)
        {
            Position += new Point(dx, dy);
        }

        public virtual void TakeDamage(int damage)
        {
            int armorDamage = Math.Min(damage, Armor);

            int healtDamage = damage - armorDamage;

            Armor -= armorDamage;
            Health -= healtDamage;

            if (Health < 0)
            {
                Health = 0;
                IsActive = false;
            }
        }
    }
}
