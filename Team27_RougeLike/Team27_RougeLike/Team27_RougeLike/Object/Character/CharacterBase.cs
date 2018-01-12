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

        protected string textureName;//テクスチャ名
        protected string tag;        //敵味方　タグ分け

        protected Vector3 velocity;

        public string Tag { get { return tag; } }

        public CharacterBase(CollisionSphere collision, string textureName, CharacterManager characterManager)
        {
            this.collision = collision;
            this.textureName = textureName;
            this.characterManager = characterManager;
            velocity = Vector3.Zero;
        }

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract void Attack();

        public void Draw(Renderer renderer)
        {
            renderer.DrawPolygon(textureName, collision.Position, new Vector2(collision.Radius), motion.DrawingRange(), Color.White);
        }
        public abstract void Damage(int num, Vector3 nockback);
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
