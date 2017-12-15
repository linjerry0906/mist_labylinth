using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.UI
{
    class Window
    {
        private Vector2 offsetPosition;
        private Vector2 windowSize;
        private float alpha;
        private bool alphaSwitch;

        private Renderer renderer;

        public Window(GameDevice gameDevice, Vector2 position, Vector2 size)
        {
            renderer = gameDevice.Renderer;

            offsetPosition = position;
            windowSize = size;

            alphaSwitch = false;
        }

        public void Initialize()
        {
            alphaSwitch = false;

            alpha = 0;
        }

        public void Update()
        {
            //フェード処理
            if (alphaSwitch)
            {
                if (alpha <= 0.5f)
                {
                    alpha += 0.025f;
                }
            }
            else
            {
                if (alpha >= 0)
                {
                    alpha -= 0.025f;
                }
            }
        }

        public void Switch()
        {
            if (alphaSwitch)
            {
                alphaSwitch = false;
            }
            else
            {
                alphaSwitch = true;
            }
        }

        public bool CurrentState()
        {
            return alphaSwitch;
        }

        public float CurrentAlpha()
        {
            return alpha;
        }

        public bool IsEnd()
        {
            return alpha <= 0;
        }

        public void Draw()
        {
            renderer.DrawTexture("fade", offsetPosition, windowSize, alpha);
        }

        //大きさ
        public Vector2 GetWindowSize()
        {
            return windowSize;
        }

        //左上
        public Vector2 GetOffsetPosition()
        {
            return offsetPosition;
        }

        //中央上
        public Vector2 GetCenterTop()
        {
            return offsetPosition + new Vector2(windowSize.X / 2, 0);
        }

        //右上
        public Vector2 GetRightTop()
        {
            return offsetPosition + new Vector2(windowSize.X, 0);
        }

        //左中央
        public Vector2 GetLeftCenter()
        {
            return offsetPosition + new Vector2(0, windowSize.Y / 2);
        }

        //中央
        public Vector2 GetCenter()
        {
            return offsetPosition + windowSize / 2;
        }

        //右中央
        public Vector2 GetRightCenter()
        {
            return offsetPosition + new Vector2(windowSize.X, windowSize.Y / 2);
        }

        //左下
        public Vector2 GetLeftUnder()
        {
            return offsetPosition + new Vector2(0, windowSize.Y);
        }

        //中央下
        public Vector2 GetCenterUnder()
        {
            return offsetPosition + new Vector2(windowSize.X / 2, windowSize.Y);
        }

        //右下
        public Vector2 GetRightUnder()
        {
            return offsetPosition + windowSize;
        }
    }
}
