using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object.Actor
{
    class Character
    {
        private GameDevice gameDevice;
        private Projector projector;
        private InputState input;
        private Vector3 position;
        private Vector3 velocity;
        private Cube cube;

        public Character(Vector3 position, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.position = position;
            input = gameDevice.InputState;
            projector = gameDevice.MainProjector;
            velocity = Vector3.Zero;

            cube = new Cube(position, new Vector3(0.5f, 0.5f, 0.5f), gameDevice);
            cube.SetColor(Color.Blue);
        }

        public void Update()
        {
            Move();

            position += velocity * 0.3f;
            cube = new Cube(position, new Vector3(0.5f, 0.5f, 0.5f), gameDevice);
            cube.SetColor(Color.Blue);

            projector.Focus(position);
        }

        private void Move()
        {
            velocity = Vector3.Zero;
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
            get { return position; }
            set { position = value; }
        }

        public void Draw()
        {
            cube.Draw();
        }


    }
}
