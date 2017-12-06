using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class PauseScene : IScene
    {
        private GameDevice gameDevice;          //ゲームデバイス
        private InputState input;
        private Renderer renderer;              //描画　ToDo：Blur
        private GameManager gameManager;        //Player現在の情報を取得用

        private IScene dungeonScene;            //ダンジョンシーン
        private IScene bossScene;               //ボスシーン
        private IScene townScene;               //村シーン

        private SceneType nextScene;            //次のシーン
        private bool endFlag;                   //終了フラグ

        public PauseScene(IScene dungeon, IScene boss, IScene town, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            this.gameManager = gameManager;

            this.dungeonScene = dungeon;
            this.bossScene = boss;
            this.townScene = town;
        }

        public void Draw()
        {
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
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            nextScene = scene;
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
            if (input.GetKeyTrigger(Keys.P))
                endFlag = true;
        }
    }
}
