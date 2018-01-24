using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.Box;
using Team27_RougeLike.Object.ParticleSystem;
namespace Team27_RougeLike.Object
{
    abstract class AttackBase
    {
        protected ParticleManager particleManager;
        protected CharacterManager characterManager;
        protected CharacterBase  actor;

        public AttackBase(CharacterManager manager,CharacterBase actor,ParticleManager particleManager)
        {
            this.particleManager = particleManager;
            this.characterManager = manager;
            this.actor = actor;
        }
        public abstract void Attack();        
    }
}
