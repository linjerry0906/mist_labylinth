using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Object;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.QuestSystem;

namespace Team27_RougeLike.Device
{
    class SaveData
    {
        private GameManager gameManager;
        private ItemManager itemManager;
        private Inventory playerInventory;

        private Dictionary<int, int> clearDungen;       //クリアしたフロア
        private int money;                              //所持金
        private List<Item> bag;                         //バッグの中身
        private ProtectionItem[] armor;                 //装備中の防具
        private WeaponItem leftHand;                    //左手装備
        private WeaponItem rightHand;                   //右手装備
        private ConsumptionItem arrow;                  //矢
        private AccessaryItem accessary;                //アクセサリー
        private List<Item> depotEquipment;              //倉庫の装備アイテム
        private Dictionary<int, int> depotConsumption;  //倉庫の消費アイテム
        private List<Quest> quest;                      //受けているクエスト
        private QuestLoader questLoader;                //QuestLoader
        private PlayerGuildRank guildRank;               //ギルト情報

        private string saveFileName;
        private bool isSave;
        private bool isLoad;

        public SaveData(GameManager gameManager)
        {
            this.gameManager = gameManager;
            itemManager = gameManager.ItemManager;
            playerInventory = gameManager.PlayerItem;

            clearDungen = gameManager.DungeonProcess.GetProcess();
            money = playerInventory.CurrentMoney();
            bag = playerInventory.BagList();
            armor = playerInventory.CurrentArmor();
            leftHand = playerInventory.LeftHand();
            rightHand = playerInventory.RightHand();
            //arrowをInventoruから受け取る処理
            quest = new List<Quest>();
            questLoader = gameManager.QuestManager;
            guildRank = gameManager.GuildInfo;

            saveFileName = @"Content/SaveCSV/SaveDate.csv";
            isSave = false;
            isLoad = false;
        }

        public void Set()
        {
            clearDungen = gameManager.DungeonProcess.GetProcess();
            money = playerInventory.CurrentMoney();
            bag = playerInventory.BagList();
            armor = playerInventory.CurrentArmor();
            leftHand = playerInventory.LeftHand();
            rightHand = playerInventory.RightHand();
            //arrowをInventoruから受け取る処理
            depotEquipment = playerInventory.EquipDepository();
            depotConsumption = playerInventory.DepositoryItem();
            questLoader = gameManager.QuestManager;
            quest = gameManager.PlayerQuest.CurrentQuest();
            guildRank = gameManager.GuildInfo;
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
                    item.GetItemID() + "," +
                    ((ConsumptionItem)item).GetStack();
            }
            else if (item is AccessaryItem)
            {
                return "Accessary" + "," +
                    item.GetItemID() + ",";
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

            StreamWriter sw = new StreamWriter(saveFileName);

            foreach (var clearFloor in clearDungen)
            {
                sw.WriteLine("floor," + clearFloor.Key + "," + clearFloor.Value);
            }
            sw.WriteLine("money," + money);
            sw.WriteLine("leftHand," + ItemSaveString(leftHand));
            sw.WriteLine("rightHand," + ItemSaveString(rightHand));
            sw.WriteLine("arrow," + ItemSaveString(arrow));
            foreach (Item item in armor)
            {
                sw.WriteLine("armor," + ItemSaveString(item));
            }
            //foreach (Item item in //アクセサリー)
            //{
            //    sw.WriteLine("accessary," + ItemSaveString(item));
            //}
            foreach (Item item in bag)
            {
                sw.WriteLine("bag," + ItemSaveString(item));
            }

            foreach (Item item in depotEquipment)
            {
                sw.WriteLine("depot," + ItemSaveString(item));
            }
            foreach (int id in depotConsumption.Keys)
            {
                sw.WriteLine("depot," + "Consumption," + id + "," + depotConsumption[id]);
            }

            foreach (Quest q in quest)
            {
                sw.Write("quest," + q.QuestID() + ",");
                for (int i = 0; i < 3; i++)
                {
                    if (i < q.CurrentState().Count)
                    {
                        sw.Write(q.CurrentState()[i].CurrentAmount + ",");
                        continue;
                    }
                    sw.Write("null,");
                }
                sw.WriteLine();
            }

            #region Guild
            sw.WriteLine("guild," + (int)(guildRank.Rank()) + "," + guildRank.CurrentExp());
            #endregion

            sw.Close();
            isSave = false;
        }

        //セーブデータを読み込む
        public bool Load()
        {
            if (!File.Exists(saveFileName))
            {
                return false;
            }
            StreamReader sr = new StreamReader(saveFileName);
            try
            {
                isLoad = true;

                List<string[]> itemDates = new List<string[]>();
                clearDungen = new Dictionary<int, int>();
                armor = new ProtectionItem[4];
                bag = new List<Item>();
                depotEquipment = new List<Item>();
                depotConsumption = new Dictionary<int, int>();

                int bagNum = 0;
                int depotNum = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] strings = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strings[0] == "floor")
                    {
                        clearDungen[int.Parse(strings[1])] = int.Parse(strings[2]);
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
                    else if (strings[0] == "arrow")
                    {
                        if (strings[1] != "no")
                        {
                            string[] itemDate = new string[]
                            {
                            strings[1],
                            strings[2],
                            strings[3]
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
                    else if (strings[0] == "accessary")
                    {

                    }
                    else if (strings[0] == "bag")
                    {
                        bagNum++;

                        if (strings[1] == "Consumption")
                        {
                            string[] itemDate = new string[]
                            {
                            strings[1],
                            strings[2],
                            strings[3],
                            };
                            itemDates.Add(itemDate);
                        }
                        else
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
                    else if (strings[0] == "depot")
                    {
                        if (strings[1] == "Consumption")
                        {
                            depotConsumption.Add(int.Parse(strings[2]), int.Parse(strings[3]));
                        }
                        else
                        {
                            depotNum++;
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
                    else if (strings[0] == "quest")
                    {
                        Quest q = questLoader.GetQuest(int.Parse(strings[1]));
                        for (int i = 0; i < 3; i++)
                        {
                            if (i >= q.RequireID().Length)
                            {
                                break;
                            }
                            q.SetItemAmount(q.RequireID()[i], int.Parse(strings[i + 2]));
                        }
                        quest.Add(q);
                    }
                    else if (strings[0] == "guild")
                    {
                        guildRank = new PlayerGuildRank(int.Parse(strings[1]), int.Parse(strings[2]));
                    }
                }
                sr.Close();

                List<Item> items = itemManager.LoadSaveItem(itemDates);
                leftHand = (WeaponItem)items[0];
                rightHand = (WeaponItem)items[1];
                arrow = (ConsumptionItem)items[2];
                armor[0] = (ProtectionItem)items[3];
                armor[1] = (ProtectionItem)items[4];
                armor[2] = (ProtectionItem)items[5];
                armor[3] = (ProtectionItem)items[6];
                for (int i = 7; i < 7 + bagNum; i++)
                {
                    bag.Add(items[i]);
                }
                for (int i = 7 + bagNum; i < 7 + bagNum + depotNum; i++)
                {
                    depotEquipment.Add(items[i]);
                }

                isLoad = false;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                sr.Close();
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

        public Dictionary<int, int> GetClearDungen()
        {
            return clearDungen;
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

        public List<Item> GetDepotEquipment()
        {
            return depotEquipment;
        }

        public Dictionary<int, int> GetDepotConsumption()
        {
            return depotConsumption;
        }

        public List<Quest> GetQuest()
        {
            return quest;
        }

        public PlayerGuildRank GetGuildInfo()
        {
            return guildRank;
        }
    }
}
