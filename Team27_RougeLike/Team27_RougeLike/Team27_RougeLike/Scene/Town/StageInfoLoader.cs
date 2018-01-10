//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.13
// 内容　：Stageの情報を読み込むクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Team27_RougeLike.Scene.Town
{
    public struct StageInfo
    {
        public string name;
        public int totalFloor;
        public int baseSize;
        public int bossRange;
        public float limitTime;

        public int fileNum;
    }

    class StageInfoLoader
    {
        private bool isLoad;
        private List<StageInfo> stageInfo;

        public StageInfoLoader()
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isLoad = false;
            stageInfo = new List<StageInfo>();
            stageInfo.Clear();
        }

        /// <summary>
        /// 情報を読み込む
        /// </summary>
        public void LoadStageInfo()
        {
            FileStream fs = new FileStream(@"Content/" + "StageCSV/StageInfo.csv", FileMode.Open);      //設定ファイルを開く
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("shift_jis"));

            while (!sr.EndOfStream)                     //最後まで読み込む
            {
                string line = sr.ReadLine();            //一行つず読み込む
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (data[0] == "Start")                 //最初の欄がStartの場合は定義用、Skipする
                    continue;
                if (data[0] == "End")                   //End以降は資料として使われないので、脱出
                    break;

                StageInfo info = new StageInfo();
                info.name = data[1];
                info.totalFloor = int.Parse(data[2]);
                info.baseSize = int.Parse(data[3]);
                info.bossRange = int.Parse(data[4]);
                info.limitTime = float.Parse(data[5]);
                info.fileNum = int.Parse(data[6]);

                stageInfo.Add(info);
            }
            sr.Close();                                 //読み終わったらファイルをClose
            fs.Close();

            isLoad = true;                                                      //読み終わった
        }

        /// <summary>
        /// ロードが終わっているか
        /// </summary>
        /// <returns></returns>
        public bool IsLoad()
        {
            return isLoad;
        }

        /// <summary>
        /// Stageの情報を取得
        /// </summary>
        /// <returns></returns>
        public List<StageInfo> Stages()
        {
            return stageInfo;
        }
    }
}
