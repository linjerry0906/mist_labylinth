﻿using System;
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
        public TestSimpleMeleeEnemy(Vector3 position,CharacterManager characterManager)
            : base(new Status(5, 100, 50, 5, 5, 0.3f), new CollisionSphere(position,5.0f), new AiManager_Fool(),"test",characterManager)
        {
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
        }
        public override void Update(GameTime gameTime)
        {
            aiManager.Update();
            base.Update(gameTime);
        }
        public override void NearUpdate(Player player,GameTime gameTime)
        {
           ((IEnemyAI)aiManager).NearUpdate(player);
        }
    }
}
