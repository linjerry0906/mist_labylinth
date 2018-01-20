using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.QuestSystem
{
    class PlayerGuildRank
    {
        private QuestLoader.QuestRank rank;
        private int currentExp;
        private static readonly int[] maxExp = { 50, 250, 450, 800, 1550, 3000};

        public PlayerGuildRank()
        {
            rank = QuestLoader.QuestRank.F;
            currentExp = 0;
        }

        public PlayerGuildRank(int rank, int exp)
        {
            this.rank = (QuestLoader.QuestRank)rank;
            currentExp = exp;
        }

        public QuestLoader.QuestRank Rank()
        {
            return rank;
        }

        public int CurrentExp()
        {
            return currentExp;
        }

        /// <summary>
        /// 描画用
        /// </summary>
        /// <returns></returns>
        public float Rate()
        {
            if(rank < QuestLoader.QuestRank.S)
                return currentExp * 1.0f / maxExp[(int)rank];

            return 1.0f;
        }

        public void AddExp(int exp)
        {
            if (rank >= QuestLoader.QuestRank.S)
                return;

            currentExp += exp;
            if (exp > maxExp[(int)rank])
            {
                currentExp -= maxExp[(int)rank];
                rank++;
            }
        }

        public void LoadSaveData(SaveData save)
        {
            this.currentExp = save.GetGuildInfo().currentExp;
            this.rank = save.GetGuildInfo().rank;
        }
    }
}
