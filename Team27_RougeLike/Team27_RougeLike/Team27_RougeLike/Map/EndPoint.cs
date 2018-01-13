using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    class EndPoint
    {
        private Renderer renderer;
        private Vector3 position;
        private float centerAngle;
        private float inAngle;
        private float middleAngle;
        private float outAngle;

        public EndPoint(Vector3 position, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            this.position = position;
            centerAngle = 0;
            inAngle = 0;
            middleAngle = 0;
            outAngle = 0;
        }

        public void Update()
        {
        }
    }
}
