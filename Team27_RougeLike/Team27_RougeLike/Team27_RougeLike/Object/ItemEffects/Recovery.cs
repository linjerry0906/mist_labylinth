using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.itemEffects
{
    class Recovery : ItemEffect
    {
        private int amount;

        public Recovery(int amount)
        {
            this.amount = amount;
        }

        public int GetAmount()
        {
            return amount;
        }
    }
}
