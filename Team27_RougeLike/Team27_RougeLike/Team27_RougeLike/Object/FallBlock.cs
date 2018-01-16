using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object
{
    class FallBlock : Cube
    {
        private static float collisionHeight = 4 * Map.MapDef.TILE_SIZE;

        public FallBlock(Vector3 position, Vector3 halfSize, GameDevice gameDevice)
            :base(position, halfSize, gameDevice)
        {
            SetTexture("fade");
        }

        public override BoundingBox Collision
        {
            get
            {
                Vector3 tempPos = new Vector3(position.X, Map.MapDef.TILE_SIZE, position.Z);
                Vector3 boundSize = new Vector3(size.X, collisionHeight, size.Z);
                return new BoundingBox(tempPos - boundSize, tempPos + boundSize);
            }
        }
    }
}
