//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.1.17
// 内容  ：Questを読み込むクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.QuestSystem
{
    class QuestLoader
    {
        public enum QuestRank
        {
            F = 0,
            E,
            D,
            C,
            B,
            A,
            S,
            NULL,
        }
        private bool isLoad;
        private static readonly int MAX_QUEST = 10;
        private QuestRank currentRank;
        private static List<Quest> activeQuest;
        private static List<List<Quest>> randomQuest;

        public QuestLoader()
        {
            activeQuest = new List<Quest>();
            randomQuest = new List<List<Quest>>();
            for (int i = 0; i < (int)QuestRank.NULL; i++)
            {
                randomQuest.Add(new List<Quest>());
            }
        }

        public void Initialize()
        {
            currentRank = QuestRank.F;
            activeQuest.Clear();
            for (int i = 0; i < (int)QuestRank.NULL; i++)
            {
                randomQuest[i].Clear();
            }
            isLoad = false;
        }

        public void Load(DungeonProcess dungeonProcess, bool loadAll = false)
        {
            FileStream fs = new FileStream(@"Content/" + "QuestCSV/Quest.csv", FileMode.Open);      //設定ファイルを開く
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("shift_jis"));

            while (!sr.EndOfStream)                     //最後まで読み込む
            {
                string line = sr.ReadLine();            //一行つず読み込む
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (data[0] == "Start")                 //最初の欄がStartの場合は定義用、Skipする
                    continue;
                if (data[0] == "End")                   //End以降は資料として使われないので、脱出
                    break;

                int dungeonNo = int.Parse(data[9]);
                int floor = int.Parse(data[10]);
                if (!loadAll)
                {
                    if (!dungeonProcess.HasKey(dungeonNo) && floor > 0)  //Clearしたことなかったら出現しない
                        continue;
                    if (dungeonProcess.HasKey(dungeonNo) &&
                        dungeonProcess.GetProcess()[dungeonNo] < floor)  //階層達していない
                        continue;
                }

                int id = int.Parse(data[0]);
                string type = data[1];
                string name = data[2];
                string info = data[3];
                info = info.Replace("nl", "\n");
                int difficulty = int.Parse(data[4]);
                int money = int.Parse(data[5]);

                #region 報酬アイテム
                int count = 0;
                if (data[6] != "null")
                    count++;
                if (data[7] != "null")
                    count++;
                if (data[8] != "null")
                    count++;
                int[] award = null;
                if (count > 0)
                {
                    award = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        award[i] = int.Parse(data[6 + i]);
                    }
                }
                #endregion

                #region 条件
                count = 0;
                if (data[11] != "null")
                    count++;
                if (data[13] != "null")
                    count++;
                if (data[15] != "null")
                    count++;
                List<Requirement> requires = new List<Requirement>();
                for (int i = 0; i < count; i++)
                {
                    Requirement require = new Requirement(
                        int.Parse(data[i * 2 + 11]),
                        int.Parse(data[i * 2 + 12]));
                    requires.Add(require);
                }
                #endregion

                int exp = int.Parse(data[17]);

                if (type == "Collection")
                {
                    Quest collectionQuest = new CollectQuest(
                        id, name, info, difficulty, money, award,
                        requires, dungeonNo, floor, exp);
                    activeQuest.Add(collectionQuest);
                }
                else if (type == "Battle")
                {
                    Quest battleQuest = new BattleQuest(
                        id, name, info, difficulty, money, award,
                        requires, dungeonNo, floor, exp);
                    activeQuest.Add(battleQuest);
                }
            }
            sr.Close();                                 //読み終わったらファイルをClose
            fs.Close();

            isLoad = true;                              //読み終わった
        }

        /// <summary>
        /// Questをランダムに発生
        /// </summary>
        /// <param name="gameDevice"></param>
        public void RandomQuest(GameDevice gameDevice, PlayerGuildRank playerRank)
        {
            for (int i = 0; i < activeQuest.Count; i++)
            {
                int rank = activeQuest[i].Difficulty();
                if (rank > (int)playerRank.Rank())
                    continue;

                randomQuest[rank].Add(activeQuest[i].Clone());
            }

            for (int i = 0; i < (int)QuestRank.NULL; i++)
            {
                if (randomQuest[i].Count <= MAX_QUEST)
                    continue;

                int count = randomQuest[i].Count - MAX_QUEST;
                for (int amount = 0; amount < count; amount++)
                {
                    int removeIndex = gameDevice.Random.Next(0, randomQuest[i].Count);
                    randomQuest[i].RemoveAt(removeIndex);
                }
            }
        }

        /// <summary>
        /// Loadしたか
        /// </summary>
        /// <returns></returns>
        public bool IsLoad()
        {
            return isLoad;
        }

        /// <summary>
        /// 現在のランクのリスト
        /// </summary>
        /// <returns></returns>
        public List<Quest> GetRandomQuest()
        {
            return randomQuest[(int)currentRank];
        }

        /// <summary>
        /// IDからクエストを生成
        /// </summary>
        /// <param name="id">QuestID</param>
        /// <returns></returns>
        public Quest GetQuest(int id)
        {
            return activeQuest.Find(e => e.QuestID() == id).Clone();
        }

        /// <summary>
        /// 現在のランクにあるIDを消す
        /// </summary>
        /// <param name="id"></param>
        public void RemoveQuest(int id)
        {
            if (randomQuest[(int)currentRank].Exists(q => q.QuestID() == id))
                randomQuest[(int)currentRank].RemoveAll(q => q.QuestID() == id);
        }

        /// <summary>
        /// 見るクエストを変更
        /// </summary>
        /// <param name="rank"></param>
        public void ChangeCurrentRank(QuestRank rank)
        {
            currentRank = rank;
        }
    }
}
