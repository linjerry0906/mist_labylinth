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
    class Buy
    {
        private GameManager gameManager;
        private GameDevice gameDevice;
        private Renderer renderer;
        private InputState input;
        private ItemManager itemManager;
        private Inventory playerInventory;

        private List<Item> consumptions;
        private List<Item> equipments;

        private List<Item> leftItems;
        private List<Item> rightItems;
        private List<Button> leftButtons;
        private List<Button> rightButtons;
        private List<Window> leftWindows;
        private List<Window> rightWindows;

        private int totalPrice;
        private bool isRightListFull;
        private bool isInventoryFullMessege;
        private Window messegeWindow;
        private int maxNum;
        private int currentNum;

        private int buttonWidht = 1080 / 2 - 128;
        private int buttonHeight = 20;
        
        private Vector2 buttonPosition;
        private Window buttonWindow;
        private Button buyButton;

        public Buy(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            itemManager = gameManager.ItemManager;
            playerInventory = gameManager.PlayerItem;

            totalPrice = 0;
            isInventoryFullMessege = false;
            isRightListFull = false;

            messegeWindow = new Window(gameDevice, new Vector2(1080 / 4, 720 / 2 - 10), new Vector2(1080 / 2, 720 / 12));

            buttonPosition = new Vector2(1080 - 64, 720 - 64);
            buttonWindow = new Window(gameDevice, buttonPosition, new Vector2(64, 32));
            buyButton = new Button(buttonPosition, 64, 32);
        }

        public void Initialize()
        {
            consumptions = new List<Item>();
            equipments = new List<Item>();

            leftItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();

            consumptions = itemManager.GetConsumptionList();
            equipments = itemManager.GetEquipmentList();

            isInventoryFullMessege = false;
            isRightListFull = false;
            messegeWindow.Initialize();
            messegeWindow.SetAlphaLimit(0.8f);

            for (int i = 0; i < equipments.Count; i++)
            {
                leftItems.Add(equipments[i]);
                Vector2 position = new Vector2(64, 96 + 32 * (i + 1));
                leftButtons.Add(new Button(position, buttonWidht, buttonHeight));
                leftWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
                leftWindows[i].Initialize();
                leftWindows[i].Switch();
            }
            for (int i = 0; i < consumptions.Count; i++)
            {
                leftItems.Add(consumptions[i]);
                Vector2 position = new Vector2(64, 96 + 32 * (i + equipments.Count + 1));
                leftButtons.Add(new Button(position, buttonWidht, buttonHeight));
                leftWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
                leftWindows[equipments.Count + i].Initialize();
                leftWindows[equipments.Count + i].Switch();
            }


            totalPrice = 0;

            buttonWindow.Initialize();
            buttonWindow.Switch();
        }

        public void Update()
        {
            buttonWindow.Update();
            messegeWindow.Update();

            if (isRightListFull || isInventoryFullMessege)
            {
                if (!messegeWindow.CurrentState())
                {
                    messegeWindow.Switch();
                }
            }
            else
            {
                if (messegeWindow.CurrentState())
                {
                    messegeWindow.Switch();
                }
            }

            playerInventory.ItemCount(ref currentNum, ref maxNum);

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);

            //アイテムを買う処理(お金が足りるか、バックにはいるかチェック)
            if (buyButton.IsClick(mousePos))
            {
                if (maxNum >= currentNum + rightItems.Count)
                {
                    //お金があるかチェック
                    {
                        if (input.IsLeftClick())
                        {
                            foreach (Item items in rightItems)
                            {
                                playerInventory.AddItem(items);
                            }
                            totalPrice = 0;
                            rightItems = new List<Item>();
                            rightButtons = new List<Button>();
                            rightWindows = new List<Window>();
                        }
                    }
                }
                else
                {
                    isInventoryFullMessege = true;
                }
            }
            else
            {
                isInventoryFullMessege = false;
            }

            for (int i = 0; i < leftButtons.Count; i++)
            {
                leftWindows[i].Update();

                //売り物リストから買う物リストに追加する処理(買い物リストが空いているかチェック)
                if (leftButtons[i].IsClick(mousePos))
                {
                    if (rightButtons.Count >= 15)
                    {
                        isRightListFull = true;
                    }
                    else if(input.IsLeftClick())
                    {
                        AddRightList(leftItems[i]);
                    }
                }
                else
                {
                    isRightListFull = false;
                }
            }

            for (int i = 0; i < rightButtons.Count; i++)
            {
                rightWindows[i].Update();

                //買う物リストからアイテムを削除する処理
                if (rightButtons[i].IsClick(mousePos))
                {
                    if (input.IsLeftClick())
                    {
                        RemoveRightList(i);
                    }
                }
            }
        }

        //右のリストにアイテムを追加する。
        public void AddRightList(Item item)
        {
            rightItems.Add(item);
            Vector2 position = new Vector2(1080 / 2 + 64, 96 + 32 * (rightButtons.Count + 1));
            rightButtons.Add(new Button(position, buttonWidht, buttonHeight));
            rightWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
            rightWindows[rightWindows.Count - 1].Initialize();
            rightWindows[rightWindows.Count - 1].Switch();
            totalPrice += item.GetItemPrice();
        }

        //右リストから指定されたアイテムを消す。
        public void RemoveRightList(int key)
        {
            totalPrice -= rightItems[key].GetItemPrice();
            rightItems.Remove(rightItems[key]);
            rightButtons.Remove(rightButtons[key]);
            rightWindows.Remove(rightWindows[key]);

            //上に詰める処理
            List<Item> copyRightItems = rightItems;
            totalPrice = 0;
            rightItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();
            foreach (Item item in copyRightItems)
            {
                AddRightList(item);
            }
        }

        public void DrawEquip()
        {
            buttonWindow.Draw();
            renderer.DrawString("買う", buttonPosition, new Vector2(1, 1), Color.White);

            renderer.DrawString("アイテム名", new Vector2(64, 64 + 32), new Vector2(1, 1), Color.White);
            renderer.DrawString("値段", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);
            renderer.DrawString("タイプ", new Vector2(320, 64 + 32), new Vector2(1, 1), Color.White);

            //左側のリストのアイテムの描画
            for (int i = 0; i < leftItems.Count; i++)
            {
                leftWindows[i].Draw();

                renderer.DrawString(leftItems[i].GetItemName(), leftWindows[i].GetOffsetPosition(), new Vector2(1, 1), Color.White);
                renderer.DrawString(leftItems[i].GetItemPrice().ToString(), leftWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                string type;
                if (leftItems[i] is WeaponItem)
                {
                    type = ((WeaponItem)leftItems[i]).GetWeaponType().ToString();
                }
                else if (leftItems[i] is ProtectionItem)
                {
                    type = ((ProtectionItem)leftItems[i]).GetProtectionType().ToString();
                }
                else
                {
                    type = ((ConsumptionItem)leftItems[i]).GetTypeText();
                }
                renderer.DrawString(type, leftWindows[i].GetOffsetPosition() + new Vector2(256, 0), new Vector2(1, 1), Color.White);
            }

            //右側のリストのアイテムの描画
            for (int i = 0; i < rightButtons.Count; i++)
            {
                rightWindows[i].Draw();

                renderer.DrawString(rightItems[i].GetItemName(), rightWindows[i].GetOffsetPosition(), new Vector2(1, 1), Color.White);
                renderer.DrawString(rightItems[i].GetItemPrice().ToString(), rightWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                string type;
                if (rightItems[i] is WeaponItem)
                {
                    type = ((WeaponItem)rightItems[i]).GetWeaponType().ToString();
                }
                else if (rightItems[i] is ProtectionItem)
                {
                    type = ((ProtectionItem)rightItems[i]).GetProtectionType().ToString();
                }
                else
                {
                    type = ((ConsumptionItem)rightItems[i]).GetTypeText();
                }
                renderer.DrawString(type, rightWindows[i].GetOffsetPosition() + new Vector2(256, 0), new Vector2(1, 1), Color.White);
            }

            messegeWindow.Draw();
            if (isRightListFull)
            {
                renderer.DrawString("買う物リストがいっぱいです。", new Vector2(320, 720 / 2), new Vector2(2, 2), Color.Red);
            }
            if (isInventoryFullMessege)
            {
                renderer.DrawString("バッグにはいりきりません。", new Vector2(320, 720 / 2), new Vector2(2, 2), Color.Red);
            }

            renderer.DrawString("合計金額 : " + totalPrice, new Vector2(1080 - 320, 720 - 128 + 32), new Vector2(1, 1), Color.White);

            renderer.DrawString("バッグ(" + currentNum + "/" + maxNum + ")", new Vector2(1080 - 320, 64), new Vector2(1, 1), Color.White);
        }
    }
}
