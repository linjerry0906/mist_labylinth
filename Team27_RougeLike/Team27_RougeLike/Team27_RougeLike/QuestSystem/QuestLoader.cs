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
        private bool isLoad;
        private static readonly int MAX_QUEST = 10;
        private static List<Quest> activeQuest;
        private static List<Quest> randomQuest;

        public QuestLoader()
        {
            activeQuest = new List<Quest>();
            randomQuest = new List<Quest>();
        }

        public void Initialize()
        {
            activeQuest.Clear();
            randomQuest.Clear();
            isLoad = false;
        }

        public void Load(DungeonProcess dungeonProcess)
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
                if (!dungeonProcess.HasKey(dungeonNo))  //Clearしたことなかったら出現しない
                    continue;
                if (dungeonProcess.GetProcess()[dungeonNo] < floor)             //階層達していない
                    continue;

                int id = int.Parse(data[0]);
                string type = data[1];
                string name = data[2];
                string info = data[3];
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
                if(count > 0)
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

                if (type == "Collection")
                {
                    Quest collectionQuest = new CollectQuest(
                        id, name, info, difficulty, money, award,
                        requires, dungeonNo, floor);
                    activeQuest.Add(collectionQuest);
                }
            }
            sr.Close();                                 //読み終わったらファイルをClose
            fs.Close();

            isLoad = true;                              //読み終わった
        }

        public void RandomQuest(GameDevice gameDevice)
        {
            if(activeQuest.Count < MAX_QUEST)
            {
                for (int i = 0; i < activeQuest.Count; i++)
                {
                    randomQuest.Add(activeQuest[i].Clone());
                }
                return;
            }

            for (int i = 0; i < MAX_QUEST; i++)
            {
                int index = gameDevice.Random.Next(0, activeQuest.Count);
                if (randomQuest.Exists(q => q.QuestID() == activeQuest[index].QuestID()))
                    continue;

                randomQuest.Add(activeQuest[index].Clone());
            }
        }

        public bool IsLoad()
        {
            return isLoad;
        }


        public List<Quest> GetRandomQuest()
        {
            return randomQuest;
        }

        public void RemoveQuest(int id)
        {
            if (randomQuest.Exists(q => q.QuestID() == id))
                randomQuest.RemoveAll(q => q.QuestID() == id);
        }
    }
}
