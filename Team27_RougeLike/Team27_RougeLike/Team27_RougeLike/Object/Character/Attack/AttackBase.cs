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
        protected CharacterBase actor;

        protected string seName;
        protected string textureName;
        protected float size;
        protected Buff.buff buffType;
        protected float startRange;

        public AttackBase(float size, string textureName, Buff.buff buffType, string seName,float startRange, CharacterManager characterManager)
        {
            this.characterManager = characterManager;
            this.textureName = textureName;
            this.buffType = buffType;
            this.size = size;
            this.seName = seName;
            this.startRange = startRange;
        }

        public AttackBase(float size, string textureName, Buff.buff buffType, string seName,float startRange, CharacterManager characterManager,CharacterBase actor,ParticleManager particleManager)
        {
            this.characterManager = characterManager;
            this.textureName = textureName;
            this.buffType = buffType;
            this.size = size;
            this.seName = seName;
            this.particleManager = particleManager;
            this.actor = actor;
            this.startRange = startRange;
        }

        public abstract AttackBase Clone(CharacterBase actor, ParticleManager particleManager);

        public abstract void Attack();
    }
}