using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Team27_RougeLike.Device
{
    class InputState
    {
        // キー
        private KeyboardState currentKey;   // 現在のキー
        private KeyboardState previousKey;  // 1フレーム前のキー
        private MouseState currentMouse;    //現在のマウス
        private MouseState previousMouse;   //1フレーム前のマウス
        private Vector2 mousePosition;      //マウスの位置
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
        private void UpdateMouse(MouseState mouseState)
        {
            // 現在登録されているキーを1フレーム前のキーに
            previousMouse = currentMouse;
            // 現在のキーを最新のキーに
            currentMouse = mouseState;
            mousePosition = new Vector2(currentMouse.X, currentMouse.Y);
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
        /// マウスが押されているか
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool LeftButtonDown(ButtonState button)
        {
            return currentMouse.LeftButton == button;            
        }

        /// <summary>
        /// マウスが押された瞬間か
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool LeftButtonEnter(ButtonState button)
        {
            return currentMouse.LeftButton == button && previousMouse.LeftButton != button;   
        }
        /// <summary>
        /// 右マウスが押されているか
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool RightButtonDown(ButtonState button)
        {
            return currentMouse.RightButton == button;
        }

        /// <summary>
        /// 右マウスが押された瞬間か
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool RightButtonEnter(ButtonState button)
        {
            return currentMouse.RightButton == button && previousMouse.RightButton != button;
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
            var mouseState = Mouse.GetState();
            // （var型は代入時に自動的に型が決定できる）

            // キーボード状態の更新
            UpdateKey(keyState);
            UpdateMouse(mouseState);
        }

        public Vector2 GetMousePosition()
        {
            return mousePosition;
        }
    }
}
