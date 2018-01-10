﻿//--------------------------------------------------------------------------------------------------
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

namespace Team27_RougeLike.Map
{
    class StageInfoLoader
    {
        private bool isItemLoad;            //Item読み込んだフラグ

        public StageInfoLoader()
        {
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            isItemLoad = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemManager">アイテム管理者</param>
        /// <param name="floor">階層</param>
        public void LoadFloorItem(ItemManager itemManager, int floor)
        {
            FileStream fs = new FileStream(@"Content/" + "StageCSV/StageItem.csv", FileMode.Open);      //設定ファイルを開く
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
        /// Itemが読み込んだか
        /// </summary>
        /// <returns></returns>
        public bool IsItemLoad()
        {
            return isItemLoad;
        }
    }
}
