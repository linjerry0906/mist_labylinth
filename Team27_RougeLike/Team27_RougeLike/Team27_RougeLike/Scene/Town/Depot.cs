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

            Vector2 size = new Vector2(1080 / 2 - 128, 720 - 128);
            leftWindow = new Window(gameDevice, new Vector2(64, 64), size);
            leftWindow.Initialize();
            rightWindow = new Window(gameDevice, new Vector2(1080 / 2 + 64, 64), size);
            rightWindow.Initialize();
            messegeWindow = new Window(gameDevice, new Vector2(1080 / 2 - 160, 720 / 2 - 80), new Vector2(320, 160));
            messegeWindow.Initialize();

            backButton = new Button(new Vector2(1080 - 64, 720 - 64), 64, 32);
            backWindow = new Window(gameDevice, new Vector2(1080 - 64, 720 - 64), new Vector2(64, 32));

            equipmentButton = new Button(new Vector2(1080 / 2 - 160, 720 / 2 + 80 + 32), 64, 32);
            consumptionButton = new Button(new Vector2(1080 / 2 + 160 - 64, 720 / 2 + 80 + 32), 64, 32);
            equipmentWindow = new Window(gameDevice, new Vector2(1080 / 2 - 160, 720 / 2 + 80 + 32), new Vector2(64, 32));
            equipmentWindow.Initialize();
            consumptionWindow = new Window(gameDevice, new Vector2(1080 / 2 + 160 - 64, 720 / 2 + 80 + 32), new Vector2(64, 32));
            consumptionWindow.Initialize();

            leftItems = new List<Item>();
            rightItems = new List<Item>();

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

            leftMaxPage = (leftItems.Count - 1) / 20 + 1;
            rightMaxPage = (rightItems.Count - 1) / 20 + 1;
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
                rightItems.Add(itemManager.GetConsuptionItem(i));
            }

            leftMaxPage = (leftItems.Count - 1) / 20;
            rightMaxPage = (rightItems.Count - 1) / 20;
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
            int buttonWidht = 1080 / 2 - 128;
            int buttonHeight = 20;
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
            if (leftItems[leftPage * 20 + 1] != null)
            {
                AddLeftList(leftItems[leftPage * 20 + 1]);
            }
        }

        //右のリストにアイテムを追加する。
        private void AddRightList(Item item)
        {
            rightPageItems.Add(item);
            Vector2 position = new Vector2(1080 / 2 + 64, 96 + 24 * (rightButtons.Count + 1));
            int buttonWidht = 1080 / 2 - 128;
            int buttonHeight = 20;
            rightButtons.Add(new Button(position, buttonWidht, buttonHeight));
            rightWindows.Add(new Window(gameDevice, position, new Vector2(buttonWidht, buttonHeight)));
            rightWindows[rightWindows.Count - 1].Initialize();
            rightWindows[rightWindows.Count - 1].Switch();
        }

        //右リストから指定されたアイテムを消す。
        private void RemoveRightList(int key)
        {
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
            if (rightItems[rightPage * 20 + 1] != null)
            {
                AddRightList(rightItems[rightPage * 20 + 1]);
            }
        }

        private void LeftPage(int page)
        {
            for(int i = 0 + (page - 1) * 20; i < leftItems.Count || i <= page * 20; i++)
            {
                AddLeftList(leftItems[i]);
            }
        }

        private void RightPage(int page)
        {

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
            }
            else //セレクト外共通処理
            {
                if (messegeWindow.CurrentState()) messegeWindow.Switch();
                if (equipmentWindow.CurrentState()) equipmentWindow.Switch();
                if (consumptionWindow.CurrentState()) consumptionWindow.Switch();
                if (!leftWindow.CurrentState()) leftWindow.Switch();
                if (!rightWindow.CurrentState()) rightWindow.Switch();
                
                inventory.BagItemCount(ref bagNowNum, ref bagMaxNum);
                inventory.DepositoryEquipCount(ref depotNowNum, ref depotMaxNum);

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

                if (backButton.IsClick(mousePos) && input.IsLeftClick())
                {
                    mode = DepotModeType.select;
                }
            }

            //装備品
            if (mode == DepotModeType.equipment)
            {
                //バッグ側
                for (int i = 0; i < leftButtons.Count; i++)
                {
                    if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isDepotMax)
                    {
                        AddRightList(leftPageItems[i]);
                        inventory.DepositEquip(inventory.BagItemIndex(leftPageItems[i]));
                        RemoveLeftList(i);
                    }
                }

                //倉庫側
                for (int i = 0; i < rightButtons.Count; i++)
                {
                    if (rightButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isBagMax)
                    {
                        AddLeftList(rightPageItems[i]);
                        inventory.MoveDepositEquipToBag(inventory.DepositEquipIndex(rightPageItems[i]));
                        RemoveRightList(i);
                    }
                }
            }

            //消費アイテム
            if (mode == DepotModeType.consumption)
            {
                //バッグ側
                for (int i = 0; i < leftButtons.Count; i++)
                {
                    if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isDepotMax)
                    {
                        inventory.DepositItem(inventory.BagItemIndex(leftPageItems[i]));
                        if (consumptions[leftPageItems[i].GetItemID()] - 1 <= 0)
                            AddRightList(leftPageItems[i]);
                        RemoveLeftList(i);
                    }
                }

                //倉庫側
                for (int i = 0; i < rightButtons.Count; i++)
                {
                    if (rightButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isBagMax)
                    {
                        if (consumptions[rightPageItems[i].GetItemID()] - 1 <= 0)
                        {
                            inventory.MoveDepositItemToBag(itemManager, rightPageItems[i].GetItemID());
                            RemoveRightList(i);
                        }
                        else
                        {
                            inventory.MoveDepositItemToBag(itemManager, rightPageItems[i].GetItemID());
                        }
                        AddLeftList(playerItems[playerItems.Count - 1]);
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

            if (mode == DepotModeType.select)
            {
                renderer.DrawString("装備品", equipmentButton.ButtonCenterVector(),
                    Color.White, new Vector2(1, 1), 1.0f, true, true);
                renderer.DrawString("消費アイテム", consumptionButton.ButtonCenterVector(),
                    Color.White, new Vector2(1, 1), 1.0f, true, true);
            }

            if (mode == DepotModeType.equipment)
            {
                renderer.DrawString("倉庫(" + depotNowNum + "/" + depotMaxNum + ")", new Vector2(1080 / 2 + 64, 64),
                    new Vector2(1, 1), Color.White);
            }

            if (mode == DepotModeType.consumption || mode == DepotModeType.equipment)
            {
                renderer.DrawString("バッグ(" + bagNowNum + "/" + bagMaxNum + ")", new Vector2(64,64),
                    new Vector2(1, 1), Color.White);
                renderer.DrawString("倉庫", new Vector2(1080 / 2 + 64, 64),
                    new Vector2(1, 1), Color.White);

                renderer.DrawString("アイテム名", new Vector2(64, 64 + 32), new Vector2(1, 1), Color.White);
                renderer.DrawString("タイプ", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);
                renderer.DrawString("アイテム名", new Vector2(1080 / 2 + 64, 64 + 32), new Vector2(1, 1), Color.White);
                renderer.DrawString("タイプ", new Vector2(1080 / 2 + 224, 64 + 32), new Vector2(1, 1), Color.White);
                if (mode == DepotModeType.consumption)
                    renderer.DrawString("所持数", new Vector2(1080 / 2 + 320, 64 + 32), new Vector2(1, 1), Color.White);

                //左側のリストのアイテムの描画
                for (int i = 0; i < leftPageItems.Count; i++)
                {
                    leftWindows[i].Draw();

                    //アイテム名表示
                    renderer.DrawString(leftPageItems[i].GetItemName(), leftWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);

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
                    renderer.DrawString(rightPageItems[i].GetItemName(), rightWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);

                    //アイテムタイプの表示
                    string type;
                    if (rightPageItems[i] is WeaponItem)
                    {
                        type = ((WeaponItem)rightPageItems[i]).GetWeaponType().ToString();
                    }
                    else if (rightPageItems[i] is ProtectionItem)
                    {
                        type = ((ProtectionItem)rightPageItems[i]).GetProtectionType().ToString();
                    }
                    else
                    {
                        type = ((ConsumptionItem)rightPageItems[i]).GetTypeText();
                    }
                    renderer.DrawString(type, rightWindows[i].GetOffsetPosition() + new Vector2(160, 0),
                        new Vector2(1, 1), Color.White);

                    //所持数の表示(消費アイテムのみ)
                    if (mode == DepotModeType.consumption)
                    {
                        renderer.DrawString(consumptions[rightPageItems[i].GetItemID()].ToString(),
                            rightWindows[i].GetOffsetPosition() + new Vector2(256, 0),
                            new Vector2(1, 1), Color.White);
                    }
                }

                if (isBagMaxMessaga)
                    renderer.DrawString("バッグがいっぱいです。", new Vector2(320, 720 / 2), new Vector2(2, 2), Color.Red);

                if (isDepotMaxMessaga)
                    renderer.DrawString("倉庫がいっぱいです。", new Vector2(320, 720 / 2), new Vector2(2, 2), Color.Red);
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
            return endFlag;
        }

        public SceneType Next()
        {
            return SceneType.Town;
        }
    }
}
