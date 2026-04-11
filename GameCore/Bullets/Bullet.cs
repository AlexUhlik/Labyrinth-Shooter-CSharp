using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Bullets
{
    public class Bullet : GameObject
    {
        private readonly IBullet _stats;
        public float DirX { get; }
        public float DirY { get; }
        public int OwnerId { get; }

        public Bullet(float x, float y, float dirX, float dirY, IBullet stats, int ownerId)
            : base(x, y, 12f) 
        {
            _stats = stats;
            DirX = dirX;
            DirY = dirY;
            OwnerId = ownerId;
        }

        public void Update()
        {
            float speed = _stats.GetSpeed();
            Position = new Point(Position.X + DirX * speed, Position.Y + DirY * speed);
        }

        public int GetDamage() => _stats.GetDamage(); 
    }
}
