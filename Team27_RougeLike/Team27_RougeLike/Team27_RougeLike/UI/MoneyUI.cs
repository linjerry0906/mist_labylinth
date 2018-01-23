//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.23
// 内容　：所持金を表示
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.UI
{
    class MoneyUI
    {
        private Renderer renderer;

        private Vector2 position;                                       //位置
        private static readonly Vector2 size = new Vector2(315, 30);    //大きさ
        private int currentMoney;                                       //所持金

        public MoneyUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            this.position = position;
            renderer = gameDevice.Renderer;
            currentMoney = gameManager.PlayerItem.CurrentMoney();
        }

        public void Draw(float alpha)
        {
            renderer.DrawTexture("fade",
                position, size, alpha * 0.6f);

            renderer.DrawString(
                "所持金", position + new Vector2(7, size.Y / 2),
                Color.White, new Vector2(1.1f, 1.1f), alpha,
                false, true);

            renderer.DrawString(
                currentMoney.ToString() + " G", 
                position + new Vector2(size.X / 2, size.Y / 2),
                Color.Gold, new Vector2(1.1f, 1.1f), alpha,
                false, true);
        }
    }
}
