//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：アイテム集めQuest
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.QuestSystem
{
    class CollectQuest : Quest
    {
        private List<Requirement> requires;         //クリア条件

        public CollectQuest(
            int id, string name, string explanation,
            int difficulty, int gainMoney, int[] awardID, List<Requirement> requires,
            int dungeonProcess, int dungeonFloor, int guildExp)
            : base(id, name, explanation, difficulty, gainMoney, awardID, dungeonProcess, dungeonFloor, guildExp)
        {
            this.requires = requires;
        }

        public override void AddAmount(int id, int amount)
        {
            //Itemは直接Iventoryで設定
            return;
        }

        public override void CheckClear()
        {
            foreach (Requirement r in requires)
            {
                if (r.currentAmount < r.requireAmount)  //クリアされてない条件があればFalse
                {
                    isClear = false;
                    return;
                }
            }
            isClear = true;
        }

        public override Quest Clone()
        {
            int[] award = null;
            if (awardItemID != null)
            {
                award = new int[awardItemID.Length];
                for (int i = 0; i < awardItemID.Length; i++)
                {
                    awardItemID[i] = awardItemID[i];
                }
            }

            List<Requirement> require = new List<Requirement>(requires);
            return new CollectQuest(questID, name, explanation, difficulty, gainMoney, award, requires, dungeonProcess, floorProcess, guildExp);
        }

        public override List<Requirement> CurrentState()
        {
            return requires;
        }

        public override int[] RequireID()
        {
            int[] ids = new int[requires.Count];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = requires[i].id;
            }
            return ids;
        }

        public override void SetItemAmount(int id, int amount)
        {
            foreach (Requirement r in requires)
            {
                if (r.id != id)
                    continue;

                r.SetCurrentAmount(amount);
            }
        }
    }
}
