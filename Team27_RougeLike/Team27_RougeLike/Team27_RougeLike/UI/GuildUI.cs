//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：GuildのUIクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Def;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.UI
{
    class GuildUI
    {
        private GameDevice gameDevice;
        private Renderer renderer;
        private GameManager gameManager;
        private Window backLayer;

        public GuildUI(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            renderer = gameDevice.Renderer;
            this.gameManager = gameManager;

            Initialize();
        }

        private void Initialize()
        {
            backLayer = new Window(
                gameDevice,
                new Vector2(40, 40),
                new Vector2(WindowDef.WINDOW_WIDTH - 80, WindowDef.WINDOW_HEIGHT - 80));
            backLayer.Initialize();
            backLayer.Switch(true);
        }

        public void Update()
        {
            backLayer.Update();
        }

        public void Draw()
        {
            backLayer.Draw();
        }

        public bool IsEnd()
        {
            return backLayer.IsEnd();
        }

        public void SwitchOff()
        {
            backLayer.Switch(false);
        }
    }
}
