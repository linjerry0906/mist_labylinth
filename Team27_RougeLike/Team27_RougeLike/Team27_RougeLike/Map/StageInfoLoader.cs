//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.13
// 内容　：Stageの情報を読み込むクラス（落ちるアイテムの設定とか）
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    class StageItemEnemyLoader
    {
        private bool isItemLoad;            //Item読み込んだフラグ
        private bool isMonsterLoad;         //敵の配置とスポナーを読み込んだフラグ
        private bool isBossBGMLoad;         //BossBgmロードしたか
        private string bossBgm;

        public StageItemEnemyLoader()
        {
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            isItemLoad = false;
            isMonsterLoad = false;
            isBossBGMLoad = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemManager">アイテム管理者</param>
        /// <param name="floor">階層</param>
        public void LoadFloorItem(ItemManager itemManager, int fileNum, int floor)
        {
            string fileName = "StageItem_" + fileNum + ".csv";
            FileStream fs = new FileStream(@"Content/" + "StageCSV/" + fileName, FileMode.Open);      //設定ファイルを開く
            StreamReader sr = new StreamReader(fs);
            List<int> equipList = new List<int>();              //落ちる可能性のある装備リスト
            List<int> consuptionList = new List<int>();         //落ちる可能性のある使用アイテムリスト

            while (!sr.EndOfStream)                     //最後まで読み込む
            {
                string line = sr.ReadLine();            //一行つず読み込む
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (data[0] == "Start")                 //最初の欄がStartの場合は定義用、Skipする
                    continue;
                if (data[0] == "End")                   //End以降は資料として使われないので、脱出
                    break;

                int dataFloor = int.Parse(data[1]);     //階層を読み取る
                if (dataFloor != floor)                 //違う場合
                {
                    if (dataFloor > floor)              //より下の階層だったら続ける必要がない
                        break;
                    continue;
                }

                int id = int.Parse(data[3]);            //IDを読み取る
                if(data[2] == "equipment")              //装備アイテムの場合
                {
                    equipList.Add(id);
                }

                if (data[2] == "consuption")            //使用アイテムの場合
                {
                    consuptionList.Add(id);
                }
            }
            sr.Close();                                 //読み終わったらファイルをClose
            fs.Close();

            itemManager.Load(equipList.ToArray(), consuptionList.ToArray());        //Listを渡してDictionaryを作ってくれる
            isItemLoad = true;                                                      //読み終わった
        }

        /// <summary>
        /// 敵の配置を読み込む処理
        /// </summary>
        public void LoadFloorEnemy(EnemySettingManager enemySettingManager, int fileNum, int floor)
        {
            string fileName = "StageEnemy_" + fileNum + ".csv";
            FileStream fs = new FileStream(@"Content/" + "StageCSV/" + fileName, FileMode.Open);      //設定ファイルを開く
            StreamReader sr = new StreamReader(fs);

            while (!sr.EndOfStream)                     //最後まで読み込む
            {
                string line = sr.ReadLine();            //一行つず読み込む
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (data[0] == "Start")                 //最初の欄がStartの場合は定義用、Skipする
                    continue;
                if (data[0] == "End")                   //End以降は資料として使われないので、脱出
                    break;

                int dataFloor = int.Parse(data[0]);
                if (dataFloor < floor)                  //指定の階層前は読み込まない
                    continue;
                if (dataFloor > floor)                  //指定の階層以降は読み込まない
                    break;

                EnemySetting setting;
                setting.isBoss = (data[1] == "TRUE") ? true : false;
                setting.rate = int.Parse(data[2]);
                setting.max = int.Parse(data[3]);
                setting.amountPerSpawn = int.Parse(data[4]);

                int idAmount = data.Length - 5;                 //敵の種類を解析処理
                setting.ids = new int[idAmount];
                for (int i = 0; i < setting.ids.Length; i++)
                {
                    setting.ids[i] = int.Parse(data[5 + i]);
                }
                enemySettingManager.Add(setting);
            }
            sr.Close();                                        //読み終わったらファイルをClose
            fs.Close();

            isMonsterLoad = true;
        }

        /// <summary>
        /// Itemが読み込んだか
        /// </summary>
        /// <returns></returns>
        public bool IsItemLoad()
        {
            return isItemLoad;
        }

        /// <summary>
        /// 敵のは配置は読み込んだか
        /// </summary>
        /// <returns></returns>
        public bool IsEnemyLoad()
        {
            return isMonsterLoad;
        }

        /// <summary>
        /// ボスBGMをロードする
        /// </summary>
        /// <param name="dungeonNum">ダンジョン番号</param>
        /// <param name="floor">階層</param>
        /// <param name="sound">サウンドクラス</param>
        public void LoadBossBGM(int dungeonNum, int floor, Sound sound)
        {
            FileStream fs = new FileStream(@"Content/" + "StageCSV/Boss_bgm.csv", FileMode.Open);      //設定ファイルを開く
            StreamReader sr = new StreamReader(fs);

            bossBgm = "Battle-ricercare";

            while (!sr.EndOfStream)                     //最後まで読み込む
            {
                string line = sr.ReadLine();            //一行つず読み込む
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (data[0] == "Start")                 //最初の欄がStartの場合は定義用、Skipする
                    continue;
                if (data[0] == "End")                   //End以降は資料として使われないので、脱出
                    break;

                int dataDungeon = int.Parse(data[0]);
                int dataFloor = int.Parse(data[1]);
                if (dataDungeon != dungeonNum)                 //指定のダンジョン以外は読み込まない
                    continue;
                if (dataFloor != floor)                        //指定の階層以外は読み込まない
                    continue;

                bossBgm = data[2];
                break;
            }
            sr.Close();                                        //読み終わったらファイルをClose
            fs.Close();

            sound.LoadBGM(bossBgm, "./Sound/BGM/");

            isBossBGMLoad = true;
        }

        /// <summary>
        /// BGMロードしたか
        /// </summary>
        /// <returns></returns>
        public bool IsBossBGMLoad()
        {
            return isBossBGMLoad;
        }

        /// <summary>
        /// BGM名
        /// </summary>
        /// <returns></returns>
        public string BGMName()
        {
            return bossBgm;
        }
    }
}
