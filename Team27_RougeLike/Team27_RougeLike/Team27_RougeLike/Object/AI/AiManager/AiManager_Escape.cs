using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{
    class AiManager_Escape : BaseAiManager, IEnemyAI
    {

        public AiManager_Escape()
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
                moveAi = new MoveAi_Escape(actor, player);
            }
            else
            {
                if (moveAi is MoveAi_Search) return;
                moveAi = new MoveAi_Search(actor);
            }
            #endregion
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
