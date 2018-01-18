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
        public SphereParticle(GameDevice gameDevice)
            :base(gameDevice)
        {
        }

        public override void Draw(Renderer renderer)
        {
        }

        public override void Initialize()
        {
            isDead = false;

        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
