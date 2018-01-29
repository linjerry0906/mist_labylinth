using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.Box;
using Team27_RougeLike.Object.ParticleSystem;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Object
{
    class MeleeAttack : AttackBase
    {
        
        public MeleeAttack(float size, string textureName, Buff.buff buffType, string seName,float startRange, CharacterManager characterManager)
            : base(size,textureName,buffType,seName,startRange,characterManager)
        {
        }
        public MeleeAttack(float size, string textureName, Buff.buff buffType, string seName,float startRange, CharacterManager characterManager, CharacterBase actor,ParticleManager particle)
            : base(size, textureName, buffType, seName, startRange,characterManager, actor,particle)
        {
        }

        public override void Attack()
        {
            var box = new DamageBox(
                new BoundingSphere(new Vector3(actor.Collision.Position.X, characterManager.GetPlayer().Collision.Position.Y, actor.Collision.Position.Z) + (actor.GetKeepAttackAngle() * ((actor.Collision.Radius / 2)+startRange))
                , actor.Collision.Radius)
                ,1
                , actor.Tag
                , actor.GetAttack()
                , actor.GetKeepAttackAngle()
                ,buffType
                );
            
            characterManager.AddHitBox(box);
            actor.Sound(seName);
            particleManager.AddParticle(new Slash(actor, box.collision.Center,textureName));
        }

        public override AttackBase Clone(CharacterBase actor, ParticleManager particleManager)
        {
            return new MeleeAttack
                (
                size,
                textureName,
                buffType,
                seName,
                startRange,
                characterManager,
                actor,
                particleManager
                );
        }
    }
}
