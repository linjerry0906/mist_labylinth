using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Object.AI;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object
{
    abstract class EnemyBase : CharacterBase
    {
        protected int searchRange;      //索敵範囲
        protected int attackRange;      //攻撃範囲
        protected int waitRange;        //敵のとる間合い
        protected int hitRange;         //敵のUpdate範囲
        protected int Feeling;          //気分値


        protected BaseAiManager aiManager;
        public BaseAiManager AiManager { get { return aiManager; } }

        public EnemyBase(Model model, Status status, Transform transform,BaseAiManager AiType)
            : base(model, status, transform)
        {
            aiManager = AiType;
            tag = "Enemy";
        }

        public abstract override void Initialize();
        public abstract override void Attack();
        public abstract override void Update();
        public abstract void HitUpdate(Player player);
        public int Distance(Player player) { return (int)Vector2.Distance(new Vector2(player.transform.position.X, player.transform.position.Z), new Vector2(transform.position.X, transform.position.Z)); }
        public bool SearchCheck(Player player) { return Distance(player) < searchRange; }
        public bool AttackCheck(Player player) { return Distance(player) < attackRange; }
        public override bool HitCheck(CharacterBase character)
        {
            return Vector2.Distance(new Vector2(character.transform.position.X, character.transform.position.Z), new Vector2(transform.position.X, transform.position.Z)) < hitRange/* character.transform.Width + transform.Width*/;
        }
    }
}