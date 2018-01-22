//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.20
// 内容　：PauseにあるUIをまとめたクラス
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
    class PauseUI
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private InputState input;
        private Renderer renderer;

        private Window backLayer;               //背景レイヤー
        private ParameterUI parameterUI;        //パラメータ表示UI
        private ItemUI itemUI;                  //ItemUI
        private ItemInfoUI currentInfo;         //選択されているアイテムの表示
        private EquipUI equipUI;
        private readonly float LIMIT_ALPHA = 0.1f;      //背景Alphaの最大値 

        public PauseUI(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;

            backLayer = new Window(
                gameDevice,
                new Vector2(10, 10),
                new Vector2(Def.WindowDef.WINDOW_WIDTH - 20, Def.WindowDef.WINDOW_HEIGHT - 20));
            backLayer.Initialize();         //初期化
            backLayer.SetAlphaLimit(LIMIT_ALPHA);
            backLayer.Switch();             //開く

            parameterUI = new ParameterUI(
                backLayer.GetRightTop() + new Vector2(-350, 30),        //背景レイヤーから相対位置を取る
                gameManager, gameDevice);


            currentInfo = new ItemInfoUI(
                backLayer.GetLeftUnder() + new Vector2(50, -100), gameManager, gameDevice);

            #region EquipUI
            equipUI = new EquipUI(
                backLayer.GetRightUnder() + new Vector2(-360, -220),
                gameManager, gameDevice);
            #endregion

            itemUI = new ItemUI(
                backLayer.GetOffsetPosition() + new Vector2(45, 20),
                equipUI, gameManager, gameDevice);

            equipUI.SetItemUI(itemUI);
        }

        /// <summary>
        /// UIの更新処理
        /// </summary>
        public void Update()
        {
            backLayer.Update();
            itemUI.Update();
            equipUI.Update();
            if (itemUI.IsClick())
            {
                equipUI.SetNull();
            }
            else if (equipUI.IsClick())
            {
                itemUI.SetNull();
            }


            parameterUI.RefreshInfo();
        }

        /// <summary>
        /// 終わっているか
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return backLayer.IsEnd();
        }

        /// <summary>
        /// UIを閉じる
        /// </summary>
        public void SwitchOff()
        {
            backLayer.Switch();
            itemUI.SwitchOff();
        }

        /// <summary>
        /// 描画する
        /// </summary>
        public void Draw()
        {
            float constractAlpha = 1.0f / LIMIT_ALPHA;
            renderer.DrawTexture("fade",
                backLayer.GetLeftUnder() + new Vector2(40, -115),
                new Vector2(570, 100),
                backLayer.CurrentAlpha() * constractAlpha * 0.6f);
            backLayer.Draw("white");
            parameterUI.Draw(backLayer.CurrentAlpha() * constractAlpha);
            itemUI.Draw(backLayer.CurrentAlpha() * constractAlpha);
            equipUI.Draw(backLayer.CurrentAlpha() * constractAlpha);

            if (itemUI.CurrentItem() != null)
            {
                currentInfo.Draw(itemUI.CurrentItem(), backLayer.CurrentAlpha() * constractAlpha);
            }
            else if (equipUI.CurrentItem() != null)
            {
                currentInfo.Draw(equipUI.CurrentItem(), backLayer.CurrentAlpha() * constractAlpha);
            }
        }
    }
}
