using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace Team27_RougeLike.Object
{
    class Buff
    {
        private CharacterBase actor;
        public enum buff
        {
            POISON,
            IRONBODY,
            CONFUSE,
            ACCELERATE,
            END
        }
        private Dictionary<buff, bool> buffs = new Dictionary<buff, bool>();


        public Buff(CharacterBase actor)
        {
            this.actor = actor;
            int cnt = 0;
            while (cnt != (int)buff.END)
            {
                buffs.Add((buff)cnt++, false);
            }
        }

        public Buff()
        {
            int cnt = 0;
            while (cnt != (int)buff.END)
            {
                buffs.Add((buff)cnt++, false);
            }
        }

        public void Initialize()
        {
            int cnt = 0;
            while (cnt != (int)buff.END)
            {
                buffs[(buff)cnt++] = false;
            }
        }

        public bool GetBuff(buff buffname)
        {
            return buffs[buffname];
        }
        public void AddBuff(buff buffname)
        {
            switch (buffname)
            {
                case buff.ACCELERATE:
                    break;
                case buff.CONFUSE:
                    break;
                case buff.IRONBODY:
                    break;
                case buff.POISON:
                    break;
            }
            buffs[buffname] = true;
        }
        public void ReduceBuff(buff buffname)
        {
            buffs[buffname] = false;
        }
    }
}
