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
        private List<Item> depository;
        private List<Item> equipments;
        private Dictionary<Item,int> consumptions;

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
        private List<Button> leftButtons;
        private List<Button> rightButtons;
        private List<Window> leftWindows;
        private List<Window> rightWindows;

        private int bagMaxNum;
        private int bagNowNum;

        private bool isBagMax;
        private bool isBagMaxMessaga;

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
            itemManager = gameManager.ItemManager;
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            blurRate = 0.0f;

            mode = DepotModeType.select;
            playerItems = inventory.BagList();
            depository = inventory.EquipDepository();
            equipments = new List<Item>();
            consumptions = new Dictionary<Item, int>();

            foreach(Item i in depository)
            {
                if (i is ConsumptionItem)
                {
                    if (consumptions[i] != 0)
                    {
                        consumptions[i]++;
                    }
                    else
                    {
                        consumptions[i] = 1;
                    }
                }
                else
                {
                    equipments.Add(i);
                }
            }

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
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();
        }

        private void EquipmentModeInitialize()
        {
            leftItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();

            mode = DepotModeType.equipment;
            foreach(Item i in playerItems)
            {
                if(i is WeaponItem || i is ProtectionItem)
                {
                    AddLeftList(i);
                }
            }
            foreach(Item i in equipments)
            {
                AddRightList(i);
            }
        }

        private void ConsumptionModeInitialize()
        {
            leftItems = new List<Item>();
            leftButtons = new List<Button>();
            leftWindows = new List<Window>();
            rightItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();

            mode = DepotModeType.consumption;
            foreach(Item i in playerItems)
            {
                if (i is ConsumptionItem)
                {
                    AddLeftList(i);
                }
            }
            foreach(Item i in consumptions.Keys)
            {
                foreach(Item item in rightItems)
                {
                    if (i != item) AddRightList(i);
                }
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

        //右のリストにアイテムを追加する。
        private void AddRightList(Item item)
        {
            rightItems.Add(item);
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
            rightItems.Remove(rightItems[key]);
            rightButtons.Remove(rightButtons[key]);
            rightWindows.Remove(rightWindows[key]);

            //上に詰める処理
            List<Item> copyRightItems = rightItems;
            rightItems = new List<Item>();
            rightButtons = new List<Button>();
            rightWindows = new List<Window>();
            foreach (Item item in copyRightItems)
            {
                AddRightList(item);
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

            if (mode == DepotModeType.select)
            {
                if (!messegeWindow.CurrentState()) messegeWindow.Switch();
                if (!equipmentWindow.CurrentState()) equipmentWindow.Switch();
                if (!consumptionWindow.CurrentState()) consumptionWindow.Switch();
                if (leftWindow.CurrentState()) leftWindow.Switch();
                if (rightWindow.CurrentState()) rightWindow.Switch();

                inventory.BagItemCount(ref bagNowNum, ref bagMaxNum);

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
            else
            {
                if (messegeWindow.CurrentState()) messegeWindow.Switch();
                if (equipmentWindow.CurrentState()) equipmentWindow.Switch();
                if (consumptionWindow.CurrentState()) consumptionWindow.Switch();
                if (!leftWindow.CurrentState()) leftWindow.Switch();
                if (!rightWindow.CurrentState()) rightWindow.Switch();

                if (bagNowNum >= bagMaxNum) isBagMax = true;
                foreach(Button b in rightButtons)
                {
                    if (b.IsClick(mousePos) && isBagMax)
                        isBagMaxMessaga = true;
                }
                isBagMax = false;
                isBagMaxMessaga = false;

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
                    if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick())
                    {
                        AddRightList(leftItems[i]);
                        equipments.Add(leftItems[i]);
                        inventory.RemoveItem(i);
                        RemoveLeftList(i);
                    }
                }

                //倉庫側
                for (int i = 0; i < rightButtons.Count; i++)
                {
                    if (rightButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isBagMax)
                    {
                        AddLeftList(rightItems[i]);
                        equipments.Remove(rightItems[i]);
                        inventory.AddItem(rightItems[i]);
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
                    inventory.RemoveItem(i);
                    if (leftButtons[i].IsClick(mousePos) && input.IsLeftClick())
                    {
                        foreach(Item item in consumptions.Keys)
                        {
                            if (item == leftItems[i])
                            {
                                consumptions[leftItems[i]]++;
                                RemoveLeftList(i);
                                return;
                            }
                        }
                        consumptions[leftItems[i]] = 1;
                        AddRightList(leftItems[i]);
                        RemoveLeftList(i);
                    }
                }

                //倉庫側
                for (int i = 0; i < rightButtons.Count; i++)
                {
                    if (rightButtons[i].IsClick(mousePos) && input.IsLeftClick() && !isBagMax)
                    {
                        AddLeftList(rightItems[i]);
                        inventory.AddItem(rightItems[i]);
                        if (consumptions[rightItems[i]] - 1 != 0)
                        {
                            consumptions.Remove(rightItems[i]);
                            RemoveRightList(i);
                        }
                        else
                        {
                            consumptions[rightItems[i]]--;
                        }
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

            if (mode == DepotModeType.consumption || mode == DepotModeType.equipment)
            {
                renderer.DrawString("バッグ(" + bagNowNum + "/" + bagMaxNum + ")", new Vector2(64,64),
                    new Vector2(1, 1), Color.White);
                renderer.DrawString("倉庫", new Vector2(1080 / 2 + 64, 64), new Vector2(1, 1), Color.White);

                renderer.DrawString("アイテム名", new Vector2(64, 64 + 32), new Vector2(1, 1), Color.White);
                renderer.DrawString("タイプ", new Vector2(224, 64 + 32), new Vector2(1, 1), Color.White);
                renderer.DrawString("アイテム名", new Vector2(1080 / 2 + 64, 64 + 32), new Vector2(1, 1), Color.White);
                renderer.DrawString("タイプ", new Vector2(1080 / 2 + 224, 64 + 32), new Vector2(1, 1), Color.White);
                if (mode == DepotModeType.consumption)
                    renderer.DrawString("所持数", new Vector2(1080 / 2 + 320, 64 + 32), new Vector2(1, 1), Color.White);

                if (isBagMaxMessaga)
                {
                    renderer.DrawString("バッグがいっぱいです。", new Vector2(320, 720 / 2), new Vector2(2, 2), Color.Red);
                }

                //左側のリストのアイテムの描画
                for (int i = 0; i < leftItems.Count; i++)
                {
                    leftWindows[i].Draw();

                    //アイテム名表示
                    renderer.DrawString(leftItems[i].GetItemName(), leftWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);

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

                //右側のリストのアイテムの描画
                for (int i = 0; i < rightItems.Count; i++)
                {
                    rightWindows[i].Draw();

                    //アイテム名の表示
                    renderer.DrawString(rightItems[i].GetItemName(), rightWindows[i].GetOffsetPosition(),
                        new Vector2(1, 1), Color.White);

                    //アイテムタイプの表示
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
                    renderer.DrawString(type, rightWindows[i].GetOffsetPosition() + new Vector2(160, 0),
                        new Vector2(1, 1), Color.White);

                    //所持数の表示(消費アイテムのみ)
                    if (mode == DepotModeType.consumption)
                    {
                        renderer.DrawString(consumptions[leftItems[i]].ToString(),
                            leftWindows[i].GetOffsetPosition() + new Vector2(256, 0),
                            new Vector2(1, 1), Color.White);
                    }
                }
            }

            renderer.End();
        }

        public void Shutdown()
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
