using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.UI
{
    class DungeonLog
    {
        private Renderer renderer;

        private Window window;
        private List<LogText> logs;

        private readonly int HEIGHT = 22;

        public DungeonLog(Vector2 position, Vector2 size, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;

            window = new Window(gameDevice, position, size);
            window.Switch(false);
            window.SetAlpha(0.0f);
            logs = new List<LogText>();
        }

        public void Update()
        {
            foreach (LogText text in logs)
            {
                text.Alpha -= 0.005f;
            }

            logs.RemoveAll(text => text.Alpha <= 0);

            if (logs.Count <= 0)
                window.SetAlpha(window.CurrentAlpha() - 0.005f);

            if (window.CurrentAlpha() <= 0.0f)
                window.Switch(false);
        }

        public void AddLog(string log, Color color)
        {
            logs.Add(new LogText(log, color));
            window.Switch(true);
            window.SetAlpha(0.5f);
        }

        public void AddLog(string log)
        {
            logs.Add(new LogText(log, Color.White));
            window.Switch(true);
            window.SetAlpha(0.5f);
        }

        public void Draw()
        {
            window.Draw();
            Vector2 position = window.GetOffsetPosition();
            for (int i = 0; i < logs.Count; i++)
            {
                renderer.DrawString(
                    logs[i].Log,
                    position + new Vector2(0, i * HEIGHT),
                    new Vector2(1.0f, 1.0f),
                    logs[i].Color,
                    logs[i].Alpha);
            }
        }
    }
}
