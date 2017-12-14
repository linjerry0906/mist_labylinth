/////////////////////////////////////////////////////
//・移動ＡＩ　特定位置への移動
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Object.AI
{
    class MoveAi_TargetMove : BaseAi
    {
        Vector3 targetPosition;
        float rad;        
        public MoveAi_TargetMove(CharacterBase actor ,Vector3 targetPosition)
            :base(actor)
        {
            Enter();
            this.targetPosition = targetPosition;
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
