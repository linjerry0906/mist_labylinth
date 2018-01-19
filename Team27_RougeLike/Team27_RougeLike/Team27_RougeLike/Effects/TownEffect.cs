//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.19
// 内容  ：村シーンのEffect
//--------------------------------------------------------------------------------------------------
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Effects
{
    class TownEffect
    {
        private Renderer renderer;
        private GraphicsDevice graphicsDevice;

        private float rate;                             //拡大率
        private Vector2 origin;                         //描画、拡大中心
        private RenderTarget2D texture;                 //記録用テクスチャ
        private RenderTargetBinding[] renderTargets;　　//戻す用

        public TownEffect(Vector2 origin, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            graphicsDevice = renderer.GraphicsDevice;

            this.origin = origin;
            rate = 1;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            texture = new RenderTarget2D(
                graphicsDevice, Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT);

            renderTargets = null;
        }

        /// <summary>
        /// テクスチャーの書き込む
        /// </summary>
        /// <param name="color">背景色</param>
        public void WriteRenderTarget(Color color)
        {
            if(graphicsDevice.GetRenderTargets() != null)               //RenderTarget設定ある場合は記録
            {
                renderTargets = graphicsDevice.GetRenderTargets();
            }

            graphicsDevice.SetRenderTarget(texture);                    //RenderTarget設定
            graphicsDevice.Clear(color);
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw()
        {
            renderer.Begin();
            renderer.DrawTexture(texture, origin, new Vector2(rate), origin, Color.White);
            renderer.End();
        }

        /// <summary>
        /// 描画する前に設定
        /// </summary>
        public void ReleaseRenderTarget()
        {
            if (renderTargets != null)
            {
                graphicsDevice.SetRenderTargets(renderTargets);
                renderTargets = null;
                return;
            }

            graphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// 大きさ設定
        /// </summary>
        /// <param name="rate"></param>
        public void SetScaleRate(float rate)
        {
            this.rate = rate;
        }
    }
}
