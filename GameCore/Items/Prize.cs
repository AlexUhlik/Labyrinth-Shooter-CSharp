using GameCore.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Items
{
    public abstract class Prize : GameObject
    {
        public Prize(float x, float y) : base(x, y, 30)
        {
        }

        public abstract void ApplyEffect(Player player);

    }
}
