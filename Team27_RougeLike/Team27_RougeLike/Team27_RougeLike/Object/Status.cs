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
        private int basepower;                 //装備無　攻撃力
        private int basearmor;                 //装備無　防御力
        private int attackspd;                 //攻撃速度
        private float velocity;                //移動速度 
        private float maxSpeed = 0.3f;         //限界移動速度
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
        public float MAX_SPEED { get { return maxSpeed; } }
    }
}
