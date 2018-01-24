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
        public RangeAttack(CharacterManager manager, CharacterBase actor, ParticleManager particleManager)
            : base(manager, actor, particleManager)
        {
        }

        public override void Attack()
        {
            var box = new MoveDamageBox(new BoundingSphere(new Vector3(actor.Collision.Position.X,characterManager.GetPlayer().Collision.Position.Y,actor.Collision.Position.Z)+ (actor.GetAttackAngle() * actor.Collision.Radius / 2), 4), 10000, actor.Tag, actor.GetAttack(), actor.GetAttackAngle(),particleManager);
            characterManager.AddHitBox(box);
        }
    }
}
