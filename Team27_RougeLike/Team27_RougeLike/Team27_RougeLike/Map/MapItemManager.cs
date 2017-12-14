using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    class MapItemManager
    {
        private List<Item3D> items;
        private GameDevice gameDevice;
        private ItemManager itemManager;

        public MapItemManager(ItemManager itemManager, GameDevice gameDevice)
        {
            this.itemManager = itemManager;
            this.gameDevice = gameDevice;

            items = new List<Item3D>();
        }

        public void Initialize()
        {
            items.Clear();
        }


        public void AddItem(Vector3 position)
        {
            Item itemInfo = itemManager.GetConsuptionitem();
            Item3D addItem = new Item3D(gameDevice, itemInfo, position);
            items.Add(addItem);
        }

        public void AddEquip(Vector3 position)
        {
            Item itemInfo = itemManager.GetEquipmentItem();
            Item3D addItem = new Item3D(gameDevice, itemInfo, position);
            items.Add(addItem);
        }

        public void Draw()
        {
            items.ForEach(i => i.Draw());
        }
    }
}
