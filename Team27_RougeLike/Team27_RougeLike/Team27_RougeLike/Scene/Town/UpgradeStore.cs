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
    class UpgradeStore:IScene
    {
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

        private List<Item> playerItems; //バッグ
        private Dictionary<int, int> depotConsumptions; //倉庫のアイテム
        private Item selectItem; //強化するアイテム
        private Item upgradeItem; //強化後のアイテム
        private Dictionary<int, int> materialItems; //必要な素材
        private Dictionary<int, int> consumptions; //消費アイテム

        private ItemInfoUI selectItemInfoUI;
        private ItemInfoUI upgradeItemInfoUI;

        private Button backButton;
        private Window backWindow;
        private Button upgradeButton;
        private Window upgradeWindow;

        private Window leftWindow;
        private Window rightWindow;
        private Window messegeWindow;

        private List<Item> leftItems;
        private List<Item> leftPageItems;
        private List<Button> leftButtons;
        private List<Window> leftWindows;

        private int leftPage;                   //左のページ
        private int leftMaxPage;                //左の最大ページ

        private Window leftPageRightWindow;     //左側のページを右にめくるWindow
        private Window leftPageLeftWindow;      //左側のページを左にめくるWIndow
        private Button leftPageRightButton;     //左側のページを右にめくるButton
        private Button leftPageLeftButton;      //左側のページを左にめくるButton

        private bool isSelect;                  //アイテムを選んだかどうか
        private bool isEnough;                  //素材が足りてるかどうか
        private bool isNotEnoughMessage;        //素材が足りてないメッセージ;
        private bool isBiggest;                 //レベルがマックスかどうか

        private int windowWidth;
        private int windowHeight;

        public UpgradeStore(IScene town, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            blurEffect = renderer.EffectManager.GetBlurEffect();
            townScene = town;

            inventory = gameManager.PlayerItem;
            itemManager = new ItemManager();

            windowWidth = Def.WindowDef.WINDOW_WIDTH;
            windowHeight = Def.WindowDef.WINDOW_HEIGHT;

            selectItemInfoUI = new ItemInfoUI(new Vector2(windowWidth / 2 + 64, 96), gameManager, gameDevice);
            upgradeItemInfoUI = new ItemInfoUI(new Vector2(windowWidth / 2 + 64, 224 + 32), gameManager, gameDevice);

            leftPageLeftWindow = new Window(gameDevice, new Vector2(windowWidth / 4 - 64 - 64, windowHeight - 96), new Vector2(64, 32));
            leftPageLeftWindow.Initialize();
            leftPageRightWindow = new Window(gameDevice, new Vector2(windowWidth / 4 + 64, windowHeight - 96), new Vector2(64, 32));
            leftPageRightWindow.Initialize();
            leftPageLeftButton = new Button(leftPageLeftWindow.GetOffsetPosition(), 64, 32);
            leftPageRightButton = new Button(leftPageRightWindow.GetOffsetPosition(), 64, 32);
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            blurRate = 0.0f;

            playerItems = inventory.BagList();
            depotConsumptions = inventory.DepositoryItem();
            consumptions = new Dictionary<int, int>();

            leftPage = 1;
            leftMaxPage = 1;

            foreach (Item item in playerItems)
            {
                if (item is ConsumptionItem)
                {
                    if (!consumptions.ContainsKey(item.GetItemID()))
                    {
                        consumptions.Add(item.GetItemID(), 1);
                    }
                    else
                    {
                        consumptions[item.GetItemID()]++;
                    }
                }
            }
            foreach (int id in depotConsumptions.Keys)
            {
                if (!consumptions.ContainsKey(id))
                {
                    consumptions.Add(id, 1);
                }
                else
                {
                    consumptions[id]++;
                }
            }

            itemManager.LoadAll();
            
            leftWindow = new Window(gameDevice, new Vector2(64, 64),
                new Vector2(windowWidth / 2 - 128, windowHeight - 128));
            leftWindow.Initialize();
            leftWindow.Switch();
            rightWindow = new Window(gameDevice, new Vector2(windowWidth / 2 + 64, 64),
                new Vector2(windowWidth / 2 - 128, windowHeight - 160 - 64));
            rightWindow.Initialize();
            rightWindow.SetAlphaLimit(0.6f);
            messegeWindow = new Window(gameDevice, new Vector2(windowWidth / 2 - 160, windowHeight / 2 - 80), new Vector2(384, 160));
            messegeWindow.Initialize();
            messegeWindow.SetAlphaLimit(1.0f);

            backButton = new Button(new Vector2(0, windowHeight - 64), 64, 32);
            backWindow = new Window(gameDevice, new Vector2(0, windowHeight - 64), new Vector2(64, 32));
            upgradeButton = new Button(rightWindow.GetLeftUnder() + new Vector2(0, 32), windowWidth / 2 - 128, 64);
            upgradeWindow = new Window(gameDevice, rightWindow.GetLeftUnder() + new Vector2(0, 32), new Vector2(windowWidth / 2 - 128, 64));


            leftItems = new List<Item>();
            leftPageItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();

            foreach (Item i in playerItems)
            {
                if (i is WeaponItem || i is ProtectionItem)
                {
                    leftItems.Add(i);
                }
            }

            isSelect = false;
            isEnough = false;
            isBiggest = false;
            isNotEnoughMessage = false;
            
            leftMaxPage = (leftItems.Count - 1) / 20 + 1;

            LeftPage(1);
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

        //消費素材をセット
        private void SetMaterial(int level)
        {
            materialItems = new Dictionary<int, int>();

            if (level <= 1)
            {
                materialItems.Add(7, 2);
            }
            else if (level <= 5)
            {
                materialItems.Add(7, 3);
            }
            else if (level <= 10)
            {
                materialItems.Add(7, 2);
                materialItems.Add(8, 1);
            }
            else if (level <= 15)
            {
                materialItems.Add(8, 2);
            }
            else if (level <= 20)
            {
                materialItems.Add(8, 3);
            }
            else if (level <= 25)
            {
                materialItems.Add(7, 2);
                materialItems.Add(8, 2);
            }
            else if (level <= 30)
            {
                materialItems.Add(7, 3);
                materialItems.Add(8, 2);
                materialItems.Add(9, 1);
            }
            else if (level <= 40)
            {
                materialItems.Add(7, 5);
                materialItems.Add(8, 3);
                materialItems.Add(9, 1);
            }
            else if (level <= 50)
            {
                materialItems.Add(7, 6);
                materialItems.Add(8, 4);
                materialItems.Add(9, 1);
            }
            else if (level <= 60)
            {
                materialItems.Add(7, 7);
                materialItems.Add(8, 5);
                materialItems.Add(9, 1);
            }
            else if (level <= 80)
            {
                materialItems.Add(7, 7);
                materialItems.Add(8, 5);
                materialItems.Add(9, 1);
            }
            else if (level <= 90)
            {
                materialItems.Add(7, 10);
                materialItems.Add(8, 6);
                materialItems.Add(9, 2);
            }
            else if (level <= 10)
            {
                materialItems.Add(7, 13);
                materialItems.Add(8, 8);
                materialItems.Add(9, 3);
            }
        }

        //強化するアイテムをセット
        private void SetItem(Item item)
        {
            if (item == null) return;
            isSelect = true;

            selectItem = item;
            upgradeItem = item.UniqueClone();



            int level;
            if (item is ProtectionItem)
            {
                level = ((ProtectionItem)item).GetReinforcement();
                isBiggest = ((ProtectionItem)item).IsLevelMax();
                ((ProtectionItem)upgradeItem).LevelUp();
            }
            else
            {
                level = ((WeaponItem)item).GetReinforcement();
                isBiggest = ((WeaponItem)item).IsLevelMax();
                ((WeaponItem)upgradeItem).LevelUp();
            }

            SetMaterial(level);
        }

        //消費したアイテムを消す
        private void RemoveItem(int id, int num)
        {
            foreach (Item item in playerItems)
            {
                if (id == item.GetItemID())
                {
                    inventory.RemoveItem(inventory.BagItemIndex(item));
                    playerItems.Remove(item);
                    num--;
                    if (num <= 0)
                    {
                        return;
                    }
                }
            }
            foreach (int depotID in depotConsumptions.Keys)
            {
                if (id == depotID)
                {
                    while (num <= 0)
                    {
                        if (depotConsumptions[id] - 1 <= 0)
                        {
                            inventory.RemoveDepositoryItem(id, 1);
                            depotConsumptions.Remove(id);
                        }
                        else
                        {
                            inventory.RemoveDepositoryItem(id, 1);
                            depotConsumptions[id]--;
                        }
                        num--;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateBlurRate();
            blurEffect.Update(blurRate);

            leftWindow.Update();
            rightWindow.Update();
            messegeWindow.Update();
            backWindow.Update();
            upgradeWindow.Update();
            foreach (Window w in leftWindows)
            {
                w.Update();
            }

            isBiggest = false;
            isEnough = false;
            isNotEnoughMessage = false;

            Point mousePos = new Point(
                (int)input.GetMousePosition().X,
                (int)input.GetMousePosition().Y);

            if (isSelect && !rightWindow.CurrentState())
            {
                rightWindow.Switch();
            }
            if (!upgradeWindow.CurrentState())
                upgradeWindow.Switch();
            if (!backWindow.CurrentState())
                backWindow.Switch();
            if (isNotEnoughMessage)
            {
                if (!messegeWindow.CurrentState())
                    messegeWindow.Switch();
            }
            else
            {
                if (messegeWindow.CurrentState())
                    messegeWindow.Switch();
            }
            if (isSelect)
            {
                if (!upgradeWindow.CurrentState())
                    upgradeWindow.Switch();
            }
            else
            {
                if (upgradeWindow.CurrentState())
                    upgradeWindow.Switch();
            }

            leftPageRightWindow.Update();
            leftPageLeftWindow.Update();

            //ページ関連
            if (leftPage > leftMaxPage)
            {
                leftPage = leftMaxPage;
                LeftPage(leftPage);
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

            if (backButton.IsClick(mousePos) && input.IsLeftClick())
            {
                endFlag = true;
            }

            for (int i = 0; i < leftButtons.Count; i++)
            {
                if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick())
                {
                    SetItem(leftItems[i + (leftPage - 1) * 20]);
                }
            }

            playerItems = inventory.BagList();
            leftItems = new List<Item>();
            foreach (Item i in playerItems)
            {
                if (i is WeaponItem || i is ProtectionItem)
                {
                    leftItems.Add(i);
                }
            }

            //左側
            if (upgradeButton.IsClick(mousePos) && isSelect && !isBiggest)
            {
                if (!isEnough)
                {
                    isNotEnoughMessage = true;
                }
                else
                {
                    if (input.IsLeftClick())
                    {
                        inventory.RemoveItem(inventory.BagItemIndex(selectItem));
                        if (selectItem is WeaponItem)
                        {
                            ((WeaponItem)selectItem).LevelUp();
                        }
                        else
                        {
                            ((ProtectionItem)selectItem).LevelUp();
                        }
                        inventory.AddItem(selectItem);
                        foreach (int id in materialItems.Keys)
                        {
                            RemoveItem(id, materialItems[id]);
                        }
                        Initialize(SceneType.UpgradeStore);  //変更予定
                    }
                }
            }

            //素材が足りているかどうか
            if (isSelect)
            {
                //SetItem(selectItem);

                foreach (int id in materialItems.Keys)
                {
                    if (consumptions.Keys.Contains(id))
                    {
                        if (consumptions[id] >= materialItems[id])
                        {
                            isEnough = true;
                        }
                    }
                    if (!isEnough)
                    {
                        return;
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
            upgradeWindow.Draw();

            leftWindow.Draw();
            rightWindow.Draw();

            leftPageRightWindow.Draw();
            leftPageLeftWindow.Draw();

            renderer.DrawString("バッグ", new Vector2(64, 64), new Vector2(1, 1), Color.White);
            renderer.DrawString("アイテム名", new Vector2(64, 64 + 32), new Vector2(1, 1), Color.White);
            renderer.DrawString("タイプ", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);

            renderer.DrawString("ページ(" + leftPage + "/" + leftMaxPage + ")", new Vector2(windowWidth / 4 - 48, windowHeight - 96), new Vector2(1, 1), Color.White);
            if (leftPageLeftWindow.CurrentState())
                renderer.DrawString("←", leftPageLeftWindow.GetCenter(), Color.White, new Vector2(1, 1), 1.0f, true, true);
            if (leftPageRightWindow.CurrentState())
                renderer.DrawString("→", leftPageRightWindow.GetCenter(), Color.White, new Vector2(1, 1), 1.0f, true, true);

                //左側のリストのアイテムの描画
                for (int i = 0; i < leftPageItems.Count; i++)
            {
                leftWindows[i].Draw();

                //アイテム名表示
                if (leftPageItems[i] is WeaponItem)
                {
                    renderer.DrawString(leftPageItems[i].GetItemName() + "+" + ((WeaponItem)leftPageItems[i]).GetReinforcement(), leftWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);
                }
                else
                {
                    renderer.DrawString(leftPageItems[i].GetItemName() + "+" + ((ProtectionItem)leftPageItems[i]).GetReinforcement(), leftWindows[i].GetOffsetPosition(),
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

            //右側の表示
            if (isSelect)
            {
                //強化ボタン表示
                renderer.DrawString("強化", upgradeButton.ButtonCenterVector(),
                    Color.White, new Vector2(2, 2), 1.0f, true, true);
                

                renderer.DrawString("強化前", new Vector2(windowWidth / 2 + 64, 64), new Vector2(1, 1), Color.White);
                selectItemInfoUI.Draw(selectItem, 1.0f);
                renderer.DrawString("強化後", new Vector2(windowWidth / 2 + 64, 192 + 32), new Vector2(1, 1), Color.White);
                upgradeItemInfoUI.Draw(upgradeItem, 1.0f);
                
                //必要素材
                renderer.DrawString("必要な素材", 
                    new Vector2(windowWidth / 2 + 64, 192 + 192), new Vector2(1, 1), Color.White);
                renderer.DrawString("(所持数 / 必要数)",
                    new Vector2(windowWidth / 2 + 160, 192 + 192), new Vector2(1, 1), Color.White);

                int num = 0;
                foreach (int id in materialItems.Keys)
                {
                    num++;
                    renderer.DrawString(itemManager.GetConsuptionItem(id).GetItemName(),
                        new Vector2(windowWidth / 2 + 64, 160 + 224 + 32 * num), new Vector2(1, 1), Color.White);
                    if (consumptions.Keys.Contains(id))
                    {
                        renderer.DrawString("("+ consumptions[id] + "/" + materialItems[id]+")",
                            new Vector2(windowWidth / 2 + 160, 160 + 224 + 32 * num), new Vector2(1, 1), Color.White);
                    }
                    else
                    {
                        renderer.DrawString("(0/" + materialItems[id] + ")",
                            new Vector2(windowWidth / 2 + 160, 160 + 224 + 32 * num), new Vector2(1, 1), Color.White);
                    }
                }
            }

            messegeWindow.Draw();

            if (isBiggest || !isEnough)
            {
                upgradeWindow.Draw();
            }

            if (isNotEnoughMessage)
            {
                renderer.DrawString("素材が足りていません。", messegeWindow.GetCenter(), Color.Red, new Vector2(2, 2), 1.0f, true, true);
            }

            renderer.End();
        }

        public void ShutDown()
        {

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
