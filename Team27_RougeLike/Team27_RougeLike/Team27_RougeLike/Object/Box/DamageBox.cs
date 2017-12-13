using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Object.Box
{
    class DamageBox : HitBoxBase
    {
        public DamageBox(BoundingSphere collision,int time)
            :base(collision,time)
        {

        }
        public DamageBox(BoundingSphere collision,int time,string tag)
            :base(collision,time,tag)
        {

        }
        public DamageBox(BoundingSphere collision,int time,List<string> tags)
            :base(collision,time,tags)
        {

        }

        public override void Effect(CharacterBase character)
        {
            character.Damage(50);
        }
    }
}
