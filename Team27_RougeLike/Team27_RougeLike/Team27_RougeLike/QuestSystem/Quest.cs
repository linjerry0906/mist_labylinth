//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：Questの抽象クラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.QuestSystem
{
    public struct Requirement
    {
        public int id;
        public int requireAmount;
        public int currentAmount;
        /// <summary>
        /// 必要なitem、敵ID、数
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="amount">数</param>
        public Requirement(int id, int amount)
        {
            this.id = id;
            this.requireAmount = amount;
            currentAmount = 0;
        }

        public void SetCurrentAmount(int amount)
        {
            currentAmount = amount;
        }
    }

    abstract class Quest
    {
        protected int questID;              //QuestID
        protected string name;              //Quest名
        protected string explanation;       //説明
        protected bool isClear;             //クリアしているか
        protected int difficulty;           //難易度
        protected int gainMoney;            //報酬のお金
        protected int[] awardItemID;        //報酬のアイテムID

        public Quest(
            int id, string name, string explanation, 
            int difficulty, int gainMoney, int[] awardID)
        {
            this.questID = id;
            this.name = name;
            this.explanation = explanation;
            this.difficulty = difficulty;
            this.gainMoney = gainMoney;
            this.awardItemID = awardID;
            isClear = false;
        }

        /// <summary>
        /// 完成しているか
        /// </summary>
        /// <returns></returns>
        public bool IsClear()
        {
            return isClear;
        }

        /// <summary>
        /// クエストクエストの名前
        /// </summary>
        /// <returns></returns>
        public string QuestName()
        {
            return name;
        }

        /// <summary>
        /// クエストの詳細
        /// </summary>
        /// <returns></returns>
        public string QuestInfo()
        {
            return explanation;
        }

        /// <summary>
        /// クエストの難易度
        /// </summary>
        /// <returns></returns>
        public int Difficulty()
        {
            return difficulty;
        }

        /// <summary>
        /// クリアしているかを検査
        /// </summary>
        public abstract void CheckClear();

        /// <summary>
        /// 必要なアイテムID、倒した敵のIDの数を追加
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="amount">数</param>
        public abstract void AddAmount(int id, int amount);

        /// <summary>
        /// 今の完成状態
        /// </summary>
        /// <returns></returns>
        public abstract List<Requirement> CurrentState();

        /// <summary>
        /// 報酬アイテム（消費品限定）
        /// </summary>
        /// <returns></returns>
        public int[] AwardItem()
        {
            return awardItemID;
        }

        /// <summary>
        /// 報酬のお金
        /// </summary>
        /// <returns></returns>
        public int GainMoney()
        {
            return gainMoney;
        }

        /// <summary>
        /// 現在のアイテム数で更新
        /// </summary>
        public abstract void SetItemAmount(int id, int amount);

        /// <summary>
        /// 必要なもの、敵
        /// </summary>
        /// <returns></returns>
        public abstract int[] RequireID();
    }
}
