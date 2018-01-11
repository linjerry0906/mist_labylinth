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
    class DamageBox : HitBoxBase
    {
        private int attack;
        
        public DamageBox(BoundingSphere collision,int time,int attack)
            :base(collision,time)
        {
            this.attack = attack;
        }
        public DamageBox(BoundingSphere collision,int time,string tag,int attack)
            :base(collision,time,tag)
        {
            this.attack = attack;
        }
        public DamageBox(BoundingSphere collision,int time,List<string> tags,int attack)
            :base(collision,time,tags)
        {
            this.attack = attack;
        }

        public override void Effect(CharacterBase character)
        {
            character.Damage(attack);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
