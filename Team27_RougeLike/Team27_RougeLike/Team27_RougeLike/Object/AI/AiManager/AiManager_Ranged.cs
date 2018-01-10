﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{
    class AiManager_Ranged : BaseAiManager, IEnemyAI
    {

        public AiManager_Ranged()
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
                moveAi = new MoveAi_Wait(actor);
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

                if (attackAi is AttackAi_Wait && moveAi is MoveAi_Wait)
                {
                    //時間は仮実装
                    attackAi = new AttackAi_Charge(actor, 20, 10, 30);
                }
            }
            #endregion

            #region 逃げる範囲
            if (enemyActor.WaitPointCheck(player))
            {
                if(attackAi is AttackAi_Wait)
                {
                    moveAi = new MoveAi_Escape(actor,player);
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
