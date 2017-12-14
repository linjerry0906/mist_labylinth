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

namespace Team27_RougeLike.Scene
{
    class ItemShop : IScene
    {
        private GameDevice gameDevice;          //ゲームデバイス
        private InputState input;
        private Renderer renderer;
        private GameManager gameManager;        //Player現在の情報を取得用

        private BlurEffect blurEffect;
        private float blurRate;

        private IScene townScene;               //村シーン

        private bool endFlag;                   //終了フラグ

        private Store stores;

        public ItemShop(IScene town, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            this.gameManager = gameManager;
            blurEffect = renderer.EffectManager.GetBlurEffect();

            townScene = town;
        }

        public void Draw()
        {
            blurEffect.WriteRenderTarget(renderer.FogManager.CurrentColor());
            renderer.Begin();
            townScene.Draw();                       //背景は前のシーンを描画
            renderer.End();
            blurEffect.ReleaseRenderTarget();
            blurEffect.Draw(renderer);


            renderer.Begin();
            renderer.DrawString("B key back to Town", Vector2.Zero, new Vector2(1, 1), Color.Black);
            stores.DrawEquip();
            renderer.End();


        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            blurRate = 0.0f;

            stores = new Store(gameManager, gameDevice);
            stores.Initialize();
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

        public void Update(GameTime gameTime)
        {
            if (input.GetKeyTrigger(Keys.B))
                endFlag = true;

            UpdateBlurRate();
            blurEffect.Update(blurRate);
        }

        private void UpdateBlurRate()
        {
            if (endFlag)
            {
                blurRate -= 0.03f;
                return;
            }

            if (blurRate < 0.8f)
                blurRate += 0.03f;
        }
    }
}
