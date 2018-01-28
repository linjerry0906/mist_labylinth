//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.28
// 内容  ：BloomEffect
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
    class BloomEffect
    {
        private Effect hightLightEffect;            //Shaderファイル
        private Effect bloomEffect;                 //Shaderファイル
        private BlurEffect blurEffect;              //BlurEffect
        private Color clearColor;
        private EffectParameter originTexture;      //OriginTexture

        private GraphicsDevice graphicsDevice;
        private RenderTarget2D origin;              //元の画像を保存するRenderTarget
        private RenderTarget2D blurLightPart1;      //Blurしたハイライト
        private RenderTarget2D blurLightPart2;      //Blurしたハイライト
        private RenderTarget2D blurLightPart3;      //Blurしたハイライト
        private RenderTarget2D bloomTexture;        //Bloomしたテクスチャ
        private RenderTargetBinding[] currentRenderTarget;  //現在使用しているRenderTarget（復帰用）

        public BloomEffect(GraphicsDevice graphicsDevice, Effect hightLightEffect, 
            Effect bloomEffect ,BlurEffect blurEffect)
        {
            this.graphicsDevice = graphicsDevice;
            this.hightLightEffect = hightLightEffect;
            this.bloomEffect = bloomEffect;
            this.blurEffect = blurEffect;
            blurEffect.Initialize();

            Initialize();
        }

        public void Initialize()
        {
            origin = new RenderTarget2D(
                graphicsDevice,
                Def.WindowDef.WINDOW_WIDTH,
                Def.WindowDef.WINDOW_HEIGHT,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8);           //RenderTarget設定

            blurLightPart1 = new RenderTarget2D(
                graphicsDevice,
                Def.WindowDef.WINDOW_WIDTH,
                Def.WindowDef.WINDOW_HEIGHT,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8);           //RenderTarget設定
            blurLightPart2 = new RenderTarget2D(
               graphicsDevice,
               Def.WindowDef.WINDOW_WIDTH,
               Def.WindowDef.WINDOW_HEIGHT,
               false,
               SurfaceFormat.Color,
               DepthFormat.Depth24Stencil8);           //RenderTarget設定
            blurLightPart3 = new RenderTarget2D(
               graphicsDevice,
               Def.WindowDef.WINDOW_WIDTH,
               Def.WindowDef.WINDOW_HEIGHT,
               false,
               SurfaceFormat.Color,
               DepthFormat.Depth24Stencil8);           //RenderTarget設定
            bloomTexture = new RenderTarget2D(
               graphicsDevice,
               Def.WindowDef.WINDOW_WIDTH,
               Def.WindowDef.WINDOW_HEIGHT,
               false,
               SurfaceFormat.Color,
               DepthFormat.Depth24Stencil8);           //RenderTarget設定

            originTexture = bloomEffect.Parameters["OriginColor"];
            originTexture.SetValue(origin);
        }

        /// <summary>
        /// RenderTargetに描画
        /// </summary>
        /// <param name="color">ClearColor</param>
        public void WriteRendererTarget(Color color)
        {
            if (graphicsDevice.GetRenderTargets().Length > 0)
                currentRenderTarget = graphicsDevice.GetRenderTargets();

            graphicsDevice.SetRenderTargets(origin);
            graphicsDevice.Clear(color);
            this.clearColor = color;
        }

        /// <summary>
        /// 現在のRenderTargetを解放、ハイライトとぼかしを実行する前に使用
        /// </summary>
        public void ReleaseRenderTarget()
        {
            blurEffect.WriteRenderTarget(clearColor);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin(hightLightEffect);                   //ハイライトを書き出し
            renderer.DrawTexture(origin, Vector2.Zero);
            renderer.End();

            graphicsDevice.SetRenderTargets(blurLightPart1);    //一回目ぼかし
            graphicsDevice.Clear(clearColor);
            blurEffect.Update(0.5f);
            blurEffect.Draw(renderer);

            graphicsDevice.SetRenderTargets(blurLightPart2);    //二回目ぼかし
            graphicsDevice.Clear(clearColor);
            blurEffect.Update(0.3f);
            blurEffect.Draw(renderer, blurLightPart1);

            graphicsDevice.SetRenderTargets(blurLightPart3);    //三回目ぼかし
            graphicsDevice.Clear(clearColor);
            blurEffect.Update(0.1f);
            blurEffect.Draw(renderer, blurLightPart2);


            graphicsDevice.SetRenderTargets(bloomTexture);      //Bloom
            graphicsDevice.Clear(clearColor);
            renderer.Begin(bloomEffect);
            renderer.DrawTexture(blurLightPart3, Vector2.Zero);
            renderer.End();

            ReturnRenderTargets();                              //復帰して描画
            renderer.Begin();
            renderer.DrawTexture(bloomTexture, Vector2.Zero);
            renderer.End();
        }

        /// <summary>
        /// 復帰
        /// </summary>
        private void ReturnRenderTargets()
        {
            if (currentRenderTarget != null)
            {
                graphicsDevice.SetRenderTargets(currentRenderTarget);
                currentRenderTarget = null;
                return;
            }
            graphicsDevice.SetRenderTarget(null);
        }
    }
}
