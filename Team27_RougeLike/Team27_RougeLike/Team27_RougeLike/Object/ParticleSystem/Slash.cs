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
        private Player player;
        private GameDevice gameDevice;
        private Vector3 position1;

        public Slash(GameDevice gameDevice, Player player, Vector3 position1) : base(gameDevice)
        {
            this.player = player;
            this.gameDevice = gameDevice;
            this.position1 = position1;
            name = "slash";
            alpha = 1.0f;
            size = new Vector2(20, 20);

            motion = new Motion();
            for (int i = 0; i < 4; i++)
                motion.Add(i, new Rectangle(64 * i, 0, 64, 64));
            motion.Initialize(new Range(0, 3), new Timer(0.04f));
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

        public override void Draw(Renderer renderer)
        {
            Rectangle rect = motion.DrawingRange();
            renderer.DrawPolygon(name, position1 + gameDevice.MainProjector.Front * 10, size, motion.DrawingRange(), Color.Red, alpha);
        }

    }
}
