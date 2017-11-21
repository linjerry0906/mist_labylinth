//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Def;

namespace Team27_RougeLike.Device
{
    class Projector
    {
        private Viewport viewport;      //ビューポート

        private Matrix world;           //ワールド
        private Matrix projection;      //プロジェクター
        private Matrix lookat;          //注目マトリクス

        private Vector3 target;         //注目目標
        private Vector3 position;       //プロジェクターの位置
        
        public Projector()
        {
            viewport = new Viewport(0, 0, Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT);

            position = new Vector3(0, 30, 20);
            //position = new Vector3(0, 700, 500);
            target = new Vector3(0, 0, 0);
            world = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(
                (float)(45 * Math.PI / 180),
                viewport.AspectRatio,
                0.1f,
                1000.0f);
            lookat = Matrix.CreateLookAt(position, target, Vector3.Up);
        } 

        /// <summary>
        /// 注目するターゲット
        /// </summary>
        /// <param name="target">注目する場所</param>
        public void Focus(Vector3 target)
        {
            float distance = (this.target - target).Length();
            if (distance < 0.5f)                            //距離が小さすぎると追跡を行わない
            {
                return;
            }

            Move(target - this.target, 0.03f * distance);   //移動させる
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="velocity">方向</param>
        /// <param name="speed">スピード</param>
        private void Move(Vector3 velocity, float speed)
        {
            velocity.Normalize();               //
            position += (velocity * speed);
            target += (velocity * speed);
            lookat = Matrix.CreateLookAt(position, target, Vector3.Up);
        }

        /// <summary>
        /// ワールドマトリックス
        /// </summary>
        public Matrix World
        {
            get { return world; }
        }
        /// <summary>
        /// ビューマトリックス
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
        }
        /// <summary>
        /// 注目点
        /// </summary>
        public Matrix LookAt
        {
            get { return lookat; }
        }
        /// <summary>
        /// プロジェクターの位置
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
        }
        /// <summary>
        /// ビューポート
        /// </summary>
        public Viewport ViewPort
        {
            get { return viewport; }
        }
    }
}
