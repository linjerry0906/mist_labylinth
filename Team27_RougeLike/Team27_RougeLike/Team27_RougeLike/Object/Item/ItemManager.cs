using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Team27_RougeLike.Object.Item
{
    class ItemManager
    {
        private Dictionary<int, Item> equipments;

        public ItemManager()
        {
            equipments = new Dictionary<int, Item>();
        }

        public void Load(string filename, int[] equipmentIDs)
        {

            //.csv付け忘れ対策
            if (!filename.Contains(".csv"))
            {
                filename = filename + ".csv";
            }

            FileStream datefs = new FileStream(filename, FileMode.Open);
            StreamReader dateSr = new StreamReader(datefs);

            while(!dateSr.EndOfStream)
            {
                string line = dateSr.ReadLine();
                string[] items = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length != 17) continue;

                int id = int.Parse(items[0]);

                if (!equipmentIDs.Contains(id)) continue;

                string itemName = items[1];
                string itemExplanation = items[2];
                int itemPrice = int.Parse(items[3]);
                int itemRare = int.Parse(items[4]);
                float itemWeight = float.Parse(items[5]);
                string type = items[6];
                int power = int.Parse(items[7]);
                int defence = int.Parse(items[8]);
                int reinforcement = int.Parse(items[9]);
                int reinforcementLimit = int.Parse(items[10]);
                int upPower = int.Parse(items[11]);
                int upDefence = int.Parse(items[12]);
                int randMinP = int.Parse(items[13]);
                int randMaxP = int.Parse(items[14]);
                int randMinD = int.Parse(items[15]);
                int randMaxD = int.Parse(items[16]);

                if (type == "Sword")
                {
                    equipments[id] = new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Sword,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Bow")
                {
                    equipments[id] = new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Bow,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Dagger")
                {
                    equipments[id] = new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Dagger,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Shield")
                {
                    equipments[id] = new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Shield,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Helm")
                {
                    equipments[id] = new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Helm,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Armor")
                {
                    equipments[id] = new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Armor,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Glove")
                {
                    equipments[id] = new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Glove,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Shoes")
                {
                    equipments[id] = new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Shoes,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                dateSr.Close();
            }
        }
    }
}
