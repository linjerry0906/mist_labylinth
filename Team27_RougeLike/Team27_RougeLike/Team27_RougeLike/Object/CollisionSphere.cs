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

        private Vector3 position;
        private Vector3 velocity;
        private float radius;
        public CollisionSphere(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;

            velocity = Vector3.Zero;
        }

        public bool IsCollision(BoundingSphere other)
        {
            BoundingSphere boundingSphere = new BoundingSphere(position, radius);
            return boundingSphere.Intersects(other);
        }
        public bool IsCollision(BoundingBox other)
        {
            BoundingSphere boundingSphere = new BoundingSphere(position, radius);
            return boundingSphere.Intersects(other);
        }
        public void Hit(BoundingBox other)
        {
            Direction dir = CheckDirection(other);
            switch (dir)
            {
                case Direction.Xminus:
                    while(IsCollision(other))
                        position += Vector3.Negate(velocity) * 0.1f;
                    break;
                case Direction.Xplus:
                    while (IsCollision(other))
                        position += Vector3.Negate(velocity) * 0.1f;
                    break;
                case Direction.Yminus:
                    position.Y = other.Min.Y - radius;
                    break;
                case Direction.Yplus:
                    position.Y = other.Max.Y + radius;
                    break;
                case Direction.Zminus:
                    while (IsCollision(other))
                        position += Vector3.Negate(velocity) * 0.1f;
                    break;
                case Direction.Zplus:
                    while (IsCollision(other))
                        position += Vector3.Negate(velocity) * 0.1f;
                    break;
            }
        }
        private Direction CheckDirection(BoundingBox other)
        {
            Vector3 otherCenter = (other.Max + other.Min) / 2.0f;
            Vector3 dir = position - otherCenter;
            if (Math.Abs(dir.X) > Math.Abs(dir.Y) && Math.Abs(dir.X) > Math.Abs(dir.Y))
            {
                if (dir.X > 0)
                    return Direction.Xplus;
                else
                    return Direction.Xminus;
            }
            else if (Math.Abs(dir.Z) > Math.Abs(dir.X) && Math.Abs(dir.Z) > Math.Abs(dir.Y))
            {
                if (dir.Z > 0)
                    return Direction.Zplus;
                else
                    return Direction.Zminus;
            }

            if (dir.Y > 0)
                return Direction.Yplus;

            return Direction.Yminus;
        }
        public void Force(Vector3 velocity, float speed)
        {
            this.velocity = velocity;
            position += velocity * speed;
        }
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public BoundingSphere Collision
        {
            get { return new BoundingSphere(position, radius); }
        }
    }
}
