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
    class DamageBox : HitBoxBase, iDamageBox
    {
        private int attack;
        private Vector3 angle;
        public DamageBox(BoundingSphere collision, int time, int attack, Vector3 angle,Buff.buff buff)
            : base(collision, time,buff)
        {
            this.attack = attack;
            this.angle = angle;
        }
        public DamageBox(BoundingSphere collision, int time, string tag, int attack, Vector3 angle, Buff.buff buff)
            : base(collision, time, tag,buff)
        {
            this.attack = attack;
            this.angle = angle;
        }
        public DamageBox(BoundingSphere collision, int time, List<string> tags, int attack, Vector3 angle,Buff.buff buff)
            : base(collision, time, tags,buff)
        {
            this.attack = attack;
            this.angle = angle;
        }

        public override void Effect(CharacterBase character)
        {
            base.Effect(character);
            effectedCharacters.Add(character);
            character.Damage(attack, angle * 2);
        }

        public override void Update()
        {
            base.Update();
        }
        public int Damage()
        {
            return attack;
        }
    }
}
