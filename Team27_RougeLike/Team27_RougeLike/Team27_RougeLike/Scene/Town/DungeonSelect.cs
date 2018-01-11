//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.10
// 内容  ：ダンジョン選択シーン
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
using Team27_RougeLike.Scene.Town;

namespace Team27_RougeLike.Scene
{
    class DungeonSelect : IScene
    {
        private GameDevice gameDevice;          //ゲームデバイス
        private InputState input;
        private Renderer renderer;
        private GameManager gameManager;        //Player現在の情報を取得用

        private BlurEffect blurEffect;
        private float blurRate;

        private StageInfoLoader stageInfoLoader;

        private IScene townScene;               //村シーン

        private SceneType nextScene;            //次のシーン
        private bool endFlag;                   //終了フラグ

        private DungeonSelectUI ui;

        public DungeonSelect(IScene town, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            this.gameManager = gameManager;
            blurEffect = renderer.EffectManager.GetBlurEffect();

            this.townScene = town;
        }

        public void Draw()
        {
            blurEffect.WriteRenderTarget(renderer.FogManager.CurrentColor());
            townScene.Draw();           //前のシーンを描画

            renderer.Begin();
            ui.DrawBackGround();        //ダンジョンイメージ
            renderer.End();

            blurEffect.ReleaseRenderTarget();
            blurEffect.Draw(renderer);


            renderer.Begin();

            ui.Draw();

            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            nextScene = SceneType.Town;

            blurRate = 0.0f;

            stageInfoLoader = new StageInfoLoader();
            stageInfoLoader.Initialize();
            stageInfoLoader.LoadStageInfo();
            if (!stageInfoLoader.IsLoad())           //エラー対策
                endFlag = true;

            ui = new DungeonSelectUI(gameManager, gameDevice);
            ui.SetStageInfo(stageInfoLoader.Stages());
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
            if (endFlag)
                return;

            ui.Update();

            UpdateBlurRate();

            CheckSceneEnd();
        }

        /// <summary>
        /// ぼかし更新
        /// </summary>
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

            if (ui.IsEnd())
            {
                if (ui.Next() == DungeonSelectUI.DungeonSelectButtonEnum.ダンジョン)
                {
                    nextScene = SceneType.LoadMap;
                    ui.InitStage();
                }
            }
        }
    }
}
