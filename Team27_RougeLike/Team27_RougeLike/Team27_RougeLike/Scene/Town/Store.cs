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
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.UI;

namespace Team27_RougeLike.Scene.Town
{
    class Store
    {
        private GameManager gameManager;
        private GameDevice gameDevice;
        private Renderer renderer;
        private InputState input;
        private ItemManager itemManager;
        private Inventory playerInventory;

        private List<Item> consumptions;
        private List<Item> equipments;

        private List<ItemButton> leftItemButtons;
        private List<ItemButton> rightItemButtons;

        private int totalPrice;
        private bool isFull;
        private bool isInventoryFull;
        private int maxNum;
        private int currentNum;

        private Vector2 buttonPosition;
        private Window buttonWindow;
        private Rectangle buyRect;

        public Store(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            itemManager = gameManager.ItemManager;
            playerInventory = gameManager.PlayerItem;

            totalPrice = 0;
            isInventoryFull = false;

            //仮
            buttonPosition = new Vector2(1080 - 64, 720 - 64);
            buttonWindow = new Window(gameDevice, buttonPosition, new Vector2(1080, 720));
            buyRect = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, 320, 64);
        }

        public void Initialize()
        {
            consumptions = new List<Item>();
            equipments = new List<Item>();
            leftItemButtons = new List<ItemButton>();
            rightItemButtons = new List<ItemButton>();

            consumptions = itemManager.GetConsumptionList();
            equipments = itemManager.GetEquipmentList();

            isFull = false;

            for (int i = 0; i < equipments.Count; i++)
            {
                leftItemButtons.Add(new ItemButton(gameDevice, new Vector2(64, 96 + 32 * (i + 1)),
                    equipments[i]));
                leftItemButtons[i].Initialize();
            }
            for (int i = 0; i < consumptions.Count; i++)
            {
                leftItemButtons.Add(new ItemButton(gameDevice, new Vector2(64, 96 + 32 * (i + 1 + equipments.Count)),
                    consumptions[i]));
                leftItemButtons[i + equipments.Count].Initialize();
            }
            totalPrice = 0;

            buttonWindow.Initialize();
            buttonWindow.Switch();
        }

        public void Update()
        {
            buttonWindow.Update();

            playerInventory.ItemCount(ref currentNum, ref maxNum);


            Point mousePos = new Point(
                (int)input.GetMousePosition().X,
                (int)input.GetMousePosition().Y);

            if (buyRect.Contains(mousePos) && input.IsLeftClick())
            {
                //お金があるかチェック

                if (maxNum >= currentNum + rightItemButtons.Count)
                {
                    foreach (ItemButton buttons in rightItemButtons)
                    {
                        playerInventory.AddItem(buttons.GetItem());
                    }
                    totalPrice = 0;
                    rightItemButtons = new List<ItemButton>();
                }
            }

            for (int i = 0; i < leftItemButtons.Count; i++)
            {
                leftItemButtons[i].Update(mousePos);
                if (leftItemButtons[i].IsClick())
                {
                    if (!isFull)
                    {
                        rightItemButtons.Add(new ItemButton(gameDevice,
                            new Vector2(1080 / 2 + 64, 96 + 32 * (rightItemButtons.Count + 1)),
                            leftItemButtons[i].GetItem()));
                        rightItemButtons[rightItemButtons.Count - 1].Initialize();

                        totalPrice += leftItemButtons[i].GetItem().GetItemPrice();
                    }
                }
            }
            for (int i = 0; i < rightItemButtons.Count; i++)
            {
                rightItemButtons[i].Update(mousePos);
                if (rightItemButtons[i].IsClick())
                {
                    totalPrice -= rightItemButtons[i].GetItem().GetItemPrice();

                    rightItemButtons.Remove(rightItemButtons[i]);
                }
            }

            if (rightItemButtons.Count() >= 15)
            {
                isFull = true;
                return;
            }
            else
            {
                isFull = false;
            }
        }

        public void DrawEquip()
        {
            buttonWindow.Draw();
            renderer.DrawString("買う", buttonPosition, new Vector2(1, 1), Color.White);

            renderer.DrawString("アイテム名", new Vector2(64, 64 + 32), new Vector2(1, 1), Color.White);
            renderer.DrawString("値段", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);
            renderer.DrawString("タイプ", new Vector2(320, 64 + 32), new Vector2(1, 1), Color.White);

            for (int i = 0; i < leftItemButtons.Count; i++)
            {
                leftItemButtons[i].DrawButton();
            }
            for (int i = 0; i < rightItemButtons.Count; i++)
            {
                rightItemButtons[i].DrawButton();
            }

            if (isFull)
            {
                renderer.DrawString("買い物リストがいっぱいです。", new Vector2(320, 720 / 2), new Vector2(2, 2), Color.Red);
            }

            renderer.DrawString("合計金額 : " + totalPrice, new Vector2(1080 - 320, 720 - 128 + 32), new Vector2(1, 1), Color.White);

            renderer.DrawString("バッグ(" + currentNum + "/" + maxNum + ")", new Vector2(1080 - 320, 64), new Vector2(1, 1), Color.White);

            //renderer.DrawString("銅貨銀金塊薬強化素材換金ゴミ回復痛1234(特大中小)系", Vector2.Zero, new Vector2(1, 1), Color.Black);
        }
    }
}
