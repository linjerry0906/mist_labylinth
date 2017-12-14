/////////////////////////////////////////////////////
//・移動ＡＩ　プレイヤーから離れていく
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Utility;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{
    class MoveAi_Escape : BaseAi
    {
        private Player player;
        public MoveAi_Escape(CharacterBase actor, Player player)
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
            actor.angle = Angle.CheckAngle(player.Collision.Position, actor.Collision.Position);
            var dx = Math.Cos(actor.angle * Math.PI / 180) * actor.status.Movespeed;
            var dz = Math.Sin(actor.angle * Math.PI / 180) * actor.status.Movespeed;
            
            actor.Collision.Position = new Vector3
                (
                actor.Collision.Position.X - (float)dx,
                actor.Collision.Position.Y,
                actor.Collision.Position.Z - (float)dz
                );

        }
    }
}
