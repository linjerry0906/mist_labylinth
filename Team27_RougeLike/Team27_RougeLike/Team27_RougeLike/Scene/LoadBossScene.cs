//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.11
// 内容  ：ボスシーンをロードするシーン
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
    class LoadBossScene : IScene
    {
        private GameDevice gameDevice;          //デバイス系管理者
        private Renderer renderer;
        private GameManager gameManager;        //ゲーム情報管理者
        private StageManager stageManager;      //ステージ管理者
        private StageInfoLoader stageInfoLoader;        //Stage情報をロードするクラス

        private bool endFlag;                   //シーンの終わるフラグ

        private MapGenerator mapGenerator;      //マップ生成者

        public LoadBossScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;

            renderer = gameDevice.Renderer;
            stageManager = gameManager.StageManager;
        }
        public void Draw()
        {
            //ToDo：Loading画面
            renderer.Begin();
            renderer.DrawTexture("test", Vector2.Zero);
            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            mapGenerator = new MapGenerator(stageManager.StageSize(), gameDevice);
            stageInfoLoader = new StageInfoLoader();
            stageInfoLoader.Initialize();
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return SceneType.Boss;
        }

        public void Shutdown()
        {
            mapGenerator = null;            //マップ生成者のメモリーを解放
        }

        public void Update(GameTime gameTime)
        {
            if (!mapGenerator.IsEnd())      //生成が終わってなかったら生成し続ける
            {
                mapGenerator.LoadFormFile();
                return;
            }
            if (!stageInfoLoader.IsItemLoad())
            {
                stageInfoLoader.LoadFloorItem(gameManager.ItemManager, gameManager.StageNum, stageManager.CurrentFloor());
                return;
            }


            gameManager.GenerateMapInstance(mapGenerator.MapChip);      //実体を生成し、シーンを終わらせる
            endFlag = true;
        }
    }
}
