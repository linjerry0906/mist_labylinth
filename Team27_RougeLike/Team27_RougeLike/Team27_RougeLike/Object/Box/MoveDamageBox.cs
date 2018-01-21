using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.ParticleSystem;

namespace Team27_RougeLike.Object.Box
{
    class MoveDamageBox : HitBoxBase, iDamageBox
    {
        private int attack;
        private Vector3 angle;
        private ParticleManager pManager;
        public MoveDamageBox(BoundingSphere collision, int time, int attack, Vector3 angle)
            : base(collision, time)
        {
            this.attack = attack;
            this.angle = angle;
        }
        public MoveDamageBox(BoundingSphere collision, int time, List<string> tags, int attack, Vector3 angle)
            : base(collision, time, tags)
        {
            this.attack = attack;
            this.angle = angle;
        }
        public MoveDamageBox(BoundingSphere collision, int time, string tag, int attack, Vector3 angle,ParticleManager particleManager, GameDevice gameDevice)
            : base(collision, time, tag)
        {
            this.attack = attack;
            this.angle = angle;
            this.pManager = particleManager;
            pManager.AddParticle(new Bullet(gameDevice, this,new Vector2(10,10)));
        }
        public MoveDamageBox(BoundingSphere collision, int time, int attack, Vector3 angle,Buff.buff buff)
           : base(collision, time,buff)
        {
            this.attack = attack;
            this.angle = angle;
        }
        public MoveDamageBox(BoundingSphere collision, int time, List<string> tags, int attack, Vector3 angle,Buff.buff buff)
            : base(collision, time, tags,buff)
        {
            this.attack = attack;
            this.angle = angle;
        }
        public MoveDamageBox(BoundingSphere collision, int time, string tag, int attack, Vector3 angle, ParticleManager particleManager, GameDevice gameDevice,Buff.buff buff)
            : base(collision, time, tag,buff)
        {
            this.attack = attack;
            this.angle = angle;
            this.pManager = particleManager;
            pManager.AddParticle(new Bullet(gameDevice, this, new Vector2(10, 10)));
        }

        public override void Update()
        {
            base.Update();
            collision.Center += angle * 1.3f;
        }

        public override void Effect(CharacterBase character)
        {
            base.Effect(character);
            effectedCharacters.Add(character);
            character.Damage(attack, angle);
            End();
        }
        public int Damage()
        {
            return attack;
        }
    }
}
