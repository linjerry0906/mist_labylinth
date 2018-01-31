using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;

namespace Team27_RougeLike.Scene
{   
    class SceneFader : IScene
    {
        private Renderer renderer;
        private Timer timer;
        private static float FADE_TIMER = 1.5f;
        private SceneFadeState startState;
        private SceneFadeState state;
        private IScene scene;
        private bool isEnd = false;

        public SceneFader(IScene scene, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            this.scene = scene;
            startState = SceneFadeState.In;
        }
        public SceneFader(IScene scene, SceneFadeState state, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            this.scene = scene;
            startState = state;
        }

        /// <summary>
        /// どのシーンから来た場合はFadin処理しないかを定義する
        /// </summary>
        /// <returns></returns>
        private bool NoFadeIn(SceneType lastScene)
        {
            if (lastScene == SceneType.Pause ||
                lastScene == SceneType.LoadShop ||
                lastScene == SceneType.ItemShop ||
                lastScene == SceneType.Depot ||
                lastScene == SceneType.Town ||
                lastScene == SceneType.DungeonSelect||
                lastScene == SceneType.UpgradeStore||
                lastScene == SceneType.Quest)
                return true;

            return false;
        }

        /// <summary>
        /// 次のシーンが該当すればFadeOut処理しない
        /// </summary>
        /// <param name="nextScene"></param>
        /// <returns></returns>
        private bool NoFadeOut(SceneType nextScene)
        {
            if (nextScene == SceneType.Pause ||
                nextScene == SceneType.LoadShop ||
                nextScene == SceneType.ItemShop ||
                nextScene == SceneType.Depot ||
                nextScene == SceneType.Town ||
                nextScene == SceneType.DungeonSelect||
                nextScene == SceneType.UpgradeStore||
                nextScene == SceneType.Quest)
                return true;

            return false;
        }

        public void Draw()
        {
            switch (state)
            {
                case SceneFadeState.In:
                    DrawFadeIn();
                    break;
                case SceneFadeState.Out:
                    DrawFadeOut();
                    break;
                case SceneFadeState.None:
                    DrawFadeNone();
                    break;
            }
        }
        private void DrawFadeIn()
        {
            scene.Draw();
            DrawEffect(timer.Rate());
        }
        private void DrawFadeOut()
        {
            scene.Draw();
            DrawEffect(1.0f - timer.Rate());
        }
        private void DrawFadeNone()
        {
            scene.Draw();
        }
        

        private void DrawEffect(float alpha)
        {
            renderer.Begin();
            renderer.DrawTexture(
                "fade", 
                Vector2.Zero, 
                new Vector2(Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT), 
                alpha);
            renderer.End();
        }

        public void Initialize(SceneType lastScene)
        {
            isEnd = false;
            scene.Initialize(lastScene);


            state = startState;
            timer = new Timer(FADE_TIMER);
            timer.Initialize();

            //以下のシーンから来た場合FadeIn処理しない
            if (NoFadeIn(lastScene))
            {
                state = SceneFadeState.None;
            }
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public SceneType Next()
        {
            return scene.Next();
        }

        public void ShutDown()
        {
            scene.ShutDown();
        }

        public void Update(GameTime gameTime)
        {
            switch (state)
            {
                case SceneFadeState.In:
                    UpdateFadeIn(gameTime);
                    break;
                case SceneFadeState.Out:
                    UpdateFadeOut(gameTime);
                    break;
                case SceneFadeState.None:
                    UpdateFadeNone(gameTime);
                    break;
            }
        }
        private void UpdateFadeIn(GameTime gameTime)
        {
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                state = SceneFadeState.Out;

                if (NoFadeOut(scene.Next()))
                    state = SceneFadeState.None;
            }

            timer.Update();
            if (timer.IsTime())
            {
                state = SceneFadeState.None;
            }
        }
        private void UpdateFadeOut(GameTime gameTime)
        {
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                state = SceneFadeState.Out;
            }

            timer.Update();
            if (timer.IsTime())
            {
                isEnd = true;
            }
        }
        private void UpdateFadeNone(GameTime gameTime)
        {
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                state = SceneFadeState.Out;
                timer.Initialize();

                if (NoFadeOut(scene.Next()))
                {
                    state = SceneFadeState.None;
                    isEnd = true;
                }
            }
        }
    }
}
