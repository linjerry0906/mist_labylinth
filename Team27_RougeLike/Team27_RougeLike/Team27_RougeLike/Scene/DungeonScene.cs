﻿//--------------------------------------------------------------------------------------------------
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
        private bool isChanged;                 //シーンが完全に切り替えたフラグ
        private SceneType nextScene;            //次のシーン

        private DungeonMap map;                 //マップ
        private MapItemManager mapItemManager;  //マップ内に落ちているアイテムの管理者
        
        private float angle = 0;                //カメラ回転角度

        private DungeonPopUI popUI;             //Popメッセージ

        public DungeonScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
            stageManager = gameManager.StageManager;
            characterManager = new CharacterManager(gameDevice);
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
            if (!isChanged)                     //シーンが切り替えたら別のシーンに任せる
            {
                renderer.Begin();
            }

            stageManager.DrawLimitTime();       //残り時間を表示
            popUI.Draw();                       //Popメッセージの描画

            if (!isChanged)                     //シーンが切り替えたら別のシーンに任せる
            {
                renderer.End();
            }
        }

        public void Initialize(SceneType lastScene)
        {
            endFlag = false;                        //終了フラグ初期化
            isChanged = false;                      //シーンの切り替えフラグ初期化
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
            mapItemManager = new MapItemManager(gameManager.ItemManager, gameDevice);
            mapItemManager.Initialize();
            int itemAmount = stageManager.CurrentFloor() / 10 + stageManager.CurrentFloor() % 5;    //初期落ちているアイテムの数
            itemAmount = gameDevice.Random.Next(0, itemAmount);
            for (int i = 0; i < itemAmount; i++)
            {
                //if (gameDevice.Random.Next(0, 101) < 70)            //70%が使用アイテム
                //{
                //    mapItemManager.AddItem(map.RandomSpace());
                //    continue;
                //}
                mapItemManager.AddEquip(map.RandomSpace());         //30％が装備
            }
            #endregion

            characterManager.Initialize(new Vector3(
                map.EntryPoint.X * MapDef.TILE_SIZE,
                MapDef.TILE_SIZE,
                map.EntryPoint.Y * MapDef.TILE_SIZE));

            #region カメラ初期化
            angle = 0;
            gameDevice.MainProjector.Initialize(characterManager.GetPlayer().Position);       //カメラを初期化
            #endregion

            popUI = new DungeonPopUI(gameManager, gameDevice);
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
            isChanged = true;                       //完全に切り替えたらTrue
            if (nextScene == SceneType.Pause)       //次のシーンがPauseだったら以下のものShutdownしない
                return;

            map.Clear();                            //マップ解放
            map = null;
            gameManager.ReleaseMap();

            mapItemManager.Initialize();            //Item解放
            mapItemManager = null;

            popUI = null;
        }

        public void Update(GameTime gameTime)
        {
            if (endFlag)
                return;

            popUI.Update();
            if (popUI.IsPop())                   //メッセージ表示中は以下Updateしない
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
            mapItemManager.ItemCollision(characterManager.GetPlayer(), popUI);
            

            stageManager.Update();              //時間やFog処理の更新


            CheckEnd();                         //プレイ終了をチェック
        }

        /// <summary>
        /// カメラの回転
        /// </summary>
        private void RotateCamera()
        {
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
        }

        /// <summary>
        /// シーンを変えるかのチェック
        /// </summary>
        private void CheckEnd()
        {
            if (gameDevice.InputState.GetKeyTrigger(Keys.P))            //Pause機能
            {
                endFlag = true;
                nextScene = SceneType.Pause;
            }

            if (stageManager.IsTime())                                  //時間になったら村に戻される
            {
                endFlag = true;
                nextScene = SceneType.Town;
                return;
            }

            if(map.WorldToMap(characterManager.GetPlayer().Position) == map.EndPoint)         //階段にたどり着いた場合
            {
                endFlag = true;                     //ToDo：次の階層へ行くかどうかを聞く
                nextScene = SceneType.LoadMap;
                stageManager.NextFloor();
                if (stageManager.IsBoss())
                    nextScene = SceneType.Boss;
                return;
            }
        }
    }
}
