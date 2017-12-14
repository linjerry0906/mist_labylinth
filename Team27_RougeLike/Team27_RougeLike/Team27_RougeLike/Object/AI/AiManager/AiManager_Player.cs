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
            if(attackAi is AttackAi_Wait)
            {
                playerActor.Move();
            }
            else
            {
                playerActor.Stop();
            }
            if (inputState.LeftButtonEnter(ButtonState.Pressed) && attackAi is AttackAi_Wait)
            {
                SetAttackAi(new AttackAi_Charge(actor, 15, 1, 50));
            }
        }
    }
}
