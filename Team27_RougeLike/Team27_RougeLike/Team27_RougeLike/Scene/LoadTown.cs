﻿//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.13
// 内容  ：村シーンのものを読み込む
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene.Town;
using Team27_RougeLike.QuestSystem;

namespace Team27_RougeLike.Scene
{
    class LoadTown : IScene
    {
        private GameDevice gameDevice;
        private Renderer renderer;
        private GameManager gameManager;

        private QuestLoader questManager;

        private bool endFlag;

        public LoadTown(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
        }

        public void Draw()
        {
            renderer.Begin();
            Vector2 screenSize = new Vector2(Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT);
            renderer.DrawTexture("fade", Vector2.Zero, screenSize);
            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;

            questManager = gameManager.QuestManager;
            questManager.Initialize();
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return SceneType.Town;
        }

        public void ShutDown()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (!questManager.IsLoad())
            {
                questManager.Load(
                    gameManager.DungeonProcess);
                questManager.RandomQuest(
                    gameDevice, 
                    gameManager.GuildInfo);
                return;
            }

            gameManager.PlayerQuest.UpdateQuestProcess();

            endFlag = true;
        }
    }
}
