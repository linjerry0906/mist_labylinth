using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item
{
    class AccessaryItem : Item
    {
        //アイテムの種類を追加する際はここに追加
        public enum Type
        {
            NONE = -1,
            Necklace,
            Book,
            Pet,
            Sheath,
            Amulet,
        }

        private Type type;

        //コンストラクタ
        public AccessaryItem(int itemID, string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, int amountLimit, Type type)
            : base(itemID, itemName, itemExplanation, itemPrice, itemRare, itemWeight, amountLimit)
        {
            this.type = type;
        }

        public AccessaryItem(int itemID, string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, int amountLimit, int type)
            : base(itemID, itemName, itemExplanation, itemPrice, itemRare, itemWeight, amountLimit)
        {
            this.type = (Type)type;
        }

        //コピーコンストラクタ
        public AccessaryItem(AccessaryItem other)
            : base(other.itemID, other.itemName, other.itemExplanation,
                 other.itemPrice, other.itemRare, other.itemWeight, other.amountLimit)
        {
            this.type = other.type;
        }

        public override Item Clone()
        {
            return new AccessaryItem(this);
        }

        public override Item UniqueClone()
        {
            return Clone();
        }

        public Type GetAccessaryType()
        {
            return type;
        }
    }
}
