//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.12
// 内容  ：背景の霧
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Team27_RougeLike.Object
{
    class FogBackground
    {
        private Renderer renderer;
        private float offset = Def.WindowDef.WINDOW_WIDTH;

        private Vector2 position;
        private Vector2 position2;
        private Timer timer;
        private Timer timer2;

        public FogBackground(GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            position = Vector2.Zero;
            position2 = Vector2.Zero;

            timer = new Timer(7.0f);
            timer.Initialize();
            timer2 = new Timer(10.0f);
            timer2.Initialize();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            position.X -= 0.7f;                   //ずらす
            position2.X += 1.5f;

            if (position.X < -offset)              //大きさを超えたら元に戻す
                position.X = 0;

            if (position2.X > offset)              //大きさを超えたら元に戻す
                position2.X = 0;

            timer.Update();
            if (timer.IsTime())
            {
                timer.Initialize();
            }
            timer2.Update();
            if (timer2.IsTime())
            {
                timer2.Initialize();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update(float speed)
        {
            position.X -= speed;                   //ずらす
            position2.X += speed * 2.1f;

            if (position.X < -offset)              //大きさを超えたら元に戻す
                position.X = 0;

            if (position2.X > offset)              //大きさを超えたら元に戻す
                position2.X = 0;

            timer.Update();
            if (timer.IsTime())
            {
                timer.Initialize();
            }
            timer2.Update();
            if (timer2.IsTime())
            {
                timer2.Initialize();
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw(Color color, float alpha = 0.35f)
        {
            float rate = MathHelper.Clamp(Math.Abs(timer.Rate() * 2 - 1) * 2, 0.6f, 1.0f);
            float rate2 = MathHelper.Clamp(Math.Abs(timer2.Rate() * 2 - 1), 0.3f, 0.8f);
            renderer.Begin();
            renderer.ChangeBlendState(BlendState.Additive);
            renderer.DrawTexture("fog", position, new Vector2(4, 4), color, alpha * rate);
            renderer.DrawTexture("fog", position + new Vector2(offset, 0), new Vector2(4, 4), color, alpha * rate);
            renderer.DrawTexture("fog", position2, new Vector2(4, 4), color, (alpha - 0.05f) * rate2);
            renderer.DrawTexture("fog", position2 + new Vector2(-offset, 0), new Vector2(4, 4), color, (alpha - 0.05f) * rate2);
            renderer.ChangeBlendState(BlendState.AlphaBlend);
            renderer.End();
        }
    }
}
