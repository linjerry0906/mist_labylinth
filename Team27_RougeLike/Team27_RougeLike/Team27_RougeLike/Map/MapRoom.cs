using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{

    class MapRoom
    {
        private int id;
        private int widthCell;
        private int lengthCell;
        private int xCell;
        private int zCell;

        private Cube cube;

        public MapRoom(int id, int widthCell, int lengthCell, int xCell, int zCell, GameDevice gameDevice)
        {
            this.id = id;
            this.widthCell = widthCell;
            this.lengthCell = lengthCell;
            this.xCell = xCell;
            this.zCell = zCell;

            cube = new Cube(
                new Vector3(xCell * 7, 0, zCell * 7),
                new Vector3(widthCell / 2.0f, 0.5f, lengthCell / 2.0f),
                gameDevice);
        }

        public Rectangle Rect()
        {
            return new Rectangle(
                xCell - widthCell / 2,
                zCell - lengthCell / 2,
                widthCell,
                lengthCell);
        }

        public bool RoomCollision(MapRoom other)
        {
            return Rect().Intersects(other.Rect());
        }

        public void Move(int xCell, int zCell)
        {
            this.xCell += xCell;
            this.zCell += zCell;
        }

        public void Draw()
        {
            cube.Draw();
        }
    }
}
