﻿//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29
// 内容  ：ダンジョンをロードするシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Map;

namespace Team27_RougeLike.Scene
{
    class LoadMap : IScene
    {
        private GameDevice gameDevice;          //デバイス系管理者
        private Renderer renderer;
        private GameManager gameManager;        //ゲーム情報管理者
        private StageManager stageManager;      //ステージ管理者
        private StageItemEnemyLoader stageInfoLoader;        //Stage情報をロードするクラス

        private bool endFlag;                   //シーンの終わるフラグ

        private MapGenerator mapGenerator;      //マップ生成者

        public LoadMap(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;

            renderer = gameDevice.Renderer;
            stageManager = gameManager.StageManager;
        }
        public void Draw()
        {
            renderer.Begin();
            Vector2 screenSize = new Vector2(Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT);
            renderer.DrawTexture("fade", Vector2.Zero, screenSize);
            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            mapGenerator = new MapGenerator(stageManager.StageSize(), gameDevice);
            stageInfoLoader = new StageItemEnemyLoader();
            stageInfoLoader.Initialize();
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return SceneType.Dungeon;
        }

        public void ShutDown()
        {
            mapGenerator = null;            //マップ生成者のメモリーを解放
        }

        public void Update(GameTime gameTime)
        {
            if (!mapGenerator.IsEnd())               //生成が終わってなかったら生成し続ける
            {
                mapGenerator.Update();
                return;
            }
            if (!stageInfoLoader.IsItemLoad())       //Item読み込む
            {
                stageInfoLoader.LoadFloorItem(
                    gameManager.ItemManager, 
                    gameManager.StageNum, 
                    stageManager.CurrentFloor());
                return;
            }
            if (!stageInfoLoader.IsEnemyLoad())      //敵の配置を読み込む
            {
                stageInfoLoader.LoadFloorEnemy(
                    gameManager.EnemySetting,
                    gameManager.StageNum,
                    stageManager.CurrentFloor());
                return;
            }


            gameManager.GenerateMapInstance(mapGenerator.MapChip);      //実体を生成し、シーンを終わらせる
            endFlag = true;
        }
    }
}
