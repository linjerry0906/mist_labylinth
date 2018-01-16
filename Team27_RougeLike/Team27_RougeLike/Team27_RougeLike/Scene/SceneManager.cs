using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class SceneManager
    {
        private Dictionary<SceneType, IScene> scenes = new Dictionary<SceneType, IScene>();
        private IScene currentScene = null;
        private GameDevice gameDevice;

        private SceneType currenetSceneName;

        public SceneManager(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
        }

        public void AddScene(SceneType sceneType, IScene scene)
        {
            scenes[sceneType] = scene;
        }

        public void Change(SceneType sceneType)
        {
            //現在のシーンを終了
            if (currentScene != null)
            {
                currentScene.ShutDown();
            }
            //現在のシーンをlastSceneに記録
            SceneType lastScene = currenetSceneName;

            //次のシーンをcurrenetSceneNameに記録
            currenetSceneName = sceneType;
            
            currentScene = this.scenes[sceneType];

            //次のシーンを初期化
            currentScene.Initialize(lastScene);
        }

        public void Update(GameTime gameTime)
        {
            //ガード節、条件次第では終了
            if (currentScene == null)
            {
                return;
            }

            currentScene.Update(gameTime);

            //シーン管理からシーン終了を問い合わせる
            if(currentScene.IsEnd())
            {
                Change(currentScene.Next());
            }
        }

        public void Draw()
        {
            if (currentScene == null)
            {
                return;
            }

            //各シーンのDraw実行、前後に
            currentScene.Draw();
        }
    }
}
