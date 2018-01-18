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
using Team27_RougeLike.UI;

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

        private Button DButton, SButton, DeButton, UButton, GButton;

        public TownScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            this.gameManager = gameManager;
            DButton = new Button("Dungeonbutton", new Vector2(10, 30), 256, 52, input);
            SButton = new Button("Shopbutton", new Vector2(10, 115), 256, 52, input);
            DeButton = new Button("Depotbutton", new Vector2(10, 200), 256, 52, input);
            UButton = new Button("Upgradebutton", new Vector2(10, 285), 256, 52, input);
            GButton = new Button("Guildtbutton", new Vector2(10, 370), 256, 52, input);
        }

        public void Draw()
        {
            DrawUI();
        }

        private void DrawUI()
        {
            renderer.Begin();

            renderer.DrawTexture("town", Vector2.Zero);

            renderer.DrawString("Town\nPress D key to Dungeon", Vector2.Zero, new Vector2(1, 1), new Color(1, 1, 1));
            DButton.Draw(renderer);
            SButton.Draw(renderer);
            DeButton.Draw(renderer);
            UButton.Draw(renderer);
            GButton.Draw(renderer);
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
            DButton.Update();
            SButton.Update();
            DeButton.Update();
            UButton.Update();
            GButton.Update();
            CheckIsEnd();
        }


        private void CheckIsEnd()
        {
            if (/*input.GetKeyTrigger(Keys.D)*/DButton.IsClick(input))
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

            if (/*input.GetKeyTrigger(Keys.S)*/SButton.IsClick(input))
            {
                nextScene = SceneType.ItemShop;
                endFlag = true;
                return;
            }

            if (/*input.GetKeyTrigger(Keys.A)*/DeButton.IsClick(input))
            {
                nextScene = SceneType.Depot;
                endFlag = true;
                return;
            }

            if (/*input.GetKeyTrigger(Keys.U)*/UButton.IsClick(input))
            {
                nextScene = SceneType.UpgradeStore;
                endFlag = true;
                return;
            }
        }
    }
}
