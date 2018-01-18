using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object.AI
{
    class MoveAi_PlayerMove : BaseAi
    {
        private InputState inputState;
        private Projector projector;
        private Vector3 velocity;
        public MoveAi_PlayerMove(CharacterBase actor, InputState input, Projector projector)
            : base(actor)
        {
            Enter();
            this.inputState = input;
            this.projector = projector;
        }
        public override void Enter()
        {

        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            if (inputState.GetKeyState(Keys.W))
            {
                velocity = actor.GetBuffs().GetBuff(Buff.buff.CONFUSE) ? velocity - projector.Front : velocity + projector.Front;
            }
            if (inputState.GetKeyState(Keys.S))
            {
                velocity = actor.GetBuffs().GetBuff(Buff.buff.CONFUSE) ? velocity - projector.Back : velocity + projector.Back;
            }
            if (inputState.GetKeyState(Keys.A))
            {
                velocity = actor.GetBuffs().GetBuff(Buff.buff.CONFUSE) ? velocity - projector.Left : velocity + projector.Left;
            }
            if (inputState.GetKeyState(Keys.D))
            {
                velocity = actor.GetBuffs().GetBuff(Buff.buff.CONFUSE) ? velocity - projector.Right : velocity + projector.Right;
            }
            if (velocity.LengthSquared() != 0)
            {
                velocity.Normalize();
            }

            if (inputState.GetKeyState(Keys.LeftShift) && !actor.Dodge())
            {
                velocity = actor.GetBuffs().GetBuff(Buff.buff.ACCELERATE) ? velocity * 2: velocity * 1.5f;
            }

            if (!actor.Dodge())
            {
                var old = velocity;

                //goto使ってるので変なのを入れないように
                if (inputState.DualkeyDown(Keys.W))
                {
                    velocity = actor.GetBuffs().GetBuff(Buff.buff.CONFUSE) ? velocity - projector.Front * 5.5f : velocity + projector.Front * 5.5f;

                    goto END;
                }
                if (inputState.DualkeyDown(Keys.D))
                {
                    velocity = actor.GetBuffs().GetBuff(Buff.buff.CONFUSE) ? velocity - projector.Right * 5.5f : velocity + projector.Right * 5.5f;
                    goto END;
                }
                if (inputState.DualkeyDown(Keys.A))
                {
                    velocity = actor.GetBuffs().GetBuff(Buff.buff.CONFUSE) ? velocity - projector.Left * 5.5f : velocity + projector.Left * 5.5f;
                    goto END;
                }
                if (inputState.DualkeyDown(Keys.S))
                {
                    velocity = actor.GetBuffs().GetBuff(Buff.buff.CONFUSE) ? velocity - projector.Back * 5.5f : velocity + projector.Back * 5.5f;
                }

                END:

                if (velocity != old)
                {
                    actor.AiManager().SetStateAi(new StateAi_Dodge(actor));
                }
                actor.Velocity = velocity;

            }
        }
    }
}