using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Effects;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.UI;

namespace Team27_RougeLike.Scene
{
    class Depot : IScene
    {
        private enum DepotModeType
        {
            select,
            equipment,
            consumption,
            end
        }

        private DepotModeType mode;

        private GameDevice gameDevice;
        private GameManager gameManager;
        private Renderer renderer;
        private InputState input;

        private BlurEffect blurEffect;
        private float blurRate;

        private IScene townScene;

        private bool endFlag;

        private Inventory inventory;
        private ItemManager itemManager;

        private List<Item> playerItems;
        private List<Item> equipments;
        private Dictionary<int,int> consumptions;

        private Window leftWindow;
        private Window rightWindow;
        private Window messegeWindow;

        private Button backButton;
        private Window backWindow;

        private Button equipmentButton;
        private Button consumptionButton;
        private Window equipmentWindow;
        private Window consumptionWindow;

        private List<Item> leftItems;
        private List<Item> rightItems;

        private List<Item> leftPageItems;
        private List<Item> rightPageItems;
        private List<Button> leftButtons;
        private List<Button> rightButtons;
        private List<Window> leftWindows;
        private List<Window> rightWindows;

        private Window rightPageRightWindow;    //右側のページを右にめくるWindow
        private Window rightPageLeftWindow;     //右側のページを左にめくるWindow
        private Button rightPageRightButton;    //右側のページを右にめくるButton
        private Button rightPageLeftButton;     //右側のページを左にめくるButton
        private Window leftPageRightWindow;     //左側のページを右にめくるWindow
        private Window leftPageLeftWindow;      //左側のページを左にめくるWIndow
        private Button leftPageRightButton;     //左側のページを右にめくるButton
        private Button leftPageLeftButton;      //左側のページを左にめくるButton

        private int bagMaxNum;                  //バッグの容量
        private int bagNowNum;                  //バッグの現在のアイテム量
        private int depotMaxNum;　              //倉庫の容量
        private int depotNowNum;                //倉庫の現在のアイテム量

        private int leftPage;                   //左のページ
        private int leftMaxPage;                //左の最大ページ
        private int rightPage;                  //右のページ
        private int rightMaxPage;               //右の最大ページ

        private bool isBagMax;                  //バッグがいっぱいかどうか
        private bool isBagMaxMessaga;           //バッグがいっぱいというメッセージ表示
        private bool isDepotMax;                //倉庫がいっぱいかどうか
        private bool isDepotMaxMessaga;         //倉庫がいっぱいというメッセージ表示

        private int windowWidth;
        private int windowHeight;

