/////////////////////////////////////////////////////
//・移動ＡＩ　目的もなくふらふらと移動
/////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Utility;
using Microsoft.Xna.Framework;
using System.Diagnostics;
namespace Team27_RougeLike.Object.AI
{
    class MoveAi_Search : BaseAi
    {
        private Vector3 vector;
        private static Random rand = new Random();
        private int time = 300;
        private int currenttime;
        public MoveAi_Search(CharacterBase actor)
            : base(actor)
        {
            currenttime = 0;
            VectorReset();
            Enter();
        }
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
           
            currenttime++;
            actor.Velocity = vector;
            if (currenttime > time)
            {
                VectorReset();
            }
        }
        private void VectorReset()
        {
            int angle = rand.Next(1, 360);
            vector = new Vector3((float)(Math.Cos(angle)), 0, (float)Math.Sin(angle));
            vector = new Vector3(vector.X / 2, 0, vector.Z / 2);
            currenttime = 0;
        }
    }
}
