/////////////////////////////////////////////////////
//・攻撃ＡＩ　タメ、予備動作中
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.AI
{
    class AttackAi_Charge : BaseAi
    {
        private int chargeTime;
        private int attackTime;
        private int coolTime;

        private int currenttime = 0;

        public AttackAi_Charge(CharacterBase actor,int chargeTime,int attackTime,int coolTime)
            :base (actor)
        {
            this.chargeTime = chargeTime;
            this.attackTime = attackTime;
            this.coolTime = coolTime;
            Enter();
        }

        public override void Enter()
        {
            actor.SetAttackAngle();
        }

        public override void Exit()
        {
            actor.AiManager().SetAttackAi(new AttackAi_Attack(actor,attackTime,coolTime));
        }

        public override void Update()
        {
            currenttime++;
            if (currenttime > chargeTime)
            {
                Exit();
            }
        }
    }
}
