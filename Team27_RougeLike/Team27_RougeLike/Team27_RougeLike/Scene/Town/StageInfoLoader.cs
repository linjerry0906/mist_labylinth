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
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.Scene.Town
{
    public struct StageInfo
    {
        public int dungeonNo;           //ダンジョンの番号
        public string name;             //ダンジョン名
        public int totalFloor;          //総階層
        public int baseSize;            //基本サイズ
        public int bossRange;           //何階ごとにボス
        public float limitTime;         //制限時間

        public int fileNum;             //Item詳細の番号
        public string imageName;        //イメージのAsset名
        public int expandRate;       　 //拡大する比率

        public string wallTexture;      //壁テクスチャ
        public string groundTexture;    //地面テクスチャ
        public Vector3 fogColor;        //霧の色

        public Vector3 constractColor;  //対比する色
        public bool useParticle;        //環境パーティクル使用する
        public string bgmName;          //Bgmのアセット名
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
                info.dungeonNo = int.Parse(data[0]);
                info.name = data[1];
                info.totalFloor = int.Parse(data[2]);
                info.baseSize = int.Parse(data[3]);
                info.bossRange = int.Parse(data[4]);
                info.limitTime = float.Parse(data[5]);
                info.fileNum = int.Parse(data[6]);
                info.imageName = data[7];
                info.expandRate = int.Parse(data[8]);
                info.groundTexture = data[9];
                info.wallTexture = data[10];
                info.fogColor = new Vector3(float.Parse(data[11]), float.Parse(data[12]), float.Parse(data[13]));
                info.useParticle = false;
                if (data[14] == "TRUE")
                {
                    info.useParticle = true;
                }
                info.constractColor = new Vector3(int.Parse(data[15]), int.Parse(data[16]), int.Parse(data[17]));
                info.bgmName = data[18];

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
