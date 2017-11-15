using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Team27_RougeLike.Device
{
    class InputState
    {
        // キー
        private KeyboardState currentKey;   // 現在のキー
        private KeyboardState previousKey;  // 1フレーム前のキー

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputState()
        {
        }

        /// <summary>
        /// キー情報の更新
        /// </summary>
        /// <param name="keyState">キーボード状態</param>
        private void UpdateKey(KeyboardState keyState)
        {
            // 現在登録されているキーを1フレーム前のキーに
            previousKey = currentKey;
            // 現在のキーを最新のキーに
            currentKey = keyState;
        }

        /// <summary>
        /// キーが押された瞬間か？
        /// </summary>
        /// <param name="key">調べたいキー</param>
        /// <returns>キーが押されていて、
        /// 1フレーム前におされていなければtrue</returns>
        public bool IsKeyDown(Keys key)
        {
            // 現在チェックしたいキーが押されているか
            bool current = currentKey.IsKeyDown(key);
            // 1フレーム前に押されていたか
            bool previous = previousKey.IsKeyDown(key);

            // 現在おされていて、1フレーム前に押されていなければtrue
            return current && !previous;
        }

        /// <summary>
        /// キー入力のトリガー判定
        /// </summary>
        /// <param name="key"></param>
        /// <returns>1フレーム前に押されていたらfalse</returns>
        public bool GetKeyTrigger(Keys key)
        {
            return IsKeyDown(key);
        }

        /// <summary>
        /// キーが押されているか離されているかの判定
        /// </summary>
        /// <param name="key">調べるキー</param>
        /// <returns>押されていたらtrue</returns>
        public bool GetKeyState(Keys key)
        {
            return currentKey.IsKeyDown(key);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 現在のキーボードの状態を取得
            var keyState = Keyboard.GetState();
            // （var型は代入時に自動的に型が決定できる）

            // キーボード状態の更新
            UpdateKey(keyState);
        }
    }
}
