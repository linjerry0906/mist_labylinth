using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item
{
    class ProtectionItem : Item
    {

        //防具の種類
        public enum ProtectionType
        {
            Helm, //頭
            Armor,　//鎧
            Glove,　//手
            Shoes, //靴
        }
        private ProtectionType protectionType;
        private EquipmentEffect effect;

        private int itemPower;
        private int itemDefense;
        private int reinforcement;
        private int reinforcementLimit;
        private int upPower;
        private int upDefense;
        private int randomMinP;
        private int randomMaxP;
        private int randomMinD;
        private int randomMaxD;

        //強化値ランダム
        public ProtectionItem(int itemID, string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, ProtectionType protectionType,
            int itemPower, int itemDefense, int reinforcement, int reinforcementLimit, int upPower, int upDefense,
            int randomMinP, int randomMaxP, int randomMinD, int randomMaxD)
            :base(itemID, itemName, itemExplanation, itemPrice, itemRare, itemWeight, 1)
        {
            this.protectionType = protectionType;

            this.itemPower = itemPower;
            this.itemDefense = itemDefense;
            this.reinforcement = reinforcement;
            this.reinforcementLimit = reinforcementLimit;
            this.upPower = upPower;
            this.upDefense = upDefense;
            this.randomMinP = randomMinP;
            this.randomMaxP = randomMaxP;
            this.randomMinD = randomMinD;
            this.randomMaxD = randomMaxD;

            effect = new EquipmentEffect(itemPower, itemDefense, reinforcementLimit,
                upPower, upDefense, 0, 0);
        }

        //強化値指定
        public ProtectionItem(int itemID, string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, ProtectionType protectionType,
            int itemPower, int itemDefense, int reinforcement, int reinforcementLimit,
            int upPower, int upDefense, int addPower, int addDefence)
            : base(itemID, itemName, itemExplanation, itemPrice, itemRare, itemWeight, 1)
        {
            this.protectionType = protectionType;

            effect = new EquipmentEffect(itemPower, itemDefense, reinforcementLimit,
                upPower, upDefense, addPower, addDefence, reinforcement);
        }

        //コピーコンストラクタ
        public ProtectionItem(ProtectionItem other)
            :base(other.itemID, other.itemName, other.itemExplanation,
                 other.itemPrice, other.itemRare, other.itemWeight, other.amountLimit)
        {
            this.protectionType = other.protectionType;

            effect = new EquipmentEffect(other.itemPower, other.itemDefense, other.reinforcementLimit,
                other.upPower, other.upDefense, other.randomMinP, other.randomMaxP, other.randomMinD, other.randomMaxD);
        }
        
        //コピー
        public override Item Clone()
        {
            return new ProtectionItem(this);
        }

        //装備強化
        public void LevelUp()
        {
            if (effect.IsLevelMax() == false)
            {
                effect.LevelUp();
            }
        }

        public ProtectionType GetProtectionType()
        {
            return protectionType;
        }

        public int GetPower()
        {
            return effect.GetPower();
        }

        public int GetDefense()
        {
            return effect.GetDefense();
        }

        public bool IsBiggestLevel()
        {
            return effect.IsLevelMax();
        }

        public bool IsLevelMax()
        {
            return effect.IsLevelMax();
        }

        //セーブ用
        public int GetAddPower()
        {
            return effect.GetAddPower();
        }
        public int GetAddDefence()
        {
            return effect.GetAddDefense();
        }
        public int GetReinforcement()
        {
            return effect.GetReinforcement();
        }

        public override Item UniqueClone()
        {
            ProtectionItem newItem = new ProtectionItem(itemID, itemName, itemExplanation,
            itemPrice, itemRare, itemWeight, protectionType,
            effect.GetPower() - effect.GetAddPower(), effect.GetDefense() - effect.GetAddDefense(), 
            effect.GetReinforcement(), effect.GetReinforcementLimit(), effect.GetUpPower(), effect.GetUpDefence(),
            effect.GetAddPower(), effect.GetAddDefense());

            return newItem;
        }
    }
}
