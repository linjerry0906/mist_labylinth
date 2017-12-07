/////////////////////////////////////////////////////
//・移動ＡＩ　プレイヤーから離れていく
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Object.AI
{
    class MoveAi_Escape : BaseAi
    {
        private Player player;
        public MoveAi_Escape(EnemyBase actor, Player player)
            : base(actor)
        {
            Enter();
            this.player = player;
        }
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            actor.transform.angle = Angle.CheckAngle(player.transform.position, actor.transform.position);
            var dx = Math.Cos(actor.transform.angle * Math.PI / 180) * actor.status.Movespeed;
            var dz = Math.Sin(actor.transform.angle * Math.PI / 180) * actor.status.Movespeed;
            actor.transform.position.X -= (float)dx;
            actor.transform.position.Z -= (float)dz;
        }
    }
}
