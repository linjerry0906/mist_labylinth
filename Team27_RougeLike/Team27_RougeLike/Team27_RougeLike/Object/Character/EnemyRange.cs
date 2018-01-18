﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Team27_RougeLike.Object.Character
{
    public struct EnemyRange
    {
        public EnemyRange(int searchRange,int waitRange, int attackRange)
        {
            this.searchRange = searchRange;
            this.waitRange = waitRange;
            this.attackRange = attackRange;
        }

        public int searchRange;      //索敵範囲
        public int waitRange;        //敵のとる間合い
        public int attackRange;      //攻撃範囲

        public static EnemyRange Fool()
        {
            return new EnemyRange(50, 20, 10);
        }
        public static EnemyRange Ranged()
        {
            return new EnemyRange(60, 25, 50);
        }
        public static EnemyRange Melee()
        {
            return new EnemyRange(50, 30, 15);
        }
        public static EnemyRange Totem()
        {
            return new EnemyRange(0, 0, 60);
        }
    }
}
