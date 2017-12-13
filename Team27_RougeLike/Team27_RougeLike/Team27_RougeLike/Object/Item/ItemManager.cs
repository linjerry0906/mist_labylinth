using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Team27_RougeLike.Object.Item.ItemEffects;

namespace Team27_RougeLike.Object.Item
{
    class ItemManager
    {
        private Dictionary<int, Item> equipments;
        private Dictionary<int, Item> consumptions;
        private string equipmentFilename;
        private string consuptionFilename;

        public ItemManager()
        {
            equipments = new Dictionary<int, Item>();
            consumptions = new Dictionary<int, Item>();

            equipmentFilename = "./ItemCSV/EquipmentItems.csv";
            consuptionFilename = "./ItemCSV/ConsuptionItems.csv";
        }

        public void Load(int[] equipmentIDs, int[] consuptionIDs)
        {
            Clear();

            //装備読み込み
            FileStream datefs = new FileStream(equipmentFilename, FileMode.Open);
            StreamReader equipmentDate = new StreamReader(datefs);

            //装備生成
            while (!equipmentDate.EndOfStream)
            {
                string line = equipmentDate.ReadLine();
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
            }
            equipmentDate.Close();



            //消費アイテム読み込み
            datefs = new FileStream(consuptionFilename, FileMode.Open);
            StreamReader consuptionDate = new StreamReader(datefs);

            while (!consuptionDate.EndOfStream)
            {
                string line = equipmentDate.ReadLine();
                string[] items = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length != 9) continue;

                int id = int.Parse(items[0]);

                if (!consuptionIDs.Contains(id)) continue;

                string itemName = items[1];
                string itemExplanation = items[2];
                int itemPrice = int.Parse(items[3]);
                int itemRare = int.Parse(items[4]);
                float itemWeight = float.Parse(items[5]);
                int amountLimit = int.Parse(items[6]);
                string type = items[7];
                int amount = int.Parse(items[8]);

                if (type == "recovary")
                {
                    consumptions[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amount,
                        ConsumptionItem.ItemEffectType.recovery,
                        new Recovery(amount));
                }
                else if (type == "damage")
                {
                    consumptions[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amount,
                        ConsumptionItem.ItemEffectType.damage,
                        new Damage(amount));
                }
                else if (type == "noEffect")
                {
                    consumptions[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amount,
                        ConsumptionItem.ItemEffectType.noEffect,
                        new NoEffect());
                }
            }
            consuptionDate.Close();
        }

        public List<Item> LoadPlayerItem(string[] saveItems)
            //saveItems{ id, addPower, addDefence, reinforcment }
        {
            List<Item> itemList = new List<Item>();
            //string配列読み込み
            int saveID = int.Parse(saveItems[0]);
            int addPower = int.Parse(saveItems[1]);
            int addDefence = int.Parse(saveItems[2]);
            int reinforcement = int.Parse(saveItems[3]);

            //装備読み込み
            FileStream datefs = new FileStream(equipmentFilename, FileMode.Open);
            StreamReader equipmentDate = new StreamReader(datefs);

            //装備生成
            while (!equipmentDate.EndOfStream)
            {
                string line = equipmentDate.ReadLine();
                string[] items = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length != 17) continue;

                int id = int.Parse(items[0]);

                if (saveID != id) continue;

                string itemName = items[1];
                string itemExplanation = items[2];
                int itemPrice = int.Parse(items[3]);
                int itemRare = int.Parse(items[4]);
                float itemWeight = float.Parse(items[5]);
                string type = items[6];
                int power = int.Parse(items[7]);
                int defence = int.Parse(items[8]);
                int reinforcementLimit = int.Parse(items[10]);
                int upPower = int.Parse(items[11]);
                int upDefence = int.Parse(items[12]);

                if (type == "Sword")
                {
                    itemList.Add(new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Sword,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, addPower, addDefence));
                }
                else if (type == "Bow")
                {
                    itemList.Add(new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Bow,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, addPower, addDefence));
                }
                else if (type == "Dagger")
                {
                    itemList.Add(new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Dagger,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, addPower, addDefence));
                }
                else if (type == "Shield")
                {
                    itemList.Add(new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Shield,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, addPower, addDefence));
                }
                else if (type == "Helm")
                {
                    itemList.Add(new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Helm,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, addPower, addDefence));
                }
                else if (type == "Armor")
                {
                    itemList.Add(new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Armor,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, addPower, addDefence));
                }
                else if (type == "Glove")
                {
                    itemList.Add(new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Glove,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, addPower, addDefence));
                }
                else if (type == "Shoes")
                {
                    itemList.Add(new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Shoes,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, addPower, addDefence));
                }
            }
            equipmentDate.Close();

            return itemList;
        }

        public void Clear()
        {
            equipments.Clear();
            consumptions.Clear();
        }

        public Item GetEquipmentItem(int id)
        {
            return equipments[id].Clone();
        }

        public Item GetConsuptionItem(int id)
        {
            return consumptions[id].Clone();
        }
    }
}
