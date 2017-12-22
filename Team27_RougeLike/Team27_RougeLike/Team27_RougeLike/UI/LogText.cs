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
        private string logText;
        private Color color;
        private float alpha;

        public LogText(string logText, Color color, float alpha = 1.0f)
        {
            this.logText = logText;
            this.color = color;
            this.alpha = alpha;
        }

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public string Log
        {
            get { return logText; }
        }

        public Color Color
        {
            get { return color; }
        }
    }
}
