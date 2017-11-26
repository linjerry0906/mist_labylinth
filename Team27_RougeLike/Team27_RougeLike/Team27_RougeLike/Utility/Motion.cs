using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.Utility
{
    class Motion
    {
        private Range range;
        private Timer timer;
        private int motionNumber;
        private Dictionary<int, Rectangle> rectangles = new Dictionary<int, Rectangle>();

        public Motion()
        {
            Initialize(new Range(0, 0), new Timer());
        }
        public Motion(Range range, Timer timer)
        {
            Initialize(range, timer);
        }
        public void Initialize(Range range, Timer timer)
        {
            this.range = range;
            this.timer = timer;
            motionNumber = range.First();
        }
        public void Add(int index, Rectangle rect)
        {
            if (rectangles.ContainsKey(index))
            {
                return;
            }
            rectangles.Add(index, rect);
        }
        public void MotionUpdate()
        {
            motionNumber++;
            if (range.IsOutOfRange(motionNumber))
            {
                motionNumber = range.First();
            }
        }
        public void Update(GameTime gameTime)
        {
            if (range.IsOutOfRange())
            {
                return;
            }
            timer.Update();
            if (timer.IsTime())
            {
                timer.Initialize();
                MotionUpdate();
            }

        }
        public Rectangle DrawingRange()
        {
            return rectangles[motionNumber];
        }
    }
}
