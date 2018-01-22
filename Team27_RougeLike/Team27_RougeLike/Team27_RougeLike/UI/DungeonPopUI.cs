//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.15
// 内容  ：ダンジョン内のUIメッセージ表示
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.UI
{
    class DungeonPopUI
    {
        private GameDevice gameDevice;
        private Window window;
        private Renderer renderer;

        private bool isPop;

        private string text;
        private Vector2 textOffset;

        public DungeonPopUI(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            renderer = gameDevice.Renderer;

            Vector2 size = new Vector2(400, 200);                   //メッセージボックスのサイズ
            Vector2 pos = new Vector2(                              //位置
                Def.WindowDef.WINDOW_WIDTH / 2 - size.X / 2,
                Def.WindowDef.WINDOW_HEIGHT / 2 - size.Y / 2);
            window = new Window(gameDevice, pos, size);             //背景レイアウト
            window.Initialize();

            isPop = false;                                          //Pop状態

            text = "";                                              //表示する文字
            textOffset = new Vector2(0, -20);
        }

        /// <summary>
        /// Sizeを設定
        /// </summary>
        /// <param name="size">サイズ</param>
        public void SetSize(Vector2 size)
        {
            Vector2 pos = new Vector2(                              //位置
                Def.WindowDef.WINDOW_WIDTH / 2 - size.X / 2,
                Def.WindowDef.WINDOW_HEIGHT / 2 - size.Y / 2);
            window = new Window(gameDevice, pos, size);             //背景レイアウト
            window.Initialize();
            window.SetAlphaLimit(0.4f);
        }

        public void SetAlphaLimit(float alpha)
        {
            window.SetAlphaLimit(alpha);
        }

        public Vector2 Center
        {
            get { return window.GetCenter(); }
        }

        /// <summary>
        /// メッセージボックスをポップオン
        /// </summary>
        public void PopUp()
        {
            isPop = true;
            window.Switch();
        }

        /// <summary>
        /// メッセージボックスをポップオフ
        /// </summary>
        public void PopOff()
        {
            window.Switch(false);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update()
        {
            if (!isPop)                     //オフの場合は更新しない
                return;

            window.Update();                //背景レイアウトの透明度更新
            if (window.IsEnd())             //背景レイアウトの透明度が0になったらメッセージボックスを閉める
                isPop = false;
        }
        

        /// <summary>
        /// 今のポップ状態
        /// </summary>
        /// <returns></returns>
        public bool IsPop()
        {
            return isPop;
        }

        /// <summary>
        /// 表示するメッセージを設定
        /// </summary>
        /// <param name="text">メッセージ</param>
        public void SetMessage(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw()
        {
            if (!isPop)                             //ポップアップ状態以外は描画しない
                return;

            window.Draw();                          //背景レイアウト
            renderer.DrawString(                    //アイテム情報を仮描画
                text, 
                window.GetCenter() + textOffset, 
                Color.White, 
                new Vector2(1.0f, 1.0f), 
                window.CurrentAlpha() * 3.0f, 
                true, true);
        }

        public void SetTextOffset(Vector2 offset)
        {
            textOffset = offset;
        }

        /// <summary>
        /// 現在のAlpha値
        /// </summary>
        public float Alpha
        {
            get { return window.CurrentAlpha(); }
        }
    }
}
