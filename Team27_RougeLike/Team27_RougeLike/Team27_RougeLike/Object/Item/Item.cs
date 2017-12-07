using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item
{
    abstract class Item
    {
        protected int itemID; //アイテムID
        protected string itemName; //アイテム名
        protected string itemExplanation; //説明文
        protected int itemPrice; //値段
        protected int itemRare; //レア度
        protected float itemWeight; //重量
        protected int amountLimit; //限界所持個数
        
        
        public Item(int itemID, string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, int amountLimit)
        {
            this.itemID = itemID;
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

        public int GetItemID()
        {
            return itemID;
        }

        public abstract Item Clone();
    }
}
