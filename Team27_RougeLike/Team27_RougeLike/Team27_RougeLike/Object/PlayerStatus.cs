using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.Object
{
    class PlayerStatus
    {
        private Status status;
        private Status baseStatus;
        private Inventory inventory;

        private int addPower;
        private int addDefence;
        private float weight;

        public PlayerStatus(Status status)
        {
            this.baseStatus = status;
            inventory = new Inventory();
        }

        public void Initialize()
        {
            status = new Status(baseStatus.Level, baseStatus.Health, baseStatus.BasePower, baseStatus.BaseArmor, baseStatus.Attackspd, baseStatus.Movespeed);
        }

        public void AddStatus()
        {
            inventory.GetStatus(ref addPower, ref addDefence, ref weight);
        }

        public int GetPower()
        {
            return status.BasePower + addPower;
        }

        public int GetDefence()
        {
            return status.BaseArmor + addDefence;
        }

        public int GetAttackSpeed()
        {
            return status.Attackspd + (int)(weight / 10 / status.BasePower);
        }

        public float GetVelocty()
        {
            return status.Movespeed - weight / 100 / status.BasePower;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }
    }
}
