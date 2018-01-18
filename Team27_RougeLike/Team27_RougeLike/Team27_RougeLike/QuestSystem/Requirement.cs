using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.QuestSystem
{
    class Requirement
    {
        private int id;
        private int requireAmount;
        private int currentAmount;
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

        public void AddCurrentAmount(int amount)
        {
            currentAmount += amount;
        }

        public int ID
        {
            get { return id; }
        }

        public int RequireAmount
        {
            get { return requireAmount; }
        }

        public int CurrentAmount
        {
            get { return currentAmount; }
        }
    }
}
