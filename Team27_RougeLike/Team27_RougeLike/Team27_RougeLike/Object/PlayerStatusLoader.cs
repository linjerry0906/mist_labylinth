using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Team27_RougeLike.Object
{
    class PlayerStatusLoader
    {
        public PlayerStatusLoader() { }

        public int[] LoadStatus()
        {
            string fileName = @"Content/" + "PlayerCSV/PlayerStatus.csv";
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            int[] status = new int[4];

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] split = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (split[0].Contains("Start"))
                    continue;
                if (split[0].Contains("End"))
                    break;

                status[0] = int.Parse(split[1]);
                status[1] = int.Parse(split[2]);
                status[2] = int.Parse(split[3]);
                status[3] = int.Parse(split[4]);
            }

            return status;
        }
    }
}
