//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.20
// 内容　：パラメータを表示するクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Object;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.UI
{
    class ParameterUI
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private Renderer renderer;

        private Vector2 position;       //描画位置

        private PlayerStatus status;    //Playerのステータス
        private string[] parameter;     //文字列化したステータス
        private string[] info;          //説明欄

        private readonly float LINE_HEIGHT = 30.0f;     //行の間隔
        private readonly float COLUM_WIDTH = 150.0f;    //説明と数字の間隔

        public ParameterUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            this.position = position;

            renderer = gameDevice.Renderer;
            status = gameManager.PlayerInfo;

            RefreshInfo();          //パラメータを更新
        }

        /// <summary>
        /// パラメータの更新処理
        /// </summary>
        public void RefreshInfo()
        {
            status.CaculateStatus();        //必要な数字を計算
            parameter = new string[8];
            parameter[0] = status.GetLevel().ToString();
            parameter[1] = "";
            parameter[2] = status.GetHP() + " / " + status.GetMaxHP();
            parameter[3] = status.GetPower().ToString();
            parameter[4] = status.GetDefence().ToString();
            parameter[5] = status.GetVelocty().ToString();
            parameter[6] = "";
            parameter[7] = status.GetWeight().ToString();

            info = new string[8];
            info[0] = "Lv";
            info[1] = "";
            info[2] = "HP";
            info[3] = "ATK";
            info[4] = "DEF";
            info[5] = "SPD";
            info[6] = "";
            info[7] = "Wt.";
        }

        /// <summary>
        /// パラメータを描画
        /// </summary>
        /// <param name="alpha">透明度</param>
        public void Draw(float alpha)
        {
            renderer.DrawTexture(
                "fade", position + new Vector2(-7, -15), 
                new Vector2(COLUM_WIDTH * 2.1f, LINE_HEIGHT * 8), alpha * 0.6f);

            for(int i = 0; i < parameter.Length; i++)
            {
                renderer.DrawString(
                    info[i],
                    position + new Vector2(0, i * LINE_HEIGHT),
                    new Color(1.0f, 1.0f, 1.0f),
                    new Vector2(1.2f, 1.2f),
                    alpha, false, true);

                renderer.DrawString(
                    parameter[i],
                    position + new Vector2(COLUM_WIDTH, i * LINE_HEIGHT),
                    new Color(1.0f, 1.0f, 1.0f),
                    new Vector2(1.2f, 1.2f),
                    alpha, false, true);
            }
        }
    }
}
