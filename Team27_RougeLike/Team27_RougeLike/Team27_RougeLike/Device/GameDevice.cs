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
        private InputState  input;              //インプット
        private Renderer    renderer;           //レンダラー
        private Random      random;             //ランダム
        private Sound       sound;              //サウンド
        private Projector   mainProjector;      //メインプロジェクター

        /// <summary>
        /// インプット
        /// </summary>
        public InputState InputState
        {
            get { return input; }
        }

        /// <summary>
        /// レンダラー
        /// </summary>
        public Renderer Renderer
        {
            get { return renderer; }
        }

        /// <summary>
        /// ランダム
        /// </summary>
        public Random Random
        {
            get { return random; }
        }

        /// <summary>
        /// サウンド
        /// </summary>
        public Sound Sound
        {
            get { return sound; }
        }

        /// <summary>
        /// メインのプロジェクター
        /// </summary>
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
