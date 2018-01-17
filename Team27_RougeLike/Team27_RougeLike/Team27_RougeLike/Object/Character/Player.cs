using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;
using Team27_RougeLike.Object.Box;
using Team27_RougeLike.Object.AI;
using Team27_RougeLike.Object.ParticleSystem;
using Team27_RougeLike.Scene;
using Team27_RougeLike.UI;
using Team27_RougeLike.Object.Item;

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
        private DungeonUI ui;
        public Player(Vector3 position, PlayerStatus status, GameDevice gameDevice, CharacterManager characterManager, ParticleManager pManager, GameManager gameManager, DungeonUI ui)
            : base(new CollisionSphere(position, 5.0f), "test", characterManager)
        {
            tag = "Player";

            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            projector = gameDevice.MainProjector;
            this.gameManager = gameManager;
            this.pManager = pManager;
            this.status = status;
            this.ui = ui;
            aiManager = new AiManager_Player(gameDevice.InputState, status);
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

        public Vector3 GetPosition
        {
            get { return collision.Position; }
        }
        public override void Initialize()
        {
        }
        public override void Attack()
        {
            HitBoxBase DBox;
            var t = status.GetInventory().LeftHand();

            if (t == null)
            {
                DBox = new DamageBox(new BoundingSphere(GetPosition + projector.Front * 10, 10), 100, tag, status.GetPower(), projector.Front);
            }
            else
            {
                switch (t.GetWeaponType())
                {
                    case WeaponItem.WeaponType.Bow:
                        DBox = new MoveDamageBox(new BoundingSphere(GetPosition + projector.Front * 3, 0.5f), 100, tag, status.GetPower(), projector.Front);
                        ui.LogUI.AddLog("弓による攻撃");
                        break;
                    case WeaponItem.WeaponType.Sword:
                        DBox = new DamageBox(new BoundingSphere(GetPosition + projector.Front * 2, 1), 100, tag, status.GetPower(), projector.Front);
                        ui.LogUI.AddLog("剣での攻撃");
                        break;
                    case WeaponItem.WeaponType.Shield:
                        DBox = new DamageBox(new BoundingSphere(GetPosition + projector.Front, 2), 100, tag, status.GetPower(), projector.Front);
                        ui.LogUI.AddLog("盾での攻撃");
                        break;
                    case WeaponItem.WeaponType.Dagger:
                        DBox = new DamageBox(new BoundingSphere(GetPosition + projector.Front, 3), 100, tag, status.GetPower(), projector.Front);
                        ui.LogUI.AddLog("短剣での攻撃");
                        break;
                    default:
                        DBox = new DamageBox(new BoundingSphere(GetPosition + projector.Front * 10, 10), 100, tag, status.GetPower(), projector.Front);
                        break;
                }
            }
            
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
            if (damage > 0)
            {
                status.Damage(damage);
            }
            velocity += nockback;
        }

        public override bool IsDead()
        {
            return status.GetHP() <= 0;
        }
        public PlayerStatus GetPlayerStatus()
        {
            return status;
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
            collision.Force(velocity, status.GetVelocty() / 2); //ベースを1とするととても速いので半減
        }
    }
}
