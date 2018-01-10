using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{
    abstract class BaseAiManager
    {
        protected BaseAi moveAi;
        protected BaseAi attackAi;
        protected BaseAi stateAi;
        protected CharacterBase actor;

        public BaseAiManager()
        {
            this.moveAi = new MoveAi_Wait(actor);
            this.attackAi = new AttackAi_Wait(actor);
            this.stateAi = new StateAi_Normal(actor);
        }

        public virtual void Initialize(CharacterBase actor)
        {
            this.actor = actor;
        }
        public virtual void Update()
        {
            stateAi.Update();
            moveAi.Update();
            attackAi.Update();
        }

        public void SetMoveAi(BaseAi moveAi) { this.moveAi = moveAi; }
        public void SetAttackAi(BaseAi attackAi) { this.attackAi = attackAi; }
        public void SetStateAi(BaseAi stateAi) { this.stateAi = stateAi; }
        public BaseAi GetStateAi() { return stateAi; }
    }
}
