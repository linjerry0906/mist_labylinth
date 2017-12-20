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
using Team27_RougeLike.Object.AI;

namespace Team27_RougeLike.Object.Character
{
    class Player : CharacterBase
    {
        private GameDevice gameDevice;
        private Projector projector;
        private InputState input;

        public Player(Vector3 position, GameDevice gameDevice, CharacterManager characterManager)
            : base(new Status(5, 100, 50, 5, 5, 0.3f), new CollisionSphere(position, 2.5f), "test", characterManager)
        {
            tag = "Player";

            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            projector = gameDevice.MainProjector;
            aiManager = new AiManager_Player(gameDevice.InputState);
            aiManager.Initialize(this);

            motion = new Motion();
            for (int i = 0; i < 6; i++)
            {
                motion.Add(i, new Rectangle(i * 64, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 5), new Timer(0.1f));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            projector.Trace(collision.Position);
            gameDevice.Renderer.MiniMapProjector.Trace(collision.Position);
            motion.Update(gameTime);
            aiManager.Update();
        }

        public void Move()
        {
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
            characterManager.AddHitBox(new DamageBox(new BoundingSphere(Position, 10), 1, tag,status.BasePower));
        }

        public Projector Projecter
        {
            get { return projector; }
        }
        public void Stop()
        {
            velocity = Vector3.Zero;
        }
    }
}
