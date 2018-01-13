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

            equipmentFilename = @"Content/" + "ItemCSV/EquipmentItems.csv";
            consuptionFilename = @"Content/" + "ItemCSV/ConsumptionItems.csv";
        }

        public void LoadAll()
        {
            int equipmentNum = 8;
            int consumptionNum = 14;

            int[] equipmentIDs = new int[equipmentNum];
            for(int i = 1; i <= equipmentNum; i++)
            {
                equipmentIDs[i - 1] = i;
            }

            int[] consumptionIDs = new int[consumptionNum];
            for(int i = 1; i <= consumptionNum; i++)
            {
                consumptionIDs[i - 1] = i;
            }

            Load(equipmentIDs, consumptionIDs);
        }

        public void Load(int[] equipmentIDs, int[] consuptionIDs)
        {
            Clear();

            //装備読み込み
            FileStream datefs = new FileStream(equipmentFilename, FileMode.Open);
            StreamReader equipmentDate = new StreamReader(datefs, Encoding.GetEncoding("shift_jis"));

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
            datefs.Close();


            //消費アイテム読み込み
            datefs = new FileStream(consuptionFilename, FileMode.Open);
            StreamReader consuptionDate = new StreamReader(datefs, Encoding.GetEncoding("shift_jis"));

            while (!consuptionDate.EndOfStream)
            {
                string line = consuptionDate.ReadLine();
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
            datefs.Close();
        }

        //セーブデータからアイテム再現
        public List<Item> LoadSaveItem(List<string[]> saveItems)
            //saveItem{ 種類, id, addPower, addDefence, reinforcment }
        {
            List<Item> save = new List<Item>();

            for (int i = 0; i < saveItems.Count; i++)
            {
                //string配列読み込み
                string[] saveItem = saveItems[i];
                if (saveItem == null)
                {
                    save.Add(null);
                    continue;
                }
                string kind = saveItem[0];
                int saveID = int.Parse(saveItem[1]);

                if (kind == "Weapon" || kind == "Protection")
                {
                    int addPower = int.Parse(saveItem[2]);
                    int addDefence = int.Parse(saveItem[3]);
                    int reinforcement = int.Parse(saveItem[4]);

                    //装備読み込み
                    FileStream datefs = new FileStream(equipmentFilename, FileMode.Open);
                    StreamReader equipmentDate = new StreamReader(datefs, Encoding.GetEncoding("shift_jis"));

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
                            save.Add(new WeaponItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Sword,
                                power, defence, reinforcement, reinforcementLimit,
                                upPower, upDefence, addPower, addDefence));
                        }
                        else if (type == "Bow")
                        {
                            save.Add(new WeaponItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Bow,
                                power, defence, reinforcement, reinforcementLimit,
                                upPower, upDefence, addPower, addDefence));
                        }
                        else if (type == "Dagger")
                        {
                            save.Add(new WeaponItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Dagger,
                                power, defence, reinforcement, reinforcementLimit,
                                upPower, upDefence, addPower, addDefence));
                        }
                        else if (type == "Shield")
                        {
                            save.Add(new WeaponItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Shield,
                                power, defence, reinforcement, reinforcementLimit,
                                upPower, upDefence, addPower, addDefence));
                        }
                        else if (type == "Helm")
                        {
                            save.Add(new ProtectionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Helm,
                                power, defence, reinforcement, reinforcementLimit,
                                upPower, upDefence, addPower, addDefence));
                        }
                        else if (type == "Armor")
                        {
                            save.Add(new ProtectionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Armor,
                                power, defence, reinforcement, reinforcementLimit,
                                upPower, upDefence, addPower, addDefence));
                        }
                        else if (type == "Glove")
                        {
                            save.Add(new ProtectionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Glove,
                                power, defence, reinforcement, reinforcementLimit,
                                upPower, upDefence, addPower, addDefence));
                        }
                        else if (type == "Shoes")
                        {
                            save.Add(new ProtectionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Shoes,
                                power, defence, reinforcement, reinforcementLimit,
                                upPower, upDefence, addPower, addDefence));
                        }
                    }
                    equipmentDate.Close();
                    datefs.Close();
                }
                else if (kind == "Consumption")
                {

                    //消費アイテム読み込み
                    FileStream datefs = new FileStream(consuptionFilename, FileMode.Open);
                    StreamReader consuptionDate = new StreamReader(datefs, Encoding.GetEncoding("shift_jis"));

                    while (!consuptionDate.EndOfStream)
                    {
                        string line = consuptionDate.ReadLine();
                        string[] items = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (items.Length != 9) continue;

                        int id = int.Parse(items[0]);

                        if (saveID != id) continue;

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
                            save.Add(new ConsumptionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, amount,
                                ConsumptionItem.ItemEffectType.recovery,
                                new Recovery(amount)));
                        }
                        else if (type == "damage")
                        {
                            save.Add(new ConsumptionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, amount,
                                ConsumptionItem.ItemEffectType.damage,
                                new Damage(amount)));
                        }
                        else if (type == "noEffect")
                        {
                            save.Add(new ConsumptionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, amount,
                                ConsumptionItem.ItemEffectType.noEffect,
                                new NoEffect()));
                        }
                    }
                    consuptionDate.Close();
                    datefs.Close();
                }
            }

            return save;
        }

        //Dictionary初期化
        public void Clear()
        {
            equipments.Clear();
            consumptions.Clear();
        }

        //指定された装備アイテムを送る
        public Item GetEquipmentItem(int id)
        {
            return equipments[id].Clone();
        }

        //指定された消費アイテムを送る
        public Item GetConsuptionItem(int id)
        {
            return consumptions[id].Clone();
        }

        //Dictionary内からランダムに装備アイテムを送る
        public Item GetEquipmentItem()
        {
            if (equipments.Count <= 0)
                return null;

            int id = new Random().Next(0, equipments.Count);

            return equipments.Values.ToList<Item>()[id].Clone();
        }

        //Dictionary内からランダムに消費アイテムを送る
        public Item GetConsuptionitem()
        {
            if (consumptions.Count <= 0)
                return null;
            int id = new Random().Next(0, consumptions.Count);

            return consumptions.Values.ToList<Item>()[id].Clone();
        }

        //Store用 装備アイテムListを送る
        public List<Item> GetEquipmentList()
        {
            List<Item> equipmentList = new List<Item>();
            foreach(var page in equipments)
            {
                equipmentList.Add(page.Value);
            }

            return equipmentList;
        }

        //Store用　消費アイテムListを送る
        public List<Item> GetConsumptionList()
        {
            List<Item> consumptionList = new List<Item>();
            foreach (var page in consumptions)
            {
                consumptionList.Add(page.Value);
            }

            return consumptionList;
        }

        public void Debug()
        {
            foreach (var page in equipments)
            {
                Console.WriteLine(page.Key);
                Console.WriteLine(page.Value.GetType().ToString());
                Console.WriteLine(page.Value.GetItemRare());
            }

            foreach (var page in consumptions)
            {
                Console.WriteLine(page.Key);
                Console.WriteLine(page.Value.GetType().ToString());
                Console.WriteLine(page.Value.GetItemRare());
            }
        }
    }
}
