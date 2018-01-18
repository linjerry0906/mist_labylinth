//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.21
// 内容  ：簡単のボタン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.UI
{
    class Button
    {
        private Vector2 position;       //位置
        private Rectangle rect;         //判定場所
        private Vector2 size;
        private int width, height;
        private InputState mouse;
        private Point mousePos;
        private string name;
        private bool isClick;

        public Button(Vector2 position, int width, int height)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            rect = new Rectangle(
                (int)position.X,
                (int)position.Y,
                width, height);
        }

        public Button(string name, Vector2 position, int width, int height, InputState mouse)
        {
            isClick = false;
            this.name = name;
            this.position = position;
            this.width = width;
            this.height = height;
            this.mouse = mouse;
            mousePos = new Point(
                (int)mouse.GetMousePosition().X,
                (int)mouse.GetMousePosition().Y);
            rect = new Rectangle(
                (int)position.X,
                (int)position.Y,
                width, height);
            size = new Vector2(1, 1);
        }

        /// <summary>
        /// クリックされたフレームはTrue
        /// </summary>
        /// <param name="mousePos">マウスの位置</param>
        /// <returns></returns>
        public bool IsMouseOn(Point mousePos)
        {
            return rect.Contains(mousePos);
        }

        /// <summary>
        /// ボタンの中心位置
        /// </summary>
        /// <returns></returns>
        public Point ButtonCenter()
        {
            return rect.Center;
        }

        public Vector2 ButtonCenterVector()
        {
            return new Vector2(rect.Center.X, rect.Center.Y);
        }

        /// <summary>
        /// 位置
        /// </summary>
        /// <returns></returns>
        public Point Position()
        {
            return rect.Location;
        }

        /// <summary>
        /// 大きさ
        /// </summary>
        /// <returns></returns>
        public Vector2 Size()
        {
            return new Vector2(rect.Width, rect.Height);
        }

        public bool IsClick(InputState mouse)
        {
            return IsMouseOn(mousePos) && mouse.IsLeftClick();
        }

        public void Update()
        {
            mousePos = new Point(
                (int)mouse.GetMousePosition().X,
                (int)mouse.GetMousePosition().Y);
            if (IsMouseOn(mousePos))
            {
                size = new Vector2(1.2f, 1.2f);
                rect = new Rectangle(
                (int)position.X,
                (int)position.Y,
                 (int)(width * 1.2f), (int)(height * 1.2));
            }
            else
            {
                size = new Vector2(1, 1);
                rect = new Rectangle(
                (int)position.X,
                (int)position.Y,
                width, height);
            }
        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position, size);
        }
    }
}
