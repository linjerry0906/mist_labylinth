using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Team27_RougeLike.Object.Item
{
    abstract class Item 
    {
        protected static long commonID = DateTime.Now.Ticks;
        protected string uniqueID; //ユニークID
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
            commonID++;
            uniqueID = commonID.ToString();
        }

        public string GetItemName()
        {
            return itemName;
        }

        public string GetItemExplanation()
        {
            return itemExplanation;
        }

        public virtual int GetItemPrice()
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

        public string GetUniqueID()
        {
            return uniqueID;
        }

        public void ResetID(int num)
        {
            commonID = DateTime.Now.Ticks + num;
            uniqueID = commonID.ToString();
        }

        public abstract Item Clone();

        public abstract Item UniqueClone();
    }
}
