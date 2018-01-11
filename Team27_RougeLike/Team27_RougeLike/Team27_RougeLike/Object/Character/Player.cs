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
using Team27_RougeLike.Object.ParticleSystem;

namespace Team27_RougeLike.Object.Character
{
    class Player : CharacterBase
    {
        private GameDevice gameDevice;
        private Projector projector;
        private InputState input;
        private ParticleManager pManager;

        public Player(Vector3 position, Status status, GameDevice gameDevice, CharacterManager characterManager, ParticleManager pManager)
            : base(status, new CollisionSphere(position, 5.0f), "test", characterManager)
        {
            tag = "Player";

            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            projector = gameDevice.MainProjector;
            aiManager = new AiManager_Player(gameDevice.InputState);
            aiManager.Initialize(this);
            this.pManager = pManager;
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

        public Vector3 Position
        {
            get { return collision.Position; }
        }

        public override void Initialize()
        {
        }

        public override void Attack()
        {
            HitBoxBase DBox= new MoveDamageBox(new BoundingSphere(Position + projector.Front*10, 10), 100,tag,status.BasePower,projector.Front);
            characterManager.AddHitBox(DBox);
            pManager.AddParticle(new Slash(gameDevice,this,DBox.Position()));
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
