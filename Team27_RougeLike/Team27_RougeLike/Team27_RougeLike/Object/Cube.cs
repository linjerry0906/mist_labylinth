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
            renderer.RenderMainProjector();
            renderer.DrawModel("ItemModel", position, size  * 2, color);
            
            //VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            ////z+
            //vertices[0] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, size.Z), color, texcoord[0]);
            //vertices[1] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, size.Z), color, texcoord[1]);
            //vertices[2] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, size.Z), color, texcoord[2]);
            //vertices[3] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, size.Z), color, texcoord[3]);
            //renderer.DrawPolygon("cubeTest", vertices);
            ////z-
            //vertices[0] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, -size.Z), color, texcoord[0]);
            //vertices[1] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, -size.Z), color, texcoord[1]);
            //vertices[2] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, -size.Z), color, texcoord[2]);
            //vertices[3] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, -size.Z), color, texcoord[3]);
            //renderer.DrawPolygon("cubeTest", vertices);
            ////x+
            //vertices[0] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, size.Z), color, texcoord[0]);
            //vertices[1] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, size.Z), color, texcoord[1]);
            //vertices[2] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, -size.Z), color, texcoord[2]);
            //vertices[3] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, -size.Z), color, texcoord[3]);
            //renderer.DrawPolygon("cubeTest", vertices);
            ////x-
            //vertices[0] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, -size.Z), color, texcoord[0]);
            //vertices[1] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, -size.Z), color, texcoord[1]);
            //vertices[2] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, size.Z), color, texcoord[2]);
            //vertices[3] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, size.Z), color, texcoord[3]);
            //renderer.DrawPolygon("cubeTest", vertices);
            ////y+
            //vertices[0] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, size.Z), color, texcoord[0]);
            //vertices[1] = new VertexPositionColorTexture(position + new Vector3(-size.X, size.Y, -size.Z), color, texcoord[1]);
            //vertices[2] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, size.Z), color, texcoord[2]);
            //vertices[3] = new VertexPositionColorTexture(position + new Vector3(size.X, size.Y, -size.Z), color, texcoord[3]);
            //renderer.DrawPolygon("cubeTest", vertices);
            //y-
            //vertices[0] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, -size.Z), color, texcoord[0]);
            //vertices[1] = new VertexPositionColorTexture(position + new Vector3(-size.X, -size.Y, size.Z), color, texcoord[1]);
            //vertices[2] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, -size.Z), color, texcoord[2]);
            //vertices[3] = new VertexPositionColorTexture(position + new Vector3(size.X, -size.Y, size.Z), color, texcoord[3]);
            //renderer.DrawPolygon("", vertices);
        }

        public void DrawMiniMap()
        {
            Color temp = new Color(color.R + 70, color.G + 70, color.B + 70, color.A);
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            vertices[0] = new VertexPositionColorTexture(new Vector3(position.X + -size.X, 0, position.Z + size.X), temp, texcoord[0]);
            vertices[1] = new VertexPositionColorTexture(new Vector3(position.X + -size.X, 0, position.Z -size.X), temp, texcoord[1]);
            vertices[2] = new VertexPositionColorTexture(new Vector3(position.X + size.X, 0, position.Z + size.X), temp, texcoord[2]);
            vertices[3] = new VertexPositionColorTexture(new Vector3(position.X + size.X, 0, position.Z -size.X), temp, texcoord[3]);

            renderer.DrawPolygon("cubeTest", vertices, 0.9f);
        }

        public BoundingBox Collision
        {
            get{ return new BoundingBox(position - size, position + size); }
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }
    }
}
