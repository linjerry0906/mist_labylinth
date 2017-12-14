//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.13
// 内容　：Storeのクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.Scene.Town
{
    class Store
    {
        private GameDevice gameDevice;
        private Renderer renderer;
        private GameManager gameManager;
        private ItemManager itemManager;

        private List<Item> consumptions;
        private List<Item> equipments;

        public Store(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
            itemManager = gameManager.ItemManager;
        }

        public void Initialize()
        {
            consumptions = new List<Item>();
            equipments = new List<Item>();

            consumptions = itemManager.GetConsumptionList();
            equipments = itemManager.GetEquipmentList();
        }

        public void DrawEquip()
        {
            int amount = 0;
            foreach(Item i in equipments)
            {
                string name = i.GetItemName();
                int ID = i.GetItemID();
                renderer.DrawString(i.GetItemID().ToString(), new Vector2(20, 40 + amount * 40), new Vector2(1.5f, 1.5f), Color.Black);
                renderer.DrawString(i.GetItemName(), new Vector2(80, 40 + amount * 40), new Vector2(1.5f, 1.5f), Color.Black);
                renderer.DrawString(i.GetItemRare().ToString(), new Vector2(250, 40 + amount * 40), new Vector2(1.5f, 1.5f), Color.Black);
                renderer.DrawString(i.GetItemPrice().ToString(), new Vector2(310, 40 + amount * 40), new Vector2(1.5f, 1.5f), Color.Black);
                renderer.DrawString(i.GetItemWeight().ToString(), new Vector2(360, 40 + amount * 40), new Vector2(1.5f, 1.5f), Color.Black);
                renderer.DrawString(i.GetItemExplanation(), new Vector2(400, 40 + amount * 40), new Vector2(1.5f, 1.5f), Color.Black);
                amount++;
            }
        }
    }
}
