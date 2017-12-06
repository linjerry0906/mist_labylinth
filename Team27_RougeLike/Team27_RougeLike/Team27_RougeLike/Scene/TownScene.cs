using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class TownScene : IScene
    {
        private GameDevice gameDevice;
        private InputState input;
        private Renderer renderer;
        private GameManager gameManager;

        private bool endFlag;

        private SceneType nextScene;

        public TownScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            this.gameManager = gameManager;
        }

        public void Draw()
        {
            renderer.Begin();
            renderer.DrawString("Town", Vector2.Zero, new Vector2(1, 1), new Color(1, 1, 1));
            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            nextScene = SceneType.LoadMap;
            endFlag = false;

            if (scene == SceneType.Pause)
                return;

            gameManager.InitStage(5 * 60, 1, 20);
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
        }

        public void Update(GameTime gameTime)
        {
            if (input.GetKeyTrigger(Keys.D))
            {
                nextScene = SceneType.LoadMap;
                endFlag = true;
                return;
            }

            if (input.GetKeyTrigger(Keys.P))
            {
                nextScene = SceneType.Pause;
                endFlag = true;
                return;
            }

        }
    }
}
