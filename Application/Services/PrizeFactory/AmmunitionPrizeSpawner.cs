using GameCore.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PrizeFactory
{
    public class AmmunitionPrizeSpawner : PrizeSpawner
    {
        public override Prize CreatePrize(float x, float y) => new AmmunitionPrize(x, y);
    }
}
