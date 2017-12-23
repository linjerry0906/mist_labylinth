//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.23
// 内容　：Logの単体Object
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.UI
{
    class LogText
    {
        private Vector2 position;   //位置
        private string logText;     //Log情報
        private Color color;        //色
        private float alpha;        //透明度

        public LogText(string logText, Color color, Vector2 position, float alpha = 1.0f)
        {
            this.logText = logText;
            this.color = color;
            this.alpha = alpha;
            this.position = position;
        }

        /// <summary>
        /// 透明度
        /// </summary>
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        /// <summary>
        /// 位置更新
        /// </summary>
        /// <param name="destPos">目標位置</param>
        public void Update(Vector2 destPos)
        {
            position = Vector2.Lerp(position, destPos, 0.05f);
        }

        /// <summary>
        /// Log情報
        /// </summary>
        public string Log
        {
            get { return logText; }
        }

        /// <summary>
        /// 色
        /// </summary>
        public Color Color
        {
            get { return color; }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }
    }
}
