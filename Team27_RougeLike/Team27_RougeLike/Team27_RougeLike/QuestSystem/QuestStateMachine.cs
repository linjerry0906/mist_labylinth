//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：GuildのUIState
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;

namespace Team27_RougeLike.QuestSystem
{
    public enum GuildState
    {
        Menu,
        GetQuest,
        SelectQuestRank,
        GetAward,
        End,
    }
    class QuestStateMachine
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private GuildState currentState;
        private IGuildState currentSystem;
        private Dictionary<GuildState, IGuildState> states;

        public QuestStateMachine(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            states = new Dictionary<GuildState, IGuildState>();
            states.Add(GuildState.Menu, new GuildMenu(gameManager, gameDevice));
            states.Add(GuildState.SelectQuestRank, new SelectQuestRank(gameManager, gameDevice));
            states.Add(GuildState.GetQuest, new GetQuest(gameManager, gameDevice));
            states.Add(GuildState.GetAward, new GetReward(gameManager, gameDevice));

            currentState = GuildState.Menu;
            currentSystem = states[GuildState.Menu];
            currentSystem.Initialize();
            gameManager.EnemyName.Load();
        }

        public void Update()
        {
            if (currentState == GuildState.End)
                return;

            currentSystem.Update();

            if (currentSystem.IsEnd())
            {
                currentState = currentSystem.NextState();
                currentSystem.ShutDown();

                if (currentState == GuildState.End)
                    return;
                currentSystem = states[currentState];
                currentSystem.Initialize();
            }
        }

        public void Draw(float constractAlpha, float currentAlpha)
        {
            currentSystem.Draw(constractAlpha, currentAlpha);
        }

        public bool IsEnd()
        {
            return currentState == GuildState.End;
        }

        public void ShutDown()
        {
            states.Clear();
            gameManager.EnemyName.Clear();
        }
    }
}
