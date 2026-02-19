using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Bullets
{
    public interface IBullet
    {
        int GetDamage();
        float GetSpeed();

        IBullet GetBase();
    }
}
