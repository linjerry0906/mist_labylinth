using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item
{
    class WeaponItem : Item
    {
        //武器の種類
        public enum WeaponType
        {
            Sword, //剣
            Bow, //弓
            Dagger, //短剣
            Shield, //盾
        }
        private WeaponType weaponType;
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
        public WeaponItem(int itemID, string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, WeaponType weaponType,
            int itemPower, int itemDefense, int reinforcement, int reinforcementLimit,
            int upPower, int upDefense, int randomMinP, int randomMaxP, int randomMinD, int randomMaxD)
            :base(itemID, itemName, itemExplanation, itemPrice, itemRare, itemWeight, 1)
        {
            this.weaponType = weaponType;

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
        public WeaponItem(int itemID, string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, WeaponType weaponType,
            int itemPower, int itemDefense, int reinforcement, int reinforcementLimit, int upPower, int upDefense,
            int addPower, int addDefence)
            : base(itemID, itemName, itemExplanation, itemPrice, itemRare, itemWeight, 1)
        {
            this.weaponType = weaponType;

            effect = new EquipmentEffect(itemPower, itemDefense, reinforcementLimit,
                upPower, upDefense, addPower, addDefence);
        }

        //コピーコンストラクタ
        public WeaponItem(WeaponItem other)
            :base(other.itemID, other.itemName, other.itemExplanation,
                 other.itemPrice, other.itemRare, other.itemWeight, other.amountLimit)
        {
            this.weaponType = other.weaponType;

            effect = new EquipmentEffect(other.itemPower, other.itemDefense, other.reinforcementLimit,
                 other.upPower, other.upDefense, other.randomMinP, other.randomMaxP, other.randomMinD, other.randomMaxD);
        }

        //コピー
        public override Item Clone()
        {
            return new WeaponItem(this);
        }

        //装備強化
        public void LevelUp()
        {
            if (effect.IsLevelMax() == false)
            {
                effect.LevelUp();
            }
        }

        public WeaponType GetWeaponType()
        {
            return weaponType;
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
    }
}
