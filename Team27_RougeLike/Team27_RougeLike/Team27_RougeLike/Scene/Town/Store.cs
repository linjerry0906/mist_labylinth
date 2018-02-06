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

        private List<Item> leftPageItems;
        private List<Item> rightPageItems;
        private List<Button> leftButtons;
        private List<Button> rightButtons;
        private List<Window> leftWindows;
        private List<Window> rightWindows;

        private List<Item> leftItems;
        private List<Item> rightItems;

        private Window rightPageRightWindow;    //右側のページを右にめくるWindow
        private Window rightPageLeftWindow;     //右側のページを左にめくるWindow
        private Button rightPageRightButton;    //右側のページを右にめくるButton
        private Button rightPageLeftButton;     //右側のページを左にめくるButton
        private Window leftPageRightWindow;     //左側のページを右にめくるWindow
        private Window leftPageLeftWindow;      //左側のページを左にめくるWIndow
        private Button leftPageRightButton;     //左側のページを右にめくるButton
        private Button leftPageLeftButton;      //左側のページを左にめくるButton

        private int leftPage;                   //左のページ
        private int leftMaxPage;                //左の最大ページ
        private int rightPage;                  //右のページ
        private int rightMaxPage;               //右の最大ページ

        private int totalPrice;
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

        private ItemInfoUI hintInfo;
        private bool isHintDraw;
        private Item hintItem;

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
            isNoMoney = false;

            windowWidth = Def.WindowDef.WINDOW_WIDTH;
            windowHeight = Def.WindowDef.WINDOW_HEIGHT;

            buttonWidht = windowWidth / 2 - 128;

            messegeWindow = new Window(gameDevice, new Vector2(windowWidth / 4, windowHeight / 2 - 10), new Vector2(windowWidth / 2, 720 / 12));

            buttonPosition = new Vector2(windowWidth - 64, windowHeight - 64);
            buttonWindow = new Window(gameDevice, buttonPosition, new Vector2(64, 32));
            button = new Button(buttonPosition, 64, 32);
            
            hintInfo = new ItemInfoUI(Vector2.Zero, gameManager, gameDevice);
        }

        public void Initialize()
        {
            modeType = ModeType.No;

            playerItems = new List<Item>();
            consumptions = new List<Item>();
            equipments = new List<Item>();

            leftPageItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightPageItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();

            isInventoryFullMessege = false;
            isNoMoney = false;
            messegeWindow.Initialize();
            messegeWindow.SetAlphaLimit(0.8f);

            totalPrice = 0;

            buttonWindow.Initialize();
            buttonWindow.Switch();

            leftItems = new List<Item>();
            rightItems = new List<Item>();

            rightPageLeftWindow = new Window(gameDevice, new Vector2(windowWidth - windowWidth / 4 - 64 - 64, windowHeight - 96), new Vector2(64, 32));
            rightPageLeftWindow.Initialize();
            rightPageRightWindow = new Window(gameDevice, new Vector2(windowWidth - windowWidth / 4 + 64, windowHeight - 96), new Vector2(64, 32));
            rightPageRightWindow.Initialize();
            rightPageLeftButton = new Button(rightPageLeftWindow.GetOffsetPosition(), 64, 32);
            rightPageRightButton = new Button(rightPageRightWindow.GetOffsetPosition(), 64, 32);
            leftPageLeftWindow = new Window(gameDevice, new Vector2(windowWidth / 4 - 64 - 64, windowHeight - 96), new Vector2(64, 32));
            leftPageLeftWindow.Initialize();
            leftPageRightWindow = new Window(gameDevice, new Vector2(windowWidth / 4 + 64, windowHeight - 96), new Vector2(64, 32));
            leftPageRightWindow.Initialize();
            leftPageLeftButton = new Button(leftPageLeftWindow.GetOffsetPosition(), 64, 32);
            leftPageRightButton = new Button(leftPageRightWindow.GetOffsetPosition(), 64, 32);

            leftPageItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightPageItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();

            leftPage = 1;
            leftMaxPage = 1;
            rightPage = 1;
            rightMaxPage = 1;
        }

        public void Buy()
        {
            modeType = ModeType.Buy;

            text = "買う";

            equipments = itemManager.GetEquipmentList();
            consumptions = itemManager.GetConsumptionList();

            foreach (Item item in equipments)
            {
                leftItems.Add(item);
            }
            foreach (Item item in consumptions)
            {
                if(((ConsumptionItem)item).GetTypeText() == "矢")
                {
                    ((ConsumptionItem)item).SetStack(30);
                }
                leftItems.Add(item);
            }

            leftPage = 1;
            rightPage = 1;

            leftMaxPage = (leftItems.Count) / 20 + 1;
            rightMaxPage = (rightItems.Count) / 20 + 1;

            LeftPage(1);
            RightPage(1);
        }

        public void Sell()
        {
            modeType = ModeType.Sell;

            text = "売る";

            playerItems = playerInventory.BagList();

            foreach(Item item in playerItems)
            {
                leftItems.Add(item);
            }

            leftPage = 1;
            rightPage = 1;

            leftMaxPage = (leftItems.Count) / 20 + 1;
            rightMaxPage = (rightItems.Count) / 20 + 1;

            LeftPage(1);
            RightPage(1);
        }

        public void Update()
        {
            buttonWindow.Update();
            messegeWindow.Update();

            rightPageRightWindow.Update();
            rightPageLeftWindow.Update();
            leftPageRightWindow.Update();
            leftPageLeftWindow.Update();

            if (isInventoryFullMessege  || isNoMoney)
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

            //ページ関連
            if (leftPage > leftMaxPage)
            {
                leftPage = leftMaxPage;
                LeftPage(leftPage);
            }
            if (rightPage > rightMaxPage)
            {
                rightPage = rightMaxPage;
                RightPage(rightPage);
            }
            //左側
            if (leftPage > 1)
            {
                if (!leftPageLeftWindow.CurrentState())
                {
                    leftPageLeftWindow.Switch();
                }
                if (leftPageLeftButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    leftPage--;
                    LeftPage(leftPage);
                }
            }
            else
            {
                if (leftPageLeftWindow.CurrentState())
                {
                    leftPageLeftWindow.Switch(); //消す
                }
            }
            if (leftPage < leftMaxPage)
            {
                if (!leftPageRightWindow.CurrentState())
                {
                    leftPageRightWindow.Switch();
                }
                if (leftPageRightButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    leftPage++;
                    LeftPage(leftPage);
                }
            }
            else
            {
                if (leftPageRightWindow.CurrentState())
                {
                    leftPageRightWindow.Switch(); //消す
                }
            }
            //右側
            if (rightPage > 1)
            {
                if (!rightPageLeftWindow.CurrentState())
                {
                    rightPageLeftWindow.Switch();
                }
                if (rightPageLeftButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    rightPage--;
                    RightPage(rightPage);
                }
            }
            else
            {
                if (rightPageLeftWindow.CurrentState())
                {
                    rightPageLeftWindow.Switch(); //消す
                }
            }
            if (rightPage < rightMaxPage)
            {
                if (!rightPageRightWindow.CurrentState())
                {
                    rightPageRightWindow.Switch();
                }
                if (rightPageRightButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    rightPage++;
                    RightPage(rightPage);
                }
            }
            else
            {
                if (rightPageRightWindow.CurrentState())
                {
                    rightPageRightWindow.Switch(); //消す
                }
            }

        }

        public void BuyUpdate(Point mousePos)
        {

            playerInventory.BagItemCount(ref currentNum, ref maxNum);
            
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
                            rightPageItems = new List<Item>();
                            rightButtons = new List<Button>();
                            rightWindows = new List<Window>();
                            rightMaxPage = 1;
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
                    if (input.IsLeftClick())
                    {
                        {
                            if (rightPageItems.Count < 20)
                            {
                                AddRightList(leftItems[i + (leftPage - 1) * 20]);
                            }
                            rightItems.Add(leftItems[i + (leftPage - 1) * 20]);
                        }
                        rightMaxPage = (rightItems.Count - 1) / 20 + 1;
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
                        rightItems.Remove(rightItems[i + (rightPage - 1) * 20]);
                        RemoveRightList(i);

                        rightMaxPage = (rightItems.Count - 1) / 20 + 1;
                    }
                }
            }
        }

        public void SellUpdate(Point mousePos)
        {
            isHintDraw = false;
            hintItem = null;
            hintInfo.Position = input.GetMousePosition() + new Vector2(35, 50);

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
                    rightPageItems = new List<Item>();
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
                    hintItem = leftPageItems[i];
                    isHintDraw = true;

                    if (input.IsLeftClick())
                    {
                        if (rightPageItems.Count < 20)
                        {
                            AddRightList(leftItems[i + (leftPage - 1) * 20]);
                        }
                        rightItems.Add(leftItems[i + (leftPage - 1) * 20]);
                        leftItems.Remove(leftItems[i + (leftPage - 1) * 20]);
                        RemoveLeftList(i);

                        leftMaxPage = (leftItems.Count - 1) / 20 + 1;
                        rightMaxPage = (rightItems.Count - 1) / 20 + 1;
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
                        if (leftPageItems.Count < 20)
                        {
                            AddLeftList(rightItems[i + (rightPage - 1) * 20]);
                        }
                        leftItems.Add(rightItems[i + (leftPage - 1) * 20]);
                        rightItems.Remove(rightItems[i + (rightPage - 1) * 20]);
                        RemoveRightList(i);

                        leftMaxPage = (leftItems.Count - 1) / 20 + 1;
                        rightMaxPage = (rightItems.Count - 1) / 20 + 1;
                    }
                }
            }
        }

        //左のリストにアイテムを追加する。
        private void AddLeftList(Item item)
        {
            leftPageItems.Add(item);
            Vector2 position = new Vector2(64, 96 + 24 * (leftButtons.Count + 1));
            leftButtons.Add(new Button(position, buttonWidht, buttonHeight));
            leftWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
            leftWindows[leftWindows.Count - 1].Initialize();
            leftWindows[leftWindows.Count - 1].Switch();
        }

        //左のリストから指定されたアイテムを消す。
        private void RemoveLeftList(int key)
        {
            leftPageItems.Remove(leftPageItems[key]);
            leftButtons.Remove(leftButtons[key]);
            leftWindows.Remove(leftWindows[key]);

            //上に詰める処理
            List<Item> copyLeftItems = leftPageItems;
            leftPageItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            foreach (Item item in copyLeftItems)
            {
                AddLeftList(item);
            }
            if (leftItems.Count >= rightPage * 20)
            {
                AddLeftList(leftItems[leftPage * 20 - 1]);
            }
        }

        //右のリストにアイテムを追加する。
        private void AddRightList(Item item)
        {
            rightPageItems.Add(item);
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
            rightPageItems.Remove(rightPageItems[key]);
            rightButtons.Remove(rightButtons[key]);
            rightWindows.Remove(rightWindows[key]);

            //上に詰める処理
            List<Item> copyRightItems = rightPageItems;
            totalPrice = 0;
            rightPageItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();
            foreach (Item item in copyRightItems)
            {
                AddRightList(item);
            }
            if (rightItems.Count >= rightPage * 20)
            {
                AddRightList(rightItems[rightPage * 20 - 1]);
            }
        }

        private void LeftPage(int page)
        {
            leftPageItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            for (int i = 0; i < leftItems.Count - (page - 1) * 20 && i < 20; i++)
            {
                AddLeftList(leftItems[i + (page - 1) * 20]);
            }
        }

        private void RightPage(int page)
        {
            rightPageItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();
            for (int i = 0; i < rightItems.Count - (page - 1) * 20 && i < 20; i++)
            {
                AddRightList(rightItems[i + (page - 1) * 20]);
            }
        }

        public void DrawEquip()
        {
            buttonWindow.Draw();

            rightPageRightWindow.Draw();
            rightPageLeftWindow.Draw();
            leftPageRightWindow.Draw();
            leftPageLeftWindow.Draw();

            renderer.DrawString(text, buttonPosition, new Vector2(1, 1), Color.White);

            if (modeType == ModeType.Buy)
            {
                renderer.DrawString("バッグ(" + currentNum + "/" + maxNum + ")", new Vector2(windowWidth - 240, 64), new Vector2(1, 1), Color.White);
                renderer.DrawString("所持金 : " + playerInventory.CurrentMoney(), new Vector2(windowWidth / 2 + 120, windowHeight - 128 + 6), new Vector2(1, 1), Color.White);
            }

            renderer.DrawString("ページ(" + leftPage + "/" + leftMaxPage + ")", new Vector2(windowWidth / 4 - 48, windowHeight - 96), new Vector2(1, 1), Color.White);
            renderer.DrawString("ページ(" + rightPage + "/" + rightMaxPage + ")", new Vector2(windowWidth - windowWidth / 4 - 48, windowHeight - 96), new Vector2(1, 1), Color.White);

            if (leftPageLeftWindow.CurrentState())
                renderer.DrawString("←", leftPageLeftWindow.GetCenter(), Color.White, new Vector2(1, 1), 1.0f, true, true);
            if (leftPageRightWindow.CurrentState())
                renderer.DrawString("→", leftPageRightWindow.GetCenter(), Color.White, new Vector2(1, 1), 1.0f, true, true);
            if (rightPageLeftWindow.CurrentState())
                renderer.DrawString("←", rightPageLeftWindow.GetCenter(), Color.White, new Vector2(1, 1), 1.0f, true, true);
            if (rightPageRightWindow.CurrentState())
                renderer.DrawString("→", rightPageRightWindow.GetCenter(), Color.White, new Vector2(1, 1), 1.0f, true, true);

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
            for (int i = 0; i < leftPageItems.Count; i++)
            {
                leftWindows[i].Draw();

                //アイテム名表示
                if (leftPageItems[i] is ConsumptionItem)
                {
                    if (((ConsumptionItem)leftPageItems[i]).GetTypeText() != "矢")
                    {
                        renderer.DrawString(leftPageItems[i].GetItemName(), leftWindows[i].GetOffsetPosition(),
                            new Vector2(1, 1), Color.White);
                    }
                    else
                    {
                        renderer.DrawString(leftPageItems[i].GetItemName() + "x" + ((ConsumptionItem)leftPageItems[i]).GetStack(),
                            leftWindows[i].GetOffsetPosition(),
                            new Vector2(1, 1), Color.White);
                    }
                }
                else
                {
                    renderer.DrawString(leftPageItems[i].GetItemName(), leftWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);
                }
                if (modeType == ModeType.Buy)
                {
                    renderer.DrawString(leftPageItems[i].GetItemPrice().ToString(),
                        leftWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                }
                else if (modeType == ModeType.Sell)
                {
                    renderer.DrawString((leftPageItems[i].GetItemPrice() / 2).ToString(), 
                        leftWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                }
                string type;
                if (leftPageItems[i] is WeaponItem)
                {
                    type = ((WeaponItem)leftPageItems[i]).GetWeaponType().ToString();
                }
                else if (leftPageItems[i] is ProtectionItem)
                {
                    type = ((ProtectionItem)leftPageItems[i]).GetProtectionType().ToString();
                }
                else if (leftPageItems[i] is AccessaryItem)
                {
                    type = "装飾品";
                }
                else
                {
                    type = ((ConsumptionItem)leftPageItems[i]).GetTypeText();
                }
                renderer.DrawString(type, leftWindows[i].GetOffsetPosition() + new Vector2(256, 0), new Vector2(1, 1), Color.White);
            }

            //右側のリストのアイテムの描画
            for (int i = 0; i < rightPageItems.Count; i++)
            {
                rightWindows[i].Draw();

                if (rightPageItems[i] is ConsumptionItem)
                {
                    if (((ConsumptionItem)rightPageItems[i]).GetTypeText() != "矢")
                    {
                        renderer.DrawString(rightPageItems[i].GetItemName(), rightWindows[i].GetOffsetPosition(),
                            new Vector2(1, 1), Color.White);
                    }
                    else
                    {
                        renderer.DrawString(rightPageItems[i].GetItemName() + "x" + ((ConsumptionItem)rightPageItems[i]).GetStack(),
                            rightWindows[i].GetOffsetPosition(),
                            new Vector2(1, 1), Color.White);
                    }
                }
                else
                {
                    renderer.DrawString(rightPageItems[i].GetItemName(), rightWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);
                }
                if (modeType == ModeType.Buy)
                {
                    renderer.DrawString(rightPageItems[i].GetItemPrice().ToString(), 
                        rightWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                }
                else if (modeType == ModeType.Sell)
                {
                    renderer.DrawString((rightPageItems[i].GetItemPrice() / 2).ToString(), 
                        rightWindows[i].GetOffsetPosition() + new Vector2(160, 0), new Vector2(1, 1), Color.White);
                }
                string type;
                if (rightPageItems[i] is WeaponItem)
                {
                    type = ((WeaponItem)rightPageItems[i]).GetWeaponType().ToString();
                }
                else if (rightPageItems[i] is ProtectionItem)
                {
                    type = ((ProtectionItem)rightPageItems[i]).GetProtectionType().ToString();
                }
                else if (rightPageItems[i] is AccessaryItem)
                {
                    type = "装飾品";
                }
                else
                {
                    type = ((ConsumptionItem)rightPageItems[i]).GetTypeText();
                }
                renderer.DrawString(type, rightWindows[i].GetOffsetPosition() + new Vector2(256, 0), new Vector2(1, 1), Color.White);
            }

            //アイテムの詳細表示
            if (modeType == ModeType.Sell)
            {
                if (isHintDraw)
                {
                    renderer.DrawTexture("fade", hintInfo.Position + new Vector2(-10, -15),
                        new Vector2(420, 100), 0.75f);

                    hintInfo.Draw(hintItem, 1.0f);
                }
            }


            //メッセージ関連
            messegeWindow.Draw();
            if (isInventoryFullMessege)
            {
                renderer.DrawString("バッグにはいりきりません。", new Vector2(320, windowHeight / 2), new Vector2(2, 2), Color.Red);
            }
            if (isNoMoney)
            {
                renderer.DrawString("所持金が足りません。", new Vector2(320, windowHeight / 2), new Vector2(2, 2), Color.Red);
            }

            renderer.DrawString("合計金額 : " + totalPrice, new Vector2(windowWidth - 240, windowHeight - 128 + 6), new Vector2(1, 1), Color.White);
        }
    }
}
