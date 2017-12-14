/////////////////////////////////////////////////////
//・移動ＡＩ　待機状態　何もしない
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.AI
{
    class MoveAi_Wait : BaseAi
    {
        public MoveAi_Wait(CharacterBase actor)
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
