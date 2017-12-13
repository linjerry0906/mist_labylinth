using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.Scene.Town
{
    class Store
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private ItemManager itemManager;

        private List<Item> consumptions;
        private List<Item> equipments;

        public Store(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            itemManager = gameManager.ItemManager;
        }

        public void Initialize()
        {
            
        }
    }
}
