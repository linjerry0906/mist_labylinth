using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Device
{
    class Item
    {
        //アイテムIDはListのKey
        private string itemName; //アイテム名
        private string itemExplanation; //説明文
        private int itemPrice; //値段
        private int itemRare; //レア度
        private float itemWeight; //重量
        private int amountLimit; //限界所持個数
        
        
        public Item(string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, int amountLimit)
        {
            this.itemName = itemName;
            this.itemExplanation = itemExplanation;
            this.itemPrice = itemPrice;
            this.itemRare = itemRare;
            this.itemWeight = itemWeight;
            this.amountLimit = amountLimit;
        }

        public string GetItemName()
        {
            return itemName;
        }

        public string GetItemExplanation()
        {
            return itemExplanation;
        }

        public int GetItemPrice()
        {
            return itemPrice;
        }

        public int GetItemRare()
        {
            return itemRare;
        }

        public float GetItemWeight()
        {
            return itemWeight;
        }

        public int GetAmountLimit()
        {
            return amountLimit;
        }
    }
}
