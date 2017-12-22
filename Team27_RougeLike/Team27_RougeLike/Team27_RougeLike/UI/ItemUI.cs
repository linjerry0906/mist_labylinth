//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.21
// 内容  ：Itemを表示するUI
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.Scene;

namespace Team27_RougeLike.UI
{
    class ItemUI
    {
        private Renderer renderer;
        private InputState input;

        private Vector2 position;
        private Inventory playerItem;
        private List<Item> itemList;

        private List<Button> buttons;
        private Item currentItem;

        private ItemInfoUI currentInfo;

        private readonly int WIDTH = 200;
        private readonly int HEIGHT = 20;

        public ItemUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            this.position = position;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            playerItem = gameManager.PlayerItem;

            buttons = new List<Button>();
            itemList = playerItem.BagList();

            for(int i = 0; i < itemList.Count; i++)
            {
                buttons.Add(
                    new Button(position + new Vector2(0, i * HEIGHT), WIDTH, HEIGHT));
            }

            currentItem = null;
            currentInfo = new ItemInfoUI(position + new Vector2(0, 540), gameDevice);
        }

        /// <summary>
        /// ボタンの更新
        /// </summary>
        public void Update()
        {
            if (!input.IsLeftClick())       //clickしていなかったら判定
                return;

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);
            int index = 0;
            foreach(Button b in buttons)
            {
                if (b.IsClick(mousePos))
                {
                    break;
                }
                index++;
            }
            if (index == buttons.Count)
                return;

            currentItem = itemList[index];
        }

        /// <summary>
        /// Listを表示
        /// </summary>
        /// <param name="alpha">透明値</param>
        public void Draw(float alpha)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                renderer.DrawString(
                    itemList[i].GetItemName(),
                    position + new Vector2(0, i * HEIGHT),
                    Color.White,
                    new Vector2(1.2f, 1.2f),
                    alpha, false, true);
            }

            if (currentItem == null)
                return;

            currentInfo.Draw(currentItem, alpha);
        }
    }
}
