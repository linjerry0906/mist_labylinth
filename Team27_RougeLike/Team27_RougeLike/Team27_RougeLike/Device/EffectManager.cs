using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Team27_RougeLike.Device
{
    public enum BasicEffectType
    {
        Basic,
        MiniMap,
    }

    class EffectManager
    {
        private GraphicsDevice graphicsDevice;  // グラフィック機器

        private BasicEffect currentEffect;
        private BasicEffect basicEffect;
        private BasicEffect miniMapEffect;

        public EffectManager(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            basicEffect = new BasicEffect(graphicsDevice);
            miniMapEffect = new BasicEffect(graphicsDevice);

            basicEffect.VertexColorEnabled = true;
            miniMapEffect.VertexColorEnabled = true;

            currentEffect = basicEffect;
        }

        public BasicEffect CurrentEffect
        {
            get { return currentEffect; }
        }

        public BasicEffect GetCurrentEffect()
        {
            return currentEffect;
        }

        public void ChangeEffect(BasicEffectType type)
        {
            switch (type)
            {
                case BasicEffectType.Basic:
                    currentEffect = basicEffect;
                    break;
                case BasicEffectType.MiniMap:
                    currentEffect = miniMapEffect;
                    break;
            }
        }
    }
}
