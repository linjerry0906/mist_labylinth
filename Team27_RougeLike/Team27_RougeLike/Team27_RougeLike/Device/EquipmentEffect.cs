using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Device
{
    class EquipmentEffect
    {
        private int power;
        private int defense;

        public EquipmentEffect(int power, int defense)
        {
            this.power = power;
            this.defense = defense;
        }

        public int GetPower()
        {
            return power;
        }

        public int GetDefense()
        {
            return defense;
        }
    }
}
