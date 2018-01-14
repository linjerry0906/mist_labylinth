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

namespace Team27_RougeLike.Scene.Town
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

        private List<Item> playerItems;
        private Dictionary<int, int> depotConsumptions;
        private Item upgradeItem;
        private Dictionary<int, int> materialItems;
        private Dictionary<int, int> consumptions;

        private Button backButton;
        private Window backWindow;
        private Button UpgradeButton;
        private Window UpgradeWindow;

        private Window leftWindow;
        private Window rightWindow;
        private Window messegeWindow;

        private List<Item> leftItems;
        private List<Button> leftButtons;
        private List<Window> leftWindows;

        private bool isSelect; //アイテムを選んだかどうか
        private bool isNotEnough; //素材が足りてるかどうか
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
                new Vector2(1080 / 2 - 128, 720 - 128 - 64));
            rightWindow.Initialize();
            messegeWindow = new Window(gameDevice, new Vector2(1080 / 2 - 160, 720 / 2 - 80), new Vector2(320, 160));
            messegeWindow.Initialize();

            backButton = new Button(new Vector2(0, 720 - 64), 64, 32);
            backWindow = new Window(gameDevice, new Vector2(0, 720 - 64), new Vector2(64, 32));
            UpgradeButton = new Button(new Vector2(1080 - 64, 720 - 64), 64, 32);
            UpgradeWindow = new Window(gameDevice, new Vector2(1080 - 64, 720 - 64), new Vector2(64, 32));

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
            isNotEnough = false;
            isBiggest = false;
            isNotEnoughMessage = false;
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

        //消費素材をセット
        private void SetMaterial(int level)
        {
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
            materialItems = new Dictionary<int, int>();

            upgradeItem = item;

            int level;
            if (item is ProtectionItem)
            {
                level = ((ProtectionItem)item).GetReinforcement();
                isBiggest = ((ProtectionItem)item).IsLevelMax();
            }
            else
            {
                level = ((WeaponItem)item).GetReinforcement();
                isBiggest = ((WeaponItem)item).IsLevelMax();
            }
        }

        //消費したアイテムを消す
        private void RemoveItem(int id, int num)
        {
            foreach (Item item in playerItems)
            {
                if (id == item.GetItemID())
                {
                    //Inventoryのbagから指定されたアイテムを削除するメソッド
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
                            //InventoryのitemDepositoryから指定されたアイテムを削除するメソッド
                            depotConsumptions.Remove(id);
                        }
                        else
                        {
                            //InventoryのitemDepositoryから指定されたアイテムを削除するメソッド
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
            UpgradeWindow.Update();
            foreach (Window w in leftWindows)
            {
                w.Update();
            }

            if (isSelect && !rightWindow.CurrentState())
            {
                rightWindow.Switch();
            }
            else
            {
                rightWindow.Switch();
            }   

            Point mousePos = new Point(
                (int)input.GetMousePosition().X,
                (int)input.GetMousePosition().Y);

            if (UpgradeButton.IsClick(mousePos))
            {
                if (isNotEnough)
                {
                    isNotEnoughMessage = true;
                }
                else
                {
                    if (!isBiggest)
                    {

                    }
                }
            }

            for (int i = 0; i < leftButtons.Count; i++)
            {
                if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick())
                {
                    SetItem(leftItems[i]);
                    isSelect = true;
                }
            }
        }

        public void Draw()
        {

        }

        public void Shutdown()
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
