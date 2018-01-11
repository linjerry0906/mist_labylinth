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
        public Status status;        //様々なパラメータ
        protected CollisionSphere collision;
        protected CharacterManager characterManager;
        protected BaseAiManager aiManager;
        protected Motion motion;

        protected string textureName;//テクスチャ名
        protected string tag;        //敵味方　タグ分け

        protected Vector3 velocity;

        public string Tag { get { return tag; } }

        public CharacterBase(Status status, CollisionSphere collision, string textureName, CharacterManager characterManager)
        {
            this.status = status;
            this.collision = collision;
            this.textureName = textureName;
            this.characterManager = characterManager;
            velocity = Vector3.Zero;
        }

        public abstract void Initialize();

        public virtual void Update(GameTime gameTime)
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

            collision.Force(velocity, status.Movespeed);//移動
        }

        public abstract void Attack();

        public void Draw(Renderer renderer)
        {
            renderer.DrawPolygon(textureName, collision.Position, new Vector2(collision.Radius), motion.DrawingRange(), Color.White);
        }
        public void Damage(int num)
        {
            status.Health = status.Health - num;
        }
        public bool IsDead()
        {
            return status.Health <= 0;
        }
        public CollisionSphere Collision
        {
            get { return collision; }
        }
        public BaseAiManager AiManager()
        {
            return aiManager;
        }
        public bool Dodge  ()
        {
            return aiManager.GetStateAi() is StateAi_Dodge;
        }
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

    }
}
