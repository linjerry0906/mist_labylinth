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
        private EquipUI equipUI;                //装備欄
        private PlayerQuestUI questUI;          //受けているクエストUI
        private MoneyUI moneyUI;                //所持金を表示するUI
        private readonly float LIMIT_ALPHA = 0.1f;      //背景Alphaの最大値 

        public PauseUI(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;

            #region 背景
            backLayer = new Window(
                gameDevice,
                new Vector2(10, 10),
                new Vector2(Def.WindowDef.WINDOW_WIDTH - 20, Def.WindowDef.WINDOW_HEIGHT - 20));
            backLayer.Initialize();         //初期化
            backLayer.SetAlphaLimit(LIMIT_ALPHA);
            backLayer.Switch();             //開く
            #endregion

            #region Parameter
            parameterUI = new ParameterUI(
                backLayer.GetRightTop() + new Vector2(-360, 35),        //背景レイヤーから相対位置を取る
                gameManager, gameDevice);
            #endregion

            #region アイテム詳細
            currentInfo = new ItemInfoUI(
                backLayer.GetLeftUnder() + new Vector2(55, -102), gameManager, gameDevice);
            #endregion

            #region EquipUI
            equipUI = new EquipUI(
                backLayer.GetRightUnder() + new Vector2(-360, -220),
                gameManager, gameDevice);
            #endregion

            #region アイテム欄
            itemUI = new ItemUI(
                backLayer.GetOffsetPosition() + new Vector2(45, 20),
                equipUI, gameManager, gameDevice);

            equipUI.SetItemUI(itemUI);
            #endregion

            #region Quest
            questUI = new PlayerQuestUI(
                backLayer.GetCenterTop() + new Vector2(-200, 20),
                gameManager, gameDevice);
            #endregion

            #region Money
            moneyUI = new MoneyUI(
                backLayer.GetRightTop() + new Vector2(-360, 280),
                gameManager, gameDevice);
            #endregion
        }

        /// <summary>
        /// UIの更新処理
        /// </summary>
        public void Update()
        {
            backLayer.Update();     //背景更新
            itemUI.Update();        //アイテムリスト更新
            if (itemUI.IsPop())     //メッセージボックスPopした状態は以降更新しない
                return;

            questUI.Update();       //クエスト
            equipUI.Update();       //装備欄

            #region 詳細設定
            if (itemUI.IsClick())
            {
                equipUI.SetNull();
            }
            else if (equipUI.IsClick())
            {
                itemUI.SetNull();
            }
            #endregion

            parameterUI.RefreshInfo();      //パラメータ更新
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
            backLayer.Switch();     //背景を閉じる
            itemUI.SwitchOff();     //メッセージボックスを閉じる
        }

        /// <summary>
        /// 描画する
        /// </summary>
        public void Draw()
        {
            float alpha = backLayer.CurrentAlpha() / LIMIT_ALPHA;       //背景Alphaから逆算
            backLayer.Draw("white");        //背景
            renderer.DrawTexture("fade",    
                 backLayer.GetLeftUnder() + new Vector2(45, -120),
                 new Vector2(670, 105),
                 alpha * 0.6f);

            parameterUI.Draw(alpha);        //能力欄
            moneyUI.Draw(alpha);            //所持金
            questUI.Draw(alpha);            //クエスト欄
            equipUI.Draw(alpha);            //装備欄

            #region Item詳細

            if (itemUI.CurrentItem() != null)
            {
                currentInfo.Draw(itemUI.CurrentItem(), alpha);
            }
            else if (equipUI.CurrentItem() != null)
            {
                currentInfo.Draw(equipUI.CurrentItem(), alpha);
            }
            #endregion

            itemUI.Draw(alpha);             //所持アイテム
            questUI.DrawQuestInfo(alpha);   //カーソルに合わせて表示するクエスト詳細
        }
    }
}
