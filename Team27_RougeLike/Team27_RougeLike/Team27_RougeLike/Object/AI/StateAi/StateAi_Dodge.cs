/////////////////////////////////////////////////////
//・状態ＡＩ　回避状態
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.AI
{
    class StateAi_Dodge : BaseAi
    {
        private int time = 30;
        public StateAi_Dodge(CharacterBase actor)
            : base(actor)
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
            time--;
            if(time <= 0)
            {
                actor.AiManager().SetStateAi(new StateAi_Normal(actor));
            }
        }
    }
}
