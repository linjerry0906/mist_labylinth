﻿//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.07
// 内容  ：Bossシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Character;

namespace Team27_RougeLike.Scene
{
    class BossScene : IScene
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private Renderer renderer;
        private InputState input;

        private bool endFlag;
        private SceneType nextScene;

        private CharacterManager characterManager;
        private float angle;

        Model model;

        public BossScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;

            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
        }

        public void Draw()
        {
            renderer.DrawModel("B_01", Vector3.Zero, new Vector3(50, 50, 50), Color.White);

            characterManager.Draw();

            DrawUI();
        }

        public void DrawUI()
        {
            renderer.Begin();

            renderer.DrawString("Boss Scene\n P Key:Pause\n T Key: Back to Town", Vector2.Zero, new Vector2(1, 1), new Color(1, 1, 1));
            renderer.DrawString("ぼす は ただいま　がいしゅつちゅう　ですよ-----",
                new Vector2(Def.WindowDef.WINDOW_WIDTH / 2, Def.WindowDef.WINDOW_HEIGHT / 2),
                new Color(1.0f, 0.0f, 0.0f), new Vector2(1.2f, 1.2f), 1.0f, true, true);

            renderer.DrawString("", new Vector2(0, 300), new Vector2(2.0f, 2.0f), Color.White);

            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            nextScene = SceneType.LoadTown;

            if (scene == SceneType.Pause)
                return;

            characterManager = new CharacterManager(gameDevice);
            characterManager.Initialize(new Vector3(0, 30, 0));

            #region カメラ初期化
            angle = 0;
            gameDevice.MainProjector.Initialize(characterManager.GetPlayer().Position);       //カメラを初期化
            #endregion

            model = renderer.GetModel("B_01");

        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return nextScene;
        }

        public void Shutdown()
        {
        }

        public void Update(GameTime gameTime)
        {
            RotateCamera();

            //Chara処理
            characterManager.Update(gameTime);

            Plane p = new Plane(Vector3.Up, 0);
            characterManager.GetCharacters().ForEach(c =>
            {
                Ray r = new Ray(c.Collision.Position, Vector3.Down);

                if (r.Intersects(p).HasValue)
                {
                    float length = r.Intersects(p).Value;
                    if (length <= c.Collision.Radius)
                    {
                        c.Collision.Force(Vector3.Up, c.Collision.Radius - length, false);
                        c.Collision.Ground();
                    }
                }
            });

            //Debug 村シーンへ
            if (input.GetKeyTrigger(Keys.T))
            {
                endFlag = true;
                return;
            }

            //Pause機能
            if (input.GetKeyTrigger(Keys.P))
            {
                nextScene = SceneType.Pause;
                endFlag = true;
                return;
            }
        }

        /// <summary>
        /// カメラの回転
        /// </summary>
        private void RotateCamera()
        {
            if (gameDevice.InputState.GetKeyState(Keys.Q))
            {
                angle += 1;
                angle = (angle > 360) ? angle - 360 : angle;
            }
            else if (gameDevice.InputState.GetKeyState(Keys.E))
            {
                angle -= 1;
                angle = (angle < 0) ? angle + 360 : angle;
            }
            gameDevice.MainProjector.Rotate(angle);
        }
    }
}
