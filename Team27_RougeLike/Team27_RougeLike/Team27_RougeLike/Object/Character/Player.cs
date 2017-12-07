
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Team27_RougeLike.Device;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Team27_RougeLike.Object

{
    class Player : CharacterBase
    {
        private int luck;       //幸運値
        private int weight;     //重さ
        private int stamina;    //持久力

        private InputState input;

        public Player(Model model,GameDevice gameDevice)
            : base(model, new Status(5, 100, 50, 5, 5, 5), new Transform(Vector3.Zero,5))
        {
            input = gameDevice.InputState;
            tag = "Player";
        }
        public override void Initialize()
        {

        }
        public override void Update()
        {
            Move();
        }

        private void Move()
        {
            //if (input.GetKeyState(Keys.Q))
            //{
            //    transform.angle--;
            //}
            //if (input.GetKeyState(Keys.P))
            //{
            //    transform.angle++;
            //}
            if (input.GetKeyState(Keys.A))
            {
                transform.position = new Vector3(transform.position.X - 1, transform.position.Y, transform.position.Z);
            }
            if (input.GetKeyState(Keys.W))
            {
                transform.position = new Vector3(transform.position.X, transform.position.Y, transform.position.Z - 1);
            }
            if (input.GetKeyState(Keys.S))
            {
                transform.position = new Vector3(transform.position.X, transform.position.Y, transform.position.Z + 1);
            }
            if (input.GetKeyState(Keys.D))
            {
                transform.position = new Vector3(transform.position.X + 1, transform.position.Y, transform.position.Z);
            }
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
