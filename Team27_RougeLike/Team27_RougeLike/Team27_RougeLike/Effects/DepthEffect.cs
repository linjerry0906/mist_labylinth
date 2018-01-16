//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.1.16
// 内容  ：(軽くBlurだけ)　ToDo：深度を読み取る
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
    class DepthEffect
    {
        private Effect depthEffect;                 //Depth Shaderファイル
        private EffectParameter projection;         //描画のProjectionマトリクス
        private EffectParameter texture;            //（Shader変数）RenderTarget

        private GraphicsDevice graphicsDevice;
        private RenderTargetBinding[] currentRenderTarget;  //現在使用しているRenderTarget（復帰用）

        private RenderTarget2D depthTexture;        //DepthTexture

        public DepthEffect(GraphicsDevice graphicsDevice, Effect effect)
        {
            this.graphicsDevice = graphicsDevice;
            depthEffect = effect;

            Initialize();
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

            depthTexture = new RenderTarget2D(
                graphicsDevice,
                Def.WindowDef.WINDOW_WIDTH,
                Def.WindowDef.WINDOW_HEIGHT,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8);           //RenderTarget設定

            texture = depthEffect.Parameters["Texture0"];    //RenderTarget設定用のハンドル
            texture.SetValue(depthTexture);
        }

        private void SetProjection(Matrix projection)
        {
            this.projection = depthEffect.Parameters["Projection"];      //ProjectionMatrixのハンドル
            this.projection.SetValue(projection);                   //Matrixを設定
        }


        /// <summary>
        /// 現在のRenderTargetを解放
        /// </summary>
        public void ReleaseRenderTarget()
        {
            if (currentRenderTarget != null)
            {
                graphicsDevice.SetRenderTargets(currentRenderTarget);
                currentRenderTarget = null;
                return;
            }

            graphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// RenderTargetに描画
        /// </summary>
        /// <param name="color">ClearColor</param>
        public void WriteRenderTarget(Color color)
        {
            if (graphicsDevice.GetRenderTargets().Length > 0)
                currentRenderTarget = graphicsDevice.GetRenderTargets();

            graphicsDevice.SetRenderTargets(depthTexture);
            graphicsDevice.Clear(color);
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin(depthEffect);
            renderer.DrawTexture(depthTexture, Vector2.Zero);
            renderer.End();
        }
    }
}
