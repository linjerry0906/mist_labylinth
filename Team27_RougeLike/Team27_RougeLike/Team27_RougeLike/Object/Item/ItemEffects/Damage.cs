using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item.ItemEffects
{
    class Damage : ItemEffect
    {
        private int amount;

        public Damage(int amount)
        {
            this.amount = amount;
        }

        public int GetAmount()
        {
            return amount;
        }
    }
}
