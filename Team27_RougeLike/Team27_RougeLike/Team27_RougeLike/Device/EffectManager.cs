//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.02
// 内容  ：RenderEffectを管理するクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Team27_RougeLike.Effects;

namespace Team27_RougeLike.Device
{
    public enum BasicEffectType
    {
        Basic,          //XNA　DefaultのEffect
        MiniMap,        //MiniMap描画用のEffect
    }

    class EffectManager
    {
        private GraphicsDevice graphicsDevice;  //グラフィック機器

        private BasicEffect currentEffect;      //現在使用中のEffect
        private BasicEffect basicEffect;        //XNA　DefaultのEffect
        private BasicEffect miniMapEffect;      //MiniMap描画用のEffect

        private BlurEffect blurEffect;          //BlurEffect
        private PolygonEffect polygonEffect;    //透明のポリゴン用のEffect

        /// <summary>
        /// Effectを管理するクラス
        /// </summary>
        /// <param name="graphicsDevice">グラフィック機器</param>
        /// <param name="contents">コンテントマネージャー</param>
        public EffectManager(GraphicsDevice graphicsDevice, ContentManager contents)
        {
            this.graphicsDevice = graphicsDevice;
            basicEffect = new BasicEffect(graphicsDevice);
            miniMapEffect = new BasicEffect(graphicsDevice);

            basicEffect.VertexColorEnabled = true;          //頂点色を有効
            miniMapEffect.VertexColorEnabled = true;        //頂点色を有効

            currentEffect = basicEffect;

            blurEffect = new BlurEffect(                    //BlurEffectをShaderから読み取って生成する
                graphicsDevice,
                contents.Load<Effect>("./Effect/blur"));
            blurEffect.Initialize();                        //初期化

            polygonEffect = new PolygonEffect(
                contents.Load<Effect>("./Effect/polygon"));
        }

        /// <summary>
        /// 現在使用中のEffectを取得
        /// </summary>
        public BasicEffect CurrentEffect
        {
            get { return currentEffect; }
        }

        /// <summary>
        /// Effectを交換
        /// </summary>
        /// <param name="type">交換するEffect</param>
        public void ChangeEffect(BasicEffectType type)
        {
            switch (type)
            {
                case BasicEffectType.Basic:
                    currentEffect = basicEffect;
                    break;
                case BasicEffectType.MiniMap:
                    currentEffect = miniMapEffect;
                    break;
            }
        }

        /// <summary>
        /// BlurEffectを取得
        /// </summary>
        /// <returns></returns>
        public BlurEffect GetBlurEffect()
        {
            return blurEffect;
        }

        /// <summary>
        /// PolygonEffectを取得
        /// </summary>
        /// <returns></returns>
        public PolygonEffect GetPolygonEffect()
        {
            return polygonEffect;
        }
    }
}
