using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Team27_RougeLike.Device
{
    class SaveData
    {
        private GameDevice gameDevice;

        public SaveData(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
        }

        public void Load()
        {
            //if (!File.Exists("score.txt"))
            //{
            //    return;
            //}

            ////usingを使うと自動的File closeしてくれる
            ////他のエラー処理も
            //using (StreamReader sr = new StreamReader("score.txt"))
            //{
            //    string str = sr.ReadLine();
            //    int num;
            //    bool result = int.TryParse(str, out num);
            //    if (result)
            //    {
            //        gameDevice.HiScore = num;
            //    }
            //}
        }

        public void Save()
        {
            //using (StreamWriter sw = new StreamWriter("score.txt"))
            //{
            //    sw.Write(gameDevice.HiScore);
            //}
        }
    }
}
