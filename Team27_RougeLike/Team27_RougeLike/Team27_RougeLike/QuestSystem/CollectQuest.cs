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
            int difficulty, int gainMoney, int[] awardID, List<Requirement> requires)
            : base(id, name, explanation, difficulty, gainMoney, awardID)
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
            foreach(Requirement r in requires)
            {
                if (r.currentAmount < r.requireAmount)  //クリアされてない条件があればFalse
                {
                    isClear = false;
                    return;
                }
            }
            isClear = true;
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
