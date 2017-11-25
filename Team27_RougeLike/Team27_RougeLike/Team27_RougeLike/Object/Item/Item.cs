using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item
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
        private ItemEffect itemEffect;
        private EquipmentEffect equipmentEffect; //装備効果

        private List<Item> items = new List<Item>();

        //消費アイテム
        public Item(string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, int amountLimit,
            ItemEffect itemEffect)
        {
            this.itemName = itemName;
            this.itemExplanation = itemExplanation;
            this.itemPrice = itemPrice;
            this.itemRare = itemRare;
            this.itemWeight = itemWeight;
            this.amountLimit = amountLimit;
            this.itemEffect = itemEffect;
        }

        //装備アイテム
        public Item(string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, int amountLimit,
            EquipmentEffect equipmentEffect)
        {
            this.itemName = itemName;
            this.itemExplanation = itemExplanation;
            this.itemPrice = itemPrice;
            this.itemRare = itemRare;
            this.itemWeight = itemWeight;
            this.amountLimit = amountLimit;
            this.equipmentEffect = equipmentEffect;
        }

        public void Initialize()
        {
            //消費アイテム
            //items.Add(new Item("回復薬", "使うことで体力が回復する。", 50, 1, 1, 99));

            //装備アイテム
            items.Add(new Item("ソード", "いたって普通の剣", 150, 1, 10, 99, new EquipmentEffect(5, 0)));
        }

        public string GetItemName(int key)
        {
            return items[key].itemName;
        }

        public string GetItemExplanation(int key)
        {
            return items[key].itemExplanation;
        }

        public int GetItemPrice(int key)
        {
            return items[key].itemPrice;
        }

        public int GetItemRare(int key)
        {
            return items[key].itemRare;
        }

        public float GetItemWeight(int key)
        {
            return items[key].itemWeight;
        }

        public int GetAmountLimit(int key)
        {
            return items[key].amountLimit;
        }

        public EquipmentEffect GetEquipmentEffect(int key)
        {
            if (items[key].equipmentEffect != null)
            {
                return equipmentEffect;
            }
            return new EquipmentEffect(0, 0);
        }

        


    }
}
