using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.Box;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.ParticleSystem;
namespace Team27_RougeLike.Object
{
    class RangeAttack : AttackBase
    {
        public RangeAttack(float size, string textureName, Buff.buff buffType, string seName, float startRange, CharacterManager characterManager)
            : base(size, textureName, buffType, seName, startRange, characterManager)
        {
        }
        public RangeAttack(float size, string textureName, Buff.buff buffType, string seName, float startRange, CharacterManager characterManager, CharacterBase actor, ParticleManager particle)
            : base(size, textureName, buffType, seName, startRange, characterManager, actor, particle)
        {
        }
        public override void Attack()
        {
            var box = new MoveDamageBox(
                new BoundingSphere(new Vector3(actor.Collision.Position.X, characterManager.GetPlayer().Collision.Position.Y, actor.Collision.Position.Z) + (actor.GetAttackAngle() * ((actor.Collision.Radius / 2) + startRange)), 3),
                10000,
                actor.Tag,
                actor.GetAttack(),
                actor.GetAttackAngle(),
                buffType);
            characterManager.AddHitBox(box);
            characterManager.Sound(seName);
            particleManager.AddParticle(new Bullet(box, new Vector2(10, 10), textureName));
        }
        public override AttackBase Clone(CharacterBase actor, ParticleManager particleManager)
        {
            return new RangeAttack
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