        public Depot(IScene town, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            blurEffect = renderer.EffectManager.GetBlurEffect();
            townScene = town;

            mode = DepotModeType.select;
            inventory = gameManager.PlayerItem;
            itemManager = new ItemManager();
            
            windowWidth = Def.WindowDef.WINDOW_WIDTH;
            windowHeight = Def.WindowDef.WINDOW_HEIGHT;
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            blurRate = 0.0f;

            mode = DepotModeType.select;
            playerItems = inventory.BagList();
            equipments = inventory.EquipDepository();
            consumptions = inventory.DepositoryItem();

            itemManager.LoadAll();

            Vector2 size = new Vector2(windowWidth / 2 - 128, 720 - 128);
            leftWindow = new Window(gameDevice, new Vector2(64, 64), size);
            leftWindow.Initialize();
            rightWindow = new Window(gameDevice, new Vector2(windowWidth / 2 + 64, 64), size);
            rightWindow.Initialize();
            messegeWindow = new Window(gameDevice, new Vector2(windowWidth / 2 - 160, windowHeight / 2 - 80), new Vector2(320, 160));
            messegeWindow.Initialize();

            backButton = new Button(new Vector2(0, windowHeight - 64), 64, 32);
            backWindow = new Window(gameDevice, new Vector2(0, windowHeight - 64), new Vector2(64, 32));

            equipmentButton = new Button(new Vector2(windowWidth / 2 - 160, windowHeight / 2 + 80 + 32), 96, 32);
            consumptionButton = new Button(new Vector2(windowWidth / 2 + 160 - 64 - 32, windowHeight / 2 + 80 + 32), 96, 32);
            equipmentWindow = new Window(gameDevice, new Vector2(windowWidth / 2 - 160, windowHeight / 2 + 80 + 32), new Vector2(96, 32));
            equipmentWindow.Initialize();
            consumptionWindow = new Window(gameDevice, new Vector2(windowWidth / 2 + 160 - 64 - 32, windowHeight / 2 + 80 + 32), new Vector2(96, 32));
            consumptionWindow.Initialize();

            leftItems = new List<Item>();
            rightItems = new List<Item>();

            rightPageLeftWindow = new Window(gameDevice, new Vector2(windowWidth - windowWidth / 4 - 64 - 64, windowHeight - 96), new Vector2(64, 32));
            rightPageLeftWindow.Initialize();
            rightPageRightWindow = new Window(gameDevice, new Vector2(windowWidth - windowWidth / 4 + 64 , windowHeight - 96), new Vector2(64, 32));
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

        private void EquipmentModeInitialize()
        {
            leftItems = new List<Item>();
            rightItems = new List<Item>();

            leftPageItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightPageItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();

            mode = DepotModeType.equipment;
            foreach(Item i in playerItems)
            {
                if(i is WeaponItem || i is ProtectionItem)
                {
                    leftItems.Add(i);
                }
            }
            foreach(Item i in equipments)
            {
                rightItems.Add(i);
            }

            leftPage = 1;
            rightPage = 1;

            leftMaxPage = (leftItems.Count) / 20 + 1;
            rightMaxPage = (rightItems.Count) / 20 + 1;

            LeftPage(1);
            RightPage(1);
        }

        private void ConsumptionModeInitialize()
        {
            leftItems = new List<Item>();
            rightItems = new List<Item>();

            leftPageItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightPageItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();

            mode = DepotModeType.consumption;
            foreach(Item i in playerItems)
            {
                if (i is ConsumptionItem)
                {
                    leftItems.Add(i);
                }
            }
            foreach(int i in consumptions.Keys)
            {
                rightItems.Add(itemManager.GetConsumption(i));
            }

            leftPage = 1;
            rightPage = 1;

            leftMaxPage = (leftItems.Count) / 20 + 1;
            rightMaxPage = (rightItems.Count) / 20 + 1;

            LeftPage(1);
            RightPage(1);
        }

        private void UpdateBlurRate()
        {
            if (endFlag)
            {
                blurRate -= 0.05f;
                return;
            }

            if (blurRate < 0.6f)
                blurRate += 0.05f;
        }

        //左のリストにアイテムを追加する。
        private void AddLeftList(Item item)
        {
            leftPageItems.Add(item);
            Vector2 position = new Vector2(64, 96 + 24 * (leftButtons.Count + 1));
            int buttonWidht = windowWidth / 2 - 128;
            int buttonHeight = 20;
            leftButtons.Add(new Button(position, buttonWidht, buttonHeight));
            leftWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
            leftWindows[leftWindows.Count - 1].Initialize();
            leftWindows[leftWindows.Count - 1].Switch();
        }

        //左のリストから指定されたアイテムを消す。
        private void RemoveLeftList(int key)
        {
            leftItems.Remove(leftPageItems[key]);
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
            if (leftItems.Count >= leftPage * 20)
            {
                AddLeftList(leftItems[leftPage * 20 - 1]);
            }
        }

        //右のリストにアイテムを追加する。
        private void AddRightList(Item item)
        {
            rightPageItems.Add(item);
            Vector2 position = new Vector2(windowWidth / 2 + 64, 96 + 24 * (rightButtons.Count + 1));
            int buttonWidht = windowWidth / 2 - 128;
            int buttonHeight = 20;
            rightButtons.Add(new Button(position, buttonWidht, buttonHeight));
            rightWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
            rightWindows[rightWindows.Count - 1].Initialize();
            rightWindows[rightWindows.Count - 1].Switch();
        }

        //右リストから指定されたアイテムを消す。
        private void RemoveRightList(int key)
        {
            rightItems.Remove(rightPageItems[key]);
            rightPageItems.Remove(rightPageItems[key]);
            rightButtons.Remove(rightButtons[key]);
            rightWindows.Remove(rightWindows[key]);

            //上に詰める処理
            List<Item> copyRightItems = rightPageItems;
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
                leftWindows[i].SetAlpha(0.5f);

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

        public void Update(GameTime gameTime)
        {
            UpdateBlurRate();
            blurEffect.Update(blurRate);

            leftWindow.Update();
            rightWindow.Update();
            messegeWindow.Update();
            equipmentWindow.Update();
            consumptionWindow.Update();

            rightPageRightWindow.Update();
            rightPageLeftWindow.Update();
            leftPageRightWindow.Update();
            leftPageLeftWindow.Update();

            foreach (Window w in leftWindows)
            {
                w.Update();
            }
            foreach (Window w in rightWindows)
            {
                w.Update();
            }

            backWindow.Update();

            Point mousePos = new Point(
                (int)input.GetMousePosition().X,
                (int)input.GetMousePosition().Y);

            if (!backWindow.CurrentState() && !endFlag) backWindow.Switch();

            //セレクト処理
            if (mode == DepotModeType.select)
            {
                if (!messegeWindow.CurrentState()) messegeWindow.Switch();
                if (!equipmentWindow.CurrentState()) equipmentWindow.Switch();
                if (!consumptionWindow.CurrentState()) consumptionWindow.Switch();
                if (leftWindow.CurrentState()) leftWindow.Switch();
                if (rightWindow.CurrentState()) rightWindow.Switch();

                if (equipmentButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    mode = DepotModeType.equipment;
                    EquipmentModeInitialize();
                }

                if (consumptionButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    mode = DepotModeType.consumption;
                    ConsumptionModeInitialize();
                }
                if (backButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    mode = DepotModeType.end;
                    endFlag = true;
                }

                if (leftPageLeftWindow.CurrentState())
                    leftPageLeftWindow.Switch();
                if (leftPageRightWindow.CurrentState())
                    leftPageRightWindow.Switch();
                if (rightPageLeftWindow.CurrentState())
                    rightPageLeftWindow.CurrentState();
                if (rightPageRightWindow.CurrentState())
                    rightPageRightWindow.Switch();
            }
            else if (mode != DepotModeType.end) //セレクト外共通処理
            {
                if (messegeWindow.CurrentState()) messegeWindow.Switch();
                if (equipmentWindow.CurrentState()) equipmentWindow.Switch();
                if (consumptionWindow.CurrentState()) consumptionWindow.Switch();
                if (!leftWindow.CurrentState()) leftWindow.Switch();
                if (!rightWindow.CurrentState()) rightWindow.Switch();
                
                inventory.BagItemCount(ref bagNowNum, ref bagMaxNum);
                inventory.DepositoryEquipCount(ref depotNowNum, ref depotMaxNum);

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


                //メッセージ関連
                isBagMax = false;
                isBagMaxMessaga = false;
                if (bagNowNum >= bagMaxNum) isBagMax = true;
                foreach(Button b in rightButtons)
                {
                    if (b.IsClick(mousePos) && isBagMax)
                        isBagMaxMessaga = true;
                }

                isDepotMax = false;
                isDepotMaxMessaga = false;
                if (depotNowNum >= depotMaxNum) isDepotMax = true;
                foreach(Button b in leftButtons)
                {
                    if (b.IsClick(mousePos) && isDepotMax)
                        isDepotMaxMessaga = true;
                }

                //戻るボタン
                if (backButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    mode = DepotModeType.select;
                }
            }

            //装備品
            if (mode == DepotModeType.equipment)
            {
                playerItems = inventory.BagList();
                leftItems = new List<Item>();
                foreach (Item i in playerItems)
                {
                    if (i is WeaponItem || i is ProtectionItem)
                    {
                        leftItems.Add(i);
                    }
                }

                rightItems = new List<Item>();
                equipments = inventory.EquipDepository();
                foreach (Item i in equipments)
                {
                    rightItems.Add(i);
                }

                //バッグ側
                for (int i = 0; i < leftButtons.Count; i++)
                {
                    if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isDepotMax)
                    {
                        if (rightPageItems.Count < 20)
                        {
                            AddRightList(leftItems[i + (leftPage - 1) * 20]);
                        }
                        inventory.DepositEquip(inventory.BagItemIndex(leftItems[i + (leftPage - 1) * 20]));
                        rightItems.Add(leftItems[i + (leftPage - 1) * 20]);
                        RemoveLeftList(i);

                        leftMaxPage = (leftItems.Count - 1) / 20 + 1;
                        rightMaxPage = (rightItems.Count - 1) / 20 + 1;
                    }
                }

                //倉庫側
                for (int i = 0; i < rightButtons.Count; i++)
                {
                    if (rightButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isBagMax)
                    {
                        if (leftPageItems.Count < 20)
                        {
                            AddLeftList(rightItems[i + (rightPage - 1) * 20]);
                        }
                        inventory.MoveDepositEquipToBag(inventory.DepositEquipIndex(rightItems[i + (rightPage - 1) * 20]));
                        RemoveRightList(i);

                        leftMaxPage = (leftItems.Count - 1) / 20 + 1;
                        rightMaxPage = (rightItems.Count - 1) / 20 + 1;
                    }
                }
            }

            //消費アイテム
            if (mode == DepotModeType.consumption)
            {
                playerItems = inventory.BagList();
                leftItems = new List<Item>();
                foreach (Item item in playerItems)
                {
                    if (item is ConsumptionItem)
                    {
                        leftItems.Add(item);
                    }
                }

                consumptions = inventory.DepositoryItem();
                rightItems = new List<Item>();
                foreach (int id in consumptions.Keys)
                {
                    rightItems.Add(itemManager.GetConsumption(id));
                }

                //バッグ側
                for (int i = 0; i < leftButtons.Count; i++)
                {
                    if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick())
                    {
                        inventory.DepositItem(inventory.BagItemIndex(leftItems[i + (leftPage - 1) * 20]));
                        if (consumptions[leftItems[i + (leftPage - 1) * 20].GetItemID()] - 1 * ((ConsumptionItem)leftItems[i + (leftPage - 1) * 20]).GetStack() <= 0)
                        {
                            if (rightPageItems.Count < 20)
                            {
                                AddRightList(leftItems[i + (leftPage - 1) * 20]);
                            }
                        }
                        rightItems.Add(leftItems[i + (leftPage - 1) * 20]);
                        RemoveLeftList(i);

                        leftMaxPage = (leftItems.Count - 1) / 20 + 1;
                        rightMaxPage = (rightItems.Count - 1) / 20 + 1;
                    }
                }

                //倉庫側
                for (int i = 0; i < rightButtons.Count; i++)
                {
                    if (rightButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isBagMax)
                    {
                        if (consumptions[rightItems[i + (rightPage - 1) * 20].GetItemID()] - 1 <= 0)
                        {
                            inventory.MoveDepositItemToBag(itemManager, rightItems[i + (rightPage - 1) * 20].GetItemID());
                            RemoveRightList(i + (rightPage - 1));
                        }
                        else
                        {
                            inventory.MoveDepositItemToBag(itemManager, rightItems[i + (rightPage - 1) * 20].GetItemID());
                        }

                        playerItems = inventory.BagList();
                        leftItems = new List<Item>();
                        foreach (Item item in playerItems)
                        {
                            if (item is ConsumptionItem)
                            {
                                leftItems.Add(item);
                            }
                        }
                        LeftPage(leftPage);

                        leftMaxPage = (leftItems.Count - 1) / 20 + 1;
                        rightMaxPage = (rightItems.Count - 1) / 20 + 1;
                    }
                }
            }
        }

