using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Characters
{
    public class Enemy : Unit
    {
        public Enemy(float x, float y) : base(x, y, 50)
        {
            Health = 50;
            Armor = 50;
            Speed = 2.0f;
        }

        public override void Draw()
        {
            
        }

        public void UpdatePosition() 
        { 
        }
    }
}
