//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.23
// 内容　：LogのUI
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.UI
{
    class DungeonLog
    {
        private Renderer renderer;

        private Window window;              //背景レイアウト
        private List<LogText> logs;         //LogList

        private readonly int HEIGHT = 22;   //Logの間隔

        public DungeonLog(Vector2 position, Vector2 size, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;

            window = new Window(gameDevice, position, size);
            window.Switch(false);
            window.SetAlpha(0.0f);
            logs = new List<LogText>();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update()
        {
            if (!window.CurrentState())                 //Window開いていなかったら更新する必要がない
                return;

            foreach (LogText text in logs)              //Logの透明どを下げる
            {
                text.Alpha -= 0.005f;
            }

            logs.RemoveAll(text => text.Alpha <= 0);    //完全透明になったらLog情報を削除

            if (logs.Count <= 0)                        //LogがなかったらWindowの消える処理
                window.SetAlpha(window.CurrentAlpha() - 0.005f);

            if (window.CurrentAlpha() <= 0.0f)          //透明になったらWindowをClose
                window.Switch(false);
        }

        /// <summary>
        /// Logを追加
        /// </summary>
        /// <param name="log">Log情報</param>
        /// <param name="color">色</param>
        public void AddLog(string log, Color color)
        {
            logs.Add(new LogText(log, color));      //Log追加
            window.Switch(true);                    //Windowを開く
            window.SetAlpha(0.5f);                  //透明度設定
        }

        /// <summary>
        /// Logを追加
        /// </summary>
        /// <param name="log">Log情報</param>
        public void AddLog(string log)
        {
            logs.Add(new LogText(log, Color.White));      //Log追加
            window.Switch(true);                          //Windowを開く
            window.SetAlpha(0.5f);                        //透明度設定
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        public void Draw()
        {
            window.Draw();          //背景描画
            Vector2 position = window.GetOffsetPosition();
            //Log情報を表示
            for (int i = 0; i < logs.Count; i++)
            {
                renderer.DrawString(
                    logs[i].Log,
                    position + new Vector2(2, i * HEIGHT),
                    new Vector2(1.0f, 1.0f),
                    logs[i].Color,
                    logs[i].Alpha);
            }
        }
    }
}
