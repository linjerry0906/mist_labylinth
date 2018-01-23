using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Team27_RougeLike.QuestSystem
{
    class EnemyNameLoader
    {
        private static Dictionary<int, string> enemyName;

        public EnemyNameLoader()
        {
            enemyName = new Dictionary<int, string>();
        }

        public void Clear()
        {
            enemyName.Clear();
        }

        public void Load()
        {
            FileStream fs = new FileStream(@"Content/" + "EnemysCSV/Enemy.csv", FileMode.Open);      //設定ファイルを開く
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("shift_jis"));

            //ごみ捨て

            var Dust = sr.ReadLine();

            while (!sr.EndOfStream)                     //最後まで読み込む
            {
                string line = sr.ReadLine();            //一行つず読み込む
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int id = int.Parse(data[0]);
                string name = data[10];
                if(!enemyName.ContainsKey(id))
                    enemyName.Add(id, name);
            }

            sr.Close();
            fs.Close();
        }

        public string GetEnemyName(int id)
        {
            string name = "エラー";
            if (enemyName.ContainsKey(id))
                name = enemyName[id];

            return name;
        }
    }
}
