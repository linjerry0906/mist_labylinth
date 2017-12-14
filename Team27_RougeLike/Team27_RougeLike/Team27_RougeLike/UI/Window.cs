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

        public bool IsEnd()
        {
            return alpha <= 0;
        }

        public void Draw()
        {
            renderer.Begin();

            renderer.DrawTexture("black", offsetPosition, windowSize, alpha);

            renderer.End();
        }

        public Vector2 GetOffsetPosition()
        {
            return offsetPosition;
        }

        public Vector2 GetWindowSize()
        {
            return windowSize;
        }

        public Vector2 GetCenter()
        {
            return offsetPosition + windowSize / 2;
        }

        public Vector2 GetCenterTop()
        {
            return offsetPosition + new Vector2(windowSize.X / 2, 0);
        }

        public Vector2 GetRightTop()
        {
            return offsetPosition + new Vector2(windowSize.X, 0);
        }

        public Vector2 GetRightCenter()
        {
            return offsetPosition + new Vector2(windowSize.X, windowSize.Y / 2);
        }
    }
}
