using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Team27_RougeLike.Object.Character
{
    class AttackLoader
    {
        private string attackFilename;
        private Dictionary<int, AttackBase> attacks = new Dictionary<int, AttackBase>();

        public AttackBase GetAttack(int ID)
        {
            return attacks[ID];
        }

        public void Initialize(CharacterManager characterManager)
        {
            attackFilename = @"Content/" + "AttackCSV/Attack.csv";
            FileStream fs = new FileStream(attackFilename, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("shift_jis"));

            //ごみ捨て
            sr.ReadLine();

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int id = int.Parse(data[0]);
                string name = data[1];
                float size = float.Parse(data[2]);
                string texName = data[3];
                Buff.buff buff =  (Buff.buff)Enum.Parse(typeof(Buff.buff),data[4]);
                string attackType = data[5];
                string seName = data[6];
                float startRange = float.Parse(data[7]);

                if (attackType == "Melee")
                {
                    attacks.Add(id, new MeleeAttack(size, texName,buff, seName,startRange,characterManager));
                    continue;
                }
                if(attackType == "Ranged")
                {
                    attacks.Add(id, new RangeAttack(size, texName, buff, seName,startRange,characterManager));
                    continue;
                }
            }
        }
    }
}
