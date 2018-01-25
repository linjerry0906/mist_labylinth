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
    class DaggerAttack : AttackBase
    {
        public DaggerAttack(CharacterManager manager, CharacterBase actor, ParticleManager particleManager)
            : base(manager, actor, particleManager)
        {
        }

        public override void Attack()
        {
            var box = new DamageBox(new BoundingSphere(new Vector3(actor.Collision.Position.X, characterManager.GetPlayer().Collision.Position.Y, actor.Collision.Position.Z) + (actor.GetKeepAttackAngle() * actor.Collision.Radius), 3), 1, actor.Tag, actor.GetAttack(), actor.GetKeepAttackAngle());
            characterManager.AddHitBox(box);
            actor.Sound("attack1");
            particleManager.AddParticle(new Slash(actor, box.collision.Center));
        }
    }
}
