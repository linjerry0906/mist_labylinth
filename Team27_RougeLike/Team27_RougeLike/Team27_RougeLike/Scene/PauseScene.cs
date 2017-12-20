//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.06
// 内容  ：Pauseシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Effects;
using Team27_RougeLike.UI;

namespace Team27_RougeLike.Scene
{
    class PauseScene : IScene
    {
        private GameDevice gameDevice;          //ゲームデバイス
        private InputState input;
        private Renderer renderer;
        private GameManager gameManager;        //Player現在の情報を取得用

        private BlurEffect blurEffect;
        private float blurRate;

        private IScene dungeonScene;            //ダンジョンシーン
        private IScene bossScene;               //ボスシーン
        private IScene townScene;               //村シーン

        private SceneType nextScene;            //次のシーン
        private bool endFlag;                   //終了フラグ

        private PauseUI ui;

        public PauseScene(IScene dungeon, IScene boss, IScene town, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            this.gameManager = gameManager;
            blurEffect = renderer.EffectManager.GetBlurEffect();

            this.dungeonScene = dungeon;
            this.bossScene = boss;
            this.townScene = town;
        }

        public void Draw()
        {
            blurEffect.WriteRenderTarget(renderer.FogManager.CurrentColor());
            switch (nextScene)               //背景は前のシーンを描画
            {
                case SceneType.Dungeon:
                    dungeonScene.Draw();
                    break;
                case SceneType.Town:
                    townScene.Draw();
                    break;
                case SceneType.Boss:
                    bossScene.Draw();
                    break;
            }
            blurEffect.ReleaseRenderTarget();
            blurEffect.Draw(renderer);


            renderer.Begin();

            ui.Draw();

            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            nextScene = scene;

            blurRate = 0.0f;

            ui = new PauseUI(gameManager, gameDevice);
        }

        public bool IsEnd()
        {
            return endFlag && blurRate <= 0;
        }

        public SceneType Next()
        {
            return nextScene;
        }

        public void Shutdown()
        {
            ui = null;
        }

        public void Update(GameTime gameTime)
        {
            if (input.GetKeyTrigger(Keys.P))
                ui.SwitchOff();

            ui.Update();

            UpdateBlurRate();

            CheckSceneEnd();
        }

        private void UpdateBlurRate()
        {
            if (ui.IsEnd())
            {
                blurRate -= 0.05f;
                blurEffect.Update(blurRate);
                return;
            }

            if (blurRate < 0.6f)
                blurRate += 0.05f;

            blurEffect.Update(blurRate);
        }

        private void CheckSceneEnd()
        {
            if (blurRate <= 0.0f)
                endFlag = true;
        }
    }
}
