using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Device;
using Microsoft.Xna.Framework.Input;
namespace Team27_RougeLike.Object.AI
{
    class AiManager_Player : BaseAiManager
    {
        private InputState inputState;
        public AiManager_Player(InputState inputState)
        {
            this.inputState = inputState;
        }

        public override void Initialize(CharacterBase actor)
        {
            base.Initialize(actor);
        }

        public override void Update()
        {
            //プレイヤー専用ＡＩなので事前に変換しておく
            Player playerActor = (Player)actor;
            base.Update();

            if (PlessMoveKey() && !(moveAi is MoveAi_PlayerMove))
            {
                moveAi = new MoveAi_PlayerMove(actor, inputState, playerActor.Projecter);
            }

            if (!PlessMoveKey() && !(moveAi is MoveAi_Wait))
            {
                moveAi = new MoveAi_Wait(actor);
            }

            if (attackAi is AttackAi_Wait)
            {
            }
            else
            {
                playerActor.Stop();
            }
            if (inputState.LeftButtonEnter(ButtonState.Pressed) && attackAi is AttackAi_Wait)
            {
                SetAttackAi(new AttackAi_Charge(actor, 15, 1, 5));
            }
        }

        public bool PlessMoveKey()
        {
            return (inputState.GetKeyState(Keys.A) || inputState.GetKeyState(Keys.W) || inputState.GetKeyState(Keys.S) || inputState.GetKeyState(Keys.D));
        }
    }
}
