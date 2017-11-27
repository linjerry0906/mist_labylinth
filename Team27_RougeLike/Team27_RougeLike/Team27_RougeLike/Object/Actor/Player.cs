using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;

namespace Team27_RougeLike.Object.Actor
{
    class Player
    {
        private GameDevice gameDevice;
        private Projector projector;
        private InputState input;

        private CollisionSphere collision;
        private Vector3 velocity;

        private Motion motion;

        public Player(Vector3 position, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            projector = gameDevice.MainProjector;

            collision = new CollisionSphere(position, 2.5f);
            velocity = Vector3.Zero;

            motion = new Motion();
            for (int i = 0; i < 6; i++)
            {
                motion.Add(i, new Rectangle(i * 64, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 5), new Timer(0.1f));
        }

        public void Update(GameTime gameTime)
        {
            Move();

            collision.Force(velocity, 0.3f);

            projector.Focus(collision.Position);

            motion.Update(gameTime);
        }

        private void Move()
        {
            velocity = Vector3.Zero;
            velocity -= new Vector3(0, 1 / 6.0f, 0);
            if (input.GetKeyState(Keys.W))
            {
                velocity += projector.Front;
            }
            if (input.GetKeyState(Keys.S))
            {
                velocity += projector.Back;
            }
            if (input.GetKeyState(Keys.A))
            {
                velocity += projector.Left;
            }
            if (input.GetKeyState(Keys.D))
            {
                velocity += projector.Right;
            }
            if (velocity.Length() > 0)
            {
                velocity.Normalize();
            }
        }

        public Vector3 Position
        {
            get { return collision.Position; }
            set { collision.Position = value; }
        }

        public void Draw()
        {
            gameDevice.Renderer.DrawPolygon("test", collision.Position, new Vector2(5, 5), motion.DrawingRange(), Color.White);
        }

        public CollisionSphere Collision
        {
            get { return collision; }
        }
    }
}
