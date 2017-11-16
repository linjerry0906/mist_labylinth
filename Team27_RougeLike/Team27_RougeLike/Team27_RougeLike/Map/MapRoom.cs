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
        private enum Direction
        {
            Xplus,
            Xminus,
            Zplus,
            Zminus
        }

        private float tileSize = 2;

        private int id;
        private int widthCell;
        private int lengthCell;
        private int xCell;
        private int zCell;

        private Cube cube;
        private GameDevice gameDevice;

        public MapRoom(int id, int widthCell, int lengthCell, int xCell, int zCell, GameDevice gameDevice)
        {
            this.id = id;
            this.widthCell = widthCell;
            this.lengthCell = lengthCell;
            this.xCell = xCell;
            this.zCell = zCell;

            cube = new Cube(
                new Vector3(xCell * tileSize, 0, zCell * tileSize),
                new Vector3(widthCell * tileSize / 2.0f, 0.5f, lengthCell * tileSize / 2.0f),
                gameDevice);
            this.gameDevice = gameDevice;
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

        public void Draw()
        {
            cube.Draw();
        }

        public void Hit(MapRoom other)
        {
            Direction direction = CheckDirection(other);

            switch (direction)
            {
                case Direction.Xplus:
                    xCell += 1;
                    break;
                case Direction.Xminus:
                    xCell -= 1;
                    break;
                case Direction.Zplus:
                    zCell += 1;
                    break;
                case Direction.Zminus:
                    zCell -= 1;
                    break;
            }

            cube = new Cube(
                new Vector3(xCell * tileSize, 0, zCell * tileSize),
                new Vector3(widthCell * tileSize / 2.0f, 0.5f, lengthCell * tileSize / 2.0f),
                gameDevice);
        }

        private Direction CheckDirection(MapRoom other)
        {
            Vector2 dir = new Vector2(
                xCell - other.xCell,
                zCell - other.zCell);

            if (Math.Abs(dir.X) > Math.Abs(dir.Y))
            {
                if (dir.X > 0)
                    return Direction.Xplus;
                else
                    return Direction.Xminus;
            }

            if (dir.Y > 0)
            {
                return Direction.Zplus;
            }
            return Direction.Zminus;
        }

        public int Length
        {
            get { return lengthCell; }
        }

        public int Width
        {
            get { return widthCell; } 
        }

        /// <summary>
        /// Debug用
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            cube.SetColor(color);
        }
    }
}
