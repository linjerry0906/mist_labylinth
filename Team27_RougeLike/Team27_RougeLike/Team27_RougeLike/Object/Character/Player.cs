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
        private Random rand;
        public Player(Vector3 position, PlayerStatus status, GameDevice gameDevice, CharacterManager characterManager, ParticleManager pManager, GameManager gameManager, DungeonUI ui)
            : base(new CollisionSphere(position, 5.0f), "player", characterManager, "プレイヤー", "White")
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
            rand = new Random();
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
            buff.Update();
            Move();
            textureName = status.GetInventory().Accessary() != null ? status.GetInventory().Accessary().GetAccessaryType() == AccessaryItem.Type.Pet ? "test" :  "player" : "player";
        }

        public override void SetAttackAngle()
        {
            keepAttackAngle = projector.Front;
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
            var lefthand = status.GetInventory().LeftHand();
            var accesary = status.GetInventory().Accessary();
            WeaponItem.WeaponType weapontype = lefthand == null ? WeaponItem.WeaponType.Dagger : lefthand.GetWeaponType();
            AccessaryItem.Type accsesarytype = accesary == null ? AccessaryItem.Type.NONE : accesary.GetAccessaryType();

            while (true)
            {
                var attacknum = 1;
                switch (weapontype)
                {
                    case WeaponItem.WeaponType.Bow:
                        if (status.GetInventory().IsArrowEquiped())
                        {
                            attacknum = accsesarytype == AccessaryItem.Type.Book ? 3 : 6;
                        }
                        else
                        {
                            ui.LogUI.AddLog("矢を装備していません");
                            return;
                        }
                        break;
                    case WeaponItem.WeaponType.Sword:
                        attacknum = 4;
                        break;
                    case WeaponItem.WeaponType.Shield:
                        attacknum = 1;
                        break;
                    case WeaponItem.WeaponType.Dagger:
                        attacknum = 8;
                        break;
                    default:
                        break;
                }
                if (accsesarytype == AccessaryItem.Type.Sheath) attacknum++;
                if (accsesarytype == AccessaryItem.Type.Necklace) characterManager.AreaDamage(status.GetPower() / 3);
                attack = characterManager.GetAttack(attacknum).Clone(this, pManager);
                base.Attack();
                if (accsesarytype == AccessaryItem.Type.Amulet && rand.Next(0, 5) == 0) continue;
                if (weapontype == WeaponItem.WeaponType.Bow) status.GetInventory().DecreaseArrow();
                break;
            }
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
            else
            {
                status.Damage(1);
            }
            if (!buff.GetBuff(Buff.buff.剛体化))
            {
                this.nockback = nockback;
            }
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
            if (NockBacking())
            {
                velocity = nockback;
                NockBackUpdate();
            }

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
            if (Math.Abs(velocity.X) > 0.3f || Math.Abs(velocity.Z) > 0.3f)
            {
                TexChange("_run");
            }
            else
            {
                TexChange(string.Empty);
            }
            collision.Force(velocity, status.GetVelocty() / 2); //ベースを1とするととても速いので半減
        }
        public override void TrueDamage(int num)
        {
            status.Damage(num);
        }

        public override int GetDiffence()
        {
            return status.GetDefence();
        }
        public override int GetAttack()
        {
            return status.GetPower();
        }

        public override int GetHealth()
        {
            return status.GetHP();
        }

        public override Vector3 GetAttackAngle()
        {
            return projector.Front;
        }
    }
}
