using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Utility
{
    class Angle
    {
        /// <summary>
        /// 相手の座標　自分の座標
        /// </summary>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <returns></returns>
        public static float CheckAngle(Vector3 position1,Vector3 position2)
        {
            var d = (float)Math.Atan2(position1.Z - position2.Z, position1.X - position2.X);
            return d * 180 / (float)Math.PI;
        }
        public static Vector3 CheckAngleVector(Vector3 position1, Vector3 position2)
        {
            Vector3 v = new Vector3(position1.X - position2.X,0,position1.Z - position2.Z);
            v.Normalize();
            return v;
        }
    }
}
