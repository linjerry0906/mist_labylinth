using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.AI;
namespace Team27_RougeLike.Object.Character
{
    class TestSimpleMeleeEnemy : EnemyBase
    {
        public TestSimpleMeleeEnemy(Model model, Vector3 position)
            : base(model, new Status(5, 100, 50, 5, 5, 0.6f), new Transform(position, 5), new AiManager_Fool())
        {
            hitRange = 100;
            searchRange = 50;
            attackRange = 10;
            aiManager.Initialize(this);
           
        }

        public override void Initialize()
        {
        }
        public override void Attack()
        {
            var targetPosition = new Vector2(transform.angle);
            
        }
        public override void Update()
        {
            
        }
        public override void HitUpdate(Player player)
        {
            aiManager.Update(player);
        }
    }
}
