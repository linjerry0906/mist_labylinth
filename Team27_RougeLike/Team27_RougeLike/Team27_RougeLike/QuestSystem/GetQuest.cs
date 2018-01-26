//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：GuildのUIState
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Device;
using Team27_RougeLike.UI;
using Team27_RougeLike.Def;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.QuestSystem
{
    class GetQuest : IGuildState
    {
        private enum ButtonEnum
        {
            戻る = 0,
            受注,
            Null,
        }

        private GameManager gameManager;
        private ItemManager itemManager;
        private GameDevice gameDevice;
        private InputState input;
        private Renderer renderer;
        private GuildState nextState;           //次の画面
        private bool isEnd;                     //終了フラグ

        private Window leftBackLayer;           //左半分の背景レイヤー
        private Window rightBackLayer;          //右半分の背景レイヤー
        private Button[] buttons;               //メインボタン
        private ButtonEnum currentButton;       //現在位置メインボタン

        private List<Quest> quests;             //クエスト
        private List<Button> questButtons;      //Questのリストボタン
        private int questIndex;
        private Quest currentInfo;              //現在選択のクエストの詳細
        private EnemyNameLoader enemyName;

        private DungeonPopUI popUI;

        public GetQuest(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameManager = gameManager;
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            itemManager = gameManager.ItemManager;
        }

        public void Draw(float constractAlpha, float currentAlpha)
        {
            leftBackLayer.Draw(currentAlpha * 1.2f);
            rightBackLayer.Draw(currentAlpha * 1.2f);

            DrawMainButton(constractAlpha, currentAlpha);
            DrawQuestList(constractAlpha, currentAlpha);
            DrawInfo(constractAlpha, currentAlpha);
            popUI.Draw();
        }

        /// <summary>
        /// ボタンを描画
        /// </summary>
        /// <param name="constractAlpha">対比透明度（UI）</param>
        /// <param name="currentAlpha">現在透明度（UI）</param>
        private void DrawMainButton(float constractAlpha, float currentAlpha)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == (int)ButtonEnum.受注 &&
                    currentInfo == null)
                {
                    continue;
                }

                Color color = Color.Black;
                float adjustAlpha = 1.0f;
                if (i == (int)currentButton)    //OnButton色, 透明度
                {
                    adjustAlpha = 0.8f;
                    color = Color.Gold;
                }

                Vector2 position = new Vector2(buttons[i].Position().X, buttons[i].Position().Y);
                renderer.DrawTexture("white", position, buttons[i].Size(), currentAlpha * adjustAlpha);

                renderer.DrawString(            //Button文字
                    ((ButtonEnum)i).ToString(),
                    buttons[i].ButtonCenterVector(),
                    color, new Vector2(1.1f, 1.1f), constractAlpha * currentAlpha,
                    true, true);
            }
        }

        /// <summary>
        /// QuestListを描画
        /// </summary>
        /// <param name="constractAlpha"></param>
        /// <param name="currentAlpha"></param>
        private void DrawQuestList(float constractAlpha, float currentAlpha)
        {
            for (int i = 0; i < quests.Count; i++)
            {
                Color color = Color.Black;
                float adjustAlpha = 1.0f;
                if (i == questIndex)            //OnButton色, 透明度
                {
                    adjustAlpha = 0.8f;
                    color = Color.Gold;
                }

                Vector2 position = new Vector2(questButtons[i].Position().X, questButtons[i].Position().Y);
                renderer.DrawTexture("white", position, questButtons[i].Size(), currentAlpha * adjustAlpha);

                renderer.DrawString(            //Button文字
                    quests[i].QuestName(),
                    questButtons[i].ButtonCenterVector(),
                    color, new Vector2(1.1f, 1.1f), constractAlpha * currentAlpha,
                    true, true);
            }
        }

        /// <summary>
        /// Quest詳細を表示
        /// </summary>
        /// <param name="constractAlpha"></param>
        /// <param name="currentAlpha"></param>
        private void DrawInfo(float constractAlpha, float currentAlpha)
        {
            if (currentInfo == null)
                return;

            Vector2 position = rightBackLayer.GetOffsetPosition() + new Vector2(10, 10);
            Vector2 line = new Vector2(0, 40);
            Color color = Color.Lerp(Color.Gold, Color.Red, currentInfo.Difficulty() / 7.0f);
            //QuestName
            renderer.DrawString(
                currentInfo.QuestName(), position, new Vector2(1.2f, 1.2f),
                color, constractAlpha * currentAlpha);

            Vector2 fontSize = new Vector2(1.1f, 1.1f);
            //Quest説明
            renderer.DrawString(
                currentInfo.QuestInfo(), position + line, fontSize,
                Color.White, constractAlpha * currentAlpha);
            //報酬
            Vector2 offsetX = new Vector2(15, 0);
            renderer.DrawString(
                "報酬", position + 4 * line, fontSize,
                Color.Gold, constractAlpha * currentAlpha);
            renderer.DrawString(
                "賞金", position + 4.5f * line + offsetX, fontSize,
                Color.White, constractAlpha * currentAlpha);
            renderer.DrawString(
                currentInfo.GainMoney().ToString(), position + 5 * line + 2 * offsetX, fontSize,
                Color.White, constractAlpha * currentAlpha);
            renderer.DrawString(
                "ギルドポイント", position + 6 * line + offsetX, fontSize,
                Color.White, constractAlpha * currentAlpha);
            renderer.DrawString(
                currentInfo.GuildExp().ToString(), position + 6.5f * line + 2 * offsetX, fontSize,
                Color.White, constractAlpha * currentAlpha);
            renderer.DrawString(
                "アイテム ", position + 7.5f * line + offsetX, fontSize,
                Color.White, constractAlpha * currentAlpha);

            if (currentInfo.AwardItem() != null)
            {
                for (int i = 0; i < currentInfo.AwardItem().Length; i++)
                {
                    Item item = null;
                    if (currentInfo.AwardType()[i] == "i")
                        item = itemManager.GetConsumption(currentInfo.AwardItem()[i]);
                    else if (currentInfo.AwardType()[i] == "e")
                        item = itemManager.GetEquipment(currentInfo.AwardItem()[i]);

                    renderer.DrawString(
                        item.GetItemName(), position + (8 + i * 0.5f) * line + 2 * offsetX, fontSize,
                        Color.White, constractAlpha * currentAlpha);
                }
            }

            renderer.DrawString(
                "達成条件 ", position + 10 * line, fontSize,
                Color.Red, constractAlpha * currentAlpha);

            for (int i = 0; i < currentInfo.RequireID().Length; i++)
            {
                if (currentInfo is CollectQuest)
                {
                    Item item = itemManager.GetConsumption(currentInfo.RequireID()[i]);
                    renderer.DrawString(
                        item.GetItemName(), position + (10.5f + i * 0.5f) * line + offsetX, fontSize,
                        Color.White, constractAlpha * currentAlpha);

                    Vector2 numPos = position + (10.5f + i * 0.5f) * line;
                    numPos.X = rightBackLayer.GetCenter().X + 120;
                    renderer.DrawString(
                        currentInfo.CurrentState()[i].RequireAmount + "個",
                        numPos, fontSize,
                        Color.White, constractAlpha * currentAlpha);
                }
                else if (currentInfo is BattleQuest)
                {
                    string name = "まだ";
                    name = enemyName.GetEnemyName(currentInfo.RequireID()[i]);
                    renderer.DrawString(
                        name, position + (10.5f + i * 0.5f) * line + offsetX, fontSize,
                        Color.White, constractAlpha * currentAlpha);

                    Vector2 numPos = position + (10.5f + i * 0.5f) * line;
                    numPos.X = rightBackLayer.GetCenter().X + 120;
                    renderer.DrawString(
                        currentInfo.CurrentState()[i].RequireAmount + "体",
                        numPos, fontSize,
                        Color.White, constractAlpha * currentAlpha);
                }
            }
        }

        public void Initialize()
        {
            #region 背景
            leftBackLayer = new Window(
                gameDevice,
                new Vector2(60, 60),
                new Vector2(WindowDef.WINDOW_WIDTH / 2 - 200, WindowDef.WINDOW_HEIGHT - 120));
            leftBackLayer.Initialize();
            leftBackLayer.Switch(true);

            rightBackLayer = new Window(
                gameDevice,
                new Vector2(WindowDef.WINDOW_WIDTH / 2 + 100, 60),
                new Vector2(WindowDef.WINDOW_WIDTH / 2 - 200, WindowDef.WINDOW_HEIGHT - 120));
            rightBackLayer.Initialize();
            rightBackLayer.Switch(true);
            #endregion

            isEnd = false;
            nextState = GuildState.SelectQuestRank;

            #region メインボタン
            currentButton = ButtonEnum.Null;
            float buttonWidth = leftBackLayer.GetWindowSize().X - 20;
            buttons = new Button[(int)ButtonEnum.Null];
            Vector2 position = leftBackLayer.GetLeftUnder() + new Vector2(10, -40);
            buttons[(int)ButtonEnum.戻る] = new Button(position, (int)buttonWidth, 30);
            position = rightBackLayer.GetLeftUnder() + new Vector2(10, -40);
            buttons[(int)ButtonEnum.受注] = new Button(position, (int)buttonWidth, 30);
            #endregion

            InitQuest();

            #region PopUI
            popUI = new DungeonPopUI(gameDevice);
            popUI.SetSize(new Vector2(450, 80));
            popUI.SetAlphaLimit(0.7f);
            popUI.SetTextOffset(Vector2.Zero);
            #endregion

            enemyName = gameManager.EnemyName;
        }

        private void InitQuest()
        {
            questIndex = -1;
            currentInfo = null;

            quests = gameManager.QuestManager.GetRandomQuest();

            #region QuestButton
            float buttonWidth = leftBackLayer.GetWindowSize().X - 20;
            questButtons = new List<Button>();
            for (int i = 0; i < quests.Count; i++)
            {
                Vector2 position = leftBackLayer.GetOffsetPosition() + new Vector2(10, 10 + (i * 40));
                questButtons.Add(
                    new Button(position, (int)buttonWidth, 30));
            }
            #endregion
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public GuildState NextState()
        {
            return nextState;
        }

        public void Update()
        {
            popUI.Update();
            if (popUI.IsPop())
            {
                if (input.IsLeftClick())
                    popUI.PopOff();
                return;
            }

            currentButton = ButtonEnum.Null;

            Point mousePos =
                new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);
            CheckMainButton(mousePos);
            CheckQuestList(mousePos);
        }

        /// <summary>
        /// Button上か、押したか
        /// </summary>
        /// <param name="mousePos">マウス位置</param>
        private void CheckMainButton(Point mousePos)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].IsClick(mousePos))
                {
                    currentButton = ((ButtonEnum)i);        //色付け用
                    if (input.IsLeftClick())
                    {
                        SwitchState(currentButton);
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// QuestListがクリックされているかをチェック
        /// </summary>
        /// <param name="mousePos"></param>
        private void CheckQuestList(Point mousePos)
        {
            questIndex = -1;
            for (int i = 0; i < questButtons.Count; i++)
            {
                if (questButtons[i].IsClick(mousePos))
                {
                    questIndex = i;
                    if (input.IsLeftClick())
                    {
                        currentInfo = quests[i];
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 次のStateを決める
        /// </summary>
        /// <param name="current">選択したボタン</param>
        private void SwitchState(ButtonEnum current)
        {
            switch (current)
            {
                case ButtonEnum.戻る:
                    nextState = GuildState.SelectQuestRank;
                    isEnd = true;
                    break;
                case ButtonEnum.受注:
                    if (currentInfo == null)
                        return;

                    AddQuest();
                    break;
                case ButtonEnum.Null:
                    break;
            }
        }

        private void AddQuest()
        {
            int current = 0, max = 0;
            gameManager.PlayerQuest.Limit(ref current, ref max);
            if(current >= max)
            {
                popUI.SetMessage("これ以上のクエストを受けることができない");
                popUI.PopUp();
                return;
            }

            if (gameManager.PlayerQuest.AddQuest(currentInfo))
            {
                questIndex = -1;
                gameManager.QuestManager.RemoveQuest(currentInfo.QuestID());
                currentInfo = null;
                Initialize();
                return;
            }

            popUI.SetMessage("すでにこのクエストを受けている");
            popUI.PopUp();
        }

        public void ShutDown()
        {
            questButtons.Clear();
            buttons = null;
        }
    }
}
