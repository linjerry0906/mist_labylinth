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
        private static readonly int MaxItemCount = 25;      //Bagのサイズ
        private List<Item> bag;                             //Bagの内容

        private ProtectionItem[] armor;
        private WeaponItem rightHand;
        private WeaponItem leftHand;


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
            if (bag.Count > MaxItemCount)
                return false;

            bag.Add(item);
            return true;
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

            foreach (Item i in bag)
            {
                weight += i.GetItemWeight();
            }

            foreach (ProtectionItem p in armor)
            {
                power += p.GetPower();
                defence += p.GetDefense();
                weight += p.GetItemWeight();
            }

            power += leftHand.GetPower();
            defence += leftHand.GetDefense();
            weight += leftHand.GetItemWeight();

            power += rightHand.GetPower();
            defence += rightHand.GetDefense();
            weight += rightHand.GetItemWeight();
        }
    }
}
