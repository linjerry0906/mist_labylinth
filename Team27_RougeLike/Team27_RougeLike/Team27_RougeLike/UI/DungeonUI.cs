//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.20
// 内容  ：ダンジョン内のUIをまとめたクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.UI
{
    class DungeonUI
    {
        private DungeonPopUI popUI;         //PopメッセージUI
        private DungeonHintUI hintUI;       //ヒントUI

        public DungeonUI(GameManager gameManager, GameDevice gameDevice)
        {
            popUI = new DungeonPopUI(gameManager, gameDevice);
            hintUI = new DungeonHintUI(gameDevice);
        }

        /// <summary>
        /// 全UIを描画
        /// </summary>
        public void Draw()
        {
            popUI.Draw();
            hintUI.Draw();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update()
        {
            popUI.Update();
            hintUI.Update();
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
    }
}
