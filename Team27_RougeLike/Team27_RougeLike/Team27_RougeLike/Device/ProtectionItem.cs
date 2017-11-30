﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Device
{
    class ProtectionItem : Item
    {

        //防具の種類
        public enum ProtectionType
        {
            Head,
            Armor,
            Arm,
            Shoes,
        }
        private ProtectionType protectionType;

        private EquipmentEffect effect;

        //強化値ランダム
        public ProtectionItem(string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, ProtectionType protectionType,
            int itemPower, int itemDefense, int reinforcement, int reinforcementLimit, int upPower, int upDefense,
            int randomMinP, int randomMaxP, int randomMinD, int randomMaxD)
            :base(itemName, itemExplanation, itemPrice, itemRare, itemWeight, 1)
        {
            this.protectionType = protectionType;

            effect = new EquipmentEffect(itemPower, itemDefense, reinforcementLimit,
                upPower, upDefense, randomMinP, randomMaxP, randomMinD, randomMaxD);
        }

        //強化値指定
        public ProtectionItem(string itemName, string itemExplanation,
            int itemPrice, int itemRare, float itemWeight, ProtectionType protectionType,
            int itemPower, int itemDefense, int reinforcement, int reinforcementLimit, int upPower, int upDefense,
            int addPower, int addDefence)
            : base(itemName, itemExplanation, itemPrice, itemRare, itemWeight, 1)
        {
            this.protectionType = protectionType;

            effect = new EquipmentEffect(itemPower, itemDefense, reinforcementLimit,
                upPower, upDefense, addPower, addDefence);
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
            return effect.GetPower();
        }
        public int GetAddDefence()
        {
            return effect.GetDefense();
        }
        public int GetReinforcement()
        {
            return effect.GetReinforcement();
        }
    }
}
