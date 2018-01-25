using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Team27_RougeLike.Object
{
    class Status
    {
        private int level;                     //現在レベル
        private int health;                    //体力現在値
        private int maxhealth;                 //体力最大値
        private int basePower;                 //装備無　攻撃力
        private int baseDiffence;              //装備無　防御力
        private int attackspd;                 //攻撃速度
        private float speed;                   //移動速度 
        private float maxSpeed = 0.6f;         //限界移動速度
        public Status(int level, int maxhealth, int basePower, int baseDiffence, int attackspd, float speed)
        {
            this.level = level;
            this.maxhealth = maxhealth;
            this.health = this.maxhealth;
            this.basePower = basePower;
            this.baseDiffence = baseDiffence;
            this.speed = speed;
            this.attackspd = attackspd;
        }
        public Status(int level, int maxhealth, int health, int basePower, int baseDiffence, int attackspd, float speed)
        {
            this.level = level;
            this.maxhealth = maxhealth;
            this.health = health;
            this.basePower = basePower;
            this.baseDiffence = baseDiffence;
            this.speed = speed;
            this.attackspd = attackspd;
        }
        public Status() { }

        /// <summary>
        /// このステータスはレベル比例にしたいのでgetOnly
        /// </summary>
        public int Attackspd { get { return attackspd; } }
        public float Movespeed { get { return speed; } }
        public int BasePower { get { return basePower + (level - 1) * 1; } }
        public int BaseArmor { get { return baseDiffence + (level - 1) * 1; } }
        public int Level { get { return level; } }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public int MaxHealth
        {
            get { return maxhealth + (level - 1) * 2; }
        }
        public void LevelUp()
        {
            level++;
        }
        public void LevelReset()
        {
            level = 1;
        }
        public float MAX_SPEED { get { return maxSpeed; } }
        public float Speed { get { return speed; } set { if (value > MAX_SPEED) { speed = MAX_SPEED; } else { speed = value; } } }
    }
}
