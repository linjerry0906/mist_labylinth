using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        private List<Item> playerItems;                 //バッグ
        private Dictionary<int, int> depotConsumptions; //倉庫のアイテム
        private Item selectItem;                        //強化するアイテム
        private Item upgradeItem;                       //強化後のアイテム
        private Dictionary<int, int> materialItems;     //必要な素材
        private Dictionary<int, int> consumptions;      //消費アイテム
        private int myMoney;                            //所持金
        private int useMoney;                           //消費金額

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
        private bool isNotEnoughMessage;        //素材が足りてないメッセージ
        private bool isMoney;                   //お金が足りているかどうか
        private bool isNoMoneyMessage;          //お金が足りていない時のメッセージ
        private bool isBiggest;                 //レベルがマックスかどうか
        private bool isBiggestMessage;          //装備レベルが最大時のメッセージ

        private int windowWidth;
        private int windowHeight;

        private string materialFilename = @"Content/ItemCSV/MaterialItem.csv";

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

            myMoney = inventory.CurrentMoney();
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
                    consumptions.Add(id, depotConsumptions[id]);
                }
                else
                {
                    consumptions[id] += depotConsumptions[id];
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
            isMoney = false;
            isBiggest = false;
            isNotEnoughMessage = false;
            isNoMoneyMessage = false;
            isBiggestMessage = false;
            
            leftMaxPage = (leftItems.Count - 1) / 20 + 1;

            LeftPage(1);
        }

        public void Reset()
        {
            myMoney = inventory.CurrentMoney();
            playerItems = inventory.BagList();
            depotConsumptions = inventory.DepositoryItem();
            consumptions = new Dictionary<int, int>();

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
                    consumptions.Add(id, depotConsumptions[id]);
                }
                else
                {
                    consumptions[id] += depotConsumptions[id];
                }
            }

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
            isMoney = false;
            isBiggest = false;
            isNotEnoughMessage = false;
            isNoMoneyMessage = false;
            isBiggestMessage = false;

            LeftPage(leftPage);
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
        private void SetMaterial(int level, string type, int rare)
        {
            //csvで読み込む要素
            //type, rare, level, useMone, material1ID, 個数 , material2ID, 個数 , material3ID, 個数 

            materialItems = new Dictionary<int, int>();

            FileStream fs = new FileStream(materialFilename, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("shift_jis"));

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] csvDate = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (type == csvDate[0] && rare == int.Parse(csvDate[1]) && level <= int.Parse(csvDate[2]))
                {
                    useMoney = int.Parse(csvDate[3]);
                    for (int i = 4; i < csvDate.Length; i += 2)
                    {
                        if (csvDate[i] == "no")
                        {
                            break;
                        }
                        materialItems.Add(int.Parse(csvDate[i]), int.Parse(csvDate[i + 1]));
                    }
                    break;
                }
            }

            sr.Close();
            fs.Close();
        }

        //強化するアイテムをセット
        private void SetItem(Item item)
        {
            if (item == null) return;
            isSelect = true;

            selectItem = item;
            upgradeItem = item.UniqueClone();
            
            int level;
            string type = "no";
            int rare = selectItem.GetItemRare();
            if (item is ProtectionItem)
            {
                level = ((ProtectionItem)item).GetReinforcement();
                isBiggest = ((ProtectionItem)item).IsLevelMax();
                if (!isBiggest)
                {
                    ((ProtectionItem)upgradeItem).LevelUp();
                    type = ((ProtectionItem)selectItem).GetProtectionType().ToString();
                }
            }
            else
            {
                level = ((WeaponItem)item).GetReinforcement();
                isBiggest = ((WeaponItem)item).IsLevelMax();
                if (!isBiggest)
                {
                    ((WeaponItem)upgradeItem).LevelUp();
                    type = ((WeaponItem)selectItem).GetWeaponType().ToString();
                }
            }

            SetMaterial(level,type, rare);
        }

        //消費したアイテムを消す
        private void RemoveItem(int id, int num)
        {
            playerItems = inventory.BagList();
            foreach (Item item in playerItems)
            {
                if (item is ConsumptionItem)
                {
                    if (id == item.GetItemID())
                    {
                        inventory.RemoveItem(inventory.BagItemIndex(item));
                        playerItems.Remove(item);
                        num--;
                        if (num <= 0)
                        {
                            break;
                        }
                    }
                }
            }
            inventory.RemoveDepositoryItem(id, num);
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
            if (isNotEnoughMessage || isNoMoneyMessage || isBiggestMessage)
            {
                if (!messegeWindow.CurrentState())
                    messegeWindow.Switch();
            }
            else if (!isNotEnoughMessage && !isNoMoneyMessage && !isBiggestMessage)
            {
                if (messegeWindow.CurrentState())
                    messegeWindow.Switch();
            }

            isNotEnoughMessage = false;
            isNoMoneyMessage = false;
            isBiggestMessage = false;

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

            //強化ボタン
            if (upgradeButton.IsClick(mousePos) && isSelect)
            {
                if (!isEnough)
                {
                    isNotEnoughMessage = true;
                }
                if (!isMoney)
                {
                    isNoMoneyMessage = true;
                }
                if (isBiggest)
                {
                    isBiggestMessage = true;
                }
                if (isEnough && isMoney && !isBiggest)
                {
                    if (input.IsLeftClick())
                    {
                        inventory.SpendMoney(useMoney);
                        if (selectItem is WeaponItem)
                        {
                            ((WeaponItem)selectItem).LevelUp();
                        }
                        else
                        {
                            ((ProtectionItem)selectItem).LevelUp();
                        }
                        foreach (int id in materialItems.Keys)
                        {
                            RemoveItem(id, materialItems[id]);
                        }
                        Reset();
                    }
                }
            }
            if (isSelect && !isBiggest)
            {
                //お金が足りているか
                if (myMoney >= useMoney)
                {
                    isMoney = true;
                }
                else
                {
                    isMoney = false;
                }


                //素材が足りているかどうか
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
                        isEnough = false;
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
                
                //選択されたアイテム
                renderer.DrawString("強化前", new Vector2(windowWidth / 2 + 64, 64), new Vector2(1, 1), Color.White);
                selectItemInfoUI.Draw(selectItem, 1.0f);

                //選択されたアイテムの強化後
                if (!isBiggest)
                {
                    renderer.DrawString("強化後", new Vector2(windowWidth / 2 + 64, 192 + 32), new Vector2(1, 1), Color.White);
                    upgradeItemInfoUI.Draw(upgradeItem, 1.0f);
                }

                if (!isBiggest)
                {
                    //必要素材
                    renderer.DrawString("必要な素材",
                        new Vector2(windowWidth / 2 + 64, 192 + 192), new Vector2(1, 1), Color.White);
                    renderer.DrawString("(所持数 / 必要数)",
                        new Vector2(windowWidth / 2 + 160, 192 + 192), new Vector2(1, 1), Color.White);

                    int num = 0;
                    foreach (int id in materialItems.Keys)
                    {
                        num++;
                        if (consumptions.Keys.Contains(id))
                        {
                            renderer.DrawString(itemManager.GetConsuptionItem(id).GetItemName() + "(" + consumptions[id] + "/" + materialItems[id] + ")",
                                new Vector2(windowWidth / 2 + 64, 160 + 224 + 32 * num), new Vector2(1, 1), Color.White);
                        }
                        else
                        {
                            renderer.DrawString(itemManager.GetConsuptionItem(id).GetItemName() + "(0/" + materialItems[id] + ")",
                                new Vector2(windowWidth / 2 + 64, 160 + 224 + 32 * num), new Vector2(1, 1), Color.White);
                        }
                    }

                    //お金
                    renderer.DrawString("消費金額 : " + useMoney, 
                        rightWindow.GetLeftUnder() + new Vector2(0, -32), new Vector2(1, 1), Color.White);
                }
            }

            messegeWindow.Draw();

            if (isBiggest || !isEnough || !isMoney)
            {
                upgradeWindow.Draw();
            }
            string messageText = "noMessage";

            if (isNotEnoughMessage)
            {
                messageText = "素材が足りていません";
            }
            if (isNoMoneyMessage)
            {
                if(messageText == "noMessage")
                {
                    messageText = "お金が足りていません";
                }
                else
                {
                    messageText = "素材とお金が足りていません。";
                }
            }
            if (isBiggestMessage)
            {
                messageText = "選択された装備はレベルマックスです。";
            }

            if (messageText != "noMessage")
            {
                renderer.DrawString(messageText, messegeWindow.GetCenter(), Color.Red, new Vector2(2, 2), 1.0f, true, true);
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
