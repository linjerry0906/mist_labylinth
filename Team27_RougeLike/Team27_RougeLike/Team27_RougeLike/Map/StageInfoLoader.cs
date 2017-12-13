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
        private bool isItemLoad;

        public StageInfoLoader()
        {
        }

        public void Initialize()
        {
            isItemLoad = false;
        }

        public void LoadFloorItem(ItemManager itemManager, int floor)
        {
            FileStream fs = new FileStream(@"Content/" + "StageCSV/StageItem.csv", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<int> equipList = new List<int>();
            List<int> consuptionList = new List<int>();

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (data[0] == "Start")
                    continue;
                if (data[0] == "End")
                    break;

                int dataFloor = int.Parse(data[1]);     //階層を読み取る
                if (dataFloor != floor)                 //違う場合
                {
                    if (dataFloor > floor)              //より下の階層だったら続ける必要がない
                        break;
                    continue;
                }

                int id = int.Parse(data[3]);
                if(data[2] == "equipment")
                {
                    equipList.Add(id);
                }

                if (data[2] == "consuption")
                {
                    consuptionList.Add(id);
                }
            }
            sr.Close();

            itemManager.Load(equipList.ToArray(), consuptionList.ToArray());
            isItemLoad = true;
        }

        public bool IsItemLoad()
        {
            return isItemLoad;
        }
    }
}
