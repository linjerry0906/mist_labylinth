using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Box;
namespace Team27_RougeLike.Object.Character
{
    abstract class AttackBase
    {
        private CharacterManager manager;

        public AttackBase(CharacterManager manager)
        {
            this.manager = manager;
        }
        public abstract void Attack();
        
    }
}
