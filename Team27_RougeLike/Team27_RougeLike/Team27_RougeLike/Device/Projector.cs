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
        private Vector3 baseDistance;   //注目目標との相対位置関係
        
        public Projector()
        {
            viewport = new Viewport(0, 0, Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT);

            position = new Vector3(0, 8, 10);
            //position = new Vector3(0, 700, 500);      //Debug広い視野
            target = new Vector3(0, 0, 0);
            baseDistance = new Vector3(10, 8, 10);
            world = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(
                (float)(100 * Math.PI / 180),   //FOV
                viewport.AspectRatio,           //Aspect
                0.1f,                           //近い
                1000.0f);                       //遠い
            lookat = Matrix.CreateLookAt(position, target, Vector3.Up);
        } 

        /// <summary>
        /// 注目するターゲット
        /// </summary>
        /// <param name="target">注目する場所</param>
        public void Focus(Vector3 target)
        {
            float distance = (this.target - target).Length();
            if (distance < 0.7f)                            //距離が小さすぎると追跡を行わない
            {
                return;
            }

            Move(target - this.target, 0.1f * distance);   //移動させる
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="velocity">方向</param>
        /// <param name="speed">スピード</param>
        private void Move(Vector3 velocity, float speed)
        {
            velocity.Normalize();
            position += (velocity * speed);         //移動
            target += (velocity * speed);
            lookat = Matrix.CreateLookAt(position, target, Vector3.Up);     //マトリクス更新
        }

        /// <summary>
        /// Y軸に対して回転
        /// </summary>
        /// <param name="angle">回転角度</param>
        public void Rotate(float angle)
        {
            position = new Vector3(
                target.X + baseDistance.X * (float)Math.Sin(MathHelper.ToRadians(angle)), 
                target.Y + baseDistance.Y, 
                target.Z + baseDistance.Z * (float)Math.Cos(MathHelper.ToRadians(angle)));
            lookat = Matrix.CreateLookAt(position, target, Vector3.Up);     //マトリクス更新
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

        /// <summary>
        /// プロジェクター向いている方向の上方向（単位ベクトル）
        /// </summary>
        public Vector3 Front
        {
            get
            {
                Vector3 front = new Vector3(target.X - position.X, 0, target.Z - position.Z);
                front.Normalize();
                return front;
            }
        }
        /// <summary>
        /// プロジェクター向いている方向の下方向（単位ベクトル）
        /// </summary>
        public Vector3 Back
        {
            get
            {
                Vector3 front = new Vector3(target.X - position.X, 0, target.Z - position.Z);
                front.Normalize();
                return -front;
            }
        }
        /// <summary>
        /// プロジェクター向いている方向の右方向（単位ベクトル）
        /// </summary>
        public Vector3 Right
        {
            get
            {
                Vector3 front = new Vector3(target.X - position.X, 0, target.Z - position.Z);
                Vector3 right = Vector3.Cross(front, Vector3.Up);
                right.Normalize();
                return right;
            }
        }
        /// <summary>
        /// プロジェクター向いている方向の左方向（単位ベクトル）
        /// </summary>
        public Vector3 Left
        {
            get
            {
                Vector3 front = new Vector3(target.X - position.X, 0, target.Z - position.Z);
                Vector3 left = Vector3.Cross(front, Vector3.Up);
                left.Normalize();
                return -left;
            }
        }
    }
}
