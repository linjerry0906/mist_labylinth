using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;
using Team27_RougeLike.QuestSystem;

namespace Team27_RougeLike.UI
{
    class PlayerQuestUI
    {
        private Renderer renderer;
        private InputState input;
        private GameManager gameManager;
        private PlayerQuest playerQuest;

        private Vector2 position;           //Offset位置

        private List<Quest> quests;         //受けているクエスト
        private Button[] buttons;           //判定用ボタン
        private int currentQuest;           //カーソルに合わせたボタンの添え字

        private static readonly int BUTTON_HEIGHT = 42;
        private static readonly int BUTTON_WIDTH = 280;

        public PlayerQuestUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            this.position = position;
            this.gameManager = gameManager;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            playerQuest = gameManager.PlayerQuest;

            quests = playerQuest.CurrentQuest();
            InitButtons();
            currentQuest = -1;
        }

        /// <summary>
        /// Buttonを初期化
        /// </summary>
        private void InitButtons()
        {
            buttons = new Button[quests.Count];             //Quest数に合わせて生成
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new Button(
                    position + new Vector2(0, 30 + i * (BUTTON_HEIGHT + 5)), BUTTON_WIDTH, BUTTON_HEIGHT);
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update()
        {
            currentQuest = -1;

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);
            for (int i = 0; i < buttons.Length; i++)
            {
                if (!buttons[i].IsClick(mousePos))
                    continue;

                currentQuest = i;
                break;
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="alpha">透明度</param>
        public void Draw(float alpha)
        {
            DrawQuestList(alpha);
        }

        /// <summary>
        /// Listを描画
        /// </summary>
        /// <param name="alpha"></param>
        private void DrawQuestList(float alpha)
        {
            int currentCount = 0, maxCount = 0;
            playerQuest.Limit(ref currentCount, ref maxCount);
            renderer.DrawTexture("fade", position, new Vector2(BUTTON_WIDTH, 22), alpha * 0.6f);
            renderer.DrawString(
                "クエスト（" + currentCount + "/" + maxCount + "）",
                position + new Vector2(BUTTON_WIDTH / 2, 0), Color.White,
                new Vector2(1.1f, 1.1f), alpha * 1.5f, true);

            #region List描画
            for (int i = 0; i < buttons.Length; i++)
            {
                Vector2 drawPos = new Vector2(buttons[i].Position().X, buttons[i].Position().Y);
                float drawAlpha = alpha * 0.6f;
                Color color = Color.White;
                if(i == currentQuest)
                {
                    drawAlpha = alpha * 0.8f;
                    color = Color.Gold;
                }

                string questName = quests[i].QuestName();

                renderer.DrawTexture("fade", drawPos, buttons[i].Size(), drawAlpha);
                renderer.DrawString(
                    questName, 
                    drawPos + new Vector2(10, BUTTON_HEIGHT / 2),
                    color,
                    new Vector2(1.1f, 1.1f),
                    alpha, false, true);
            }
            #endregion
        }
    }
}
