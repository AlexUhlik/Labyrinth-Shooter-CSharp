using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameCore
{
    public abstract class GameObject
    {
        public Point Position { get; set; }
        public float Size { get; set; }
        public int TextureId { get; set; }
        public bool IsActive { get; set; } = true;

        public GameObject(float x, float y, float size)
        {
            Position = new Point(x, y);
            Size = size;
        }

        public abstract void Draw();

        public RectangleF GetBounds()
        {
            return new RectangleF(Position.X, Position.Y, Size, Size);
        }
    }
}
