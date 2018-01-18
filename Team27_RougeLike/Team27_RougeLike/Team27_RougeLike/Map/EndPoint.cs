//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.14
// 内容　：終点の印
//--------------------------------------------------------------------------------------------------
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    class EndPoint
    {
        private Renderer renderer;
        private Vector3 position;       //位置
        private static Vector3 size = new Vector3(MapDef.TILE_SIZE * 0.9f);    //大きさ
        private static Vector3 up = new Vector3(0, 0.001f, 0);                 //上に乗せる
        private bool alphaSwitch;       //透明度の切り替え
        private float alpha;            //透明度
        private float centerAngle;      //中心回転角度
        private float inAngle;          //内側回転角度
        private float middleAngle;      //中部回転角度
        private float outAngle;         //外側回転角度

        public EndPoint(Vector3 position, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            this.position = position;
            centerAngle = 0;
            inAngle = 0;
            middleAngle = 0;
            outAngle = 0;
            alpha = 0.4f;
            alphaSwitch = true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            #region 角度
            centerAngle += 0.1f;
            inAngle -= 0.4f;
            middleAngle += 0.2f;
            outAngle += 1.0f;

            if (centerAngle > 360)
                centerAngle -= 360;
            if (inAngle < -360)
                inAngle += 360;
            if (middleAngle > 360)
                middleAngle -= 360;
            if (outAngle > 360)
                outAngle -= 360;
            #endregion

            #region 透明度
            if (alphaSwitch)
            {
                alpha += 0.007f;
                if (alpha > 1.0f)
                    alphaSwitch = false;
            }
            else
            {
                alpha -= 0.007f;
                if (alpha < 0.7f)
                    alphaSwitch = true;
            }
            #endregion
        }

        public void Reset()
        {
            alpha = 0;
            alphaSwitch = true;
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw()
        {
            renderer.ChangeBlendState(BlendState.Additive);
            renderer.DrawModel("magic_circle", "magic_out", position, size, MathHelper.ToRadians(outAngle), Color.White, alpha);
            renderer.DrawModel("magic_circle", "magic_middle", position + up, size, MathHelper.ToRadians(middleAngle), Color.White, alpha);
            renderer.DrawModel("magic_circle", "magic_in", position + 2 * up, size, MathHelper.ToRadians(inAngle), Color.White, alpha);
            renderer.DrawModel("magic_circle", "magic_center", position + 3 * up, size, MathHelper.ToRadians(centerAngle), Color.White, alpha);
            renderer.ChangeBlendState(BlendState.AlphaBlend);
        }
    }
}
