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
        private List<Button> leftButtons;
        private List<Window> leftWindows;

        private bool isSelect; //アイテムを選んだかどうか
        private bool isEnough; //素材が足りてるかどうか
        private bool isNotEnoughMessage; //素材が足りてないメッセージ;
        private bool isBiggest; //レベルがマックスかどうか

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

            selectItemInfoUI = new ItemInfoUI(new Vector2(1080 / 2 + 64, 96), gameManager, gameDevice);
            upgradeItemInfoUI = new ItemInfoUI(new Vector2(1080 / 2 + 64, 224 + 32), gameManager, gameDevice);
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            blurRate = 0.0f;

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
                    consumptions.Add(id, 1);
                }
                else
                {
                    consumptions[id]++;
                }
            }

            itemManager.LoadAll();
            
            leftWindow = new Window(gameDevice, new Vector2(64, 64),
                new Vector2(1080 / 2 - 128, 720 - 128));
            leftWindow.Initialize();
            leftWindow.Switch();
            rightWindow = new Window(gameDevice, new Vector2(1080 / 2 + 64, 64),
                new Vector2(1080 / 2 - 128, 720 - 160 - 64));
            rightWindow.Initialize();
            rightWindow.SetAlphaLimit(0.6f);
            messegeWindow = new Window(gameDevice, new Vector2(1080 / 2 - 160, 720 / 2 - 80), new Vector2(384, 160));
            messegeWindow.Initialize();
            messegeWindow.SetAlphaLimit(1.0f);

            backButton = new Button(new Vector2(0, 720 - 64), 64, 32);
            backWindow = new Window(gameDevice, new Vector2(0, 720 - 64), new Vector2(64, 32));
            upgradeButton = new Button(rightWindow.GetLeftUnder() + new Vector2(0, 32), 1080 / 2 - 128, 64);
            upgradeWindow = new Window(gameDevice, rightWindow.GetLeftUnder() + new Vector2(0, 32), new Vector2(1080 / 2 - 128, 64));

            leftItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();

            foreach (Item i in playerItems)
            {
                if (i is WeaponItem || i is ProtectionItem)
                {
                    AddLeftList(i);
                }
            }

            isSelect = false;
            isEnough = false;
            isBiggest = false;
            isNotEnoughMessage = false;
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
            leftItems.Add(item);
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

            isBiggest = false;
            isEnough = false;
            isNotEnoughMessage = false; 

            Point mousePos = new Point(
                (int)input.GetMousePosition().X,
                (int)input.GetMousePosition().Y);

            if (backButton.IsClick(mousePos) && input.IsLeftClick())
            {
                endFlag = true;
            }

            for (int i = 0; i < leftButtons.Count; i++)
            {
                if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick())
                {
                    SetItem(leftItems[i]);
                }
            }

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

            renderer.DrawString("バッグ", new Vector2(64, 64), new Vector2(1, 1), Color.White);
            renderer.DrawString("アイテム名", new Vector2(64, 64 + 32), new Vector2(1, 1), Color.White);
            renderer.DrawString("タイプ", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);

            //左側のリストのアイテムの描画
            for (int i = 0; i < leftItems.Count; i++)
            {
                leftWindows[i].Draw();

                //アイテム名表示
                if (leftItems[i] is WeaponItem)
                {
                    renderer.DrawString(leftItems[i].GetItemName() + "+" + ((WeaponItem)leftItems[i]).GetReinforcement(), leftWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);
                }
                else
                {
                    renderer.DrawString(leftItems[i].GetItemName() + "+" + ((ProtectionItem)leftItems[i]).GetReinforcement(), leftWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);
                }
                
                //アイテムタイプの表示
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
                renderer.DrawString(type, leftWindows[i].GetOffsetPosition() + new Vector2(160, 0),
                    new Vector2(1, 1), Color.White);
            }

            //右側の表示
            if (isSelect)
            {
                //強化ボタン表示
                renderer.DrawString("強化", upgradeButton.ButtonCenterVector(),
                    Color.White, new Vector2(2, 2), 1.0f, true, true);
                

                renderer.DrawString("強化前", new Vector2(1080 / 2 + 64, 64), new Vector2(1, 1), Color.White);
                selectItemInfoUI.Draw(selectItem, 1.0f);
                renderer.DrawString("強化後", new Vector2(1080 / 2 + 64, 192 + 32), new Vector2(1, 1), Color.White);
                upgradeItemInfoUI.Draw(upgradeItem, 1.0f);
                
                //必要素材
                renderer.DrawString("必要な素材", 
                    new Vector2(1080 / 2 + 64, 192 + 192), new Vector2(1, 1), Color.White);
                renderer.DrawString("(必要数 / 所持数)",
                    new Vector2(1080 / 2 + 160, 192 + 192), new Vector2(1, 1), Color.White);

                int num = 0;
                foreach (int id in materialItems.Keys)
                {
                    num++;
                    renderer.DrawString(itemManager.GetConsuptionItem(id).GetItemName(),
                        new Vector2(1080 / 2 + 64, 160 + 224 + 32 * num), new Vector2(1, 1), Color.White);
                    if (consumptions.Keys.Contains(id))
                    {
                        renderer.DrawString("("+materialItems[id] + "/" + consumptions[id]+")",
                            new Vector2(1080 / 2 + 160, 160 + 224 + 32 * num), new Vector2(1, 1), Color.White);
                    }
                    else
                    {
                        renderer.DrawString("("+materialItems[id] + "/0)",
                            new Vector2(1080 / 2 + 160, 160 + 224 + 32 * num), new Vector2(1, 1), Color.White);
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
