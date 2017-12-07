using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;

namespace Team27_RougeLike.Object.Character
{
    class Player : CharacterBase
    {
        private GameDevice gameDevice;
        private Projector projector;
        private InputState input;

        private CollisionSphere collision;
        private Vector3 velocity;
        private readonly float MAX_SPEED = 0.3f;
        private float speed;

        private Motion motion;

        public Player(Vector3 position, GameDevice gameDevice)
            : base(new Status(5, 100, 50, 5, 5, 5), new Transform(position))
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            projector = gameDevice.MainProjector;

            tag = "Player";
            collision = new CollisionSphere(transform.position, 2.5f);

            velocity = Vector3.Zero;
            speed = 0;

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

            collision.Force(velocity, speed);
            collision.Force(-Vector3.UnitY, 1 / 6.0f);

            projector.Trace(collision.Position);
            gameDevice.Renderer.MiniMapProjector.Trace(collision.Position);

            motion.Update(gameTime);
        }

        private void Move()
        {
            speed = (speed > 0) ? speed - 0.01f : 0;
            if (input.GetKeyState(Keys.W))
            {
                speed = (speed < MAX_SPEED) ? speed + 0.05f : MAX_SPEED;
                velocity += projector.Front;
            }
            if (input.GetKeyState(Keys.S))
            {
                speed = (speed < MAX_SPEED) ? speed + 0.05f : MAX_SPEED;
                velocity += projector.Back;
            }
            if (input.GetKeyState(Keys.A))
            {
                speed = (speed < MAX_SPEED) ? speed + 0.05f : MAX_SPEED;
                velocity += projector.Left;
            }
            if (input.GetKeyState(Keys.D))
            {
                speed = (speed < MAX_SPEED) ? speed + 0.05f : MAX_SPEED;
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
        }

        public void Draw()
        {
            gameDevice.Renderer.DrawPolygon("test", collision.Position, new Vector2(5, 5), motion.DrawingRange(), Color.White);
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
        }

        public override void Attack()
        {
        }

        public override bool HitCheck(CharacterBase character)
        {
            throw new NotImplementedException();
        }

        public CollisionSphere Collision
        {
            get { return collision; }
        }
    }
}
