//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.20
// 内容  ：ダンジョン内のUIをまとめたクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.UI
{
    class DungeonUI
    {
        private DungeonPopUI popUI;         //PopメッセージUI
        private DungeonHintUI hintUI;       //ヒントUI
        private DungeonLog logUI;           //LogUI

        public DungeonUI(GameManager gameManager, GameDevice gameDevice)
        {
            popUI = new DungeonPopUI(gameDevice);
            hintUI = new DungeonHintUI(gameDevice);
            logUI = new DungeonLog(
                new Vector2(30, Def.WindowDef.WINDOW_HEIGHT - 250),
                new Vector2(350, 150), gameDevice);
        }

        /// <summary>
        /// 全UIを描画
        /// </summary>
        public void Draw()
        {
            popUI.Draw();
            hintUI.Draw();
            logUI.Draw();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update()
        {
            popUI.Update();
            hintUI.Update();
            logUI.Update();
        }

        /// <summary>
        /// Popメッセージ有効しているか
        /// </summary>
        /// <returns></returns>
        public bool IsPop()
        {
            return popUI.IsPop();
        }

        /// <summary>
        /// 下に表示するヒントUI
        /// </summary>
        public DungeonHintUI HintUI
        {
            get { return hintUI; }
        }

        /// <summary>
        /// Log情報を表示するUI
        /// </summary>
        public DungeonLog LogUI
        {
            get { return logUI; }
        }
    }
}
