using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.AI;
namespace Team27_RougeLike.Object
{
    abstract class CharacterBase
    {
        protected CollisionSphere collision;
        protected CharacterManager characterManager;
        protected BaseAiManager aiManager;
        protected Motion motion;
        protected Buff buff;
        protected AttackBase attack;
        protected string color;
        protected string textureName;   //テクスチャ名
        protected string tag;           //敵味方　タグ分け
        protected string name;          //個体名
        protected string plusalpha;     //ほんとはもっとスマートにできるけど、間に合わせ目的でのもの
        protected Vector3 velocity;     //移動ベクトル0
        protected Vector3 keepAttackAngle;  //攻撃方向
        protected Vector3 nockback;
        public string Tag { get { return tag; } }

        public CharacterBase(CollisionSphere collision, string textureName, CharacterManager characterManager, string name, string color)
        {
            this.collision = collision;
            this.textureName = textureName;
            this.characterManager = characterManager;
            this.name = name;
            this.color = color;
            velocity = Vector3.Zero;
            buff = new Buff(this);
        }

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public virtual void Attack()
        {
            attack.Attack();
        }

        public void AttackChange(AttackBase attack)
        {
            this.attack = attack;
        }

        public virtual void Draw(Renderer renderer)
        {
            if (NockBacking())
            {
                renderer.DrawPolygon(textureName + plusalpha, collision.Position, new Vector2(collision.Radius*2), motion.DrawingRange(), Color.Red);
            }
            else
            {
                renderer.DrawPolygon(textureName + plusalpha, collision.Position, new Vector2(collision.Radius*2), motion.DrawingRange(), ColorLoad.GetColor(color));
            }
        }
        public bool Dodge()
        {
            return aiManager.GetStateAi() is StateAi_Dodge;
        }
        protected void NockBackUpdate()
        {
            var n = nockback;
            n.Y = 0;
            nockback -= n * 0.1f;
            nockback.Y = 0;
            if (nockback.X < 0.2f) nockback.X = 0;
            if (nockback.Z < 0.2f) nockback.Z = 0;
        }
        public bool NockBacking()
        {
            return nockback != Vector3.Zero;
        }
        public abstract void SetAttackAngle();
        public Vector3 GetKeepAttackAngle()
        {
            return keepAttackAngle;
        }
        public abstract Vector3 GetAttackAngle();
        public abstract void Damage(int num, Vector3 nockback);
        public abstract void TrueDamage(int num);
        public abstract bool IsDead();
        public abstract void Move();
        public CollisionSphere Collision
        {
            get { return collision; }
        }
        public BaseAiManager AiManager()
        {
            return aiManager;
        }
        public Buff GetBuffs()
        {
            return buff;
        }
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public void Log(string log)
        {
            characterManager.Log(log);
        }

        public string GetName()
        {
            return name;
        }

        public void TexChange(string plusalpha)
        {
            this.plusalpha = plusalpha;
        }

        public abstract int GetDiffence();
        public abstract int GetAttack();
    }
}
