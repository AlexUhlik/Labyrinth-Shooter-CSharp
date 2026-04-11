using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameCore.Items;

namespace Application
{
    public static class PrizeFactory
    {
        private static Random rnd = new Random();

        public static Prize CreatePrize(float x, float y)
        {
            int prizeType = rnd.Next(0, 4);

            switch (prizeType)
            {
                case 0:
                    return new HealthPrize(x, y);
                case 1:
                    return new AmmunitionPrize(x, y);
                case 2:
                    return new ExplosivePrize(x, y);
                case 3:
                    return new FastPrize(x, y);
                default:
                    return new HealthPrize(x, y);
            }
        }
    }
}
