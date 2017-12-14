using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.Device
{
    class SaveData
    {
        private int clearFloor;
        private List<Item> itemList;
        private List<Item> equipList;

        public SaveData()
        {
            clearFloor = 1;
            itemList = new List<Item>();
            equipList = new List<Item>();
        }

        
    }
}
