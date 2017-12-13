using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{
    class AiManager_Fool : BaseAiManager
    {
        public AiManager_Fool()
        {

        }

        public override void Initialize(EnemyBase actor)
        {
            base.Initialize(actor);
        }

        public override void Update(Player player)
        {
            #region 索敵範囲
            if (actor.SearchCheck(player))
            {
                //攻撃可能状態なら必ず向かってくる
                if (!(moveAi is MoveAi_Chase) && attackAi is AttackAi_Wait)
                {
                    moveAi = new MoveAi_Chase(actor,player);
                }
            }
            else
            {

            }
            #endregion

            #region 索敵範囲内攻撃範囲外
            if (actor.SearchCheck(player) && !actor.AttackCheck(player))
            {

            }
            #endregion

            #region 攻撃範囲
            if (actor.AttackCheck(player))
            {
                if (attackAi is AttackAi_Wait)
                {
                    //時間は仮実装
                    attackAi = new AttackAi_Charge(actor,30,10,30);
                }

                if(moveAi is MoveAi_Chase)
                {
                    moveAi = new MoveAi_Wait(actor);
                }
                if (moveAi is MoveAi_Wait && attackAi is AttackAi_CoolDown)
                {
                    moveAi = new MoveAi_Escape(actor,player);
                }
            }
            else
            {
                if(moveAi is MoveAi_Escape && actor.WaitPointCheck(player))
                {
                    moveAi = new MoveAi_Wait(actor);
                }
            }
            #endregion

            base.Update(player);
        }
    }
}
