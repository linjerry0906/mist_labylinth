/////////////////////////////////////////////////////
//・攻撃ＡＩ　攻撃後、元に戻す
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.AI
{
    class AttackAi_CoolDown : BaseAi
    {
        private int coolTime;

        private int currenttime  = 0;
        public AttackAi_CoolDown(CharacterBase actor,int coolTime)
            :base (actor)
        {
            this.coolTime = coolTime;
            Enter();            
        }
        
        public override void Enter()
        {
        }

        public override void Exit()
        {
            actor.AiManager().SetAttackAi(new AttackAi_Wait(actor));
        }

        public override void Update()
        {
            currenttime++;
            if (currenttime > coolTime)
            {
                Exit();
            }
        }

    }
}
