//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29
// 内容  ：タイトルシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;

namespace Team27_RougeLike.Scene
{
    class Title : IScene
    {
        private GameDevice gameDevice;
        private InputState input;
        private Renderer renderer;
        private Sound sound;
        private Motion motion;
        private bool endFlag;

        public Title(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            sound = gameDevice.Sound;
            motion = new Motion();
        }
        public void Draw()
        {
            renderer.Begin();
            renderer.DrawTexture("title", Vector2.Zero);
            renderer.DrawTexture("titlelogo", new Vector2(150, 150), new Vector2(0.6f, 0.6f));
            renderer.DrawTexture("pressspace", new Vector2(580, 500), motion.DrawingRange());
            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            for (int i = 0; i < 30; i++)
                motion.Add(i, new Rectangle(170 * 7, 0, 170, 70));
            for (int i = 0; i < 7; i++)
                motion.Add(30 + i, new Rectangle(170 * i, 0, 170, 70));
            motion.Initialize(new Range(0, 36), new Timer(0.05f));

        }

        public bool IsEnd()
        {
            return endFlag;
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
            motion.Update(gameTime);

            if (input.IsLeftClick())
            {
                sound.PlaySE("press");
                endFlag = true;
            }
        }
    }
}
