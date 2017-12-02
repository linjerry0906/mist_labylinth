//--------------------------------------------------------------------------------------------------
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

        private bool endFlag;                   //シーンの終わるフラグ

        private MapGenerator mapGenerator;      //マップ生成者

        public LoadMap(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;

            renderer = gameDevice.Renderer;
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

            //ToDo：GameManagerから今の進捗状況によってマップのサイズを指定
            mapGenerator = new MapGenerator(30, gameDevice);
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return SceneType.Dungeon;
        }

        public void Shutdown()
        {
            mapGenerator = null;            //マップ生成者のメモリーを解放
        }

        public void Update(GameTime gameTime)
        {
            if (!mapGenerator.IsEnd())      //生成が終わってなかったら生成し続ける
            {
                mapGenerator.Update();
            }
            else
            {
                gameManager.GenerateMapInstance(mapGenerator.MapChip);      //実体を生成し、シーンを終わらせる
                endFlag = true;
            }
        }
    }
}
