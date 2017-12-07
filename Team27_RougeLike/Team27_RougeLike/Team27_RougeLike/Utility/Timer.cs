using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.Utility
{
    class Timer
    {
        private float currentTime;
        private float limitTime;

        public Timer()
        {
            limitTime = 60;
        }
        public Timer(float second)
        {
            limitTime = 60 * second;
        }
        public void Initialize()
        {
            currentTime = limitTime;
        }
        public void Update()
        {
            currentTime -= 1;
            if (currentTime < 0.0f)
            {
                currentTime = 0.0f;
            }
        }
        public float Now()
        {
            return currentTime;
        }
        public bool IsTime()
        {
            return currentTime <= 0;
        }
        public void Change(float limitTime)
        {
            this.limitTime = limitTime;
            Initialize();
        }
        public float Rate()
        {
            return currentTime / limitTime;
        }
    }
}
