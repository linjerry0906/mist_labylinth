//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.22
// 内容　：受けているクエストを表示
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;
using Team27_RougeLike.QuestSystem;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.UI
{
    class PlayerQuestUI
    {
        private Renderer renderer;
        private InputState input;
        private GameManager gameManager;
        private PlayerQuest playerQuest;
        private EnemyNameLoader enemyName;
        private ItemManager itemManager;

        private Vector2 offsetPosition;           //Offset位置

        private List<Quest> quests;         //受けているクエスト
        private Button[] buttons;           //判定用ボタン
        private int currentQuest;           //カーソルに合わせたボタンの添え字

        private static readonly int BUTTON_HEIGHT = 42;
        private static readonly int BUTTON_WIDTH = 280;

        public PlayerQuestUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            this.offsetPosition = position;
            this.gameManager = gameManager;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            enemyName = gameManager.EnemyName;
            enemyName.Load();
            itemManager = gameManager.ItemManager;
            playerQuest = gameManager.PlayerQuest;
            playerQuest.UpdateQuestProcess();

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
                    offsetPosition + new Vector2(0, 30 + i * (BUTTON_HEIGHT + 5)), BUTTON_WIDTH, BUTTON_HEIGHT);
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
        /// クエスト詳細を表示
        /// </summary>
        /// <param name="alpha"></param>
        public void DrawQuestInfo(float alpha)
        {
            if (currentQuest < 0)
                return;

            Quest currentQuestInfo = quests[currentQuest];

            Vector2 position = input.GetMousePosition() + new Vector2(35, 50);
            if (position.Y > 210)
                position.Y = 210;

            renderer.DrawTexture("fade", position + new Vector2(-10, -15),
                new Vector2(450, 520), alpha * 0.8f);

            Vector2 line = new Vector2(0, 40);
            Color color = Color.Lerp(Color.Gold, Color.Red, currentQuestInfo.Difficulty() / 7.0f);
            //QuestName
            renderer.DrawString(
                currentQuestInfo.QuestName(), position, new Vector2(1.2f, 1.2f),
                color, alpha);

            Vector2 fontSize = new Vector2(1.1f, 1.1f);
            //Quest説明
            renderer.DrawString(
                currentQuestInfo.QuestInfo(), position + line, fontSize,
                Color.White, alpha);
            //報酬
            Vector2 offsetX = new Vector2(15, 0);
            renderer.DrawString(
                "報酬", position + 4 * line, fontSize,
                Color.Gold, alpha);
            renderer.DrawString(
                "賞金", position + 4.5f * line + offsetX, fontSize,
                Color.White, alpha);
            renderer.DrawString(
                currentQuestInfo.GainMoney().ToString(), position + 5 * line + 2 * offsetX, fontSize,
                Color.White, alpha);
            renderer.DrawString(
                "ギルドポイント", position + 6 * line + offsetX, fontSize,
                Color.White, alpha);
            renderer.DrawString(
                currentQuestInfo.GuildExp().ToString(), position + 6.5f * line + 2 * offsetX, fontSize,
                Color.White, alpha);
            renderer.DrawString(
                "アイテム ", position + 7.5f * line + offsetX, fontSize,
                Color.White, alpha);

            if (currentQuestInfo.AwardItem() != null)
            {
                for (int i = 0; i < currentQuestInfo.AwardItem().Length; i++)
                {
                    Item item = itemManager.GetConsumption(currentQuestInfo.AwardItem()[i]);
                    renderer.DrawString(
                        item.GetItemName(), position + (8 + i * 0.5f) * line + 2 * offsetX, fontSize,
                        Color.White, alpha);
                }
            }

            renderer.DrawString(
                "達成条件 ", position + 10 * line, fontSize,
                Color.Red, alpha);

            for (int i = 0; i < currentQuestInfo.RequireID().Length; i++)
            {
                if (currentQuestInfo is CollectQuest)
                {
                    Item item = itemManager.GetConsumption(currentQuestInfo.RequireID()[i]);
                    renderer.DrawString(
                        item.GetItemName(), position + (10.5f + i * 0.5f) * line + offsetX, fontSize,
                        Color.White, alpha);

                    Vector2 numPos = position + (10.5f + i * 0.5f) * line;
                    numPos.X += 350;
                    renderer.DrawString(
                        currentQuestInfo.CurrentState()[i].CurrentAmount + " / " +
                        currentQuestInfo.CurrentState()[i].RequireAmount,
                        numPos, fontSize,
                        Color.White, alpha);
                }
                else if (currentQuestInfo is BattleQuest)
                {
                    string name = "まだ";
                    name = enemyName.GetEnemyName(currentQuestInfo.RequireID()[i]);
                    renderer.DrawString(
                        name, position + (10.5f + i * 0.5f) * line + offsetX, fontSize,
                        Color.White, alpha);

                    Vector2 numPos = position + (10.5f + i * 0.5f) * line;
                    numPos.X += 350;
                    renderer.DrawString(
                        currentQuestInfo.CurrentState()[i].CurrentAmount + " / " +
                        currentQuestInfo.CurrentState()[i].RequireAmount,
                        numPos, fontSize,
                        Color.White, alpha);
                }
            }
        }

        /// <summary>
        /// Listを描画
        /// </summary>
        /// <param name="alpha"></param>
        private void DrawQuestList(float alpha)
        {
            int currentCount = 0, maxCount = 0;
            playerQuest.Limit(ref currentCount, ref maxCount);
            renderer.DrawTexture("fade", offsetPosition, new Vector2(BUTTON_WIDTH, 22), alpha * 0.6f);
            renderer.DrawString(
                "クエスト（" + currentCount + "/" + maxCount + "）",
                offsetPosition + new Vector2(BUTTON_WIDTH / 2, 0), Color.White,
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
