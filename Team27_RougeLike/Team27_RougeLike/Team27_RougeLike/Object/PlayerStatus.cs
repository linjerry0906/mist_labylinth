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
        private Inventory inventory;

        private int addPower;
        private int addDefence;
        private float wight;

        public PlayerStatus(Status status)
        {
            this.status = status;
            inventory = new Inventory();
        }

        public void Initialize()
        {
            
        }

        public void AddStatus()
        {

        }
    }
}
