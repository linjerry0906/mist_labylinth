//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.07
// 内容  ：Bossシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class BossScene : IScene
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private Renderer renderer;
        private InputState input;

        private bool endFlag;
        private bool isChanged;
        private SceneType nextScene;

        public BossScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;

            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
        }

        public void Draw()
        {
            DrawUI();
        }

        public void DrawUI()
        {
            if (!isChanged)
            {
                renderer.Begin();
            }

            renderer.DrawString("Boss Scene\n P Key:Pause\n T Key: Back to Town", Vector2.Zero, new Vector2(1, 1), new Color(1, 1, 1));

            if (!isChanged)
            {
                renderer.End();
            }
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            isChanged = false;
            nextScene = SceneType.Town;

            if (scene == SceneType.Pause)
                return;
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return nextScene;
        }

        public void Shutdown()
        {
            isChanged = true;
        }

        public void Update(GameTime gameTime)
        {
            //Debug 村シーンへ
            if (input.GetKeyTrigger(Keys.T))
            {
                endFlag = true;
                return;
            }

            //Pause機能
            if(input.GetKeyTrigger(Keys.P))
            {
                nextScene = SceneType.Pause;
                endFlag = true;
                return;
            }
        }
    }
}
