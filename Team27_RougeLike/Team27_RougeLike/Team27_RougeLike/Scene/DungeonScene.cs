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
        private StageManager stageManager;

        private bool endFlag;
        private SceneType nextScene;

        private DungeonMap map;
        private Player player;          //テスト

        private float angle = 0;

        public DungeonScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            stageManager = gameManager.StageManager;
        }

        public void Draw()
        {
            map.Draw();
            player.Draw();

            map.DrawMiniMap();
            stageManager.DrawLimitTime();
        }

        public void Initialize(SceneType lastScene)
        {
            endFlag = false;
            nextScene = SceneType.LoadMap;

            if (lastScene == SceneType.Pause)       //Pauseから来た場合は初期化しない
                return;

            angle = 0;

            map = gameManager.GetDungeonMap();
            if(map == null)                         //エラー対策　マップが正常に生成されてなかったらLoadingに戻る
            {
                nextScene = SceneType.LoadMap;
                endFlag = true;
                return;
            }

            map.Initialize();
            player = new Player(
            new Vector3(
                map.EntryPoint.X * MapDef.TILE_SIZE,
                MapDef.TILE_SIZE,
                map.EntryPoint.Y * MapDef.TILE_SIZE),
            gameDevice);

            gameDevice.MainProjector.Initialize(player.Position);
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
        }

        public void Update(GameTime gameTime)
        {
            if (endFlag)
                return;

            //Rotate Test
            if (gameDevice.InputState.GetKeyState(Keys.Q))
            {
                angle += 1;
                angle = (angle > 360) ? angle - 360 : angle;
            }
            else if (gameDevice.InputState.GetKeyState(Keys.E))
            {
                angle -= 1;
                angle = (angle < 0) ? angle + 360 : angle;
            }
            gameDevice.MainProjector.Rotate(angle);

            //Chara処理
            player.Update(gameTime);
            map.MapCollision(gameDevice.Renderer.MainProjector);
            map.FocusCenter(player.Position);
            map.Update();
            map.MapCollision(player);

            stageManager.Update();              //時間やFog処理の更新


            //Pause機能
            if (gameDevice.InputState.GetKeyTrigger(Keys.P))
            {
                endFlag = true;
                nextScene = SceneType.Pause;
            }

            CheckEnd();         //プレイ終了をチェック
        }

        private void CheckEnd()
        {
            if (stageManager.IsTime())              //時間になったら村に戻される
            {
                endFlag = true;
                nextScene = SceneType.Town;
                return;
            }

            if(map.WorldToMap(player.Position) == map.EndPoint)
            {
                endFlag = true;                     //ToDo：次の階層へ行くかどうかを聞く
                nextScene = SceneType.LoadMap;
                stageManager.NextFloor();
                return;
            }
        }
    }
}
