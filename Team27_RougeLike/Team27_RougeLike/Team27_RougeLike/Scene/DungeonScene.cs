//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29
// 内容  ：ダンジョンシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class DungeonScene : IScene
    {
        private GameDevice gameDevice;

        private bool endFlag;
        private SceneType nextScene;

        public DungeonScene(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
        }
        public void Draw()
        {
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            nextScene = SceneType.Town;
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

        public void Update()
        {
        }
    }
}
