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
        private Vector3 velocity;
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
            velocity = player.Collision.Position - actor.Collision.Position;
            velocity.Normalize();
            if (actor.AiManager() is AiManager_Melee)
            {
                actor.Velocity =  new Vector3(-velocity.X / 2 ,0, -velocity.Z / 2);
            }
            else
            {
                actor.Velocity = -velocity;
            }
        }
    }
}
