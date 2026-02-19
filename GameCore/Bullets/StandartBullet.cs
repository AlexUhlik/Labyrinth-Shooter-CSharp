using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Bullets
{
    public class StandartBullet : IBullet
    {
        public int GetDamage()
        {
            return 10;
        }

        public float GetSpeed()
        {
            return 8.0f;
        }

        public IBullet GetBase()
        {
            return this;
        }
    }
}
