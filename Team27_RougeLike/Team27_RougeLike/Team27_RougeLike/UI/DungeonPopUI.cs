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
    //選択
    public enum ChooseState
    {
        Y,          //はい
        N,          //いいえ
        Null,       //未回答
    }

    class DungeonPopUI
    {

        private Window window;
        private GameDevice gameDevice;
        private Renderer renderer;
        private InputState input;
        private GameManager gameManager;

        private bool isPop;
        private ChooseState chooseState;

        private Item itemInfo;

        public DungeonPopUI(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;

            Vector2 size = new Vector2(400, 200);                   //メッセージボックスのサイズ
            Vector2 pos = new Vector2(                              //位置
                Def.WindowDef.WINDOW_WIDTH / 2 - size.X / 2,
                Def.WindowDef.WINDOW_HEIGHT / 2 - size.Y / 2);
            window = new Window(gameDevice, pos, size);             //背景レイアウト
            window.Initialize();

            isPop = false;                                          //Pop状態
            chooseState = ChooseState.Null;

            itemInfo = null;                                        //表示するアイテム
        }

        /// <summary>
        /// メッセージボックスをポップオン
        /// </summary>
        public void PopUp()
        {
            isPop = true;
            window.Switch();
            chooseState = ChooseState.Null;
        }

        /// <summary>
        /// メッセージボックスをポップオフ
        /// </summary>
        public void PopOff()
        {
            isPop = false;
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
                PopOff();

            if(window.CurrentState())       //閉める以外の状態は入力有効
                Listener();
        }

        /// <summary>
        /// 入力を待つ
        /// </summary>
        private void Listener()
        {
            if (input.IsLeftClick())
            {
                window.Switch();            //WindowをClose
            }
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
        /// 選んだ選択
        /// </summary>
        /// <returns></returns>
        public ChooseState Choose()
        {
            return chooseState;
        }

        /// <summary>
        /// 表示するアイテムの情報
        /// </summary>
        /// <param name="item">アイテム</param>
        public void SetItemInfo(Item item)
        {
            itemInfo = item;
            PopUp();
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
                itemInfo.GetItemName(), 
                window.GetCenter(), 
                Color.White, 
                new Vector2(1.0f, 1.0f), 
                window.CurrentAlpha() + 0.3f, 
                true, true);
        }
    }
}
