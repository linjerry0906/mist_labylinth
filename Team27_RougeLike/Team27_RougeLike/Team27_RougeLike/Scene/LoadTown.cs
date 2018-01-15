//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.13
// 内容  ：村シーンのものを読み込む
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene.Town;

namespace Team27_RougeLike.Scene
{
    class LoadTown : IScene
    {
        private GameDevice gameDevice;
        private Renderer renderer;
        private GameManager gameManager;

        private TownInfoLoader townInfoLoader;

        private bool endFlag;

        public LoadTown(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
        }

        public void Draw()
        {
            //ToDo：Loading画面
            renderer.Begin();
            Vector2 screenSize = new Vector2(Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT);
            renderer.DrawTexture("fade", Vector2.Zero, screenSize);
            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            townInfoLoader = new TownInfoLoader();
            townInfoLoader.Initialize();
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return SceneType.Town;
        }

        public void Shutdown()
        {
            townInfoLoader = null;
        }

        public void Update(GameTime gameTime)
        {
            if (!townInfoLoader.IsItemLoad())
            {
                townInfoLoader.LoadStoreItem(gameManager.ItemManager, 1);       //Todo:到達階層を読む
                return;
            }

            endFlag = true;
        }
    }
}
