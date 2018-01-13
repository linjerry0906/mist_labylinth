﻿//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.12
// 内容  ：背景の霧
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Team27_RougeLike.Object
{
    class FogBackground
    {
        private Renderer renderer;
        private float offset = Def.WindowDef.WINDOW_WIDTH;
        private float angle;

        private Vector2 position;

        public FogBackground(GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            position = Vector2.Zero;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            position.X -= 1.0f;                   //ずらす

            if (position.X < -offset)              //大きさを超えたら元に戻す
                position.X = 0;
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw(Color color)
        {
            renderer.Begin();
            renderer.ChangeBlendState(BlendState.Additive);
            renderer.DrawTexture("fog", position, new Vector2(4, 4), color, 0.2f);
            renderer.DrawTexture("fog", position + new Vector2(offset, 0), new Vector2(4, 4), color, 0.2f);
            renderer.ChangeBlendState(BlendState.AlphaBlend);
            renderer.End();
        }
    }
}