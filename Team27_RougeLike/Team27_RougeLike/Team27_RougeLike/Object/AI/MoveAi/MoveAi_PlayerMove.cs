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
            if (PlessMoveKey())
            {
                Vector3 velocity = Vector3.Zero;
                if (inputState.GetKeyState(Keys.W))
                {
                    velocity += projector.Front;
                }
                if (inputState.GetKeyState(Keys.S))
                {
                    velocity += projector.Back;
                }
                if (inputState.GetKeyState(Keys.A))
                {
                    velocity += projector.Left;
                }
                if (inputState.GetKeyState(Keys.D))
                {
                    velocity += projector.Right;
                }

                velocity.Normalize();

                if (inputState.GetKeyState(Keys.LeftShift))
                {
                    actor.Velocity = actor.Velocity * 1.5f;
                }
                actor.Velocity = velocity;
            }
        }
        public bool PlessMoveKey()
        {
            return (inputState.GetKeyState(Keys.A) || inputState.GetKeyState(Keys.W) || inputState.GetKeyState(Keys.S) || inputState.GetKeyState(Keys.D));
        }
    }
}
