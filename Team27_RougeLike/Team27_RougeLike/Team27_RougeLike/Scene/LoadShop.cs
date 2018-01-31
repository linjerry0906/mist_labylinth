//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.31
// 内容  ：ショップものを読み込む
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
    class LoadShop : IScene
    {
        private GameDevice gameDevice;
        private Renderer renderer;
        private GameManager gameManager;

        private IScene townScene;

        private TownInfoLoader townInfoLoader;

        private bool endFlag;

        public LoadShop(IScene town, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            townScene = town;
            renderer = gameDevice.Renderer;
        }
        public void Draw()
        {
            townScene.Draw();
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
            return SceneType.ItemShop;
        }

        public void ShutDown()
        {
            townInfoLoader = null;
        }

        public void Update(GameTime gameTime)
        {
            if (!townInfoLoader.IsItemLoad())
            {
                //攻略状況により、ショップのアイテムも変わる
                townInfoLoader.LoadStoreItem(
                    gameManager.ItemManager,
                    gameManager.DungeonProcess);
                return;
            }

            endFlag = true;
        }
    }
}
