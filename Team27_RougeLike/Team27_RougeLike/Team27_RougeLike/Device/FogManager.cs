//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.30
// 内容  ：霧を管理するクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Team27_RougeLike.Device
{
    class FogManager
    {
        private bool isActive;      //霧が有効か
        private Vector3 fogColor;     //霧の色
        private float near;         //近い
        private float far;          //遠い

        public FogManager()
        {
            isActive = false;
            fogColor = new Vector3(0.3f, 0.3f, 0.3f);
            near = 40;
            far = 200;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void FogOn()
        {
            isActive = true;
        }

        public void FogOff()
        {
            isActive = false;
        }

        public void SetColor(Vector3 color)
        {
            fogColor = color;
        }

        public void SetNear(float near)
        {
            this.near = near;
        }

        public void SetFar(float far)
        {
            this.far = far;
        }

        public void SetFog(ref BasicEffect basicEffect)
        {
            basicEffect.FogEnabled = isActive;
            basicEffect.FogColor = fogColor;
            basicEffect.FogStart = near;
            basicEffect.FogEnd = far;
        }
    }
}
