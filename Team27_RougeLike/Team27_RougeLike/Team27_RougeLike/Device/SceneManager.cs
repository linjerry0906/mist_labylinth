using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Device
{
    class SceneManager
    {
        private Dictionary<SceneType, IScene> scenes = new Dictionary<SceneType, IScene>();
        private IScene currentScene = null;
        //private GameManger gameManager;
        private Renderer renderer;

        private SceneType currenetSceneName;

        public SceneManager()
        {
            //renderer = gameManager.Renderer;
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
                currentScene.Shutdown();
            }
            //現在のシーンをlastSceneに記録
            SceneType lastScene = currenetSceneName;

            //次のシーンをcurrenetSceneNameに記録
            currenetSceneName = sceneType;
            
            currentScene = this.scenes[sceneType];

            //次のシーンを初期化
            currentScene.Initialize(lastScene);
        }

        public void Update()
        {
            //ガード節、条件次第では終了
            if (currentScene == null)
            {
                return;
            }
            //ゲームマネージャーの更新後に各シーンのUpdateを実行
            //gameManager.Update();
            currentScene.Update();

            //シーン管理からシーン終了を問い合わせる
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
            renderer.Begin();
            currentScene.Draw();
            renderer.End();
        }
    }
}
