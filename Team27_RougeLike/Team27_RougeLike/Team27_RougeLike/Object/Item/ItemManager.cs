﻿using System;
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
        private Dictionary<int, Item> accessarys;
        private Dictionary<int, Item> equipment;
        private Dictionary<int, Item> consumption;
        private Dictionary<int, Item> accessary;
        private string equipmentFilename;
        private string consuptionFilename;
        private string accessaryFilename;

        public ItemManager()
        {
            equipments = new Dictionary<int, Item>();
            consumptions = new Dictionary<int, Item>();
            accessarys = new Dictionary<int, Item>();
            equipment = new Dictionary<int, Item>();
            consumption = new Dictionary<int, Item>();
            accessary = new Dictionary<int, Item>();

            equipmentFilename = @"Content/" + "ItemCSV/EquipmentItems.csv";
            consuptionFilename = @"Content/" + "ItemCSV/ConsumptionItems.csv";
            accessaryFilename = @"Content/" + "ItemCSV/AccessaryItem.csv";
        }

        public void LoadAll()
        {
            int equipmentNum = 56;//装備アイテムのIDで一番大きなID
            int consumptionNum = 91;//消費アイテムのIDで一番大きなID

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
                itemExplanation = itemExplanation.Replace("nl", "\n");
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
                itemExplanation = itemExplanation.Replace("nl", "\n");
                int itemPrice = int.Parse(items[3]);
                int itemRare = int.Parse(items[4]);
                float itemWeight = float.Parse(items[5]);
                int amountLimit = int.Parse(items[6]);
                string type = items[7];
                int amount = int.Parse(items[8]);

                if (type == "recovary")
                {
                    consumptions[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amountLimit,
                        ConsumptionItem.ItemEffectType.recovery,
                        new Recovery(amount));
                }
                else if (type == "damage")
                {
                    consumptions[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amountLimit,
                        ConsumptionItem.ItemEffectType.damage,
                        new Damage(amount));
                }
                else if (type == "arrow")
                {
                    consumptions[id] = ((new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amountLimit,
                        ConsumptionItem.ItemEffectType.arrow,
                        new ArrowEffect(amount))));
                }
                else if (type == "noEffect")
                {
                    consumptions[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amountLimit,
                        ConsumptionItem.ItemEffectType.noEffect,
                        new NoEffect());
                }
            }
            consuptionDate.Close();
            datefs.Close();

            //アクセサリーアイテム読み込み
            //datefs = new FileStream(accessaryFilename, FileMode.Open);
            //StreamReader accessaryDate = new StreamReader(datefs, Encoding.GetEncoding("shift_jis"));

            //while (!accessaryDate.EndOfStream)
            //{
            //    string line = accessaryDate.ReadLine();
            //    string[] items = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (items.Length != 9) continue;

            //    int id = int.Parse(items[0]);

            //    if (!accessaryIDs.Contains(id)) continue;

            //    string itemName = items[1];
            //    string itemExplanation = items[2];
            //    itemExplanation = itemExplanation.Replace("nl", "\n");
            //    int itemPrice = int.Parse(items[3]);
            //    int itemRare = int.Parse(items[4]);
            //    float itemWeight = float.Parse(items[5]);
            //    int amountLimit = int.Parse(items[6]);
            //    string type = items[7];
            //    int intType = -1;

            //    //アクセサリーアイテムの種類を追加したらここにif文を追加する。
            //    if (type == "Earring")
            //    {
            //        intType = 0;
            //    }

            //    accessarys[id] = (new AccessaryItem(id, itemName, itemExplanation, itemPrice, itemRare, itemWeight, amountLimit, intType));
            //}
            //accessaryDate.Close();
            //datefs.Close();

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
                        itemExplanation = itemExplanation.Replace("nl", "\n");
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
                    int stack = int.Parse(saveItem[2]);

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
                        itemExplanation = itemExplanation.Replace("nl", "\n");
                        int itemPrice = int.Parse(items[3]);
                        int itemRare = int.Parse(items[4]);
                        float itemWeight = float.Parse(items[5]);
                        int amountLimit = int.Parse(items[6]);
                        string type = items[7];
                        int amount = int.Parse(items[8]);

                        if (type == "recovary")
                        {
                            save.Add(new ConsumptionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, amountLimit,
                                ConsumptionItem.ItemEffectType.recovery,
                                new Recovery(amount)));
                        }
                        else if (type == "damage")
                        {
                            save.Add(new ConsumptionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, amountLimit,
                                ConsumptionItem.ItemEffectType.damage,
                                new Damage(amount)));
                        }
                        else if (type == "arrow")
                        {
                            ConsumptionItem arrow = ((new ConsumptionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, amountLimit,
                                ConsumptionItem.ItemEffectType.arrow,
                                new ArrowEffect(amount))));
                            arrow.SetStack(stack);
                            save.Add(arrow);
                        }
                        else if (type == "noEffect")
                        {
                            save.Add(new ConsumptionItem(id, itemName, itemExplanation,
                                itemPrice, itemRare, itemWeight, amountLimit,
                                ConsumptionItem.ItemEffectType.noEffect,
                                new NoEffect()));
                        }
                    }
                    consuptionDate.Close();
                    datefs.Close();
                }
                else if (kind == "Accessary")
                {
                    //アクセサリーアイテム読み込み
                    FileStream datefs = new FileStream(accessaryFilename, FileMode.Open);
                    StreamReader accessaryDate = new StreamReader(datefs, Encoding.GetEncoding("shift_jis"));

                    while(!accessaryDate.EndOfStream)
                    {
                        string line = accessaryDate.ReadLine();
                        string[] items = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (items.Length == 9) continue;

                        int id = int.Parse(items[0]);

                        if (saveID != id) continue;

                        string itemName = items[1];
                        string itemExplanation = items[2];
                        itemExplanation = itemExplanation.Replace("nl", "\n");
                        int itemPrice = int.Parse(items[3]);
                        int itemRare = int.Parse(items[4]);
                        float itemWeight = float.Parse(items[5]);
                        int amountLimit = int.Parse(items[6]);
                        string type = items[7];
                        int intType = -1;

                        //アクセサリーアイテムの種類を追加したらここにif文を追加する。
                        if (type == "Necklace")
                        {
                            intType = (int)AccessaryItem.Type.Necklace;
                        }
                        else if (type == "Book")
                        {
                            intType = (int)AccessaryItem.Type.Book;
                        }
                        else if (type == "Pet")
                        {
                            intType = (int)AccessaryItem.Type.Pet;
                        }
                        else if (type == "Sheath")
                        {
                            intType = (int)AccessaryItem.Type.Sheath;
                        }
                        else if (type == "Amulet")
                        {
                            intType = (int)AccessaryItem.Type.Amulet;
                        }

                        save.Add(new AccessaryItem(id, itemName, itemExplanation, itemPrice, itemRare, itemWeight, amountLimit, intType));
                    }
                    accessaryDate.Close();
                    datefs.Close();
                }
            }

            return save;
        }

        //指定されたアイテムをロード
        public void LoadEquipment(int selectID)
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

                if (selectID != id) continue;

                string itemName = items[1];
                string itemExplanation = items[2];
                itemExplanation = itemExplanation.Replace("nl", "\n");
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
                    equipment[id] = new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Sword,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Bow")
                {
                    equipment[id] = new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Bow,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Dagger")
                {
                    equipment[id] = new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Dagger,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Shield")
                {
                    equipment[id] = new WeaponItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, WeaponItem.WeaponType.Shield,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Helm")
                {
                    equipment[id] = new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Helm,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Armor")
                {
                    equipment[id] = new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Armor,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else if (type == "Glove")
                {
                    equipment[id] = new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Glove,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
                else
                {
                    equipment[id] = new ProtectionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, ProtectionItem.ProtectionType.Shoes,
                        power, defence, reinforcement, reinforcementLimit,
                        upPower, upDefence, randMinP, randMaxP, randMinD, randMaxD);
                }
            }
            equipmentDate.Close();
            datefs.Close();
        }
        
        public void LoadConsumption(int selectID)
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

                if (selectID != id) continue;

                string itemName = items[1];
                string itemExplanation = items[2];
                itemExplanation = itemExplanation.Replace("nl", "\n");
                int itemPrice = int.Parse(items[3]);
                int itemRare = int.Parse(items[4]);
                float itemWeight = float.Parse(items[5]);
                int amountLimit = int.Parse(items[6]);
                string type = items[7];
                int amount = int.Parse(items[8]);

                if (type == "recovary")
                {
                    consumption[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amountLimit,
                        ConsumptionItem.ItemEffectType.recovery,
                        new Recovery(amount));
                }
                else if (type == "damage")
                {
                    consumption[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amountLimit,
                        ConsumptionItem.ItemEffectType.damage,
                        new Damage(amount));
                }
                else if (type == "arrow")
                {
                    consumption[id] = ((new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amountLimit,
                        ConsumptionItem.ItemEffectType.arrow,
                        new ArrowEffect(amount))));
                }
                else
                {
                    consumption[id] = new ConsumptionItem(id, itemName, itemExplanation,
                        itemPrice, itemRare, itemWeight, amountLimit,
                        ConsumptionItem.ItemEffectType.noEffect,
                        new NoEffect());
                }
            }
            consuptionDate.Close();
            datefs.Close();
        }

        public void LoadAccessary(int selectID)
        {
            //消費アイテム読み込み
            FileStream datefs = new FileStream(accessaryFilename, FileMode.Open);
            StreamReader consuptionDate = new StreamReader(datefs, Encoding.GetEncoding("shift_jis"));

            while (!consuptionDate.EndOfStream)
            {
                string line = consuptionDate.ReadLine();
                string[] items = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length == 9) continue;

                int id = int.Parse(items[0]);

                if (selectID != id) continue;

                string itemName = items[1];
                string itemExplanation = items[2];
                itemExplanation = itemExplanation.Replace("nl", "\n");
                int itemPrice = int.Parse(items[3]);
                int itemRare = int.Parse(items[4]);
                float itemWeight = float.Parse(items[5]);
                int amountLimit = int.Parse(items[6]);
                string type = items[7];

                int intType = -1;
                if (type == "Necklace")
                {
                    intType = (int)AccessaryItem.Type.Necklace;
                }
                else if (type == "Book")
                {
                    intType = (int)AccessaryItem.Type.Book;
                }
                else if (type == "Pet")
                {
                    intType = (int)AccessaryItem.Type.Pet;
                }
                else if (type == "Sheath")
                {
                    intType = (int)AccessaryItem.Type.Sheath;
                }
                else if (type == "Amulet")
                {
                    intType = (int)AccessaryItem.Type.Amulet;
                }

                accessary[id] = new AccessaryItem(id, itemName, itemExplanation,
                    itemPrice, itemRare, itemWeight, amountLimit, intType);
            }
            consuptionDate.Close();
            datefs.Close();
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

        //指定された装飾品アイテムを送る
        public Item GetAccessaryItem(int id)
        {
            return accessarys[id].Clone();
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

        public List<Item> GetAccessaryList()
        {
            List<Item> accessaryList = new List<Item>();
            foreach(var page in accessarys)
            {
                accessaryList.Add(page.Value);
            }

            return accessaryList;
        }

        public Item GetConsumption(int id)
        {
            if (consumption.ContainsKey(id))
            {
                return consumption[id];
            }
            LoadConsumption(id);
            return consumption[id];
        }

        public Item GetEquipment(int id)
        {
            if (equipment.ContainsKey(id))
            {
                return equipment[id];
            }
            LoadEquipment(id);
            return equipment[id];
        }

        public Item GetAccessary(int id)
        {
            if (accessary.ContainsKey(id))
            {
                return accessary[id];
            }
            LoadAccessary(id);
            return accessary[id];
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
