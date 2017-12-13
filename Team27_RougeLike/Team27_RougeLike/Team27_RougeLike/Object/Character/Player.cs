using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.Box;
namespace Team27_RougeLike.Object.Character
{
    class Player : CharacterBase
    {
        private GameDevice gameDevice;
        private Projector projector;
        private InputState input;

        public Player(Vector3 position, GameDevice gameDevice,CharacterManager characterManager)
            : base(new Status(5, 100, 50, 5, 5, 5), new CollisionSphere(position,2.5f),"test",characterManager)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            projector = gameDevice.MainProjector;

            tag = "Player";

            motion = new Motion();
            for (int i = 0; i < 6; i++)
            {
                motion.Add(i, new Rectangle(i * 64, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 5), new Timer(0.1f));
        }

        public override void Update(GameTime gameTime)
        {
            Move();

            collision.Force(velocity, speed);

            projector.Trace(collision.Position);
            gameDevice.Renderer.MiniMapProjector.Trace(collision.Position);

            //デバッグ
            if (gameDevice.InputState.LeftButtonEnter(ButtonState.Pressed))
            {
                characterManager.AddHitBox(new DamageBox(new BoundingSphere(Position, 10), 1, tag));
            }

            motion.Update(gameTime);
        }

        private void Move()
        {
            speed = (speed > 0) ? speed - 0.01f : 0;
            if (input.GetKeyState(Keys.W))
            {
                speed = (speed < status.MAX_SPEED) ? speed + 0.05f : status.MAX_SPEED;
                velocity += projector.Front;
            }
            if (input.GetKeyState(Keys.S))
            {
                speed = (speed < status.MAX_SPEED) ? speed + 0.05f : status.MAX_SPEED;
                velocity += projector.Back;
            }
            if (input.GetKeyState(Keys.A))
            {
                speed = (speed < status.MAX_SPEED) ? speed + 0.05f : status.MAX_SPEED;
                velocity += projector.Left;
            }
            if (input.GetKeyState(Keys.D))
            {
                speed = (speed < status.MAX_SPEED) ? speed + 0.05f : status.MAX_SPEED;
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

        public override void Initialize()
        {
        }

        public override void Attack()
        {
        }

        public override bool HitCheck(CharacterBase character)
        {
            throw new NotImplementedException();
        }
    }
}
