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
using Team27_RougeLike.Scene;
namespace Team27_RougeLike.Object.Character
{
    class Player : CharacterBase
    {
        private GameDevice gameDevice;
        private Projector projector;
        private InputState input;
        private ParticleManager pManager;
        private GameManager gameManager;
        private PlayerStatus status;

        public Player(Vector3 position, PlayerStatus status, GameDevice gameDevice, CharacterManager characterManager, ParticleManager pManager, GameManager gameManager)
            : base(new CollisionSphere(position, 5.0f), "test", characterManager)
        {
            tag = "Player";

            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            projector = gameDevice.MainProjector;
            this.gameManager = gameManager;
            this.pManager = pManager;
            this.status = status;

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
            projector.Trace(collision.Position);
            gameDevice.Renderer.MiniMapProjector.Trace(collision.Position);
            motion.Update(gameTime);
            aiManager.Update();
            Move();
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
            HitBoxBase DBox = new MoveDamageBox(new BoundingSphere(Position + projector.Front * 10, 10), 100, tag, status.GetPower(), projector.Front);
            characterManager.AddHitBox(DBox);
            pManager.AddParticle(new Slash(gameDevice, this, DBox.Position()));
        }

        public Projector Projecter
        {
            get { return projector; }
        }
        public void Stop()
        {
            velocity = Vector3.Zero;
        }

        public override void Damage(int num, Vector3 nockback)
        {
            var damage = num - gameManager.PlayerInfo.GetDefence();
            if(damage > 0)
            {
            status.Damage(damage);
            }
            //else
            //{
            //    status.Damage(1);
            //}
            velocity += nockback;
        }

        public override bool IsDead()
        {
            return status.GetHP() <= 0;
        }

        public override void Move()
        {
            if (Math.Abs(velocity.X) < 0.01f)
            {
                velocity.X = 0;
            }
            if (Math.Abs(velocity.Z) < 0.01f)
            {
                velocity.Z = 0;
            }
            var v = velocity;
            v.Y = 0;
            velocity -= v * 0.1f;
            collision.Force(velocity, status.GetVelocty());//移動
        }
    }
}
