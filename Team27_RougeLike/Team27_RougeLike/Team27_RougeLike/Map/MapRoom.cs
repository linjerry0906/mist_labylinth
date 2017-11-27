//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17 ~ 2017.11.27
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
        private enum Direction      //部屋衝突判定の方向
        {
            Xplus,
            Xminus,
            Zplus,
            Zminus
        }

        private int id;             //部屋の番号
        private int widthCell;      //横サイズ（単位：マス）
        private int lengthCell;     //縦サイズ（単位：マス）
        private int xCell;          //X座標（単位：マス）
        private int zCell;          //Z座標（単位：マス）

        //Debug表示用
        private Cube cube;
        private GameDevice gameDevice;

        /// <summary>
        /// ダンジョン内移動できる空間（部屋、廊下）
        /// </summary>
        /// <param name="id">部屋番号</param>
        /// <param name="widthCell">横サイズ（単位：マス）</param>
        /// <param name="lengthCell">縦サイズ（単位：マス）</param>
        /// <param name="xCell">X座標（単位：マス）</param>
        /// <param name="zCell">Z座標（単位：マス）</param>
        /// <param name="gameDevice">Debug 表示用</param>
        public MapRoom(int id, int widthCell, int lengthCell, int xCell, int zCell, GameDevice gameDevice)
        {
            this.id = id;
            this.widthCell = widthCell;
            this.lengthCell = lengthCell;
            this.xCell = xCell;
            this.zCell = zCell;

            //Debug表示用
            cube = new Cube(
                new Vector3(xCell, 0, zCell),
                new Vector3(widthCell / 2.0f, 0.5f, lengthCell / 2.0f),
                gameDevice);
            this.gameDevice = gameDevice;
        }

        /// <summary>
        /// 部屋と部屋を判定用レクタングル
        /// </summary>
        /// <returns>部屋位置、サイズのレクタングル</returns>
        public Rectangle Rect()
        {
            return new Rectangle(
                xCell - widthCell / 2,
                zCell - lengthCell / 2,
                widthCell,
                lengthCell);
        }

        /// <summary>
        /// 部屋同士の当たり判定
        /// </summary>
        /// <param name="other">他の部屋</param>
        /// <returns>当たっているか</returns>
        public bool RoomCollision(MapRoom other)
        {
            return Rect().Intersects(other.Rect());
        }

        /// <summary>
        /// Debug表示用
        /// </summary>
        public void Draw()
        {
            cube.Draw();
        }

        /// <summary>
        /// 当たった部屋との位置修正
        /// </summary>
        /// <param name="other">他の部屋</param>
        public void Hit(MapRoom other)
        {
            Direction direction = CheckDirection(other);    //他の部屋のどの方向

            //一マスずつ修正
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

            //Debug 表示用
            cube = new Cube(
                new Vector3(xCell, 0, zCell),
                new Vector3(widthCell / 2.0f, 0.5f, lengthCell / 2.0f),
                gameDevice);
        }

        /// <summary>
        /// 別部屋との相対位置
        /// </summary>
        /// <param name="other">他の部屋</param>
        /// <returns>別部屋のどの辺</returns>
        private Direction CheckDirection(MapRoom other)
        {
            Vector2 dir = new Vector2(                  //中心座標で計算
                xCell - other.xCell,
                zCell - other.zCell);

            if (Math.Abs(dir.X) > Math.Abs(dir.Y))      //X軸の変化量がY軸より大きい場合
            {
                if (dir.X > 0)                          //自分のXが相手のXより大きい場合
                    return Direction.Xplus;
                else
                    return Direction.Xminus;
            }

            //Vector2で計算なのでYはZの代用
            if (dir.Y > 0)                              //自分のZが相手のZより大きい場合
            {
                return Direction.Zplus;
            }
            return Direction.Zminus;
        }

        /// <summary>
        /// 部屋番号
        /// </summary>
        public int ID
        {
            get { return id; }
        }

        /// <summary>
        /// 縦マス
        /// </summary>
        public int Length
        {
            get { return lengthCell; }
        }

        /// <summary>
        /// 横マス
        /// </summary>
        public int Width
        {
            get { return widthCell; } 
        }

        /// <summary>
        /// Xマス
        /// </summary>
        public int XCell
        {
            get { return xCell; }
        }

        /// <summary>
        /// Zマス
        /// </summary>
        public int ZCell
        {
            get { return zCell; }
        }

        /// <summary>
        /// Xが小さい辺のXマス
        /// </summary>
        public int MinX
        {
            get { return xCell - widthCell / 2; }
        }

        /// <summary>
        /// Xが大きい辺のXマス
        /// </summary>
        public int MaxX
        {
            get { return xCell + widthCell / 2; }
        }

        /// <summary>
        /// Zが小さい辺のZマス
        /// </summary>
        public int MinZ
        {
            get { return ZCell - lengthCell / 2; }
        }

        /// <summary>
        /// Zが大きい辺のZマス
        /// </summary>
        public int MaxZ
        {
            get { return ZCell + lengthCell / 2; }
        }

        /// <summary>
        /// 座標（単位：マス）
        /// </summary>
        /// <returns></returns>
        public Point Cell()
        {
            return new Point(xCell, zCell);
        }

        /// <summary>
        /// 距離計算用Vector2で表示する位置
        /// </summary>
        /// <returns></returns>
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
