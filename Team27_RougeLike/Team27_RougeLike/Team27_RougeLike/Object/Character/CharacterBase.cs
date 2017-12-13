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
namespace Team27_RougeLike.Object
{
    abstract class CharacterBase
    {
        public float angle;          //向き
        public Status status;        //様々なパラメータ
        protected CollisionSphere collision;
        protected Motion motion;
        protected string textureName;//テクスチャ名
        protected string tag;        //敵味方　タグ分け

        public string Tag { get{ return tag; }}

        public CharacterBase(Status status, CollisionSphere collision,string textureName)
        {
            this.status = status;
            this.collision = collision;
            this.textureName = textureName;
        }

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract void Attack();

        public void Draw(Renderer renderer)
        {
            renderer.DrawPolygon(textureName, collision.Position, new Vector2(5, 5), motion.DrawingRange(), Color.White);
        }

        public bool IsDead()
        {
            return status.Health <= 0;
        }
        public CollisionSphere Collision
        {
            get { return collision; }
        }
        public abstract bool HitCheck(CharacterBase character);
    }
}
