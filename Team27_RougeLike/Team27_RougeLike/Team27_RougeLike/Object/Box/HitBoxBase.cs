using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Team27_RougeLike.Object;
namespace Team27_RougeLike.Object.Box
{
    abstract class HitBoxBase
    {

        public BoundingSphere collision;        //当たり判定

        protected List<CharacterBase> effectedCharacters = new List<CharacterBase>(); //既に当たっているキャラクタ

        /// <summary>
        /// 注　このマスクに入れるものは、触らせたくないもの
        /// </summary>
        private List<string> mask;              //マスク
        private int time;                       //持続時間
        private bool isend;
        private Buff.buff buff;

        public HitBoxBase()
        {
            isend = false; 
        }

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

        /// <summary>
        /// 当たり判定:マスク無し バフあり
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="time"></param>
        public HitBoxBase(BoundingSphere collision, int time,Buff.buff buff)
        {
            this.collision = collision;
            this.time = time;
            this.buff = buff;
        }

        /// <summary>
        /// 当たり判定:単体マスクあり バフあり
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="time"></param>
        /// <param name="tag"></param>
        public HitBoxBase(BoundingSphere collision, int time, string tag,Buff.buff buff)
        {
            this.collision = collision;
            this.time = time;
            mask = new List<string>();
            mask.Add(tag);
            this.buff = buff;
        }

        /// <summary>
        /// 当たり判定:複数マスクあり バフあり
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="time"></param>
        /// <param name="tags"></param>
        public HitBoxBase(BoundingSphere collision, int time, List<string> tags,Buff.buff buff)
        {
            this.collision = collision;
            this.time = time;
            this.mask = tags;
            this.buff = buff;
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
            return time <= 0 || isend;
        }

        public void End()
        {
            isend = true;
        }
        public virtual void Effect(CharacterBase character)
        {
            character.GetBuffs().AddBuff(buff);
        }
        public Vector3 Position() { return collision.Center; }

        public List<CharacterBase> EffectedCharacters()
        {
            return effectedCharacters;
        }
    }
}
