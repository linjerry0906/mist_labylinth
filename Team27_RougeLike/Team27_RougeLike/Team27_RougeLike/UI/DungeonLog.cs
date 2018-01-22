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
        private List<LogText> logs;         //表示LogList
        private List<LogText> stockList;    //スタックLogList

        private readonly static int HEIGHT = 21;         //Logの間隔
        private readonly static int TEXT_LIMIT = 7;      //メッセージ最大の数

        public DungeonLog(Vector2 position, Vector2 size, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;

            window = new Window(gameDevice, position, size);
            window.Switch(false);
            window.SetAlpha(0.0f);
            logs = new List<LogText>();
            stockList = new List<LogText>();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update()
        {
            if (!window.CurrentState())                 //Window開いていなかったら更新する必要がない
                return;

            UpdateLogList();

            AddStock();

            if (logs.Count <= 0)                        //LogがなかったらWindowの消える処理
                window.SetAlpha(window.CurrentAlpha() - 0.005f);

            if (window.CurrentAlpha() <= 0.0f)          //透明になったらWindowをClose
                window.Switch(false);
        }

        /// <summary>
        /// Logの移動や透明度更新
        /// </summary>
        public void UpdateLogList()
        {
            Vector2 position = window.GetOffsetPosition();
            for (int i = 0; i < logs.Count; i++)
            {
                logs[i].Alpha -= 0.005f * (stockList.Count + 1);            //Logの透明どを下げる
                logs[i].Update(position + new Vector2(2, i * HEIGHT));
            }

            logs.RemoveAll(text => text.Alpha <= 0);                        //完全透明になったLog情報を削除
        }

        /// <summary>
        /// スタックから取り出す
        /// </summary>
        private void AddStock()
        {
            if (logs.Count >= TEXT_LIMIT)
                return;

            if (stockList.Count <= 0)
                return;


            if (logs.Count > 0)
            {
                LogText last = logs[logs.Count - 1];
                Vector2 position = last.Position + new Vector2(0, HEIGHT);

                //背景レイヤーを超えた場合は処理しない
                if (position.Y > (window.GetOffsetPosition() + new Vector2(2, TEXT_LIMIT * HEIGHT)).Y)
                    return;

                stockList[0].Position = position;
            }
            else
            {
                Vector2 position = window.GetOffsetPosition() + new Vector2(2, 0);
                stockList[0].Position = position;
            }

            logs.Add(stockList[0]);
            stockList.RemoveAt(0);
        }

        /// <summary>
        /// Logを追加
        /// </summary>
        /// <param name="log">Log情報</param>
        /// <param name="color">色</param>
        public void AddLog(string log, Color color)
        {
            Vector2 position = window.GetOffsetPosition() + new Vector2(2, logs.Count * HEIGHT);

            if (logs.Count > 0)                                //場所を調整
            {
                LogText last = logs[logs.Count - 1];
                position = last.Position + new Vector2(0, HEIGHT);
            }

            stockList.Add(new LogText(log, color, position)); //Stockに追加
            window.Switch(true);                              //Windowを開く
            window.SetAlpha(0.6f);                            //透明度設定
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

            stockList.Add(new LogText(log, Color.White, position)); //スタックに追加
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
