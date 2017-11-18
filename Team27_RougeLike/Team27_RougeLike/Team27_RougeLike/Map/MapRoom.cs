//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17
//--------------------------------------------------------------------------------------------------
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

        private int id;
        private int widthCell;
        private int lengthCell;
        private int xCell;
        private int zCell;

        //Debug表示用
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
                new Vector3(xCell * MapDef.TILE_SIZE, 0, zCell * MapDef.TILE_SIZE),
                new Vector3(widthCell * MapDef.TILE_SIZE / 2.0f, 0.5f, lengthCell * MapDef.TILE_SIZE / 2.0f),
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
                new Vector3(xCell * MapDef.TILE_SIZE, 0, zCell * MapDef.TILE_SIZE),
                new Vector3(widthCell * MapDef.TILE_SIZE / 2.0f, 0.5f, lengthCell * MapDef.TILE_SIZE / 2.0f),
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

        public int ID
        {
            get { return id; }
        }

        public int Length
        {
            get { return lengthCell; }
        }

        public int Width
        {
            get { return widthCell; } 
        }

        public int XCell
        {
            get { return xCell; }
        }

        public int ZCell
        {
            get { return zCell; }
        }

        public int MinX
        {
            get { return xCell - widthCell / 2; }
        }

        public int MaxX
        {
            get { return xCell + widthCell / 2; }
        }

        public int MinZ
        {
            get { return ZCell - lengthCell / 2; }
        }

        public int MaxZ
        {
            get { return ZCell + lengthCell / 2; }
        }

        public Point Cell()
        {
            return new Point(xCell, zCell);
        }

        public Vector2 Position()
        {
            return new Vector2(xCell, zCell);
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
