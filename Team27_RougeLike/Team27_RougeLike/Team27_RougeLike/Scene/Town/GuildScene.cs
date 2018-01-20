//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：Guild Scene
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
    class GuildScene : IScene
    {
        private GameDevice gameDevice;          //ゲームデバイス
        private InputState input;
        private Renderer renderer;
        private GameManager gameManager;        //Player現在の情報を取得用

        private BlurEffect blurEffect;
        private float blurRate;

        private IScene townScene;               //村シーン

        private bool endFlag;                   //終了フラグ

        private GuildUI guildUI;                //GuildUI

        public GuildScene(IScene town, GameManager gameManager, GameDevice gameDevice)
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
            townScene.Draw();                       //背景は前のシーンを描画
            renderer.Begin();
            guildUI.DrawBackground();
            renderer.End();
            blurEffect.ReleaseRenderTarget();
            blurEffect.Draw(renderer);

            renderer.Begin();
            DrawUI();
            renderer.End();
        }

        private void DrawUI()
        {
            guildUI.Draw();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            blurRate = 0.0f;

            guildUI = new GuildUI(gameManager, gameDevice);
        }

        public bool IsEnd()
        {
            return endFlag && blurRate <= 0;
        }

        public SceneType Next()
        {
            return SceneType.Town;
        }

        public void ShutDown()
        {
        }

        public void Update(GameTime gameTime)
        {
            guildUI.Update();

            UpdateBlurRate();
            blurEffect.Update(blurRate);

            CheckEnd();
        }

        private void CheckEnd()
        {
            if (guildUI.IsEnd())
            {
                endFlag = true;
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
    }
}
