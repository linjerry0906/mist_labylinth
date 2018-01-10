using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Object.AI;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Utility;
using Team27_RougeLike.Object.Box;
namespace Team27_RougeLike.Object
{
    class EnemyBase : CharacterBase
    {
        protected EnemyRange range;
        protected string aiName;

        /// <summary>
        /// オリジナル
        /// </summary>
        /// <param name="status"></param>
        /// <param name="collision"></param>
        /// <param name="aiName"></param>
        /// <param name="textureName"></param>
        /// <param name="characterManager"></param>
        public EnemyBase(Status status, CollisionSphere collision, string aiName, string textureName, CharacterManager characterManager)
            : base(status, collision, textureName, characterManager)
        {
            this.aiName = aiName;
        }


        /// <summary>
        /// クローン
        /// </summary>
        /// <param name="status"></param>
        /// <param name="collision"></param>
        /// <param name="manager"></param>
        /// <param name="textureName"></param>
        /// <param name="characterManager"></param>
        public EnemyBase(Status status, CollisionSphere collision, BaseAiManager manager, string textureName, CharacterManager characterManager)
         : base(status, collision, textureName, characterManager)
        {
            tag = "Enemy";
            aiManager = manager;
            motion = new Motion();
            for (int i = 0; i < 6; i++)
            {
                motion.Add(i, new Rectangle(i * 64, 0, 64, 64));
            }

            motion.Initialize(new Range(0, 5), new Timer(0.1f));
            aiManager.Initialize(this);
            InitRange();
        }



        public override void Initialize()
        {

        }
        public override void Attack()
        {
            characterManager.AddHitBox(new DamageBox(new BoundingSphere(collision.Position, 10), 1, tag, status.BasePower));
        }
        public override void Update(GameTime gameTime)
        {
            aiManager.Update();
            motion.Update(gameTime);
            base.Update(gameTime);
        }
        public virtual void NearUpdate(Player player, GameTime gameTime)
        {
            ((IEnemyAI)aiManager).NearUpdate(player);
        }
        public int Distance(Player player) { return (int)Vector2.Distance(new Vector2(player.Collision.Position.X, player.Collision.Position.Z), new Vector2(collision.Position.X, collision.Position.Z)); }
        public bool SearchCheck(Player player) { return Distance(player) < range.searchRange; }
        public bool AttackCheck(Player player) { return Distance(player) < range.attackRange; }
        public bool WaitPointCheck(Player player) { return Distance(player) < range.waitRange; }
        public EnemyBase Clone(Vector3 position)
        {
            return new EnemyBase
                (
                new Status(status.Level, status.Health, status.BasePower, status.BaseArmor, status.Attackspd, status.Speed),
                new CollisionSphere(position, collision.Radius),
                SwitchAi(),
                textureName,
                characterManager
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
                default:
                    return new AiManager_Fool();
            }
        }
        private void InitRange()
        {
            switch (aiManager.ToString())
            {
                case "Team27_RougeLike.Object.AI.AiManager_Fool":
                    range = new EnemyRange(50, 20, 10);
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Ranged":
                    range = new EnemyRange(50, 15, 30);
                    break;
                case "Team27_RougeLike.Object.AI.AiManager_Melee":
                    range = new EnemyRange(50, 20, 15);
                    break;
            }
        }
    }
}