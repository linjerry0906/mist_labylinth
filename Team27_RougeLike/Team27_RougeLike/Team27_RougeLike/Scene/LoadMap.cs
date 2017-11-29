//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29
// 内容  ：ダンジョンをロードするシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class LoadMap : IScene
    {
        private GameDevice gameDevice;

        private bool endFlag;

        public LoadMap(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
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
            return SceneType.Dungeon;
        }

        public void Shutdown()
        {
        }

        public void Update()
        {
        }
    }
}
