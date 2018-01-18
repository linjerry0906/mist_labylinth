//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.20
// 内容  ：Polygonの透明の部分を削除する用のShader
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Effects
{
    class PolygonEffect
    {
        private Effect effect;                      //Shaderファイル
        private EffectParameter world;              //描画のWorldマトリクス
        private EffectParameter view;               //描画のViewマトリクス
        private EffectParameter projection;         //描画のProjectionマトリクス
        private EffectParameter texture;            //Texture
        private EffectParameter alpha;              //透明度
        private EffectParameter color;              //色

        public PolygonEffect(Effect effect)
        {
            this.effect = effect;
            Initialize();
        }

        public void Initialize()
        {
            world = effect.Parameters["World"];              //描画のWorldマトリクス
            view = effect.Parameters["View"];                //描画のViewマトリクス
            projection = effect.Parameters["Projection"];    //描画のProjectionマトリクス
            texture = effect.Parameters["Texture0"];         //Texture
            alpha = effect.Parameters["alpha"];              //透明度
            color = effect.Parameters["color"];
        }

        public EffectParameter World
        {
            get { return world; }
        }
        public EffectParameter View
        {
            get { return view; }
        }
        public EffectParameter Projection
        {
            get { return projection; }
        }
        public EffectParameter Texture
        {
            get { return texture; }
        }
        public EffectParameter Alpha
        {
            get { return alpha; }
        }

        public EffectParameter Color
        {
            get { return color; }
        }

        public Effect GetEffect()
        {
            return effect;
        }
    }
}
