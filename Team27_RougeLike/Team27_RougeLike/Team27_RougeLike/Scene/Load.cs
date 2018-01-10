using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class Load : IScene
    {
        private Renderer renderer;
        private Sound sound;
        private bool endFlag;
        private TextureLoader textureLoader;
        private BGMLoader bgmLoader;
        private SELoader seLoader;
        private int totalResouceNum;

        private string[,] TextureList()
        {
            string path = "./Texture/";
            string[,] list = new string[,]
            {
                {"test" ,path},
                {"cubeTest" ,path},
                {"bat" ,path},
                {"slash" ,path},
            };
            return list;
        }

        private string[,] BGMList()
        {
            string path = "./Sound/";
            string[,] list = new string[,]
            {

            };
            return list;
        }
        private string[,] SEMList()
        {
            string path = "./Sound/";
            string[,] list = new string[,]
            {
                {"slash",path },
                {"sowrd",path },
            };
            return list;
        }

        public Load(GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            sound = gameDevice.Sound;

            //TextureLoaderの実態生成
            textureLoader = new TextureLoader(renderer, TextureList());

            //BGMLoaderの実態生成
            bgmLoader = new BGMLoader(sound, BGMList());

            //SELoaderの実態生成
            seLoader = new SELoader(sound, SEMList());
        }

        public void Initialize(SceneType sceneType)
        {
            endFlag = false;

            textureLoader.Initialize();
            bgmLoader.Initialize();
            seLoader.Initialize();

            //全リソース数を計算
            totalResouceNum = textureLoader.Count() +
                bgmLoader.Count() +
                seLoader.Count();
        }

        public void Update(GameTime gameTime)
        {
            textureLoader.Update();
            bgmLoader.Update();
            seLoader.Update();


            //読み込み処理が終わっていたらシーンを終了
            if (textureLoader.IsEnd() &&
                bgmLoader.IsEnd() &&
                seLoader.IsEnd())
            {
                endFlag = true;
            }

        }

        public void Draw()
        {
            renderer.Begin();
            
            renderer.DrawString("ロード中", Vector2.Zero, Color.Black, new Vector2(1, 1));

            //読み込んでいる数を取得
            int currentCount = textureLoader.CurrentCount() +
                bgmLoader.CurrentCount() +
                seLoader.CurrentCount();

            if (totalResouceNum != 0)
            {
                renderer.DrawString(((int)(currentCount / (float)totalResouceNum * 100)).ToString(),
                    new Vector2(0, 100), Color.Black, new Vector2(1, 1));
            }

            renderer.End();

        }

        public void Shutdown()
        {

        }

        public SceneType Next()
        {
            return SceneType.LoadTown;
        }

        public bool IsEnd()
        {
            return endFlag;
        }


    }
}
