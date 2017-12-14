/////////////////////////////////////////////////////
//・攻撃ＡＩ　攻撃タイミング　一瞬判定を出す
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.AI
{
    class AttackAi_Attack : BaseAi
    {
        private int attackTime;
        private int coolTime;

        private int currenttime = 0;
        public AttackAi_Attack(CharacterBase actor,int attackTime,int coolTime)
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
            currenttime++;
            if (currenttime > attackTime)
            {
                Exit();
            }
        }
        
    }
}
