//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.27 ~ 2017.11.28
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.Object
{
    class CollisionSphere
    {
        private enum Direction
        {
            Xminus,
            Xplus,
            Yminus,
            Yplus,
            Zminus,
            Zplus,
        }

        private Vector3 position;   //位置
        private float radius;       //半径
        private static readonly float GRAVITY = 9.8f;
        private float gSpeed;

        /// <summary>
        /// 球状のCollision
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="radius">半径</param>
        public CollisionSphere(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;

            gSpeed = 0;
        }

        /// <summary>
        /// 他の球状Collisionと当たり判定
        /// </summary>
        /// <param name="other">他のCollision</param>
        /// <returns></returns>
        public bool IsCollision(BoundingSphere other)
        {
            BoundingSphere boundingSphere = new BoundingSphere(position, radius);
            return boundingSphere.Intersects(other);
        }

        /// <summary>
        /// 他のボックス状Collisionと当たり判定
        /// </summary>
        /// <param name="other">他のCollision</param>
        /// <returns></returns>
        public bool IsCollision(BoundingBox other)
        {
            BoundingSphere boundingSphere = new BoundingSphere(position, radius);
            return boundingSphere.Intersects(other);
        }

        /// <summary>
        /// 位置修正のメソッド
        /// </summary>
        /// <param name="other">他のCollision</param>
        public void Hit(BoundingBox other)
        {
            Direction dir = CheckDirection(other);           //方向を確定
            switch (dir)
            {
                case Direction.Xminus:
                    while(IsCollision(other))
                        position += -Vector3.UnitX * 0.1f;
                    break;
                case Direction.Xplus:
                    while (IsCollision(other))
                        position += Vector3.UnitX * 0.1f;
                    break;
                case Direction.Yminus:
                    position.Y = other.Min.Y - radius;
                    break;
                case Direction.Yplus:
                    position.Y = other.Max.Y + radius;
                    gSpeed = 0;
                    break;
                case Direction.Zminus:
                    while (IsCollision(other))
                        position += -Vector3.UnitZ * 0.1f;
                    break;
                case Direction.Zplus:
                    while (IsCollision(other))
                        position += Vector3.UnitZ * 0.1f;
                    break;
            }
        }

        /// <summary>
        /// 方向を確定するメソッド
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private Direction CheckDirection(BoundingBox other)
        {
            Vector3 otherCenter = (other.Max + other.Min) / 2.0f;   //相手の中心座標
            Vector3 dir = position - otherCenter;                   //方向
            //Y軸方向
            if (Math.Abs(dir.Y) > Math.Abs(dir.X) && Math.Abs(dir.Y) > Math.Abs(dir.Z))
            {
                if (dir.Y > 0)
                    return Direction.Yplus;
                else
                    return Direction.Yminus;
            }
            //Z軸方向
            else if (Math.Abs(dir.Z) > Math.Abs(dir.X) && Math.Abs(dir.Z) > Math.Abs(dir.Y))
            {
                if (dir.Z > 0)
                    return Direction.Zplus;
                else
                    return Direction.Zminus;
            }

            //X軸方向
            if (dir.X > 0)
                return Direction.Xplus;

            return Direction.Xminus;
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="velocity">方向</param>
        /// <param name="speed">スピード</param>
        /// <param name="gravity">重力適用するか</param>>
        public void Force(Vector3 velocity, float speed, bool gravity = true)
        {
            position += velocity * speed;

            if (!gravity)
                return;
            gSpeed += GRAVITY / 60.0f;
            position.Y -= gSpeed;
        }

        /// <summary>
        /// Collisionの中心座標
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Collisionの半径
        /// </summary>
        public float Radius
        {
            get { return radius; }
        }

        /// <summary>
        /// Collision範囲
        /// </summary>
        public BoundingSphere Collision
        {
            get { return new BoundingSphere(position, radius); }
        }
    }
}
