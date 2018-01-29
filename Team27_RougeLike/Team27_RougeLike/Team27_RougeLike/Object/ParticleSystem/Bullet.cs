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
        
        public Bullet(MoveDamageBox damageBox,Vector2 size,string name):base()
        {
            this.damageBox = damageBox;
            this.size = size;
            rect = new Rectangle(0, 0, 1, 1);
            this.name =name;
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
        public override void Draw(GameDevice gameDevice)
        {
            gameDevice.Renderer.DrawPolygon(name, position, size, rect,Color.White);
        }
    }
}
