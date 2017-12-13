using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.AI;
using Team27_RougeLike.Utility;
namespace Team27_RougeLike.Object.Character
{
    class TestSimpleMeleeEnemy : EnemyBase
    {
        public TestSimpleMeleeEnemy(Vector3 position)
            : base(new Status(5, 100, 50, 5, 5, 0.6f), new CollisionSphere(position,5), new AiManager_Fool(),"test")
        {
            hitRange = 100;
            searchRange = 50;
            attackRange = 10;
            aiManager.Initialize(this);

            motion = new Motion();
            for (int i = 0; i < 6; i++)
            {
                motion.Add(i, new Rectangle(i * 64, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 5), new Timer(0.1f));
        }

        public override void Initialize()
        {
        }
        public override void Attack()
        {
            var targetPosition = new Vector2(angle);
            
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void HitUpdate(Player player,GameTime gameTime)
        {
            aiManager.Update(player);
        }
    }
}
