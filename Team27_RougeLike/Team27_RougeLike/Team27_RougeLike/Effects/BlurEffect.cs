//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：昔作ったものを持ってきた
// 内容  ：BlurEffect
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
    class BlurEffect
    {
        private Effect effect;                      //Shaderファイル
        private EffectParameter projection;         //描画のProjectionマトリクス
        private EffectParameter blurRate;           //Blurの程度
        private EffectParameter texture;            //（Shader変数）RenderTarget
        private EffectParameter offsetX;            //（Shader変数）ずらす度合いX
        private EffectParameter offsetY;            //（Shader変数）ずらす度合いY
        private readonly float _offsetX = (0.5f / Def.WindowDef.WINDOW_WIDTH); //ずらす度合いX
        private readonly float _offsetY = (0.5f / Def.WindowDef.WINDOW_HEIGHT);//ずらす度合いY

        private GraphicsDevice graphicsDevice;
        private RenderTarget2D target;              //RenderTarget

        /// <summary>
        /// BlurEffect
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice</param>
        /// <param name="effect">Shaderファイル</param>
        public BlurEffect(GraphicsDevice graphicsDevice, Effect effect)
        {
            this.graphicsDevice = graphicsDevice;

            SetEffect(effect);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            Matrix projectionLocal = Matrix.CreateOrthographicOffCenter(
            0,
            Def.WindowDef.WINDOW_WIDTH,
            Def.WindowDef.WINDOW_HEIGHT,
            0,
            0, 0.1f);
            SetProjection(projectionLocal);     //描画部分設定

            target = new RenderTarget2D(
                graphicsDevice, 
                Def.WindowDef.WINDOW_WIDTH, 
                Def.WindowDef.WINDOW_HEIGHT, 
                false, 
                SurfaceFormat.Color, 
                DepthFormat.Depth24Stencil8);   //RenderTarget設定

            blurRate = effect.Parameters["BlurRate"];   //BlurRateのハンドル

            texture = effect.Parameters["Texture0"];    //RenderTarget設定用のハンドル
            offsetX = effect.Parameters["offsetX"];     //OffsetXのハンドル
            offsetX.SetValue(_offsetX);                 //OffsetX設定
            offsetY = effect.Parameters["offsetY"];     //OffsetYのハンドル
            offsetY.SetValue(_offsetY);                 //OffsetY設定
            blurRate.SetValue(1.0f);                    //BlurRateを設定
        }
        /// <summary>
        /// Effectを内容を変数に保存
        /// </summary>
        /// <param name="effect">Shaderファイル</param>
        private void SetEffect(Effect effect)
        {
            this.effect = effect;
        }
        /// <summary>
        /// 描画のマトリクスの設定
        /// </summary>
        /// <param name="projection"></param>
        private void SetProjection(Matrix projection)
        {
            this.projection = effect.Parameters["Projection"];      //ProjectionMatrixのハンドル
            this.projection.SetValue(projection);                   //Matrixを設定
        }

        /// <summary>
        /// BlurRateの更新
        /// </summary>
        /// <param name="blurRate">Blurの度合い</param>
        public void Update(float blurRate)
        {
            this.blurRate.SetValue(blurRate / 100);     //BlurRate設定

            texture.SetValue(target);                   //RenderTarget設定
        }
        /// <summary>
        /// Effect描画
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin(effect);

            renderer.DrawTexture(target, Vector2.Zero);

            renderer.End();
        }
        /// <summary>
        /// 現在のRenderTargetを解放
        /// </summary>
        public void ReleaseRenderTarget()
        {
            graphicsDevice.SetRenderTarget(null);
        }
        /// <summary>
        /// RenderTargetに描画
        /// </summary>
        /// <param name="color">ClearColor</param>
        public void WriteRenderTarget(Color color)
        {
            graphicsDevice.SetRenderTarget(target);
            graphicsDevice.Clear(color);
        }
    }
}
