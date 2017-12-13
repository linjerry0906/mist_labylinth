using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Device
{
    class TextureLoader : Loader
    {
        private Renderer renderer;

        public TextureLoader(Renderer renderer, string[,] resources) 
            :base(resources)//親クラスで初期化
        {
            this.renderer = renderer;
        }

        public override void Update()
        {
            endFlag = true;

            //カウンタが最大に達してないか？
            if (counter < maxNum)
            {
                //画像読み込み
                renderer.LoadTexture(
                    resources[counter, 0], //アセット名
                    resources[counter, 1]);//ファイルパス
                //カウントアップ
                counter += 1;
                //まだ読み込むものがあったのでフラグを戻す
                endFlag = false;
            }
        }

    }
}
