using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object.ParticleSystem
{
    class SphereParticle : Particle
    {
        private GameDevice gameDevice;
        private float speed;
        private Vector3 velocity;
        private Color color;
        private bool alphaFlag;

        public SphereParticle(Vector3 position, Color color,GameDevice gameDevice)
            :base(gameDevice)
        {
            this.position = position;
            this.gameDevice = gameDevice;
            this.color = color;

            Initialize();
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawPolygon("particle", position, size, new Rectangle(0, 0, 16, 16), color, alpha);
        }

        public override void Initialize()
        {
            isDead = false;

            alphaFlag = true;
            size = new Vector2(
                gameDevice.Random.Next(1, 7) / 10.0f);
            alpha = gameDevice.Random.Next(1, 4) / 10.0f;
            speed = gameDevice.Random.Next(7, 15) / 100.0f;
            velocity = new Vector3(
                gameDevice.Random.Next(-70, 71) / 100.0f,
                gameDevice.Random.Next(1, 101) / 100.0f,
                gameDevice.Random.Next(-70, 71) / 100.0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (alphaFlag)
            {
                alpha += 0.002f;
                if (alpha >= 0.55f)
                    alphaFlag = false;
            }
            else
            {
                alpha -= 0.002f;
                if (alpha <= 0.0f)
                    isDead = true;
            }

            position += velocity * speed;
        }
    }
}
