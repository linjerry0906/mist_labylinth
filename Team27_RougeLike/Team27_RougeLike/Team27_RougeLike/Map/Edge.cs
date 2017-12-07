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
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    class Edge
    {
        //エッジの端点
        private Point firstPoint;
        private Point secondPoint;

        //debug表示
        private Renderer renderer;

        /// <summary>
        /// エッジ（辺）
        /// </summary>
        /// <param name="first">端点1</param>
        /// <param name="second">端点2</param>
        /// <param name="gameDevice">Debug表示用</param>
        public Edge(Point first, Point second, GameDevice gameDevice)
        {
            firstPoint = first;
            secondPoint = second;

            renderer = gameDevice.Renderer;
        }

        /// <summary>
        /// Debug表示用
        /// </summary>
        public void Draw()
        {
            VertexPositionColor[] vertices = new VertexPositionColor[2];
            vertices[0] = new VertexPositionColor(new Vector3(firstPoint.X, 1.0f, firstPoint.Y), Color.Black);
            vertices[1] = new VertexPositionColor(new Vector3(secondPoint.X, 1.0f, secondPoint.Y), Color.Black);
            renderer.DrawLine(vertices);
        }

        /// <summary>
        /// 端点1
        /// </summary>
        public Point FirstPoint
        {
            get { return firstPoint; }
        }

        /// <summary>
        /// 端点2
        /// </summary>
        public Point SecondPoint
        {
            get { return secondPoint; }
        }
    }
}
