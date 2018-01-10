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
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.UI;

namespace Team27_RougeLike.Scene
{
    class DungeonScene : IScene
    {
        private GameDevice gameDevice;          //Device系をまとめたクラス
        private Renderer renderer;              //レンダラー
        private GameManager gameManager;        //シーンの間に情報を渡す機能のクラス
        private StageManager stageManager;      //ステージ管理者
        private CharacterManager characterManager;
        private bool endFlag;                   //終了フラグ
        private SceneType nextScene;            //次のシーン

        private DungeonMap map;                 //マップ
        private MapItemManager mapItemManager;  //マップ内に落ちているアイテムの管理者
        
        private float angle = 0;                //カメラ回転角度

        private DungeonUI ui;             //Popメッセージ

        public DungeonScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
            stageManager = gameManager.StageManager;
        }

        public void Draw()
        {
            map.Draw();                 //Mapの描画
            mapItemManager.Draw();      //アイテムの描画
            characterManager.Draw();
            map.DrawMiniMap();          //MiniMapの描画

            DrawUI();                   //UIを描画
        }

        /// <summary>
        /// UIの描画
        /// </summary>
        private void DrawUI()
        {
            renderer.Begin();

            stageManager.DrawLimitTime();       //残り時間を表示
            stageManager.DrawDungeonInfo();
            ui.Draw();                          //UIの描画

            renderer.End();
        }

        public void Initialize(SceneType lastScene)
        {
            endFlag = false;                        //終了フラグ初期化
            nextScene = SceneType.LoadMap;

            if (lastScene == SceneType.Pause)       //Pauseから来た場合は以下のもの初期化しない
                return;

            #region Map初期化
            map = gameManager.GetDungeonMap();      //生成したマップを取得
            if(map == null)                         //エラー対策　マップが正常に生成されてなかったらLoadingに戻る
            {
                nextScene = SceneType.LoadMap;
                endFlag = true;
                return;
            }

            map.Initialize();                       //マップを初期化
            #endregion

            #region Item初期化
            mapItemManager = new MapItemManager(gameManager, gameDevice);
            mapItemManager.Initialize();
            int itemAmount = stageManager.CurrentFloor() / 10 + stageManager.CurrentFloor() % 5;    //初期落ちているアイテムの数
            itemAmount = gameDevice.Random.Next(0, itemAmount);
            for (int i = 0; i < itemAmount + 40; i++)
            {
                Vector3 randomSpace = map.RandomSpace();
                if (randomSpace == Vector3.Zero)                    //Error対策
                    break;

                if (gameDevice.Random.Next(0, 101) < 70)            //70%が使用アイテム
                {
                    mapItemManager.AddItem(randomSpace);
                    continue;
                }
                mapItemManager.AddEquip(randomSpace);               //30％が装備
            }
            #endregion

            characterManager = new CharacterManager(gameDevice);
            characterManager.Initialize(new Vector3(
                map.EntryPoint.X * MapDef.TILE_SIZE,
                MapDef.TILE_SIZE,
                map.EntryPoint.Y * MapDef.TILE_SIZE));

            #region カメラ初期化
            angle = 0;
            gameDevice.MainProjector.Initialize(characterManager.GetPlayer().Position);       //カメラを初期化
            #endregion

            ui = new DungeonUI(gameManager, gameDevice);
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
            if (nextScene == SceneType.Pause)       //次のシーンがPauseだったら以下のものShutdownしない
                return;

            map.Clear();                            //マップ解放
            map = null;
            gameManager.ReleaseMap();

            mapItemManager.Initialize();            //Item解放
            mapItemManager = null;

            ui = null;
        }

        public void Update(GameTime gameTime)
        {
            if (endFlag)
                return;

            ui.Update();
            if (ui.IsPop())                   //メッセージ表示中は以下Updateしない
                return;

            RotateCamera();

            //Chara処理
            characterManager.Update(gameTime);

            //マップ処理
            map.MapCollision(gameDevice.Renderer.MainProjector);
            map.FocusCenter(characterManager.GetPlayer().Position);
            map.Update();
            map.MapCollision(characterManager.GetPlayer());
            map.MapCollision(characterManager.GetCharacters());

            //アイテム処理
            mapItemManager.ItemCollision(characterManager.GetPlayer(), ui);
            

            stageManager.Update();              //時間やFog処理の更新

            //Camera Shake仮実装 ToDo:Class化
            if (gameDevice.InputState.IsLeftClick())
            {
                Vector3 offset = new Vector3(
                    gameDevice.Random.Next(-10, 10) / 50.0f,
                    gameDevice.Random.Next(-10, 10) / 50.0f,
                    gameDevice.Random.Next(-10, 10) / 50.0f);
                gameDevice.MainProjector.Collision.Position += offset;
            }


            CheckEnd();                         //プレイ終了をチェック
        }

        /// <summary>
        /// カメラの回転
        /// </summary>
        private void RotateCamera()
        {
            if (gameDevice.InputState.GetKeyState(Keys.Q))
            {
                angle += 1.5f;
                angle = (angle > 360) ? angle - 360 : angle;
            }
            else if (gameDevice.InputState.GetKeyState(Keys.E))
            {
                angle -= 1.5f;
                angle = (angle < 0) ? angle + 360 : angle;
            }
            gameDevice.MainProjector.Rotate(angle);
        }

        /// <summary>
        /// シーンを変えるかのチェック
        /// </summary>
        private void CheckEnd()
        {
            //Pause機能
            if (gameDevice.InputState.GetKeyTrigger(Keys.P))
            {
                endFlag = true;
                nextScene = SceneType.Pause;
            }

            //死んだ時
            //gameManager.PlayerItem.RemoveAll();
            //gameManager.Save();

            //時間になったら村に戻される
            if (stageManager.IsTime())
            {
                gameManager.PlayerItem.RemoveTempItem();
                endFlag = true;
                nextScene = SceneType.Town;
                gameManager.Save();
                return;
            }

            //階段にたどり着いた場合
            if (map.WorldToMap(characterManager.GetPlayer().Position) == map.EndPoint)
            {
                //ヒント文字を出す
                ui.HintUI.Switch(true);
                ui.HintUI.SetMessage("Press Space to go next");
                if (!ui.HintUI.IsPush(Keys.Space))
                    return;

                //次へ行く処理
                endFlag = true;
                nextScene = SceneType.LoadMap;
                gameManager.UpdateDungeonProcess();     //攻略状況更新
                stageManager.NextFloor();
                if (stageManager.IsBoss())
                    nextScene = SceneType.Boss;
                return;
            }
        }
    }
}