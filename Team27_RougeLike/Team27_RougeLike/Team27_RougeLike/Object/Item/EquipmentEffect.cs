using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.Item
{
    class EquipmentEffect
    {
        static Random rand = new Random();
        private int power; //攻撃力
        private int defense; //守備力
        private int addPower; //加算攻撃力
        private int addDefense; //加算防御力
        private int upPower; //レベルアップで上がる値
        private int upDefense; //レベルアップで上がる値
        private int reinforcement; //強化レベル
        private int reinforcementLimit; //強化レベル制限(5以上を想定)

        //初期加算ステータス(ランダム)
        public EquipmentEffect(int power, int defense, int reinforcementLimit, int upPower, int upDefense,
             int randomMinP, int randomMaxP, int randomMinD, int randomMaxD)
        {
            addPower = rand.Next(randomMinP, randomMaxP);
            addDefense = rand.Next(randomMinD, randomMaxD);
            
            this.power = power;
            this.defense = defense;
            this.reinforcementLimit = reinforcementLimit;
            this.upPower = upPower;
            this.upDefense = upDefense;

            //装備レベルのランダム
            reinforcement = rand.Next(0, 101);
            if (reinforcement <= 50)
            {
                reinforcement = 0;
            }
            else if (reinforcement <= 80)
            {
                reinforcement = 1;
            }
            else if (reinforcement <= 95)
            {
                reinforcement = 2;
            }
            else if (reinforcement <= 99)
            {
                reinforcement = 3;
            }
            else
            {
                reinforcement = 4;
            }

            Initialize();
        }

        //初期加算ステータス(指定)
        public EquipmentEffect(int power, int defense, int reinforcementLimit, int upPower, int upDefense,
            int addPower, int addDefense, int reinforcement = 0)
        {
            this.power = power;
            this.defense = defense;
            this.reinforcementLimit = reinforcementLimit;
            this.upPower = upPower;
            this.upDefense = upDefense;
            this.addPower = addPower;
            this.addDefense = addDefense;
            this.reinforcement = reinforcement;

            Initialize();
        }

        public void Initialize()
        {
            addPower += reinforcement * upPower;
            addDefense += reinforcement * upDefense;
        }

        public void LevelUp()
        {
            reinforcement++;

            addPower += upPower;
            addDefense += upDefense;
        }

        public int GetPower()
        {
            return power + addPower;
        }

        public int GetDefense()
        {
            return defense + addDefense;
        }

        public int GetReinforcementLimit()
        {
            return reinforcementLimit;
        }

        public bool IsLevelMax()
        {
            //条件が成立する場合はTRUE
            return (reinforcement == reinforcementLimit);
        }

        //セーブ用
        public int GetAddPower()
        {
            return addPower;
        }
        public int GetAddDefense()
        {
            return addDefense;
        }
        public int GetReinforcement()
        {
            return reinforcement;
        }
    }
}
