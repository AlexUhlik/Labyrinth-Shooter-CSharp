using GameCore.Characters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Items
{
    public abstract class Prize : GameObject
    {
        public virtual Color DisplayColor { get; set; }

        public float Age { get; private set; } = 0;
        public float MaxLifetime { get; set; } = 25f;
        public bool IsExpired => Age >= MaxLifetime;

        public void Update(float deltaTime)
        {
            Age += deltaTime;
        }
        public Prize(float x, float y) : base(x, y, 30)
        {
        }

        public abstract void ApplyEffect(Player player);

    }
}
