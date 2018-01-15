//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.06
// 内容  ：村シーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene.Town;

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
            DrawUI();
        }

        private void DrawUI()
        {
            renderer.Begin();

            renderer.DrawString("Town\nPress D key to Dungeon", Vector2.Zero, new Vector2(1, 1), new Color(1, 1, 1));
            renderer.DrawString("Press S key to ItemShop", new Vector2(0, 100), new Vector2(1, 1), new Color(1, 1, 1));
            renderer.DrawString("Press A key to Depot", new Vector2(0, 200), new Vector2(1, 1), new Color(1, 1, 1));
            renderer.DrawString("Press U key to Depot", new Vector2(0, 300), new Vector2(1, 1), new Color(1, 1, 1));

            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            nextScene = SceneType.DungeonSelect;
            endFlag = false;

            gameManager.Save();
            if (scene == SceneType.Pause ||
                scene == SceneType.ItemShop ||
                scene == SceneType.DungeonSelect)
                return;

            gameManager.PlayerItem.RemoveTemp();       //一時的なアイテムを削除
            gameManager.PlayerInfo.Initialize();       //レベルなどの初期化処理
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return nextScene;
        }

        public void ShutDown()
        {
        }

        public void Update(GameTime gameTime)
        {

            CheckIsEnd();
        }


        private void CheckIsEnd()
        {
            if (input.GetKeyTrigger(Keys.D))
            {
                nextScene = SceneType.DungeonSelect;
                endFlag = true;
                return;
            }

            if (input.GetKeyTrigger(Keys.P))
            {
                nextScene = SceneType.Pause;
                endFlag = true;
                return;
            }

            if (input.GetKeyTrigger(Keys.S))
            {
                nextScene = SceneType.ItemShop;
                endFlag = true;
                return;
            }

            if (input.GetKeyTrigger(Keys.A))
            {
                nextScene = SceneType.Depot;
                endFlag = true;
                return;
            }

            if (input.GetKeyTrigger(Keys.U))
            {
                nextScene = SceneType.UpgradeStore;
                endFlag = true;
                return;
            }
        }
    }
}
