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

        /// <summary>
        /// Default設定で生成
        /// </summary>
        public FogManager()
        {
            isActive = false;
            fogColor = new Vector3(0.3f, 0.3f, 0.3f);
            near = 40;
            far = 200;
        }

        /// <summary>
        /// Fogが有効しているか
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return isActive;
        }

        /// <summary>
        /// Fogを有効
        /// </summary>
        public void FogOn()
        {
            isActive = true;
        }

        /// <summary>
        /// Fogを無効
        /// </summary>
        public void FogOff()
        {
            isActive = false;
        }

        /// <summary>
        /// Fogの色を設定
        /// </summary>
        /// <param name="color">色</param>
        public void SetColor(Vector3 color)
        {
            fogColor = color;
        }

        public Color CurrentColor()
        {
            return new Color(fogColor);
        }

        /// <summary>
        /// Fogの近い距離
        /// </summary>
        /// <param name="near">近い距離</param>
        public void SetNear(float near)
        {
            this.near = near;
        }

        /// <summary>
        /// Fogの遠い距離
        /// </summary>
        /// <param name="far">遠い距離</param>
        public void SetFar(float far)
        {
            this.far = far;
        }

        /// <summary>
        /// エフェクトのFogを設定
        /// </summary>
        /// <param name="basicEffect">使用するエフェクト</param>
        public void SetFog(ref BasicEffect basicEffect)
        {
            basicEffect.FogEnabled = isActive;      //有効か無効かを設定
            basicEffect.FogColor = fogColor;        //色設定
            basicEffect.FogStart = near;            //近い距離を設定
            basicEffect.FogEnd = far;               //遠い距離を設定
        }
    }
}
