using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.AI;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Utility;
using Team27_RougeLike.Object.Box;
using Team27_RougeLike.Object.ParticleSystem;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object
{
    class EnemyBase : CharacterBase
    {
        protected EnemyRange range;     //各種距離構造体
        protected Status status;        //ステータスクラス 
        protected string aiName;        //キャラクタのAi名
        protected int id;
        protected int exp;              //討伐時の経験値
        private ParticleManager pManager;
        private GameDevice gameDevice;
        protected bool infinity;

        /// <summary>
        /// オリジナル
        /// </summary>
        /// <param name="status"></param>
        /// <param name="collision"></param>
        /// <param name="aiName"></param>
        /// <param name="textureName"></param>
        /// <param name="characterManager"></param>
        public EnemyBase(Status status, CollisionSphere collision, string aiName, string textureName, CharacterManager characterManager, int exp, GameDevice gameDevice, string name, int id, string color)
            : base(collision, textureName, characterManager, name, color)
        {
            this.status = status;
            this.aiName = aiName;
            this.name = name;
            this.exp = exp;
            this.gameDevice = gameDevice;
            this.id = id;
        }


        /// <summary>
        /// 非戦闘　読み込み専用EnemyBase
        /// </summary>
        /// <param name="status"></param>
        /// <param name="collision"></param>
        /// <param name="aiName"></param>
        /// <param name="textureName"></param>
        /// <param name="characterManager"></param>
        /// <param name="exp"></param>
        public EnemyBase(Status status, CollisionSphere collision, string aiName, string textureName, CharacterManager characterManager, int exp, string name, int id, string color)
        : base(collision, textureName, characterManager, name, color)
        {
            this.status = status;
            this.name = name;
            this.aiName = aiName;
            this.exp = exp;
            this.id = id;
        }


        /// <summary>
        /// クローン
        /// </summary>
        /// <param name="status"></param>
        /// <param name="collision"></param>
        /// <param name="manager"></param>
        /// <param name="textureName"></param>
        /// <param name="characterManager"></param>
        public EnemyBase(Status status, CollisionSphere collision, BaseAiManager manager, string textureName, CharacterManager characterManager, int exp, GameDevice gameDevice, string name, int id, string color)
         : base(collision, textureName, characterManager, name, color)
        {
            tag = "Enemy";
            this.status = status;
            this.exp = exp;
            this.name = name;
            this.gameDevice = gameDevice;
            this.id = id;
            aiManager = manager;
            motion = new Motion();
            pManager = new ParticleManager(gameDevice);
            for (int i = 0; i < 6; i++)
            {
                motion.Add(i, new Rectangle(i * 64, 0, 64, 64));
            }

            motion.Initialize(new Range(0, 5), new Timer(0.1f));
            aiManager.Initialize(this);
            InitRange();
            Initialize();
        }



        public override void Initialize()
        {
            switch (aiManager.ToString())
            {
                case "Team27_RougeLike.Object.AI.AiManager_Fool":
                    attack = new MeleeAttack(characterManager, this, pManager);
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Melee":
                    attack = new MeleeAttack(characterManager, this, pManager);
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Totem":
                    attack = new RangeAttack(characterManager, this, pManager);
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Ranged":
                    attack = new RangeAttack(characterManager, this, pManager);
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_AllRangedBoss":
                    attack = new RangeAttack(characterManager, this, pManager);
                    break;
                default:
                    break;
            }
        }
        public override void Update(GameTime gameTime)
        {
            aiManager.Update();
            motion.Update(gameTime);
            pManager.Update(gameTime);
            Move();
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            DrawWarning(renderer);
            pManager.Draw();
        }


        public void DrawWarning(Renderer renderer)
        {
            if (aiManager.GetAttackAi() is AttackAi_Charge)
            {
                if (aiManager is AiManager_Ranged || aiManager is AiManager_Totem || aiManager is AiManager_AllRangedBoss)
                {
                    renderer.DrawPolygon("warning", collision.Position + GetAttackAngle() * 5, new Vector2(collision.Radius), motion.DrawingRange(), Color.White);
                }
                else
                {
                    renderer.DrawPolygon("warning", collision.Position + keepAttackAngle * 5, new Vector2(collision.Radius), motion.DrawingRange(), Color.White);
                }
            }
        }

        public virtual void NearUpdate(Player player, GameTime gameTime)
        {
            ((IEnemyAI)aiManager).NearUpdate(player);
        }
        public int Distance(Player player) { return (int)Vector2.Distance(new Vector2(player.Collision.Position.X, player.Collision.Position.Z), new Vector2(collision.Position.X, collision.Position.Z)); }
        public bool SearchCheck(Player player) { return Distance(player) < range.searchRange; }
        public bool AttackCheck(Player player)
        {
            if (aiManager is AiManager_Melee || aiManager is AiManager_Fool)
            {
                return Distance(player) < collision.Radius*2;
            }
            else
            {
                return Distance(player) < range.attackRange;
            }
        }
        public bool WaitPointCheck(Player player) { return Distance(player) < range.waitRange; }
        public override void SetAttackAngle()
        {
            keepAttackAngle = Angle.CheckAngleVector(characterManager.GetPlayer().GetPosition, collision.Position);
        }
        public override Vector3 GetAttackAngle()
        {
            return Angle.CheckAngleVector(characterManager.GetPlayer().GetPosition, collision.Position);
        }
        public EnemyBase Clone(Vector3 position)
        {
            return new EnemyBase
                (
                new Status(status.Level, status.Health, status.BasePower, status.BaseArmor, status.Attackspd, status.Speed),
                new CollisionSphere(position, collision.Radius),
                SwitchAi(),
                textureName,
                characterManager,
                exp,
                gameDevice,
                name,
                id,
                color
                );
        }
        private BaseAiManager SwitchAi()
        {
            switch (aiName)
            {
                case "Fool":
                    return new AiManager_Fool();
                case "Ranged":
                    return new AiManager_Ranged();
                case "Melee":
                    return new AiManager_Melee();
                case "Totem":
                    return new AiManager_Totem();
                case "Escape":
                    return new AiManager_Escape();
                case "AllRangedBoss":
                    return new AiManager_AllRangedBoss();
                default:
                    return new AiManager_Fool();
            }
        }
        private void InitRange()
        {
            switch (aiManager.ToString())
            {
                case "Team27_RougeLike.Object.AI.AiManager_Fool":
                    range = EnemyRange.Fool();
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Ranged":
                    range = EnemyRange.Ranged();
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Melee":
                    range = EnemyRange.Melee();
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Totem":
                    range = EnemyRange.Totem();
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Escape":
                    range = EnemyRange.Escape();
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_AllRangedBoss":
                    range = EnemyRange.AllRangedBoss();
                    break;
            }
        }
        public override void Damage(int num, Vector3 nockback)
        {
            var damage = num - status.BaseArmor;
            if (damage > 0)
            {
                status.Health -= damage;
            }
            else
            {
                status.Health -= 1;
            }
            if (!buff.GetBuff(Buff.buff.IRONBODY))
            {
                this.nockback = nockback;
            }
        }
        public override bool IsDead()
        {
            return status.Health <= 0;
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

            collision.Force(velocity, status.Movespeed);//移動
        }
        public Status GetStatus()
        {
            return status;
        }
        public int GetExp()
        {
            return exp;
        }
        public int GetID()
        {
            return id;
        }

        public override void TrueDamage(int num)
        {
            status.Health -= num;
        }

        public void InfintyRange()
        {
            infinity = true;
            range.searchRange = 10000;
        }

        public bool UpdateAllRange()
        {
            return infinity;
        }

        public override int GetDiffence()
        {
            return status.BaseArmor;
        }
        public override int GetAttack()
        {
            return status.BasePower;
        }
    }
}