﻿//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.17
// 内容  ：ランクごとにクエストを整理する
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
    class SelectQuestRank : IGuildState
    {
        private enum ButtonEnum
        {
            Fランク = 0,
            Eランク,
            Dランク,
            Cランク,
            Bランク,
            Aランク,
            Sランク,
            戻る,
            Null,
        }

        private GameManager gameManager;
        private GameDevice gameDevice;
        private InputState input;
        private Renderer renderer;
        private GuildState nextState;
        private bool isEnd;

        private Window backLayer;
        private Button[] buttons;
        private ButtonEnum currentButton;

        public SelectQuestRank(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameManager = gameManager;
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
        }

        public void Draw(float constractAlpha, float currentalpha)
        {
            backLayer.Draw(currentalpha * 1.2f);

            DrawButton(constractAlpha, currentalpha);
            DrawGuildRank(constractAlpha, currentalpha);
        }

        /// <summary>
        /// ボタンを描画
        /// </summary>
        /// <param name="constractAlpha">対比透明度（UI）</param>
        /// <param name="currentalpha">現在透明度（UI）</param>
        private void DrawButton(float constractAlpha, float currentalpha)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
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

        private void DrawGuildRank(float constractAlpha, float currentalpha)
        {
            QuestLoader.QuestRank rank = gameManager.GuildInfo.Rank();
            Vector2 rankPosition =
                backLayer.GetRightCenter() +
                new Vector2(Def.WindowDef.WINDOW_WIDTH / 2 - 380, -220);

            renderer.DrawTexture(
                "guild_rank", rankPosition,
                new Rectangle((int)rank * 256, 0, 256, 256),
                constractAlpha * currentalpha);

            Vector2 gagePos = rankPosition + new Vector2(-128, 300);
            float expRate = gameManager.GuildInfo.Rate();
            renderer.DrawTexture(
                "guild_gage_back", gagePos,
                constractAlpha * currentalpha);
            renderer.DrawTexture(
                "guild_gage_middle", gagePos,
                new Rectangle(0, 0, (int)(512 * expRate), 64),
                constractAlpha * currentalpha);
            renderer.DrawTexture(
                "guild_gage_front", gagePos,
                constractAlpha * currentalpha);
        }

        public void Initialize()
        {
            backLayer = new Window(
                gameDevice,
                new Vector2(60, 60),
                new Vector2(WindowDef.WINDOW_WIDTH / 2 - 200, WindowDef.WINDOW_HEIGHT - 120));
            backLayer.Initialize();
            backLayer.Switch(true);

            isEnd = false;
            nextState = GuildState.Menu;

            currentButton = ButtonEnum.Null;
            float buttonWidth = backLayer.GetWindowSize().X - 20;
            buttons = new Button[(int)ButtonEnum.Null];
            for (int i = 0; i < buttons.Length; i++)
            {
                Vector2 position = backLayer.GetOffsetPosition() + new Vector2(10, 10 + (i * 40));
                buttons[i] = new Button(position, (int)buttonWidth, 30);
            }
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
            CheckButton(new Point((int)mousePos.X, (int)mousePos.Y));
        }

        /// <summary>
        /// Button上か、押したか
        /// </summary>
        /// <param name="mousePos">マウス位置</param>
        private void CheckButton(Point mousePos)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].IsClick(mousePos))
                {
                    currentButton = ((ButtonEnum)i);        //色付け用
                    if (input.IsLeftClick())
                    {
                        SwitchState(currentButton);
                        isEnd = true;
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
                case ButtonEnum.Fランク:
                    nextState = GuildState.GetQuest;
                    gameManager.QuestManager.ChangeCurrentRank(QuestLoader.QuestRank.F);
                    break;
                case ButtonEnum.Eランク:
                    nextState = GuildState.GetQuest;
                    gameManager.QuestManager.ChangeCurrentRank(QuestLoader.QuestRank.E);
                    break;
                case ButtonEnum.Dランク:
                    nextState = GuildState.GetQuest;
                    gameManager.QuestManager.ChangeCurrentRank(QuestLoader.QuestRank.D);
                    break;
                case ButtonEnum.Cランク:
                    nextState = GuildState.GetQuest;
                    gameManager.QuestManager.ChangeCurrentRank(QuestLoader.QuestRank.C);
                    break;
                case ButtonEnum.Bランク:
                    nextState = GuildState.GetQuest;
                    gameManager.QuestManager.ChangeCurrentRank(QuestLoader.QuestRank.B);
                    break;
                case ButtonEnum.Aランク:
                    nextState = GuildState.GetQuest;
                    gameManager.QuestManager.ChangeCurrentRank(QuestLoader.QuestRank.A);
                    break;
                case ButtonEnum.Sランク:
                    nextState = GuildState.GetQuest;
                    gameManager.QuestManager.ChangeCurrentRank(QuestLoader.QuestRank.S);
                    break;
                case ButtonEnum.戻る:
                    nextState = GuildState.Menu;
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
