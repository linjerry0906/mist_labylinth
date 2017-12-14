//////////////////////////
///・作成者　飯泉 
//////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public float angle;          //向き
        public Status status;        //様々なパラメータ
        protected CollisionSphere collision;
        protected CharacterManager characterManager;
        protected BaseAiManager aiManager;
        protected Motion motion;

        protected string textureName;//テクスチャ名
        protected string tag;        //敵味方　タグ分け

        protected Vector3 velocity;
        protected float speed;

        public string Tag { get{ return tag; }}

        public CharacterBase(Status status, CollisionSphere collision,string textureName,CharacterManager characterManager)
        {
            this.status = status;
            this.collision = collision;
            this.textureName = textureName;
            this.characterManager = characterManager;
            velocity = Vector3.Zero;
            speed = 0;
        }

        public abstract void Initialize();

        public virtual void Update(GameTime gameTime)
        {
            collision.Force(-Vector3.UnitY, 1 / 6.0f);
        }

        public abstract void Attack();

        public void Draw(Renderer renderer)
        {
            renderer.DrawPolygon(textureName, collision.Position, new Vector2(5, 5), motion.DrawingRange(), Color.White);
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
    }
}
