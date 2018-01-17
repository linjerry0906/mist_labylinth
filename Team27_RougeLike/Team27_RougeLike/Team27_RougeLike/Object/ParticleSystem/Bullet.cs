using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Box;

namespace Team27_RougeLike.Object.ParticleSystem
{
    class Bullet : Particle
    {
        private GameDevice gameDevice;
        private Rectangle rect;
        private MoveDamageBox damageBox;

        public Bullet(Vector2 size, GameDevice gameDevice, MoveDamageBox damageBox)
            : base(gameDevice)
        {
            isDead = false;
            this.gameDevice = gameDevice;
            this.size = size;
            this.damageBox = damageBox;
            position = damageBox.Position();
            name = "attack";
            rect = new Rectangle(0, 0, 1, 1);
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            position = damageBox.Position();
            if (damageBox.IsHit())
                isDead = true;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawPolygon(name, position, size, rect, Color.White, 1.0f);
        }

    }
}
