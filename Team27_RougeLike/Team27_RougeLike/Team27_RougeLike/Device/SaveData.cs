using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Object;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.Device
{
    class SaveData
    {
        private GameManager gameManager;
        private ItemManager itemManager;
        private int clearFloor;
        private int money;
        private Inventory playerInventory;
        private List<Item> bag;
        private ProtectionItem[] armor;
        private WeaponItem leftHand;
        private WeaponItem rightHand;

        private string saveFileName;

        public SaveData(GameManager gameManager)
        {
            this.gameManager = gameManager;
            itemManager = gameManager.ItemManager;
            playerInventory = gameManager.PlayerItem;

            clearFloor = 1;
            money = 0;

            bag = playerInventory.BagList();
            armor = playerInventory.CurrentArmor();
            leftHand = playerInventory.LeftHand();
            rightHand = playerInventory.RightHand();

            saveFileName = @"Content/" + "SaveCSV/SaveDate.csv";
        }

        public string ItemSaveString(Item item)
        {
            if (item is WeaponItem)
            {
                return "Weapon" + "," +
                    item.GetItemID() + "," +
                    ((WeaponItem)item).GetAddPower() + "," +
                    ((WeaponItem)item).GetAddDefence() + "," +
                    ((WeaponItem)item).GetReinforcement();
            }
            else if (item is ProtectionItem)
            {
                return "Protection" + "," +
                    item.GetItemID() + "," +
                    ((ProtectionItem)item).GetAddPower() + "," +
                    ((ProtectionItem)item).GetAddDefence() + "," +
                    ((ProtectionItem)item).GetReinforcement();
            }
            else
            {
                return "Consumption" + "," +
                    item.GetItemID().ToString();
            }
        }

        //セーブデータを書き込む
        public void Save()
        {
            StreamWriter sw = new StreamWriter(saveFileName);
            sw.WriteLine("floor," + clearFloor);
            sw.WriteLine("money," + money);
            sw.WriteLine("leftHand," + ItemSaveString(leftHand));
            sw.WriteLine("rightHand," + ItemSaveString(rightHand));
            foreach(Item item in armor)
            {
                sw.WriteLine("armor," + ItemSaveString(item));
            }
            foreach(Item item in bag)
            {
                sw.WriteLine("bag," + ItemSaveString(item));
            }

            sw.Close();
        }

        //セーブデータを読み込む
        public bool Load()
        {
            try
            {
                armor = new ProtectionItem[4];
                bag = new List<Item>();

                StreamReader sr = new StreamReader(saveFileName);
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] strings = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strings[0] == "floor")
                    {
                        clearFloor = int.Parse(strings[1]);
                    }
                    else if (strings[0] == "money")
                    {
                        money = int.Parse(strings[1]);
                    }
                    else if (strings[0] == "leftHand")
                    {
                        string[] itemDates = new string[]
                        {
                            strings[1],
                            strings[2],
                            strings[3],
                            strings[4],
                            strings[5],
                        };
                        leftHand = (WeaponItem)itemManager.LoadSaveItem(itemDates);
                    }
                    else if (strings[0] == "rightHand")
                    {
                        string[] itemDates = new string[]
                        {
                            strings[1],
                            strings[2],
                            strings[3],
                            strings[4],
                            strings[5],
                        };
                        rightHand = (WeaponItem)itemManager.LoadSaveItem(itemDates);
                    }
                    else if (strings[0] == "armor")
                    {
                        string[] itemDates = new string[]
                        {
                            strings[1],
                            strings[2],
                            strings[3],
                            strings[4],
                            strings[5],
                        };
                        armor[armor.Length - 1] = (ProtectionItem)itemManager.LoadSaveItem(itemDates);
                    }
                    else if (strings[0] == "bag")
                    {
                        string[] itemDates = new string[]
                        {
                            strings[1],
                            strings[2],
                            strings[3],
                            strings[4],
                            strings[5],
                        };
                        bag.Add(itemManager.LoadSaveItem(itemDates));
                    }

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetFloor()
        {
            return clearFloor;
        }

        public int GetMoney()
        {
            return money;
        }

        public ProtectionItem[] GetArmor()
        {
            return armor;
        }

        public WeaponItem GetLeftHand()
        {
            return leftHand;
        }

        public WeaponItem GetRightHand()
        {
            return rightHand;
        }

        public List<Item> GetBagList()
        {
            return bag;
        }

    }
}
