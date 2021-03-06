﻿//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17
// 内容：マップ用ブロック
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object
{
    class Cube
    {
        private Renderer renderer;
        private string textureName = "cubeTest";        //アセット名
        protected Vector3 position;                     //位置
        protected Vector3 size;                         //大きさ

        private Color color;                            //色付け用
        private Color wallColor;                        //MiniMap用

        public Cube(Vector3 position, Vector3 halfSize, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            this.position = position;
            this.size = halfSize;

            color = Color.White;
            wallColor = Color.White;
        }

        /// <summary>
        /// 3D描画
        /// </summary>
        public void Draw()
        {
            renderer.DefaultRenderSetting();
            renderer.RenderMainProjector();
            renderer.DrawModel("map_block", textureName, position, size * 2, color);
        }

        /// <summary>
        /// MiniMapに描画
        /// </summary>
        public void DrawMiniMap()
        {
            renderer.DrawModel("magic_circle", textureName, position, size * 2, wallColor, 0.7f);
        }

        public void SetMiniMapWallColor(Color color)
        {
            wallColor = color;
        }

        /// <summary>
        /// テクスチャーを設定
        /// </summary>
        /// <param name="textureName">アセット名</param>
        public void SetTexture(string textureName)
        {
            this.textureName = textureName;
        }

        /// <summary>
        /// あたり判定
        /// </summary>
        public virtual BoundingBox Collision
        {
            get{ return new BoundingBox(position - size, position + size); }
        }

        /// <summary>
        /// 色設定
        /// </summary>
        /// <param name="color">色</param>
        public void SetColor(Color color)
        {
            this.color = color;
        }
    }
}
