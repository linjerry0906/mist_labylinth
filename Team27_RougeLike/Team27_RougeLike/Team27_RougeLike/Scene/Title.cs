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
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class Title : IScene
    {
        private GameDevice gameDevice;
        private InputState input;

        private bool endFlag;

        public Title(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
        }
        public void Draw()
        {
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
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
        }
    }
}
