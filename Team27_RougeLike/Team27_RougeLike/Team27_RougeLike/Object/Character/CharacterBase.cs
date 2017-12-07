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
namespace Team27_RougeLike.Object
{
    abstract class CharacterBase
    {
        public Transform transform;  //位置情報、サイズ
        public Status status;        //様々なパラメータ
        protected Model model;

        protected string tag;
        public string Tag { get{ return tag; }}

        /// <summary>
        /// 3Dモデルの場合
        /// </summary>
        /// <param name="model"></param>
        /// <param name="status"></param>
        /// <param name="transform"></param>
        public CharacterBase(Model model, Status status, Transform transform)
        {
            this.model = model;
            this.status = status;
            this.transform = transform;
        }

        /// <summary>
        /// 2Dモデルの場合
        /// </summary>
        /// <param name="status"></param>
        /// <param name="transform"></param>
        public CharacterBase(Status status, Transform transform)
        {
            this.status = status;
            this.transform = transform;
        }

        public abstract void Initialize();

        public abstract void Update();

        public abstract void Attack();

        public void Draw(GameDevice gamedevice)
        {
            Matrix world = Matrix.CreateTranslation(transform.position);
            model.Draw(world, gamedevice.MainProjector.LookAt, gamedevice.MainProjector.Projection);
        }

        public bool IsDead()
        {
            return status.Health <= 0;
        }

        public abstract bool HitCheck(CharacterBase character);
    }
}
