//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.19
// 内容  ：Logoシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Utility;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class LogoScene : IScene
    {
        private Renderer renderer;
        private bool endFlag;
        Timer timer;

        public LogoScene(GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
        }
        public void Draw()
        {
            renderer.Begin();

            renderer.DrawTexture("fade",
                Vector2.Zero, new Vector2(Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT));

            float alpha = 1.0f - Math.Abs(timer.Rate() * 2 - 1);

            renderer.DrawTexture("NEEC1280black", Vector2.Zero, alpha);

            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            timer = new Timer(4);
            timer.Initialize();
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return SceneType.LoadTown;
        }

        public void ShutDown()
        {
            timer = null;
        }

        public void Update(GameTime gameTime)
        {
            timer.Update();
            if (timer.IsTime())
            {
                endFlag = true;
            }
        }
    }
}
