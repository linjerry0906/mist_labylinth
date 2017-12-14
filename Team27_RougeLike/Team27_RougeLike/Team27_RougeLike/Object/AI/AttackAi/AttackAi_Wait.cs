/////////////////////////////////////////////////////
//・攻撃ＡＩ　攻撃可能、待機状態　何もしない
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.AI
{
    class AttackAi_Wait : BaseAi
    {
        public AttackAi_Wait(CharacterBase actor)
            :base(actor)
        {
            Enter();
        }
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }

    }
}
