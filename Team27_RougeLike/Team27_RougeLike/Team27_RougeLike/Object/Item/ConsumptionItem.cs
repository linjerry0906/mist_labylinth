using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Item.ItemEffects;

namespace Team27_RougeLike.Object.Item
{
    class ConsumptionItem : Item
    {
        public enum ItemEffectType
        {
            noEffect = 0,
            recovery,
            damage,
            arrow,
        }
        private ItemEffectType effectType;

        private ItemEffect itemEffect; //効果

        private int stack; //スタック数

        public ConsumptionItem(int itemID, string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, int amountLimit, ItemEffectType effectType, ItemEffect itemEffect)
            :base(itemID, itemName, itemExplanation, itemPrice, itemRare, itemWeight, amountLimit)
        {
            this.effectType = effectType;
            this.itemEffect = itemEffect;

            stack = 1;
        }

        //コピーコンストラクタ
        public ConsumptionItem(ConsumptionItem other)
            :base(other.itemID, other.itemName, other.itemExplanation,
                 other.itemPrice, other.itemRare, other.itemWeight, other.amountLimit)
        {
            this.effectType = other.effectType;
            this.itemEffect = other.itemEffect;
            this.stack = other.stack;
        }

        //コピー
        public override Item Clone()
        {
            return new ConsumptionItem(this);
        }

        public ItemEffect GetItemEffect()
        {
            return itemEffect;
        }

        public string GetTypeText()
        {
            if (effectType == ItemEffectType.recovery)
            {
                return "回復系";
            }
            else if (effectType == ItemEffectType.damage)
            {
                return "ダメージ";
            }
            else if (effectType == ItemEffectType.arrow)
            {
                return "矢";
            }
            else
            {
                return "なし";
            }
        }

        public void SetStack(int num)
        {
            stack = num;
        }

        public void AddStack()
        {
            stack++;
        }

        public void AddStack(int num)
        {
            stack += num;
        }

        public int GetStack()
        {
            return stack;
        }

        public override int GetItemPrice()
        {
            return itemPrice * stack;
        }

        public override Item UniqueClone()
        {
            return Clone();
        }
    }
}
