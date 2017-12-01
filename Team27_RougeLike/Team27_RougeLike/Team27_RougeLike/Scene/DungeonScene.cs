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
using Microsoft.Xna.Framework.Input;
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

        private float angle = 0;

        private float fogNear;

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

            gameDevice.Renderer.StartFog();
            fogNear = 200;

            angle = 0;

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

                gameDevice.MainProjector.Initialize(player.Position);
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
            if (nextScene == SceneType.Pause)       //次のシーンがPauseだったらShutdownしない
                return;

            map.Clear();
            map = null;
            gameManager.ReleaseMap();
            gameDevice.Renderer.EndFog();
        }

        public void Update(GameTime gameTime)
        {
            //Rotate Test
            if (gameDevice.InputState.GetKeyState(Keys.Q))
            {
                angle += 1;
                angle = (angle > 360) ? angle - 360 : angle;
                gameDevice.MainProjector.Rotate(angle);
            }
            else if (gameDevice.InputState.GetKeyState(Keys.E))
            {
                angle -= 1;
                angle = (angle < 0) ? angle + 360 : angle;
                gameDevice.MainProjector.Rotate(angle);
            }
            gameDevice.MainProjector.Rotate(angle);

            player.Update(gameTime);
            map.MapCollision(gameDevice.Renderer.MainProjector);
            map.FocusCenter(player.Position);
            map.Update();
            map.MapCollision(player);


            


            //Reload Map
            if (gameDevice.InputState.GetKeyTrigger(Keys.L))
            {
                endFlag = true;
                nextScene = SceneType.LoadMap;
            }



            //Fog
            fogNear = (fogNear > -50) ?  fogNear-0.1f : fogNear;
            gameDevice.Renderer.FogManager.SetNear(fogNear);
            gameDevice.Renderer.FogManager.SetFar(fogNear + 100);
            gameDevice.Renderer.StartFog();
            if (gameDevice.InputState.GetKeyTrigger(Keys.F))
            {
                if (gameDevice.Renderer.FogManager.IsActive())
                {
                    gameDevice.Renderer.EndFog();
                }
                else
                {
                    gameDevice.Renderer.StartFog();
                }
            }
        }
    }
}
