//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.20
// 内容  ：左下に表示するHintUI
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.UI
{
    class DungeonHintUI
    {
        private Renderer renderer;
        private InputState input;
        private Vector2 position;       //描画基準点
        private string message;         //描画文字
        private bool isOn;              //表示するか
        private float alpha;            //透明度

        public DungeonHintUI(GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;

            message = "";

            position = new Vector2(15, Def.WindowDef.WINDOW_HEIGHT - 15);
            alpha = 0.0f;
        }

        /// <summary>
        /// 透明度の更新処理
        /// </summary>
        public void Update()
        {
            if (isOn)
            {
                alpha = alpha < 1.0f ? alpha + 0.05f : alpha;
            }
            else
            {
                alpha = alpha > 0.0f ? alpha - 0.05f : alpha;
            }
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        public void Draw()
        {
            if (alpha <= 0.0f)
                return;

            renderer.DrawString(
                message, 
                position, 
                new Color(1.0f, 1.0f, 1.0f), 
                new Vector2(1.2f, 1.2f),        //文字の大きさ
                alpha, false, true);
        }

        /// <summary>
        /// 描画するHintの文字を設定
        /// </summary>
        /// <param name="text">ヒント文字</param>
        public void SetMessage(string text)
        {
            message = text;
        }

        /// <summary>
        /// 表示するか
        /// </summary>
        /// <param name="onoff"></param>
        public void Switch(bool onoff)
        {
            isOn = onoff;
        }

        /// <summary>
        /// 特定のKey押されているか
        /// </summary>
        /// <param name="key">指定のKey</param>
        /// <returns></returns>
        public bool IsPush(Keys key)
        {
            return input.GetKeyTrigger(key);
        }
    }
}
