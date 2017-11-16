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
        private Viewport viewport;

        private Matrix world;
        private Matrix projection;
        private Matrix lookat;

        private Vector3 position;
        
        public Projector()
        {
            viewport = new Viewport(0, 0, Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT);

            position = new Vector3(10, 10, 10);
            world = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(
                (float)(45 * Math.PI / 180),
                viewport.AspectRatio,
                0.1f,
                1000.0f);
            lookat = Matrix.CreateLookAt(position, Vector3.Zero, Vector3.Up);
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

        public Viewport ViewPort
        {
            get { return viewport; }
        }
    }
}
