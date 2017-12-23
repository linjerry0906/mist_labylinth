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

            Vector2 position = window.GetOffsetPosition();
            for(int i = 0; i < logs.Count; i++)
            {
                logs[i].Alpha -= 0.005f;                //Logの透明どを下げる
                logs[i].Update(position + new Vector2(2, i * HEIGHT));
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
            Vector2 position = window.GetOffsetPosition() + new Vector2(2, logs.Count * HEIGHT);
            if(logs.Count > 0)
            {
                LogText last = logs[logs.Count - 1];
                position = last.Position + new Vector2(0, HEIGHT);
            }
            logs.Add(new LogText(log, color, position));      //Log追加
            window.Switch(true);                              //Windowを開く
            window.SetAlpha(0.5f);                            //透明度設定
        }

        /// <summary>
        /// Logを追加
        /// </summary>
        /// <param name="log">Log情報</param>
        public void AddLog(string log)
        {
            Vector2 position = window.GetOffsetPosition() + new Vector2(2, logs.Count * HEIGHT);
            if (logs.Count > 0)
            {
                LogText last = logs[logs.Count - 1];
                position = last.Position + new Vector2(0, HEIGHT);
            }
            logs.Add(new LogText(log, Color.White, position));      //Log追加
            window.Switch(true);                                    //Windowを開く
            window.SetAlpha(0.5f);                                  //透明度設定
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        public void Draw()
        {
            window.Draw();          //背景描画
            //Log情報を表示
            for (int i = 0; i < logs.Count; i++)
            {
                renderer.DrawString(
                    logs[i].Log,
                    logs[i].Position,
                    new Vector2(1.0f, 1.0f),
                    logs[i].Color,
                    logs[i].Alpha);
            }
        }
    }
}
