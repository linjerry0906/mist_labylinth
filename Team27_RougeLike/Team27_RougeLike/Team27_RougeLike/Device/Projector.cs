//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17 ～ 2017.12.01
// 内容　：プロジェクター、カメラワーク
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Object;

namespace Team27_RougeLike.Device
{
    class Projector
    {
        private Viewport viewport;      //ビューポート

        private Matrix world;           //ワールド
        private Matrix projection;      //プロジェクター
        private Matrix lookat;          //注目マトリクス

        private Vector3 target;         //注目目標
        private Vector3 baseDistance;   //注目目標との相対位置関係

        private CollisionSphere collision;      //Collision
        
        /// <summary>
        /// Default Settingで生成 
        /// </summary>
        public Projector()
        {
            viewport = new Viewport(0, 0, Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT);

            collision = new CollisionSphere(new Vector3(0, 8, 10), 0.05f);      //Collisions
            target = new Vector3(0, 0, 0);       //注目目標
            baseDistance = collision.Position;   //注目目標との相対位置関係  
            world = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(
                (float)(100 * Math.PI / 180),    //FOV
                viewport.AspectRatio,            //Aspect
                0.1f,                            //近い
                1000.0f);                        //遠い
            lookat = Matrix.CreateLookAt(collision.Position, target, Vector3.Up);
        } 

        /// <summary>
        /// Viewportと位置を指定できるコンストラクタ
        /// </summary>
        /// <param name="viewport">ビューポート</param>
        /// <param name="position">位置</param>
        public Projector(Viewport viewport, Vector3 position)
        {
            this.viewport = viewport;

            collision = new CollisionSphere(position, 0.05f);      //Collisions
            target = new Vector3(0, 0, 0);       //注目目標
            baseDistance = collision.Position;   //注目目標との相対位置関係  
            world = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(
                (float)(100 * Math.PI / 180),    //FOV
                viewport.AspectRatio,            //Aspect
                0.1f,                            //近い
                1000.0f);                        //遠い
            lookat = Matrix.CreateLookAt(collision.Position, target, Vector3.Up);
        }

        /// <summary>
        /// プロジェクターの位置を初期化
        /// </summary>
        /// <param name="target">注目する目標</param>
        public void Initialize(Vector3 target)
        {
            this.target = target;
            Vector3 position =
                (Matrix.CreateTranslation(baseDistance) *           //目標との相対位置へ移動
                 Matrix.CreateRotationY(0) *                        //回転角度を0
                Matrix.CreateTranslation(target)).Translation;      //目標まで平行移動
            collision = new CollisionSphere(position, 2.0f);        //Collision更新
            UpdateLook();                                           //Viewマトリクス更新
        }

        /// <summary>
        /// 追尾するターゲット
        /// </summary>
        /// <param name="target">注目する場所</param>
        public void Trace(Vector3 target)
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
            collision.Force(velocity, speed);        //移動
            target += (velocity * speed);
            lookat = Matrix.CreateLookAt(collision.Position, target, Vector3.Up);     //マトリクス更新
        }

        /// <summary>
        /// Viewマトリクスの更新
        /// </summary>
        public void UpdateLook()
        {
            lookat = Matrix.CreateLookAt(collision.Position, target, Vector3.Up);
        }

        /// <summary>
        /// Y軸に対して回転
        /// </summary>
        /// <param name="angle">回転角度</param>
        public void Rotate(float angle)
        {
            Vector3 position =
                (Matrix.CreateTranslation(baseDistance) *                             //目標との相対位置へ移動
                Matrix.CreateRotationY(angle * (float)Math.PI / 180) *                //回転角度を指定角度へ
                Matrix.CreateTranslation(target)).Translation;                      　//目標まで平行移動
            collision.Force(position - collision.Position, 0.1f);                     //Collision移動
            lookat = Matrix.CreateLookAt(collision.Position, target, Vector3.Up);     //マトリクス更新
        }

        /// <summary>
        /// プロジェクターのCollision
        /// </summary>
        public CollisionSphere Collision
        {
            get { return collision; }
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
            get { return collision.Position; }
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
                Vector3 front = new Vector3(target.X - collision.Position.X, 0, target.Z - collision.Position.Z);
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
                Vector3 front = new Vector3(target.X - collision.Position.X, 0, target.Z - collision.Position.Z);
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
                Vector3 front = new Vector3(target.X - collision.Position.X, 0, target.Z - collision.Position.Z);
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
                Vector3 front = new Vector3(target.X - collision.Position.X, 0, target.Z - collision.Position.Z);
                Vector3 left = Vector3.Cross(front, Vector3.Up);
                left.Normalize();
                return -left;
            }
        }
    }
}
