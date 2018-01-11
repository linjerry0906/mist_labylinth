using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Object.Box
{
    abstract class HitBoxBase
    {

        /// <summary>
        /// 注　このマスクに入れるものは、触らせたくないもの
        /// </summary>
        private List<string> mask;          //マスク
        public BoundingSphere collision;   //当たり判定

        private int time;                   //持続時間
        
        /// <summary>
        /// 当たり判定:マスク無し
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="time"></param>
        public HitBoxBase(BoundingSphere collision,int time)
        {
            this.collision = collision;
            this.time = time;
        }

        /// <summary>
        /// 当たり判定:単体マスクあり
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="time"></param>
        /// <param name="tag"></param>
        public HitBoxBase(BoundingSphere collision, int time,string tag)
        {
            this.collision = collision;
            this.time = time;
            mask = new List<string>();
            mask.Add(tag);
        }

        /// <summary>
        /// 当たり判定:複数マスクあり
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="time"></param>
        /// <param name="tags"></param>
        public HitBoxBase(BoundingSphere collision, int time,List<string> tags)
        {
            this.collision = collision;
            this.time = time;
            this.mask = tags;
        }

        public virtual void Update()
        {
            time--;
        }

        /// <summary>
        /// 当たり判定
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool HitCheck(CharacterBase character)
        {
            if(mask != null)
            {
                //マスクチェック 
                foreach (var m in mask)
                {
                    if(m == character.Tag)
                    {
                        return false;
                    }
                }
                if (collision.Intersects(character.Collision.Collision))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (collision.Intersects(character.Collision.Collision))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsEnd()
        {
            return time <= 0;
        }

        public abstract void Effect(CharacterBase character);
        public Vector3 Position() { return collision.Center; }
    }
}
