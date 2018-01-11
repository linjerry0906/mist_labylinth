using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Object;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.Device
{
    class SaveData
    {
        private GameManager gameManager;
        private ItemManager itemManager;
        private Inventory playerInventory;

        private int clearFloor;             //クリアしたフロア
        private int money;                  //所持金
        private List<Item> bag;             //バッグの中身
        private ProtectionItem[] armor;     //装備中の防具
        private WeaponItem leftHand;        //左手装備
        private WeaponItem rightHand;       //右手装備

        private string saveFileName;
        private bool isSave;
        private bool isLoad;

        public SaveData(GameManager gameManager)
        {
            this.gameManager = gameManager;
            itemManager = gameManager.ItemManager;
            playerInventory = gameManager.PlayerItem;

            clearFloor = 1;
            money = playerInventory.CurrentMoney();
            bag = playerInventory.BagList();
            armor = playerInventory.CurrentArmor();
            leftHand = playerInventory.LeftHand();
            rightHand = playerInventory.RightHand();

            saveFileName = @"Content/SaveCSV/SaveDate.csv";
            isSave = false;
            isLoad = false;
        }

        public void Set()
        {
            clearFloor = 1;
            money = playerInventory.CurrentMoney();
            bag = playerInventory.BagList();
            armor = playerInventory.CurrentArmor();
            leftHand = playerInventory.LeftHand();
            rightHand = playerInventory.RightHand();
        }

        //Itemをテキスト出力するためのメソッド
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
            else if (item is ConsumptionItem)
            {
                return "Consumption" + "," +
                    item.GetItemID();
            }
            else
            {
                return "no";
            }
        }

        //セーブデータを書き込む
        public void Save()
        {
            Set();

            isSave = true;

            //フォルダが存在していなかったらフォルダを生成
            DirectoryInfo di = Directory.CreateDirectory(@"Content/SaveCSV");

            
            //FileStream datefs = new FileStream(saveFileName, FileMode.Open);
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
            isSave = false;
        }

        //セーブデータを読み込む
        public bool Load()
        {
            try
            {
                isLoad = true;
                List<string[]> itemDates = new List<string[]>();
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
                        if (strings[1] != "no")
                        {
                            string[] itemDate = new string[]
                            {
                            strings[1],
                            strings[2],
                            strings[3],
                            strings[4],
                            strings[5],
                            };
                            itemDates.Add(itemDate);
                        }
                        else
                        {
                            itemDates.Add(null);
                        }
                    }
                    else if (strings[0] == "rightHand")
                    {
                        if (strings[1] != "no")
                        {
                            string[] itemDate = new string[]
                            {
                            strings[1],
                            strings[2],
                            strings[3],
                            strings[4],
                            strings[5],
                            };
                            itemDates.Add(itemDate);
                        }
                        else
                        {
                            itemDates.Add(null);
                        }
                    }
                    else if (strings[0] == "armor")
                    {
                        if (strings[1] != "no")
                        {
                            string[] itemDate = new string[]
                            {
                            strings[1],
                            strings[2],
                            strings[3],
                            strings[4],
                            strings[5],
                            };
                            itemDates.Add(itemDate);
                        }
                        else
                        {
                            itemDates.Add(null);
                        }
                    }
                    else if (strings[0] == "bag")
                    {
                        if (strings[1] != "no")
                        {
                            string[] itemDate = new string[]
                            {
                            strings[1],
                            strings[2],
                            strings[3],
                            strings[4],
                            strings[5],
                            };
                            itemDates.Add(itemDate);
                        }
                    }
                }
                sr.Close();

                leftHand = (WeaponItem)itemManager.LoadSaveItem(itemDates[0]);
                rightHand = (WeaponItem)itemManager.LoadSaveItem(itemDates[1]);
                for(int i = 0; i < 4; i++)
                {
                    armor[i] = (ProtectionItem)itemManager.LoadSaveItem(itemDates[2 + i]);
                }
                for(int i = 6; i < itemDates.Count; i++)
                {
                    bag.Add(itemManager.LoadSaveItem(itemDates[i]));
                }

                isLoad = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Draw(Renderer renderer)
        {
            if (isSave)
            {
                renderer.DrawString("セーブ中", Vector2.Zero, new Vector2(1, 1), Color.Red);
            }
            if (isLoad)
            {
                renderer.DrawString("ロード中", Vector2.Zero, new Vector2(1, 1), Color.Red);
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
