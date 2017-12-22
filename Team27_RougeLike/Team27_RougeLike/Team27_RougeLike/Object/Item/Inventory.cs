//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.14
// 内容　：道具欄
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item
{
    class Inventory
    {
        private readonly int MaxItemCount = 25;       //Bagのサイズ
        private List<Item> bag;                       //Bagの内容

        private ProtectionItem[] armor;               //装備
        private WeaponItem rightHand;               　//右手
        private WeaponItem leftHand;                　//左手


        public Inventory()
        {
            bag = new List<Item>();
            armor = new ProtectionItem[4];
            for (int i = 0; i < armor.Length; i++)
            {
                armor[i] = null;
            }
            rightHand = null;
            leftHand = null;
        }

        /// <summary>
        /// アイテムをバッグに追加
        /// </summary>
        /// <param name="item">追加するアイテム</param>
        /// <returns>フールの場合はFalseを返す</returns>
        public bool AddItem(Item item)
        {
            int count = bag.Count;
            if (count >= MaxItemCount)      //最大限に超えたら追加しない
            {
                return false;
            }

            bag.Add(item);
            return true;
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
        public void EquipLeftHand(int bagIndex)
        {
            Item item = bag[bagIndex];
            if (!(item is WeaponItem))          　//エラー対策
                return;

            if (leftHand != null)                 //装備している状態
            {
                bag.Add(leftHand);                //バッグに戻す
            }

            WeaponItem.WeaponType type = ((WeaponItem)item).GetWeaponType();
            if (type == WeaponItem.WeaponType.Bow)     //弓は両手
            {
                if (rightHand != null)                 //装備している状態
                {
                    bag.Add(rightHand);                //バッグに戻す
                }
            }

            leftHand = (WeaponItem)item;          //装備する
            bag.RemoveAt(bagIndex);
        }

        /// <summary>
        /// 右手に装備する
        /// </summary>
        /// <param name="bagIndex">バッグ内のIndex</param>
        public void EquipRightHand(int bagIndex)
        {
            Item item = bag[bagIndex];
            if (!(item is WeaponItem))             //エラー対策
                return;

            WeaponItem.WeaponType type = ((WeaponItem)item).GetWeaponType();
            if (type == WeaponItem.WeaponType.Bow) //右手は弓を装備できない
                return;

            if (rightHand != null)                 //装備している状態
            {
                bag.Add(rightHand);                //バッグに戻す
            }

            if (leftHand.GetWeaponType() == WeaponItem.WeaponType.Bow)
            {
                bag.Add(leftHand);                  //バッグに戻す
                leftHand = null;
            }

            rightHand = (WeaponItem)item;          //装備する
            bag.Remove(bag[bagIndex]);
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
        /// アイテム数量と最大値を取得
        /// </summary>
        /// <param name="current">現在量</param>
        /// <param name="maxium">最大量</param>
        public void ItemCount(ref int current, ref int maxium)
        {
            current = bag.Count;
            maxium = MaxItemCount;
        }
    }
}
