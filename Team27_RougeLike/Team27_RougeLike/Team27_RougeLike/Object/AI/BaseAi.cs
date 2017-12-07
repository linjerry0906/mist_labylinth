using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Object.AI
{
    abstract class BaseAi
    {
        protected EnemyBase actor;

        public BaseAi(EnemyBase actor)
        {
            this.actor = actor;
        }
        
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
