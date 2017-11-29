//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29
// 内容  ：ダンジョンシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Map;
using Team27_RougeLike.Object.Actor;

namespace Team27_RougeLike.Scene
{
    class DungeonScene : IScene
    {
        private GameDevice gameDevice;
        private GameManager gameManager;

        private bool endFlag;
        private SceneType nextScene;

        private DungeonMap map;
        private Player player;          //テスト

        public DungeonScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
        }
        public void Draw()
        {
            map.Draw();
            player.Draw();
        }

        public void Initialize(SceneType lastScene)
        {
            endFlag = false;
            nextScene = SceneType.LoadMap;

            if (lastScene == SceneType.Pause)
                return;

            map = gameManager.GetDungeonMap();
            if(map == null)                         //エラー対策　マップが正常に生成されてなかったらLoadingに戻る
            {
                nextScene = SceneType.LoadMap;
                endFlag = true;
            }
            else
            {
                map.Initialize();
                player = new Player(
                new Vector3(
                    map.EntryPoint.X * MapDef.TILE_SIZE,
                    MapDef.TILE_SIZE,
                    map.EntryPoint.Y * MapDef.TILE_SIZE),
                gameDevice);
            }
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
            map.Clear();
            map = null;
            gameManager.ReleaseMap();
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            map.FocusCenter(player.Position);
            map.Update();
            map.MapCollision(player);

            if (gameDevice.InputState.GetKeyTrigger(Microsoft.Xna.Framework.Input.Keys.L))
            {
                endFlag = true;
                nextScene = SceneType.LoadMap;
            }
        }
    }
}
