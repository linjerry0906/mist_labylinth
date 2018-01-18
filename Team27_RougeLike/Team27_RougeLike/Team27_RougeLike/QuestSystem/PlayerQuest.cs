//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：Playerが受注しているクエスト
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.QuestSystem
{
    class PlayerQuest
    {
        private static readonly int MAX_QUEST = 10;
        private List<Quest> quests = new List<Quest>();

        private Dictionary<int, int> killEnemy;

        public PlayerQuest()
        {
            killEnemy = new Dictionary<int, int>();
        }

        public bool AddQuest(Quest quest)
        {
            if (quests.Exists(q => q.QuestID() == quest.QuestID()))
                return false;

            quests.Add(quest);
            return true;
        }

        public void DeleteQuest(int id)
        {
            quests.RemoveAll(q => q.QuestID() == id);
        }

        public List<Quest> CurrentQuest()
        {
            return quests;
        }

        public void LoadFromSave(SaveData save)
        {
            quests = save.GetQuest();
        }

        public void Limit(ref int currentQuest, ref int maxQuest)
        {
            currentQuest = quests.Count;
            maxQuest = MAX_QUEST;
        }

        /// <summary>
        /// 倒した数を追加
        /// </summary>
        /// <param name="id">敵のID</param>
        public void AddKill(int id)
        {
            if (!killEnemy.ContainsKey(id))
            {
                killEnemy.Add(id, 1);
                return;
            }
            killEnemy[id]++;
        }

        /// <summary>
        /// クエスト情報更新
        /// </summary>
        public void UpdateQuestProcess()
        {
            foreach(Quest q in quests)
            {
                if (q is CollectQuest)
                    continue;

                for(int i = 0; i < q.RequireID().Length; i++)
                {
                    int id = q.RequireID()[i];
                    if (killEnemy.ContainsKey(id))
                        q.AddAmount(id, killEnemy[id]);
                }
            }
            killEnemy.Clear();
        }
    }
}
