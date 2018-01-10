using System;
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
    }
}
