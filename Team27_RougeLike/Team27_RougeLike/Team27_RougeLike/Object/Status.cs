using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Team27_RougeLike.Object
{
    class Status
    {
        protected int level;            //現在レベル
        protected int health;           //体力現在値
        protected int maxhealth;        //体力最大値
        protected int basepower;        //装備無　攻撃力
        protected int basearmor;        //装備無　防御力
        protected int attackspd;        //攻撃速度
        protected float velocity;       //移動速度 
        public Status(int level, int maxhealth, int power, int armor, int attackspd, float velocity)
        {
            this.level = level;
            this.maxhealth = maxhealth;
            this.health = this.maxhealth;
            this.basepower = power;
            this.basearmor = armor;
            this.velocity = velocity;
            this.attackspd = attackspd;
        }
        public Status() { }

        public int Attackspd { get { return attackspd; } }  
        public float Movespeed { get { return velocity; } }
        public int BasePower { get { return basepower; } }
        public int BaseArmor { get { return basearmor; } }
        public int Level { get { return level; } }
        public int Health
        {
            get
            { return health; }
            set
            {
                if (health + value > maxhealth) { health = maxhealth; }
                else { health = health + value; }
            }
        }
        public void LevelUp()
        {
            level++;
        }
        public void LevelReset()
        {
            level = 1;
        }
    }
}
