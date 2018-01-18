using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item.ItemEffects
{
    class ArrowEffect : ItemEffect
    {
        private int power;

        public ArrowEffect(int power)
        {
            this.power = power;
        }

        public int GetPower()
        {
            return power;
        }
    }
}
