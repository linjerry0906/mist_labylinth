/////////////////////////////////////////////////////
//・状態ＡＩ　通常状態　なにもしない
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Object.AI
{
    class StateAi_Normal : BaseAi
    {
        public StateAi_Normal(CharacterBase actor)
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
        }
    }
}
