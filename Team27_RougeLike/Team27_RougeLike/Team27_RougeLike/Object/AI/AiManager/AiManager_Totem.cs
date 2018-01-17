using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{
    class AiManager_Totem : BaseAiManager, IEnemyAI
    {

        public AiManager_Totem()
        {

        }

        public override void Initialize(CharacterBase actor)
        {
            base.Initialize(actor);
            moveAi = new MoveAi_Wait(actor);
        }

        public void NearUpdate(Player player)
        {
            //敵専用ＡＩなので事前に変換しておく
            EnemyBase enemyActor = (EnemyBase)actor;

            if (enemyActor.AttackCheck(player))
            {
                if (attackAi is AttackAi_Wait)
                {
                    attackAi = new AttackAi_Charge(actor, enemyActor.GetStatus().Attackspd * 60, 1, enemyActor.GetStatus().Attackspd * 30);
                }
            }
        }
        public override void Update()
        {
            base.Update();
        }
    }
}