        public void Draw()
        {
            blurEffect.WriteRenderTarget(renderer.FogManager.CurrentColor());
            townScene.Draw();                       //背景は前のシーンを描画
            blurEffect.ReleaseRenderTarget();
            blurEffect.Draw(renderer);

            renderer.Begin();
            
            backWindow.Draw();
            renderer.DrawString("戻る", backButton.ButtonCenterVector(),
                Color.White, new Vector2(1, 1), 1.0f, true, true);

            leftWindow.Draw();
            rightWindow.Draw();
            messegeWindow.Draw();
            equipmentWindow.Draw();
            consumptionWindow.Draw();

            rightPageRightWindow.Draw();
            rightPageLeftWindow.Draw();
            leftPageRightWindow.Draw();
            leftPageLeftWindow.Draw();

            if (mode == DepotModeType.select)
            {
                renderer.DrawString("どっちに用があるんだい？", messegeWindow.GetCenter(), 
                    Color.White, new Vector2(1, 1), 1.0f, true, true);
                renderer.DrawString("装備品", equipmentButton.ButtonCenterVector(),
                    Color.White, new Vector2(1, 1), 1.0f, true, true);
                renderer.DrawString("消費アイテム", consumptionButton.ButtonCenterVector(),
                    Color.White, new Vector2(1, 1), 1.0f, true, true);
            }

            if (mode == DepotModeType.equipment)
            {
                renderer.DrawString("倉庫(" + depotNowNum + "/" + depotMaxNum + ")", new Vector2(windowWidth / 2 + 64, 64),
                    new Vector2(1, 1), Color.White);
            }

            if (mode == DepotModeType.consumption || mode == DepotModeType.equipment)
            {
                renderer.DrawString("バッグ(" + bagNowNum + "/" + bagMaxNum + ")", new Vector2(64,64),
                    new Vector2(1, 1), Color.White);
                renderer.DrawString("倉庫", new Vector2(windowWidth / 2 + 64, 64),
                    new Vector2(1, 1), Color.White);

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
                renderer.DrawString("タイプ", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);
                renderer.DrawString("アイテム名", new Vector2(windowWidth / 2 + 64, 64 + 32), new Vector2(1, 1), Color.White);
                renderer.DrawString("タイプ", new Vector2(windowWidth / 2 + 224, 64 + 32), new Vector2(1, 1), Color.White);
                if (mode == DepotModeType.consumption)
                    renderer.DrawString("所持数", new Vector2(windowWidth / 2 + 320, 64 + 32), new Vector2(1, 1), Color.White);

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

                    //アイテムタイプの表示
                    string type;
                    if (leftPageItems[i] is WeaponItem)
                    {
                        type = ((WeaponItem)leftPageItems[i]).GetWeaponType().ToString();
                    }
                    else if (leftPageItems[i] is ProtectionItem)
                    {
                        type = ((ProtectionItem)leftPageItems[i]).GetProtectionType().ToString();
                    }
                    else
                    {
                        type = ((ConsumptionItem)leftPageItems[i]).GetTypeText();
                    }
                    renderer.DrawString(type, leftWindows[i].GetOffsetPosition() + new Vector2(160, 0),
                        new Vector2(1, 1), Color.White);
                }

                //右側のリストのアイテムの描画
                for (int i = 0; i < rightPageItems.Count; i++)
                {
                    rightWindows[i].Draw();

                    //アイテム名の表示
                    renderer.DrawString(rightItems[i + (rightPage - 1) * 20].GetItemName(), rightWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);

                    //アイテムタイプの表示
                    string type;
                    if (rightItems[i + (rightPage - 1) * 20] is WeaponItem)
                    {
                        type = ((WeaponItem)rightItems[i + (rightPage - 1) * 20]).GetWeaponType().ToString();
                    }
                    else if (rightItems[i + (rightPage - 1) * 20] is ProtectionItem)
                    {
                        type = ((ProtectionItem)rightItems[i + (rightPage - 1) * 20]).GetProtectionType().ToString();
                    }
                    else
                    {
                        type = ((ConsumptionItem)rightItems[i + (rightPage - 1) * 20]).GetTypeText();
                    }
                    renderer.DrawString(type, rightWindows[i].GetOffsetPosition() + new Vector2(160, 0),
                        new Vector2(1, 1), Color.White);

                    //所持数の表示(消費アイテムのみ)
                    if (mode == DepotModeType.consumption)
                    {
                        if (consumptions.ContainsKey(rightItems[i + (rightPage - 1) * 20].GetItemID()))
                        {
                            renderer.DrawString(consumptions[rightItems[i + (rightPage - 1) * 20].GetItemID()].ToString(),
                                rightWindows[i].GetOffsetPosition() + new Vector2(256, 0),
                                new Vector2(1, 1), Color.White);
                        }
                    }
                }

                if (isBagMaxMessaga)
                    renderer.DrawString("バッグがいっぱいです。", new Vector2(320, windowHeight / 2), new Vector2(2, 2), Color.Red);

                if (isDepotMaxMessaga && mode ==DepotModeType.equipment)
                    renderer.DrawString("倉庫がいっぱいです。", new Vector2(320, windowHeight / 2), new Vector2(2, 2), Color.Red);
            }

            renderer.End();
        }

        public void ShutDown()
        {
            if (backWindow.CurrentState()) backWindow.Switch();
            if (messegeWindow.CurrentState()) messegeWindow.Switch();
            if (equipmentWindow.CurrentState()) equipmentWindow.Switch();
            if (consumptionWindow.CurrentState()) consumptionWindow.Switch();
            if (leftWindow.CurrentState()) leftWindow.Switch();
            if (rightWindow.CurrentState()) rightWindow.Switch();
        }

        public bool IsEnd()
        {
            return endFlag && blurRate <= 0;
        }

        public SceneType Next()
        {
            return SceneType.Town;
        }
    }
}
