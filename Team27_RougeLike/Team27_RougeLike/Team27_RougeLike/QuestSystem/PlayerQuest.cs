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

        public PlayerQuest()
        {
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
    }
}
