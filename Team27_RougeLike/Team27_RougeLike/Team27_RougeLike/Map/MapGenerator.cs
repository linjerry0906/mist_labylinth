//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    class MapGenerator
    {
        private enum GenerateState
        {
            GenerateRoom,       //部屋を乱数生成
            Discrete,           //部屋を離散する
            SelectMainRoom,     //メインの部屋を確保
            LinkRoom,           //メインの部屋を接続する
            CreateHall,         //通路を生成
            ChooseSubRoom,      //通路上のサブ部屋を追加
            WriteToArray,
        }

        private int dungeonSize;            //マップの大きさ
        private int limitHeight;            //正規分布の縦の最大長さ
        private int limitWidth;             //正規文武の横の最大長さ

        private int minXRoomIndex = 0;      //マップのスタート点X用
        private int maxXRoomIndex = 0;      //マップのエンド点X用
        private int minZRoomIndex = 0;      //マップのスタート点Z用
        private int maxZRoomIndex = 0;      //マップのエンド点Z用

        private GameDevice gameDevice;
        private List<MapRoom> rooms;        //全部の部屋
        private List<MapRoom> mainRoom;     //メインの部屋
        private List<MapRoom> halls;        //通路
        private List<Edge> edges;           //接続計算用の辺

        private GenerateState currentState;     //現在の生成状態

        public MapGenerator(GameDevice gameDevice)
        {
            rooms = new List<MapRoom>();
            mainRoom = new List<MapRoom>();
            halls = new List<MapRoom>();
            edges = new List<Edge>();
            this.gameDevice = gameDevice;

            dungeonSize = 50;
            limitWidth = gameDevice.Random.Next(dungeonSize / 4, dungeonSize * 3 / 4);
            limitHeight = dungeonSize - limitWidth;

            currentState = GenerateState.GenerateRoom;
        }

        /// <summary>
        /// 正規分布
        /// </summary>
        /// <param name="width">横サイズ</param>
        /// <param name="height">縦サイズ</param>
        /// <returns></returns>
        private Point RandomPointInCircle(float width, float height)
        {
            //ネットのソースを使用
            float t = (float)(2 * Math.PI * gameDevice.Random.NextDouble());
            float u = (float)(gameDevice.Random.NextDouble() + gameDevice.Random.NextDouble());
            float r = (u > 1) ? 2 - u: u;
            return new Point((int)(width * r * Math.Cos(t)), (int)(height * r * Math.Sin(t)));
        }

        public void Update()
        {
            switch (currentState)
            {
                case GenerateState.GenerateRoom:
                    UpdateGenerate();
                    break;
                case GenerateState.Discrete:
                    UpdateDiscrete();
                    break;
                case GenerateState.SelectMainRoom:
                    UpdateSelectMainRoom();
                    break;
                case GenerateState.LinkRoom:
                    UpdateLinkRoom();
                    break;
                case GenerateState.CreateHall:
                    UpdateCreateHall();
                    break;
                case GenerateState.ChooseSubRoom:
                    UpdateChooseSubRoom();
                    break;
                case GenerateState.WriteToArray:
                    break;
            }
        }

        private void UpdateGenerate()
        {
            if (rooms.Count < dungeonSize)
            {
                Point pos = RandomPointInCircle(limitWidth, limitHeight);
                rooms.Add(
                    new MapRoom(
                        rooms.Count,
                        gameDevice.Random.Next(MapDef.MIN_ROOM_SIZE, MapDef.MAX_ROOM_SIZE) * 2,
                        gameDevice.Random.Next(MapDef.MIN_ROOM_SIZE, MapDef.MAX_ROOM_SIZE) * 2,
                        pos.X,
                        pos.Y,
                        gameDevice));
            }
            else
            {
                currentState = GenerateState.Discrete;
            }
        }

        private void UpdateDiscrete()
        {
            int Counter = 0;
            foreach (MapRoom r1 in rooms)
            {
                foreach (MapRoom r2 in rooms)
                {
                    if (r1 == r2 || !r1.RoomCollision(r2))
                        continue;
                    Counter++;
                    r1.Hit(r2);
                    r2.Hit(r1);
                }
            }
            if (Counter <= 0)
            {
                currentState = GenerateState.SelectMainRoom;
            }
        }

        private void UpdateSelectMainRoom()
        {
            foreach (MapRoom r in rooms)
            {
                if(r.Length > (int)(MapDef.MAX_ROOM_SIZE * 2 * 0.6f) &&
                   r.Width > (int)(MapDef.MAX_ROOM_SIZE * 2 * 0.6f))
                {
                    r.SetColor(Color.Red);
                    mainRoom.Add(r);
                }
                if (r.MinX < rooms[minXRoomIndex].MinX)
                {
                    minXRoomIndex = r.ID;
                }
                else if (r.MaxX > rooms[maxXRoomIndex].MaxX)
                {
                    maxXRoomIndex = r.ID;
                }
                if (r.MinZ < rooms[minZRoomIndex].MinZ)
                {
                    minZRoomIndex = r.ID;
                }
                else if(r.MaxZ > rooms[maxZRoomIndex].MaxZ)
                {
                    maxZRoomIndex = r.ID;
                }
            }
            rooms[minXRoomIndex].SetColor(Color.Black);
            rooms[minZRoomIndex].SetColor(Color.Black);
            rooms[maxXRoomIndex].SetColor(Color.Black);
            rooms[maxZRoomIndex].SetColor(Color.Black);

            currentState = GenerateState.LinkRoom;
        }

        private void UpdateLinkRoom()
        {
            for (int i = 0; i < mainRoom.Count - 1; i++)
            {
                int minIndex = i + 1;
                for (int j = i + 1; j < mainRoom.Count; j++)
                {
                    if ((mainRoom[i].Position() - mainRoom[minIndex].Position()).Length()
                         > (mainRoom[i].Position() - mainRoom[j].Position()).Length())
                    {
                        minIndex = j;
                    }
                }
                Edge edge = new Edge(mainRoom[i].Cell(), mainRoom[minIndex].Cell(), gameDevice);
                if (!edges.Contains(edge))
                {
                    edges.Add(edge);
                }
            }
            currentState = GenerateState.CreateHall;
        }

        private void UpdateCreateHall()
        {
            foreach(Edge e in edges)
            {
                Point center = new Point(
                    (e.FirstPoint.X + e.SecondPoint.X) / 2,
                    (e.FirstPoint.Y + e.SecondPoint.Y) / 2);

                //横その一
                MapRoom hall = new MapRoom(
                    halls.Count,
                    Math.Abs(e.FirstPoint.X - e.SecondPoint.X) + 1,
                    2,
                    center.X,
                    e.FirstPoint.Y,
                    gameDevice);
                hall.SetColor(Color.Blue);
                halls.Add(hall);
                //横その二
                hall = new MapRoom(
                    halls.Count,
                    Math.Abs(e.FirstPoint.X - e.SecondPoint.X) + 1,
                    2,
                    center.X,
                    e.SecondPoint.Y,
                    gameDevice);
                hall.SetColor(Color.Blue);
                halls.Add(hall);
                //縦その一
                hall = new MapRoom(
                    halls.Count,
                    2,
                    Math.Abs(e.FirstPoint.Y - e.SecondPoint.Y) + 1,
                    e.FirstPoint.X,
                    center.Y,
                    gameDevice);
                hall.SetColor(Color.Blue);
                halls.Add(hall);
                //縦その二
                hall = new MapRoom(
                    halls.Count,
                    2,
                    Math.Abs(e.FirstPoint.Y - e.SecondPoint.Y) + 1,
                    e.SecondPoint.X,
                    center.Y,
                    gameDevice);
                hall.SetColor(Color.Blue);
                halls.Add(hall);
            }
            currentState = GenerateState.ChooseSubRoom;
        }

        private void UpdateChooseSubRoom()
        {
            foreach (MapRoom hall in halls)
            {
                foreach (MapRoom r in rooms)
                {
                    if (mainRoom.Contains(r))
                    {
                        continue;
                    }
                    if (hall.RoomCollision(r))
                    {
                        r.SetColor(Color.Gold);
                        mainRoom.Add(r);
                    }
                }
            }
        }

        public void Draw()
        {
            foreach (MapRoom r in rooms)
            {
                r.Draw();
            }
            if (currentState != GenerateState.ChooseSubRoom)
            {
                foreach (Edge e in edges)
                {
                    e.Draw();
                }
            }
            foreach(MapRoom r in halls)
            {
                r.Draw();
            }
        }
    }
}
