﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{
    class AiManager_Fool : BaseAiManager, IEnemyAI
    {

        public AiManager_Fool()
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
            if (!Confuse())
            {
                if (enemyActor.SearchCheck(player))
                {
                    if (!(moveAi is MoveAi_Chase) && attackAi is AttackAi_Wait)
                    {
                        moveAi = new MoveAi_Chase(actor, player);
                    }
                }
                else
                {
                    if (moveAi is MoveAi_Search) return;
                    moveAi = new MoveAi_Search(actor);
                }
            }
            #endregion

            #region 攻撃範囲
            if (enemyActor.AttackCheck(player))
            {
                if (attackAi is AttackAi_Wait)
                {
                    //時間は仮実装
                    attackAi = new AttackAi_Charge(actor, enemyActor.GetStatus().Attackspd, 1, enemyActor.GetStatus().Attackspd);
                }

                if (moveAi is MoveAi_Chase && !Confuse())
                {
                    moveAi = new MoveAi_Wait(actor);
                }
            }
            #endregion
            if (Confuse() && !(moveAi is MoveAi_Search))
            {
                moveAi = new MoveAi_Search(actor);
            }

        }
        public override void Update()
        {
            base.Update();
        }
    }
}
