using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Team27_RougeLike.UI;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object
{
    class Buff
    {
        private CharacterBase actor;

        private const int buffTime = 1800; //30秒

        public enum buff
        {
            START,
            毒,
            剛体化,
            混乱,
            加速,
            END
        }
        private Dictionary<buff, int> reduceBuffTime = new Dictionary<buff, int>();


        public Buff(CharacterBase actor)
        {
            this.actor = actor;
            int cnt = 0;
            while (cnt != (int)buff.END)
            {
                reduceBuffTime.Add((buff)cnt++, 0);
            }
        }

        public Buff()
        {
            int cnt = 0;
            while (cnt != (int)buff.END)
            {
                reduceBuffTime.Add((buff)cnt++, 0);
            }
        }

        public void Initialize()
        {
            int cnt = 0;
            while (cnt != (int)buff.END)
            {
                reduceBuffTime.Add((buff)cnt++, 0);
            }
        }

        public void Update()
        {
            int cnt = 0;
            PoisonUpdate();
            while (cnt != (int)buff.END)
            {
                int old = reduceBuffTime[(buff)cnt];
                if (old == 0)
                {
                    cnt++;
                    continue;
                }
                else if (old == 1)
                {
                    actor.Log(actor.GetName() + "の" + (buff)cnt + "は治まった");
                }
                reduceBuffTime[(buff)cnt++]--;
            }
        }

        private void PoisonUpdate()
        {
            if (reduceBuffTime[buff.毒] != 0 && reduceBuffTime[buff.毒] % 120 == 0)
            {
                if (actor is Player && actor.GetHealth() == 1) return;
                actor.TrueDamage(1);
                actor.Log(actor.GetName() + "は毒のダメージを受けた");
            }
        }

        public bool GetBuff(buff buffname)
        {
            return reduceBuffTime[buffname] != 0;
        }
        public void AddBuff(buff buffname)
        {
            switch (buffname)
            {
                case buff.加速:
                    actor.Log(actor.GetName() + "は加速した");
                    break;
                case buff.混乱:
                    actor.Log(actor.GetName() + "は混乱した");
                    break;
                case buff.剛体化:
                    actor.Log(actor.GetName() + "は剛体化した");
                    break;
                case buff.毒:
                    actor.Log(actor.GetName() + "は毒を患った");
                    break;
            }
            reduceBuffTime[buffname] = buffTime;
        }
        public void ReduceBuff(buff buffname)
        {
            reduceBuffTime[buffname] = 0;
            actor.Log(actor.GetName() + "の" + buffname + "は治まった");
        }
    }
}
