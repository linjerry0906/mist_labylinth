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
        private enum ModeType
        {
            Buy,
            Sell,
            No,
        }

        private ModeType modeType;
        private string text = "バグ";

        private GameManager gameManager;
        private GameDevice gameDevice;
        private Renderer renderer;
        private InputState input;
        private ItemManager itemManager;
        private Inventory playerInventory;

        private List<Item> consumptions;
        private List<Item> equipments;
        private List<Item> playerItems;

        private List<Item> leftItems;
        private List<Item> rightItems;
        private List<Button> leftButtons;
        private List<Button> rightButtons;
        private List<Window> leftWindows;
        private List<Window> rightWindows;

        private int totalPrice;
        private bool isRightListFull;
        private bool isInventoryFullMessege;
        private bool isNoMoney;
        private Window messegeWindow;
        private int maxNum;
        private int currentNum;

        private int buttonWidht;
        private int buttonHeight = 20;
        
        private Vector2 buttonPosition;
        private Window buttonWindow;
        private Button button;

        private int windowWidth;
        private int windowHeight;

        public Store(GameManager gameManager, GameDevice gameDevice)
        {
            modeType = ModeType.No;

            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            itemManager = gameManager.ItemManager;
            playerInventory = gameManager.PlayerItem;

            totalPrice = 0;
            isInventoryFullMessege = false;
            isRightListFull = false;
            isNoMoney = false;

            windowWidth = Def.WindowDef.WINDOW_WIDTH;
            windowHeight = Def.WindowDef.WINDOW_HEIGHT;

            buttonWidht = windowWidth / 2 - 128;

            messegeWindow = new Window(gameDevice, new Vector2(windowWidth / 4, windowHeight / 2 - 10), new Vector2(windowWidth / 2, 720 / 12));

            buttonPosition = new Vector2(windowWidth - 64, windowHeight - 64);
            buttonWindow = new Window(gameDevice, buttonPosition, new Vector2(64, 32));
            button = new Button(buttonPosition, 64, 32);
        }

        public void Initialize()
        {
            modeType = ModeType.No;

            playerItems = new List<Item>();
            consumptions = new List<Item>();
            equipments = new List<Item>();

            leftItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();

            isInventoryFullMessege = false;
            isRightListFull = false;
            isNoMoney = false;
            messegeWindow.Initialize();
            messegeWindow.SetAlphaLimit(0.8f);

            totalPrice = 0;

            buttonWindow.Initialize();
            buttonWindow.Switch();
        }

        public void Buy()
        {
            modeType = ModeType.Buy;

            text = "買う";

            equipments = itemManager.GetEquipmentList();
            consumptions = itemManager.GetConsumptionList();

            foreach (Item item in equipments)
            {
                AddLeftList(item);
            }
            foreach (Item item in consumptions)
            {
                AddLeftList(item);
            }
        }

        public void Sell()
        {
            modeType = ModeType.Sell;

            text = "売る";

            playerItems = playerInventory.BagList();

            foreach(Item item in playerItems)
            {
                AddLeftList(item);
            }
        }

        public void Update()
        {
            buttonWindow.Update();
            messegeWindow.Update();

            if (isRightListFull || isInventoryFullMessege  || isNoMoney)
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

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);

            if (modeType == ModeType.Buy)
            {
                BuyUpdate(mousePos);
            }
            else if (modeType == ModeType.Sell)
            {
                SellUpdate(mousePos);
            }
        }

        public void BuyUpdate(Point mousePos)
        {

            playerInventory.BagItemCount(ref currentNum, ref maxNum);

            isRightListFull = false;
            isNoMoney = false;
            isInventoryFullMessege = false;

            //アイテムを買う処理(お金が足りるか、バックにはいるかチェック)
            if (button.IsClick(mousePos))
            {
                if (maxNum >= currentNum + rightItems.Count)
                {
                    if (totalPrice <= playerInventory.CurrentMoney())
                    {
                        if (input.IsLeftClick())
                        {
                            foreach (Item items in rightItems)
                            {
                                playerInventory.AddItem(items);
                            }
                            playerInventory.SpendMoney(totalPrice);
                            totalPrice = 0;
                            rightItems = new List<Item>();
                            rightButtons = new List<Button>();
                            rightWindows = new List<Window>();
                        }
                    }
                    else
                    {
                        isNoMoney = true;
                    }
                }
                else
                {
                    isInventoryFullMessege = true;
                }
            }

            for (int i = 0; i < leftButtons.Count; i++)
            {
                leftWindows[i].Update();

                //売り物リストから買う物リストに追加する処理(買い物リストが空いているかチェック)
                if (leftButtons[i].IsClick(mousePos))
                {
                    if (rightButtons.Count >= 20)
                    {
                        isRightListFull = true;
                    }
                    else if (input.IsLeftClick())
                    {
                        AddRightList(leftItems[i]);
                    }
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

        public void SellUpdate(Point mousePos)
        {
            //アイテムを売る処理
            if (button.IsClick(mousePos))
            {
                if (input.IsLeftClick())
                {
                    playerInventory.AddMoney(totalPrice);
                    foreach(Item item in rightItems)
                    {
                        playerInventory.RemoveItem(playerInventory.BagItemIndex(item));
                    }
                    totalPrice = 0;
                    rightItems = new List<Item>();
                    rightButtons = new List<Button>();
                    rightWindows = new List<Window>();
                }
            }

            for (int i = 0; i < leftButtons.Count; i++)
            {
                leftWindows[i].Update();

                //持ち物を売る物リストに移動
                if (leftButtons[i].IsClick(mousePos))
                {
                    if (input.IsLeftClick())
                    {
                        AddRightList(leftItems[i]);
                        RemoveLeftList(i);
                    }
                }
            }

            for (int i = 0; i < rightButtons.Count; i++)
            {
                rightWindows[i].Update();

                //売る物リストから持ち物リストへ移動
                if (rightButtons[i].IsClick(mousePos))
                {
                    if (input.IsLeftClick())
                    {
                        AddLeftList(rightItems[i]);
                        RemoveRightList(i);
                    }
                }
            }
        }

        //左のリストにアイテムを追加する。
        private void AddLeftList(Item item)
        {
            leftItems.Add(item);
            Vector2 position = new Vector2(64, 96 + 24 * (leftButtons.Count + 1));
            leftButtons.Add(new Button(position, buttonWidht, buttonHeight));
            leftWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
            leftWindows[leftWindows.Count - 1].Initialize();
            leftWindows[leftWindows.Count - 1].Switch();
        }

        //左のリストから指定されたアイテムを消す。
        private void RemoveLeftList(int key)
        {
            leftItems.Remove(leftItems[key]);
            leftButtons.Remove(leftButtons[key]);
            leftWindows.Remove(leftWindows[key]);

            //上に詰める処理
            List<Item> copyLeftItems = leftItems;
            leftItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            foreach (Item item in copyLeftItems)
            {
                AddLeftList(item);
            }
        }

        //右のリストにアイテムを追加する。
        private void AddRightList(Item item)
        {
            rightItems.Add(item);
            Vector2 position = new Vector2(windowWidth / 2 + 64, 96 + 24 * (rightButtons.Count + 1));
            rightButtons.Add(new Button(position, buttonWidht, buttonHeight));
            rightWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
            rightWindows[rightWindows.Count - 1].Initialize();
            rightWindows[rightWindows.Count - 1].Switch();
            if (modeType == ModeType.Buy)
            {
                totalPrice += item.GetItemPrice();
            }
            else
            {
                totalPrice += item.GetItemPrice() / 2;
            }
        }

        //右リストから指定されたアイテムを消す。
        private void RemoveRightList(int key)
        {
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
            renderer.DrawString(text, buttonPosition, new Vector2(1, 1), Color.White);

            if (modeType == ModeType.Buy)
            {
                renderer.DrawString("バッグ(" + currentNum + "/" + maxNum + ")", new Vector2(windowWidth - 240, 64), new Vector2(1, 1), Color.White);
                renderer.DrawString("所持金 : " + playerInventory.CurrentMoney(), new Vector2(windowWidth / 2 + 120, windowHeight - 128 + 32), new Vector2(1, 1), Color.White);
            }

            renderer.DrawString("アイテム名", new Vector2(64, 64 + 32), new Vector2(1, 1), Color.White);
            if (modeType == ModeType.Buy)
            {
                renderer.DrawString("値段", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);
            }
            else if (modeType == ModeType.Sell)
            {
                renderer.DrawString("買取額", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);
            }
            renderer.DrawString("タイプ", new Vector2(320, 64 + 32), new Vector2(1, 1), Color.White);

            //左側のリストのアイテムの描画
            for (int i = 0; i < leftItems.Count; i++)
            {
                leftWindows[i].Draw();

                renderer.DrawString(leftItems[i].GetItemName(), 
                    leftWindows[i].GetOffsetPosition(), new Vector2(1, 1), Color.White);
                if (modeType == ModeType.Buy)
                {
                    renderer.DrawString(leftItems[i].GetItemPrice().ToString(),
                        leftWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                }
                else if (modeType == ModeType.Sell)
                {
                    renderer.DrawString((leftItems[i].GetItemPrice() / 2).ToString(), 
                        leftWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                }
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
            for (int i = 0; i < rightItems.Count; i++)
            {
                rightWindows[i].Draw();

                renderer.DrawString(rightItems[i].GetItemName(), 
                    rightWindows[i].GetOffsetPosition(), new Vector2(1, 1), Color.White);
                if (modeType == ModeType.Buy)
                {
                    renderer.DrawString(rightItems[i].GetItemPrice().ToString(), 
                        rightWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                }
                else if (modeType == ModeType.Sell)
                {
                    renderer.DrawString((rightItems[i].GetItemPrice() / 2).ToString(), 
                        rightWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                }
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
                renderer.DrawString("買う物リストがいっぱいです。", new Vector2(320, windowHeight / 2), new Vector2(2, 2), Color.Red);
            }
            if (isInventoryFullMessege)
            {
                renderer.DrawString("バッグにはいりきりません。", new Vector2(320, windowHeight / 2), new Vector2(2, 2), Color.Red);
            }
            if (isNoMoney)
            {
                renderer.DrawString("所持金が足りません。", new Vector2(320, windowHeight / 2), new Vector2(2, 2), Color.Red);
            }

            renderer.DrawString("合計金額 : " + totalPrice, new Vector2(windowWidth - 240, windowHeight - 128 + 32), new Vector2(1, 1), Color.White);
        }
    }
}
