using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Device
{
    public abstract class Loader
    {
        protected string[,] resources;
        protected int counter;
        protected int maxNum;
        protected bool endFlag;

        public Loader(string[,] resources)
        {
            this.resources = resources;
            counter = 0;
            maxNum = 0;
            endFlag = false;
        }

        public void Initialize()
        {
            counter = 0;
            endFlag = false;
            maxNum = 0;
            if (resources != null)
            {
                //配列から登録する個数を取得
                maxNum = resources.GetLength(0);
            }
        }

        //最大所持数を取得
        public int Count()
        {
            return maxNum;
        }

        //現在登録している番号を取得
        public int CurrentCount()
        {
            return counter;
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public abstract void Update();
    }
}
