using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Team27_RougeLike.Device
{
    class GameDevice
    {
        private InputState  input;
        private Renderer    renderer;
        private Random      random;
        private Sound       sound;
        private Projector   mainProjector;

        public InputState InputState
        {
            get { return input; }
        }
        public Renderer Renderer
        {
            get { return renderer; }
        }
        public Random Random
        {
            get { return random; }
        }
        public Sound Sound
        {
            get { return sound; }
        }
        public Projector MainProjector
        {
            get { return mainProjector; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">コンテンツ管理</param>
        /// <param name="graphics">グラフィックデバイス</param>
        public GameDevice(ContentManager content, GraphicsDevice graphics)
        {
            // フィールド生成
            input = new InputState();
            renderer = new Renderer(content, graphics);
            mainProjector = renderer.MainProjector;
            sound = new Sound(content);
            random = new Random();
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        public void LoadContent()
        {
        }

        /// <summary>
        /// 開放処理
        /// </summary>
        public void UnloadContent()
        {
            renderer.Unload();
            sound.Unload();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 入力状態更新
            input.Update();
        }
    }
}
