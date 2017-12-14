/////////////////////////////////////////////////////
//・攻撃ＡＩ　攻撃タイミング　判定を出し続ける
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.AI
{
    class AttackAi_LongAttack : BaseAi
    {
        private int attackTime;
        private int coolTime;

        private int currenttime = 0;
        public AttackAi_LongAttack(CharacterBase actor,int attackTime,int coolTime)
            :base (actor)
        {
            this.attackTime = attackTime;
            this.coolTime = coolTime;
            Enter();
        }

        public override void Enter()
        {
            actor.Attack();
        }

        public override void Exit()
        {
            actor.AiManager().SetAttackAi(new AttackAi_CoolDown(actor,coolTime));
        }

        public override void Update()
        {
            actor.Attack();
            currenttime++;
            if (currenttime > attackTime)
            {
                Exit();
            }
        }
        
    }
}
