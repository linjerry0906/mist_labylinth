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
        private Vector2 position;
        private Rectangle rect;

        public Button(Vector2 position, int width, int height)
        {
            this.position = position;
            rect = new Rectangle(
                (int)position.X,
                (int)position.Y,
                width, height);
        }

        public bool IsClick(Point mousePos)
        {
            return rect.Contains(mousePos);
        }
    }
}
