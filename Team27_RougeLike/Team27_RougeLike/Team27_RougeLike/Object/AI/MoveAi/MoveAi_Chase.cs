/////////////////////////////////////////////////////
//・移動ＡＩ　プレイヤーを追跡
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Utility;
namespace Team27_RougeLike.Object.AI
{
    class MoveAi_Chase : BaseAi
    {
        private Player player;
        private Vector3 velocity;
        public MoveAi_Chase(CharacterBase actor, Player player)
            : base(actor)
        {
            Enter();
            this.player = player;
        }
        public override void Enter()
        {

        }

        public override void Update()
        {
            velocity = player.Collision.Position - actor.Collision.Position;
            velocity.Normalize();
            actor.Velocity = velocity;
        }

        public override void Exit()
        {
        }
    }
}
