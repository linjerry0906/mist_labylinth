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

namespace Team27_RougeLike.QuestSystem
{
    class GetReward : IGuildState
    {
        private enum ButtonEnum
        {
            戻る = 0,
            報告, 
            あきらめる,
            Null,
        }

        private GameManager gameManager;
        private GameDevice gameDevice;
        private InputState input;
        private Renderer renderer;
        private GuildState nextState;
        private bool isEnd;

        private Window leftBackLayer;           //左半分の背景レイヤー
        private Window rightBackLayer;          //右半分の背景レイヤー
        private Button[] buttons;
        private ButtonEnum currentButton;

        private List<Quest> quests;             //クエスト
        private List<Button> questButtons;      //Questのリストボタン
        private int questIndex;
        private Quest currentQuestInfo;         //現在の選択したクエスト

        public GetReward(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameManager = gameManager;
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
        }

        public void Draw(float constractAlpha, float currentalpha)
        {
            leftBackLayer.Draw(currentalpha * 1.2f);
            rightBackLayer.Draw(currentalpha * 1.2f);

            DrawMainButton(constractAlpha, currentalpha);
            DrawQuestList(constractAlpha, currentalpha);
        }

        /// <summary>
        /// ボタンを描画
        /// </summary>
        /// <param name="constractAlpha">対比透明度（UI）</param>
        /// <param name="currentalpha">現在透明度（UI）</param>
        private void DrawMainButton(float constractAlpha, float currentalpha)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (((ButtonEnum)i) == ButtonEnum.あきらめる ||
                    ((ButtonEnum)i) == ButtonEnum.報告 && 
                    currentQuestInfo == null)
                    continue;

                Color color = Color.Black;
                float adjustAlpha = 1.0f;
                if (i == (int)currentButton)    //OnButton色, 透明度
                {
                    adjustAlpha = 0.8f;
                    color = Color.Gold;
                }

                Vector2 position = new Vector2(buttons[i].Position().X, buttons[i].Position().Y);
                renderer.DrawTexture("white", position, buttons[i].Size(), currentalpha * adjustAlpha);

                renderer.DrawString(            //Button文字
                    ((ButtonEnum)i).ToString(),
                    buttons[i].ButtonCenterVector(),
                    color, new Vector2(1.1f, 1.1f), constractAlpha * currentalpha,
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

        public void Initialize()
        {
            #region 背景
            leftBackLayer = new Window(
                gameDevice,
                new Vector2(60, 60),
                new Vector2(WindowDef.WINDOW_WIDTH / 2 - 120, WindowDef.WINDOW_HEIGHT - 120));
            leftBackLayer.Initialize();
            leftBackLayer.Switch(true);

            rightBackLayer = new Window(
                gameDevice,
                new Vector2(WindowDef.WINDOW_WIDTH / 2 + 60, 60),
                new Vector2(WindowDef.WINDOW_WIDTH / 2 - 120, WindowDef.WINDOW_HEIGHT - 120));
            rightBackLayer.Initialize();
            rightBackLayer.Switch(true);
            #endregion

            isEnd = false;
            nextState = GuildState.Menu;

            #region メインボタン
            currentButton = ButtonEnum.Null;
            float buttonWidth = leftBackLayer.GetWindowSize().X - 20;
            buttons = new Button[(int)ButtonEnum.Null];
            Vector2 position = leftBackLayer.GetLeftUnder() + new Vector2(10, -40);
            buttons[(int)ButtonEnum.戻る] = new Button(position, (int)buttonWidth, 30);
            position = rightBackLayer.GetLeftUnder() + new Vector2(10, -40);
            buttons[(int)ButtonEnum.あきらめる] = new Button(position, (int)buttonWidth / 2 - 10, 30);
            position += new Vector2((int)buttonWidth / 2 + 10, 0);
            buttons[(int)ButtonEnum.報告] = new Button(position, (int)buttonWidth / 2 - 10, 30);
            #endregion

            InitQuest();
        }

        private void InitQuest()
        {
            currentQuestInfo = null;
            questIndex = -1;
            quests = gameManager.PlayerQuest.CurrentQuest();

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
            currentButton = ButtonEnum.Null;

            Vector2 mousePos = input.GetMousePosition();
            CheckMainButton(new Point((int)mousePos.X, (int)mousePos.Y));
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
        /// 次のStateを決める
        /// </summary>
        /// <param name="current">選択したボタン</param>
        private void SwitchState(ButtonEnum current)
        {
            switch (current)
            {
                case ButtonEnum.戻る:
                    nextState = GuildState.Menu;
                    isEnd = true;
                    break;
                case ButtonEnum.報告:
                    if (currentQuestInfo == null)
                        return;

                    if (!currentQuestInfo.IsClear())
                        return;

                    break;
                case ButtonEnum.あきらめる:
                    if (currentQuestInfo == null)
                        return;

                    break;
                case ButtonEnum.Null:
                    break;
            }
        }

        public void ShutDown()
        {
            buttons = null;
        }
    }
}
