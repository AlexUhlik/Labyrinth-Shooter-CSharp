using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Bullets
{
    public static class BulletTools
    {
        public static IBullet Cleanup(IBullet root)
        {
            while (root is BulletDecorator decorator && decorator.IsExpired)
            {
                root = decorator.Inner;
            }

            if (root is BulletDecorator current)
            {
                current.Inner = Cleanup(current.Inner);
            }

            return root;
        }

        public static bool IsDecoratorActive<T>(IBullet bullet)
        {
            if (bullet is T) return true;
            if (bullet is BulletDecorator decorator) return IsDecoratorActive<T>(decorator.Inner);
            return false;
        }
    }
}
