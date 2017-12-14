﻿//--------------------------------------------------------------------------------------------------
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
        private List<Item> equipments;                      //装備している物

        public Inventory()
        {
            bag = new List<Item>();
            equipments = new List<Item>();
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
        public void Equip(int bagIndex)
        {
            equipments.Add(bag[bagIndex]);
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
        /// 装備しているアイテム
        /// </summary>
        /// <returns></returns>
        public List<Item> Equipments()
        {
            return equipments;
        }
    }
}
