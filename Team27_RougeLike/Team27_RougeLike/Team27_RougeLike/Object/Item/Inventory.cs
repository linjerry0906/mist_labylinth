//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.14
// 内容　：道具欄
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object.Item
{
    class Inventory
    {
        private GameDevice gameDevice;
        private static readonly int MAX_ITEM_COUNT_BAG = 25;       //Bagのサイズ
        private List<Item> bag;                                    //Bagの内容
        private List<Item> tempBag;                                //一時的なバッグ

        private readonly int MAX_ITEM_COUNT_DEPOSITORY = 50;       //倉庫の大きさ
        private List<Item> equipDepository;                        //装備の倉庫
        private Dictionary<int, int> itemDepository;               //アイテムの倉庫

        private ProtectionItem[] armor;               //装備
        private WeaponItem rightHand;               　//右手
        private WeaponItem leftHand;                　//左手

        private int money;                            //所持金

        public Inventory(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            bag = new List<Item>();
            tempBag = new List<Item>();
            equipDepository = new List<Item>();
            itemDepository = new Dictionary<int, int>();
            armor = new ProtectionItem[4];
            for (int i = 0; i < armor.Length; i++)
            {
                armor[i] = null;
            }
            rightHand = null;
            leftHand = null;
        }

        #region カバン関連

        /// <summary>
        /// アイテムをバッグに追加
        /// </summary>
        /// <param name="item">追加するアイテム</param>
        /// <returns>フールの場合はFalseを返す</returns>
        public bool AddItem(Item item)
        {
            int count = bag.Count;
            if (count >= MAX_ITEM_COUNT_BAG)      //最大限に超えたら追加しない
            {
                return false;
            }

            Item newItem = item.UniqueClone();
            newItem.ResetID(gameDevice.Random.Next(-50, 50));
            bag.Add(newItem);
            return true;
        }

        /// <summary>
        /// アイテムを一時的なバッグに追加
        /// </summary>
        /// <param name="item">追加するアイテム</param>
        /// <returns>フールの場合はFalseを返す</returns>
        public bool AddTempItem(Item item)
        {
            int count = bag.Count;
            if (count >= MAX_ITEM_COUNT_BAG)                      //最大限に超えたら追加しない
            {
                return false;
            }

            item.ResetID(gameDevice.Random.Next(0, 100));   //ID変更
            tempBag.Add(item);
            bag.Add(item);
            return true;
        }

        public void RemoveTemp()
        {
            tempBag.Clear();
        }

        /// <summary>
        /// 特定のアイテムを削除
        /// </summary>
        /// <param name="bagIndex">バッグ内のIndex</param>
        public void RemoveItem(int bagIndex)
        {
            bag.RemoveAt(bagIndex);
        }

        /// <summary>
        /// 持ち帰れないアイテムを削除
        /// </summary>
        public void RemoveTempItem()
        {
            foreach (Item temp in tempBag)
            {
                bag.RemoveAll(i => i.GetUniqueID() == temp.GetUniqueID());
                for (int i = 0; i < armor.Length; i++)
                {
                    if (armor[i] != null && armor[i].GetUniqueID() == temp.GetUniqueID())
                        armor[i] = null;
                }

                if (leftHand != null && leftHand.GetUniqueID() == temp.GetUniqueID())
                    leftHand = null;
                if (rightHand != null && rightHand.GetUniqueID() == temp.GetUniqueID())
                    rightHand = null;
            }

            tempBag.Clear();
        }

        /// <summary>
        /// 手持ちのアイテムの添え字を探すメソッド
        /// </summary>
        /// <param name="item">カバン内のアイテム</param>
        /// <returns></returns>
        public int BagItemIndex(Item item)
        {
            return bag.FindIndex(i => i.Equals(item));
        }

        /// <summary>
        /// すべて削除
        /// </summary>
        public void RemoveAll()
        {
            bag.Clear();
            tempBag.Clear();
            bag = new List<Item>();
            tempBag = new List<Item>();
            armor = new ProtectionItem[4];
            for (int i = 0; i < armor.Length; i++)
            {
                armor[i] = null;
            }
            rightHand = null;
            leftHand = null;
        }

        /// <summary>
        /// バッグ内のアイテムを装備する
        /// </summary>
        /// <param name="bagIndex">バッグ内のIndex</param>
        public void EquipArmor(int bagIndex)
        {
            Item item = bag[bagIndex];
            if (!(item is ProtectionItem))
            {
                return;
            }

            ProtectionItem.ProtectionType type = ((ProtectionItem)item).GetProtectionType();
            if (armor[(int)type] != null)                 //装備している状態
            {
                bag.Add(armor[(int)type]);                //バッグに戻す
            }
            armor[(int)type] = (ProtectionItem)item;      //装備する
            bag.RemoveAt(bagIndex);
        }

        /// <summary>
        /// 左手に装備する
        /// </summary>
        /// <param name="bagIndex">バッグ内のIndex</param>
        public bool EquipLeftHand(int bagIndex)
        {
            Item item = bag[bagIndex];
            if (!(item is WeaponItem))          　//エラー対策
                return false;


            WeaponItem.WeaponType type = ((WeaponItem)item).GetWeaponType();
            if (type == WeaponItem.WeaponType.Bow)     //弓は両手
            {
                if (rightHand != null && leftHand != null &&
                    bag.Count + 2 > MAX_ITEM_COUNT_BAG)      //両手いっぱいで、弓を装備する場合はカバンの容量をチェック
                {
                    return false;
                }

                if (leftHand != null)                 //装備している状態
                {
                    bag.Add(leftHand);                //バッグに戻す
                }

                if (rightHand != null)                 //装備している状態
                {
                    bag.Add(rightHand);                //バッグに戻す
                    rightHand = null;
                }
            }
            else
            {
                if (leftHand != null)                 //装備している状態
                {
                    bag.Add(leftHand);                //バッグに戻す
                }
            }

            leftHand = (WeaponItem)item;          //装備する
            bag.RemoveAt(bagIndex);
            return true;
        }

        /// <summary>
        /// 右手に装備する
        /// </summary>
        /// <param name="bagIndex">バッグ内のIndex</param>
        public bool EquipRightHand(int bagIndex)
        {
            Item item = bag[bagIndex];
            if (!(item is WeaponItem))             //エラー対策
                return false;

            WeaponItem.WeaponType type = ((WeaponItem)item).GetWeaponType();
            if (type == WeaponItem.WeaponType.Bow) //右手は弓を装備できない
                return false;


            if (leftHand != null && leftHand.GetWeaponType() == WeaponItem.WeaponType.Bow)      //弓は同時装備できない
            {
                bag.Add(leftHand);                 //バッグに戻す
                leftHand = null;
            }

            if (rightHand != null)                 //装備している状態
            {
                bag.Add(rightHand);                //バッグに戻す
            }

            rightHand = (WeaponItem)item;          //装備する
            bag.RemoveAt(bagIndex);
            return true;
        }

        /// <summary>
        /// バッグ内のアイテム
        /// </summary>
        /// <returns></returns>
        public List<Item> BagList()
        {
            return bag;
        }

        /// <summary>
        /// ステータスを加算して返す
        /// </summary>
        /// <param name="power">攻撃力</param>
        /// <param name="defence">防御力</param>
        /// <param name="weight">重量</param>
        public void GetStatus(ref int power, ref int defence, ref float weight)
        {
            power = 0;
            defence = 0;
            weight = 0;

            foreach (Item i in bag)               //ItemListの重量計算
            {
                weight += i.GetItemWeight();
            }

            foreach (ProtectionItem p in armor)   //防具の計算
            {
                if (p == null)
                    continue;
                power += p.GetPower();
                defence += p.GetDefense();
                weight += p.GetItemWeight();
            }

            if (leftHand != null)                 //左手のものを計算
            {
                power += leftHand.GetPower();
                defence += leftHand.GetDefense();
                weight += leftHand.GetItemWeight();
            }

            if (rightHand != null)              　//右手のものを計算
            {
                power += rightHand.GetPower();
                defence += rightHand.GetDefense();
                weight += rightHand.GetItemWeight();
            }
        }

        /// <summary>
        /// 装備している防具
        /// </summary>
        /// <returns></returns>
        public ProtectionItem[] CurrentArmor()
        {
            return armor;
        }

        /// <summary>
        /// 左手に装備している武器
        /// </summary>
        /// <returns></returns>
        public WeaponItem LeftHand()
        {
            return leftHand;
        }

        /// <summary>
        /// 右手に装備している武器
        /// </summary>
        /// <returns></returns>
        public WeaponItem RightHand()
        {
            return rightHand;
        }

        /// <summary>
        /// カバンにアイテム数量と最大値を取得
        /// </summary>
        /// <param name="current">現在量</param>
        /// <param name="maxium">最大量</param>
        public void BagItemCount(ref int current, ref int maxium)
        {
            current = bag.Count;
            maxium = MAX_ITEM_COUNT_BAG;
        }

        #endregion

        #region　倉庫関連


        /// <summary>
        /// カバンから指定のIndexの装備を倉庫に入れる
        /// </summary>
        /// <param name="bagIndex">カバン内の添え字</param>
        /// <returns></returns>
        public bool DepositEquip(int bagIndex)
        {
            if (equipDepository.Count >= MAX_ITEM_COUNT_DEPOSITORY)
            {
                return false;
            }

            if (bag[bagIndex] is ConsumptionItem)
                return false;

            equipDepository.Add(bag[bagIndex]);
            bag.RemoveAt(bagIndex);
            return true;
        }

        /// <summary>
        /// 倉庫から指定のIndexのアイテムを倉庫に入れる
        /// </summary>
        /// <param name="equipDepositIndex">倉庫内の添え字</param>
        /// <returns></returns>
        public bool MoveDepositEquipToBag(int equipDepositIndex)
        {
            if (bag.Count >= MAX_ITEM_COUNT_BAG)
            {
                return false;
            }

            bag.Add(equipDepository[equipDepositIndex]);
            equipDepository.RemoveAt(equipDepositIndex);
            return true;
        }


        /// <summary>
        /// 倉庫にある装備
        /// </summary>
        /// <returns></returns>
        public List<Item> EquipDepository()
        {
            return equipDepository;
        }


        /// <summary>
        /// 倉庫に装備数量と最大値を取得
        /// </summary>
        /// <param name="current">現在量</param>
        /// <param name="maxium">最大量</param>
        public void DepositoryEquipCount(ref int current, ref int maxium)
        {
            current = equipDepository.Count;
            maxium = MAX_ITEM_COUNT_DEPOSITORY;
        }

        /// <summary>
        /// カバン内のアイテムを倉庫に入れる
        /// </summary>
        /// <param name="bagIndex">カバン内のIndex</param>
        /// <returns></returns>
        public bool DepositItem(int bagIndex)
        {
            if (!(bag[bagIndex] is ConsumptionItem))
                return false;

            if (!itemDepository.ContainsKey(bag[bagIndex].GetItemID()))
            {
                itemDepository.Add(bag[bagIndex].GetItemID(), 1);
                bag.RemoveAt(bagIndex);
                return true;
            }

            itemDepository[bag[bagIndex].GetItemID()] += 1;
            bag.RemoveAt(bagIndex);
            return true;
        }

        /// <summary>
        /// 倉庫にある装備の添え字
        /// </summary>
        /// <param name="item">倉庫にあるアイテム</param>
        /// <returns></returns>
        public int DepositEquipIndex(Item item)
        {
            return equipDepository.IndexOf(item);
        }

        /// <summary>
        /// 倉庫のアイテムリスト
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> DepositoryItem()
        {
            return itemDepository;
        }

        #endregion

        #region 金関連

        /// <summary>
        /// 所持金を増やす
        /// </summary>
        /// <param name="add">増やす量</param>
        public void AddMoney(int add)
        {
            money += add;
        }

        /// <summary>
        /// 今所持しているお金
        /// </summary>
        /// <returns></returns>
        public int CurrentMoney()
        {
            return money;
        }

        /// <summary>
        /// 所持金を使う
        /// </summary>
        /// <param name="add">使う量</param>
        public void SpendMoney(int amount)
        {
            money -= amount;
        }

        #endregion

        /// <summary>
        /// ファイルからアイテムを復元
        /// </summary>
        /// <param name="saveData"></param>
        public void LoadFromFile(SaveData saveData)
        {
            armor = saveData.GetArmor();
            leftHand = saveData.GetLeftHand();
            rightHand = saveData.GetRightHand();
            money = saveData.GetMoney();
            bag = saveData.GetBagList();
        }
    }
}
