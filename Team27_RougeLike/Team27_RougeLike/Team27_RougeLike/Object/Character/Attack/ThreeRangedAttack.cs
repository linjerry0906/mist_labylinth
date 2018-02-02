using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.Box;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.ParticleSystem;
namespace Team27_RougeLike.Object
{
    class ThreeRangedAttack : AttackBase
    {
        public ThreeRangedAttack(float size, string textureName, Buff.buff buffType, string seName, float startRange, CharacterManager characterManager)
            : base(size, textureName, buffType, seName, startRange, characterManager)
        {
        }
        public ThreeRangedAttack(float size, string textureName, Buff.buff buffType, string seName, float startRange, CharacterManager characterManager, CharacterBase actor, ParticleManager particle)
            : base(size, textureName, buffType, seName, startRange, characterManager, actor, particle)
        {
        }
        /// <summary>
        /// すごく馬鹿らしい計算だとは思いますがモチベが今ないです　あとで治すかもしれない・・
        /// </summary>
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
            particleManager.AddParticle(new Bullet(box, new Vector2(10, 10), textureName));

            var box1 = new MoveDamageBox(
                new BoundingSphere(new Vector3(actor.Collision.Position.X, characterManager.GetPlayer().Collision.Position.Y, actor.Collision.Position.Z) + (actor.GetAttackAngle() * ((actor.Collision.Radius / 2) + startRange)), 3),
                10000,
                actor.Tag,
                actor.GetAttack(),
             new Vector3((float)Math.Cos(Math.Atan2(actor.GetAttackAngle().Z, actor.GetAttackAngle().X)+0.5f), 0, (float)Math.Sin(Math.Atan2(actor.GetAttackAngle().Z, actor.GetAttackAngle().X)+0.5f)),
                buffType);
            characterManager.AddHitBox(box1);
            particleManager.AddParticle(new Bullet(box1, new Vector2(10, 10), textureName));

            var box2 = new MoveDamageBox(
                new BoundingSphere(new Vector3(actor.Collision.Position.X, characterManager.GetPlayer().Collision.Position.Y, actor.Collision.Position.Z) + (actor.GetAttackAngle() * ((actor.Collision.Radius / 2) + startRange)), 3),
                10000,
                actor.Tag,
                actor.GetAttack(),
             new Vector3((float)Math.Cos(Math.Atan2(actor.GetAttackAngle().Z, actor.GetAttackAngle().X) - 0.5f), 0, (float)Math.Sin(Math.Atan2(actor.GetAttackAngle().Z, actor.GetAttackAngle().X) - 0.5f)),
                buffType);
            characterManager.AddHitBox(box2);
            particleManager.AddParticle(new Bullet(box2, new Vector2(10, 10), textureName));

            characterManager.Sound(seName);
        }
        public override AttackBase Clone(CharacterBase actor, ParticleManager particleManager)
        {
            return new ThreeRangedAttack
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
