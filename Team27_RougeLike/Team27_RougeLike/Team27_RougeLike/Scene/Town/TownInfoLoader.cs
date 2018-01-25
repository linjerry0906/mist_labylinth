//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.13
// 内容　：Townの情報を読み込むクラス（落ちるアイテムの設定とか）
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.Scene.Town
{
    class TownInfoLoader
    {
        private bool isItemLoad;

        public TownInfoLoader()
        {
        }

        public void Initialize()
        {
            isItemLoad = false;
        }

        public void LoadStoreItem(ItemManager itemManager, DungeonProcess dungeonProcess)
        {
            FileStream fs = new FileStream(@"Content/" + "StoreItemCSV/StoreItem.csv", FileMode.Open);      //設定ファイルを開く
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

                int dataDungeon = int.Parse(data[1]);   //ダンジョン番号
                int dataFloor = int.Parse(data[2]);     //階層を読み取る

                if (dataDungeon != 0 && dataFloor != 0)
                {
                    //攻略したことのないダンジョン以降のアイテムを読み込む必要がない
                    if (!dungeonProcess.HasKey(dataDungeon))
                        continue;

                    //到達していない階層のアイテムを読み込む必要がない
                    if (dataFloor > dungeonProcess.GetProcess()[dataDungeon])
                        continue;
                }


                int id = int.Parse(data[4]);            //IDを読み取る
                if (data[3] == "equipment")             //装備アイテムの場合
                {
                    equipList.Add(id);
                }

                if (data[3] == "consuption")            //使用アイテムの場合
                {
                    consuptionList.Add(id);
                }
            }
            sr.Close();                                 //読み終わったらファイルをClose
            fs.Close();

            itemManager.Load(equipList.ToArray(), consuptionList.ToArray());        //Listを渡してDictionaryを作ってくれる
            isItemLoad = true;                                                      //読み終わった
        }

        public bool IsItemLoad()
        {
            return isItemLoad;
        }
    }
}
