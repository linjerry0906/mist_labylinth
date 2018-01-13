using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Team27_RougeLike.Object
{
    class ExpLoader
    {
        public ExpLoader() { }

        public Dictionary<int,int> LoadExp()
        {
            string fileName = @"Content/" + "PlayerCSV/PlayerExp.csv";
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            Dictionary<int, int > exps = new Dictionary<int, int>();

            var Dust = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] split = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                exps.Add(int.Parse(split[0]), int.Parse(split[1]));
            }

            fs.Close();
            sr.Close();
            return exps;
        }
    }
}
