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
        private Rectangle rect;
        private MoveDamageBox damageBox;
        private GameDevice gameDevice;

        public Bullet(GameDevice gameDevice,MoveDamageBox damageBox,Vector2 size):base(gameDevice)
        {
            this.damageBox = damageBox;
            this.gameDevice = gameDevice;
            this.size = size;
            rect = new Rectangle(0, 0, 1, 1);
            name = "attack";
            position = damageBox.Position();
        }


        public override void Initialize()
        {
            isDead = false;
        }

        public override void Update(GameTime gameTime)
        {
            position = damageBox.Position();
            if (damageBox.IsEnd())
                isDead = true;
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawPolygon(name, position, size, rect,Color.White);
        }
    }
}
