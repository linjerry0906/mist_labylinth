using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Object
{
    class Transform
    {
        public Vector3 position;
        public float angle;
        private int radius;

        //private float height;
        //private float width;

        public Transform(Vector3 position,int radius)
        {
            this.position = position;
            this.radius = radius;
            //this.height = height;
            //this.width = width;
        }

        //public float Height { get { return height; } }
        //public float Width { get { return width; } }

    }
}
