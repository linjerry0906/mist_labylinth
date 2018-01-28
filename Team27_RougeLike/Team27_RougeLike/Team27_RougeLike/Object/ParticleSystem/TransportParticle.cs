//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.28
// 内容　：出口の粒子
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object.ParticleSystem
{
    class TransportParticle : Particle
    {
        private GameDevice gameDevice;
        private float speed;
        private Color color;
        private bool alphaFlag;

        public TransportParticle(Vector3 position, Color color, GameDevice gameDevice)
        {
            this.position = position;
            this.gameDevice = gameDevice;
            this.color = color;

            Initialize();
        }

        public override void Draw(GameDevice gameDevice)
        {
            gameDevice.Renderer.DrawPolygon("particle", position, size, new Rectangle(0, 0, 16, 16), color, alpha, false);
        }

        public override void Initialize()
        {
            isDead = false;

            alphaFlag = true;
            size = new Vector2(
                gameDevice.Random.Next(1, 8) / 10.0f);
            size.X *= 0.8f;
            size.Y *= 1.2f;
            alpha = gameDevice.Random.Next(1, 4) / 10.0f;
            speed = gameDevice.Random.Next(12, 32) / 100.0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (alphaFlag)
            {
                alpha += 0.007f;
                if (alpha >= 1.0f)
                    alphaFlag = false;
            }
            else
            {
                alpha -= 0.007f;
                if (alpha <= 0.2f)
                    isDead = true;
            }

            position.Y += speed;
        }
    }
}
