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

namespace Team27_RougeLike.Object
{
    class Cube
    {
        private Renderer renderer;
        private Vector3 position;
        private Vector3 size;

        private Vector2[] texcoord;
        private Color color;            //Debug 色付け用

        public Cube(Vector3 position, Vector3 halfSize, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            this.position = position;
            this.size = halfSize;

            texcoord = new Vector2[4];
            texcoord[0] = new Vector2(0, 0);
            texcoord[1] = new Vector2(0, 1);
            texcoord[2] = new Vector2(1, 0);
            texcoord[3] = new Vector2(1, 1);

            color = Color.White;
        }

        public void Draw()
        {
            renderer.DefaultRenderSetting();
            renderer.RendererMainProjector();
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            //z+
            vertices[0] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, size.Z), color, texcoord[0]);
            vertices[1] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, size.Z), color, texcoord[1]);
            vertices[2] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, size.Z), color, texcoord[2]);
            vertices[3] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, size.Z), color, texcoord[3]);
            renderer.DrawPolygon("", vertices);
            //z-
            vertices[0] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, -size.Z), color, texcoord[0]);
            vertices[1] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, -size.Z), color, texcoord[1]);
            vertices[2] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, -size.Z), color, texcoord[2]);
            vertices[3] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, -size.Z), color, texcoord[3]);
            renderer.DrawPolygon("", vertices);
            //x+
            vertices[0] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, size.Z), color, texcoord[0]);
            vertices[1] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, size.Z), color, texcoord[1]);
            vertices[2] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, -size.Z), color, texcoord[2]);
            vertices[3] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, -size.Z), color, texcoord[3]);
            renderer.DrawPolygon("", vertices);
            //x-
            vertices[0] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, -size.Z), color, texcoord[0]);
            vertices[1] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, -size.Z), color, texcoord[1]);
            vertices[2] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, size.Z), color, texcoord[2]);
            vertices[3] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, size.Z), color, texcoord[3]);
            renderer.DrawPolygon("", vertices);
            //y+
            vertices[0] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, size.Z), color, texcoord[0]);
            vertices[1] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, -size.Z), color, texcoord[1]);
            vertices[2] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, size.Z), color, texcoord[2]);
            vertices[3] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, -size.Z), color, texcoord[3]);
            renderer.DrawPolygon("", vertices);
            //y-
            //vertices[0] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, -size.Z), color, texcoord[0]);
            //vertices[1] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, size.Z), color, texcoord[1]);
            //vertices[2] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, -size.Z), color, texcoord[2]);
            //vertices[3] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, size.Z), color, texcoord[3]);
            //renderer.DrawPolygon("", vertices);
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }
    }
}
