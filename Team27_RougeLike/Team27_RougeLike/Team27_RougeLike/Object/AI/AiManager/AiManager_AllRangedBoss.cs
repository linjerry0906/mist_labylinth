using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{
    class AiManager_AllRangedBoss : BaseAiManager, IEnemyAI
    {

        public AiManager_AllRangedBoss()
        {

        }

        public override void Initialize(CharacterBase actor)
        {
            base.Initialize(actor);
        }

        public void NearUpdate(Player player)
        {
            //敵専用ＡＩなので事前に変換しておく
            EnemyBase enemyActor = (EnemyBase)actor;
            #region 索敵範囲

            if (enemyActor.SearchCheck(player))
            {
                if ((moveAi is MoveAi_Wait || moveAi is MoveAi_Escape) && attackAi is AttackAi_Wait)
                {
                    moveAi = new MoveAi_Chase(actor, player);
                }
            }
            else
            {
                if (moveAi is MoveAi_Search) return;
                moveAi = new MoveAi_Search(actor);
            }
            #endregion

            #region 索敵範囲内攻撃範囲外

            if (enemyActor.SearchCheck(player) && !enemyActor.AttackCheck(player))
            {
                if (moveAi is MoveAi_Escape)
                {
                    moveAi = new MoveAi_Chase(actor, player);
                }

            }

            #endregion

            #region 攻撃範囲内退避範囲外

            if (enemyActor.AttackCheck(player) && !enemyActor.WaitPointCheck(player))
            {
                if (moveAi is MoveAi_Chase)
                {
                    moveAi = new MoveAi_Wait(actor);
                }

                if (attackAi is AttackAi_Wait)
                {
                    //時間は仮実装
                    attackAi = new AttackAi_Charge(actor, enemyActor.GetStatus().Attackspd, 1, enemyActor.GetStatus().Attackspd);
                }
            }
            #endregion

            #region 逃げる範囲
            if (enemyActor.WaitPointCheck(player))
            {
                if (attackAi is AttackAi_Wait)
                {
                    moveAi = new MoveAi_Escape(actor, player);
                    attackAi = new AttackAi_Charge(actor, enemyActor.GetStatus().Attackspd, 1, enemyActor.GetStatus().Attackspd);
                }
            }
            #endregion
        }
        public override void Update()
        {
            base.Update();
        }
    }
}

