//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：GuildのUIStateInterface
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.QuestSystem
{
    interface IGuildState
    {
        void Initialize();
        void Update();
        void Draw(float constractAlpha, float currentAlpha);
        bool IsEnd();
        GuildState NextState();

        void ShutDown();
    }
}
