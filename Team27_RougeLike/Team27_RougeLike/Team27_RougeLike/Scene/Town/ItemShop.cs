//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.06
// 内容  ：ショップシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Effects;
using Team27_RougeLike.Scene.Town;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.UI;

namespace Team27_RougeLike.Scene
{
    class ItemShop : IScene
    {
        public enum ShopMode
        {
            buy,
            sell,
            select,
        }

        private GameDevice gameDevice;          //ゲームデバイス
        private InputState input;
        private Renderer renderer;
        private GameManager gameManager;        //Player現在の情報を取得用

        private BlurEffect blurEffect;
        private float blurRate;

        private IScene townScene;               //村シーン

        private bool endFlag;                   //終了フラグ

        private Store store;
        private ItemManager itemManager;
        private Inventory inventory;
        private Window leftWindow;
        private Window rightWindow;
        private Window messegeWindow;

        private ShopMode mode;                  //売るか、買うか、どうするか
        private bool isPop;
        private bool isMessegePop;

        private Rectangle backRect;

        public ItemShop(IScene town, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            this.gameManager = gameManager;
            blurEffect = renderer.EffectManager.GetBlurEffect();
            itemManager = gameManager.ItemManager;
            inventory = gameManager.PlayerItem;

            townScene = town;

            backRect = new Rectangle(0, 720 - 64, 320, 64);
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            blurRate = 0.0f;

            store = new Store(gameManager, gameDevice);
            store.Initialize();

            Vector2 size = new Vector2(1080 / 2 - 128, 720 - 128);
            leftWindow = new Window(gameDevice, new Vector2(64, 64), size);
            leftWindow.Initialize();
            rightWindow = new Window(gameDevice, new Vector2(1080 / 2 + 64, 64), size);
            rightWindow.Initialize();
            messegeWindow = new Window(gameDevice, new Vector2(1080 / 2 - 160, 720 / 2 - 80), new Vector2(320, 160));
            messegeWindow.Initialize();

            mode = ShopMode.select;
            isPop = false;
            isMessegePop = false;
        }

        public void Update(GameTime gameTime)
        {
            store.Update();

            leftWindow.Update();
            rightWindow.Update();
            messegeWindow.Update();

            Point mousePos = new Point(
                (int)input.GetMousePosition().X,
                (int)input.GetMousePosition().Y);

            if (mode == ShopMode.select)
            {
                if (backRect.Contains(mousePos) && input.IsLeftClick())
                {
                    endFlag = true;
                }

                if (!isMessegePop)
                {
                    messegeWindow.Switch();
                    isMessegePop = true;
                }
                if (input.GetKeyState(Keys.D1))
                {
                    mode = ShopMode.buy;
                    store.Initialize();
                    store.Buy();
                    messegeWindow.Switch();
                    isMessegePop = false;
                }
                else if (input.GetKeyState(Keys.D2))
                {
                    mode = ShopMode.sell;
                    store.Initialize();
                    store.Sell();
                    messegeWindow.Switch();
                    isMessegePop = false;
                }
            }
            else
            {
                if (!isPop)
                {
                    leftWindow.Switch();
                    rightWindow.Switch();
                    isPop = true;
                }
                if (input.GetKeyState(Keys.D0) || backRect.Contains(mousePos) && input.IsLeftClick())
                {
                    mode = ShopMode.select;
                    leftWindow.Switch();
                    rightWindow.Switch();
                    isPop = false;
                }
            }

            if (input.GetKeyTrigger(Keys.B))
                endFlag = true;

            UpdateBlurRate();
            blurEffect.Update(blurRate);
        }

        public void Draw()
        {
            blurEffect.WriteRenderTarget(renderer.FogManager.CurrentColor());
            townScene.Draw();                       //背景は前のシーンを描画
            blurEffect.ReleaseRenderTarget();
            blurEffect.Draw(renderer);


            renderer.Begin();

            renderer.DrawString("戻る", new Vector2(0, 720 - 64), new Vector2(1, 1), Color.White);

            leftWindow.Draw();
            rightWindow.Draw();
            messegeWindow.Draw();

            renderer.DrawString("B key back to Town", Vector2.Zero, new Vector2(1, 1), Color.Black);

            if (mode == ShopMode.select)
            {
                renderer.DrawString("1で買う、2で売却", messegeWindow.GetLeftCenter(), new Vector2(1, 1), Color.White);
            }
            else
            {
                renderer.DrawString("0でselectに戻る", new Vector2(0, 32), new Vector2(1, 1), Color.Black);
                if (mode == ShopMode.buy)
                {
                    renderer.DrawString("売り物リスト", new Vector2(64, 64), new Vector2(1, 1), Color.White);
                    renderer.DrawString("買う物リスト", new Vector2(1080 / 2 + 64, 64), new Vector2(1, 1), Color.White);
                    store.DrawEquip();
                }
                else if (mode == ShopMode.sell)
                {
                    renderer.DrawString("持ち物リスト", new Vector2(64, 64), new Vector2(1, 1), Color.White);
                    renderer.DrawString("売る物のリスト", new Vector2(1080 / 2 + 64, 64), new Vector2(1, 1), Color.White);
                    store.DrawEquip();
                }
            }
            renderer.End();
        }


        public bool IsEnd()
        {
            return endFlag && blurRate <= 0;
        }

        public SceneType Next()
        {
            return SceneType.Town;
        }

        public void Shutdown()
        {
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
    }
}
