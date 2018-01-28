using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.Character;

namespace Team27_RougeLike.Object.ParticleSystem
{
    class Slash : Particle
    {
        private Motion motion;
        private CharacterBase actor;
        private GameDevice gameDevice;
        private Vector3 position1;

        public Slash(CharacterBase actor, Vector3 position1) : base()
        {
            this.actor = actor;
            this.position1 = position1;
            name = "slash";
            alpha = 1.0f;
            size = new Vector2(10, 10);

            motion = new Motion();
            for (int i = 0; i < 7; i++)
                motion.Add(i, new Rectangle(256 * i, 0, 256, 256));
            motion.Initialize(new Range(0, 6), new Timer(0.015f));
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            motion.Update(gameTime);
            if (motion.IsEnd())
                isDead = true;
        }

        public override void Draw(GameDevice gameDevice)
        {
            Rectangle rect = motion.DrawingRange();
            gameDevice.Renderer.DrawPolygon(name, position1, size, motion.DrawingRange(), Color.White, alpha);
        }

    }
}
