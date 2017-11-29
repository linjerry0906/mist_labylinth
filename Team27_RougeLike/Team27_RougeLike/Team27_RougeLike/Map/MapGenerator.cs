//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17 ～ 2017.11.27
// 内容  ：ダンジョンをランダムで自動生成
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
            CheckMapSize,       //マップサイズを確定
            WriteToArray,       //書き出し
            SetEventPoint,      //入口や出口などの設置
            End,                //処理完了
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

        private int[,] mapChip;             //マップチップ

        private GenerateState currentState;     //現在の生成状態

        /// <summary>
        /// ダンジョンを自動生成するクラス
        /// </summary>
        /// <param name="dungeonSize">ダンジョンの大きさ</param>
        /// <param name="gameDevice">ゲームディバイス</param>
        public MapGenerator(int dungeonSize, GameDevice gameDevice)
        {
            rooms = new List<MapRoom>();
            mainRoom = new List<MapRoom>();
            halls = new List<MapRoom>();
            edges = new List<Edge>();
            mapChip = new int[1,1];
            this.gameDevice = gameDevice;
            this.dungeonSize = dungeonSize;       //ダンジョンのサイズ

            //正規分布の楕円形の縦と横(集約させるためにさらに2を割る)
            limitWidth = (gameDevice.Random.Next(dungeonSize / 4, dungeonSize * 3 / 4)) / 2;
            limitHeight = (dungeonSize - limitWidth) / 2;

            currentState = GenerateState.GenerateRoom;      //生成状態
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
            //任意の円の角度（ラジアン）
            float t = (float)(2 * Math.PI * gameDevice.Random.NextDouble());
            //半径決定（単位円内に集約）
            float u = (float)(gameDevice.Random.NextDouble() + gameDevice.Random.NextDouble());
            float r = (u > 1) ? 2 - u: u;
            //単位円内の点×指定の縦、横
            return new Point((int)(width * r * Math.Cos(t)), (int)(height * r * Math.Sin(t)));
        }

        public void Update()
        {
            switch (currentState)
            {
                case GenerateState.GenerateRoom:        //部屋生成
                    UpdateGenerate();
                    break;
                case GenerateState.Discrete:            //部屋離散
                    UpdateDiscrete();
                    break;
                case GenerateState.SelectMainRoom:      //メイン部屋を選択
                    UpdateSelectMainRoom();
                    break;
                case GenerateState.LinkRoom:            //メイン部屋を接続
                    UpdateLinkRoom();
                    break;
                case GenerateState.CreateHall:          //廊下を生成
                    UpdateCreateHall();
                    break;
                case GenerateState.ChooseSubRoom:       //サブの部屋を選択
                    UpdateChooseSubRoom();
                    break;
                case GenerateState.CheckMapSize:        //マップサイズを確定
                    UpdateCheckMapSize();
                    break;
                case GenerateState.WriteToArray:        //マップチップ生成
                    UpdateWriteToArray();
                    break;
                case GenerateState.SetEventPoint:       //ランダムで特殊なマスを設置
                    UpdateSetEventPoint();
                    break;
                case GenerateState.End:
                    break;
            }
        }

        /// <summary>
        /// 部屋を生成
        /// </summary>
        private void UpdateGenerate()
        {
            if (rooms.Count < dungeonSize)      //指定のサイズまで部屋を生成し続ける
            {
                Point pos = RandomPointInCircle(limitWidth, limitHeight);
                rooms.Add(
                    new MapRoom(
                        rooms.Count,            //部屋番号
                        gameDevice.Random.Next(MapDef.MIN_ROOM_SIZE, MapDef.MAX_ROOM_SIZE) * 2,     //横サイズ（2で割れるように）
                        gameDevice.Random.Next(MapDef.MIN_ROOM_SIZE, MapDef.MAX_ROOM_SIZE) * 2,     //縦サイズ
                        pos.X,                  //X座標
                        pos.Y,                  //Z座標
                        gameDevice));
            }
            else
            {
                currentState = GenerateState.Discrete;      //部屋を離散する
            }
        }

        /// <summary>
        /// 部屋を離散
        /// </summary>
        private void UpdateDiscrete()
        {
            int counter = 0;        //部屋が重なる数
            foreach (MapRoom r1 in rooms)
            {
                foreach (MapRoom r2 in rooms)
                {
                    if (r1 == r2 || !r1.RoomCollision(r2))  //同じ部屋は判断しない、当たってないと知らせない
                        continue;
                    counter++;      //当たっている部屋まだある
                    r1.Hit(r2);     //位置修正
                    r2.Hit(r1);     //位置修正
                }
            }
            if (counter <= 0)       //全部修正完了すると次の段階へ移行
            {
                currentState = GenerateState.SelectMainRoom;
            }
        }

        /// <summary>
        /// メインとなる部屋を記録
        /// </summary>
        private void UpdateSelectMainRoom()
        {
            foreach (MapRoom r in rooms)
            {
                //縦横サイズが指定より大きい場合
                if (r.Length > (int)(MapDef.MAX_ROOM_SIZE * 2 * 0.65f) &&
                   r.Width > (int)(MapDef.MAX_ROOM_SIZE * 2 * 0.65f))
                {
                    r.SetColor(Color.Red);      //Debug情報
                    mainRoom.Add(r);            //リストに追加
                }
            }

            currentState = GenerateState.LinkRoom;      //次の段階へ移行
        }

        /// <summary>
        /// メインとなる部屋を繋ぐ（島がないように）
        /// </summary>
        private void UpdateLinkRoom()
        {
            for (int i = 0; i < mainRoom.Count - 1; i++)
            {
                int minIndex = i + 1;   //残りの部屋と一番近い部屋の番号
                for (int j = i + 1; j < mainRoom.Count; j++)        //残りの部屋との距離判定
                {
                    //今記録している部屋との距離より近い場合
                    if ((mainRoom[i].Position() - mainRoom[minIndex].Position()).Length()
                         > (mainRoom[i].Position() - mainRoom[j].Position()).Length())
                    {
                        minIndex = j;   //部屋番号を指定する
                    }
                }
                //繋ぐ線を追加
                Edge edge = new Edge(mainRoom[i].Cell(), mainRoom[minIndex].Cell(), gameDevice);
                if (!edges.Contains(edge))
                {
                    edges.Add(edge);
                }
            }
            currentState = GenerateState.CreateHall;    //次の段階へ移行
        }

        /// <summary>
        /// 廊下を追加
        /// </summary>
        private void UpdateCreateHall()
        {
            foreach(Edge e in edges)
            {
                //線分の中点
                Point center = new Point(
                    (e.FirstPoint.X + e.SecondPoint.X) / 2,
                    (e.FirstPoint.Y + e.SecondPoint.Y) / 2);
                //横
                MapRoom hall = new MapRoom(
                    halls.Count,
                    Math.Abs(e.FirstPoint.X - e.SecondPoint.X) + 2,
                    2,
                    center.X,
                    e.SecondPoint.Y,
                    gameDevice);
                hall.SetColor(Color.Blue);
                halls.Add(hall);
                //縦
                hall = new MapRoom(
                    halls.Count,
                    2,
                    Math.Abs(e.FirstPoint.Y - e.SecondPoint.Y) + 2,
                    e.FirstPoint.X,
                    center.Y,
                    gameDevice);
                hall.SetColor(Color.Blue);
                halls.Add(hall);
            }
            currentState = GenerateState.ChooseSubRoom;     //次の段階へ移行
        }

        /// <summary>
        /// サブとなる部屋を選択
        /// </summary>
        private void UpdateChooseSubRoom()
        {
            foreach (MapRoom hall in halls)
            {
                foreach (MapRoom r in rooms)    //全部の部屋と廊下と判定
                {
                    if (mainRoom.Contains(r))   //メインの部屋は判定しない
                        continue;
                    //当たっていたらメインに追加
                    if (hall.RoomCollision(r) && 
                        (r.Length > MapDef.MAX_ROOM_SIZE * 2 * 0.3f || r.Width > MapDef.MAX_ROOM_SIZE * 2 * 0.3f))
                    {
                        r.SetColor(Color.Gold); //Debug情報
                        mainRoom.Add(r);
                    }
                }
            }
            currentState = GenerateState.CheckMapSize;
        }

        /// <summary>
        /// マップのサイズを確定
        /// </summary>
        private void UpdateCheckMapSize()
        {
            minXRoomIndex = mainRoom[0].ID;     //最大最小値を先頭に設定
            maxXRoomIndex = mainRoom[0].ID;
            minZRoomIndex = mainRoom[0].ID;
            maxZRoomIndex = mainRoom[0].ID;

            foreach (MapRoom r in mainRoom)     //部屋ごとに辺を比較する
            {
                if (r.MinX < mainRoom.Find((MapRoom min) => min.ID == minXRoomIndex).MinX)
                {
                    minXRoomIndex = r.ID;
                }
                else if (r.MaxX > mainRoom.Find((MapRoom max) => max.ID == maxXRoomIndex).MaxX)
                {
                    maxXRoomIndex = r.ID;
                }
                if (r.MinZ < mainRoom.Find((MapRoom min) => min.ID == minZRoomIndex).MinZ)
                {
                    minZRoomIndex = r.ID;
                }
                else if (r.MaxZ > mainRoom.Find((MapRoom max) => max.ID == maxZRoomIndex).MaxZ)
                {
                    maxZRoomIndex = r.ID;
                }
            }

            //Debug情報
            rooms[minXRoomIndex].SetColor(Color.Black);
            rooms[minZRoomIndex].SetColor(Color.Black);
            rooms[maxXRoomIndex].SetColor(Color.Black);
            rooms[maxZRoomIndex].SetColor(Color.Black);

            currentState = GenerateState.WriteToArray;
        }

        /// <summary>
        /// マップチップに書き出す
        /// </summary>
        private void UpdateWriteToArray()
        {
            //縦と横のマス数でマップチップの配列を生成
            int width = rooms[maxXRoomIndex].MaxX - rooms[minXRoomIndex].MinX + 1;
            int length = rooms[maxZRoomIndex].MaxZ - rooms[minZRoomIndex].MinZ + 1;
            mapChip = new int[length + 2, width + 2];   //四つの辺に壁を置く

            int minX = rooms[minXRoomIndex].MinX;
            int minZ = rooms[minZRoomIndex].MinZ;

            foreach (MapRoom r in mainRoom)             //メインの部屋を先に置く
            {
                for (int z = r.MinZ - minZ + 1; z < r.MaxZ - minZ + 1; z++)         //基準となる部屋の座標を引く
                {
                    for (int x = r.MinX - minX + 1; x < r.MaxX - minX + 1; x++)     //基準となる部屋の座標を引く
                    {
                        mapChip[z, x] = (int)MapDef.BlockDef.Space;
                    }
                }
            }
            foreach (MapRoom r in halls)                 //廊下を置く
            {
                for (int z = r.MinZ - minZ + 1; z < r.MaxZ - minZ + 1; z++)         //基準となる部屋の座標を引く
                {
                    for (int x = r.MinX - minX + 1; x < r.MaxX - minX + 1; x++)     //基準となる部屋の座標を引く
                    {
                        mapChip[z, x] = (int)MapDef.BlockDef.Space;
                    }
                }
            }
            currentState = GenerateState.SetEventPoint;
        }

        /// <summary>
        /// 入口と出口などの座標をマップチップに書き込む
        /// ToDo：モンスターが湧く所
        /// </summary>
        private void UpdateSetEventPoint()
        {
            //違う部屋に設定（ランダム）
            int entryRoom = gameDevice.Random.Next(0, mainRoom.Count);      //入口の部屋（添え字）
            //メインの部屋ではないと選択しなおし
            while ((mainRoom[entryRoom].Width < (int)(MapDef.MAX_ROOM_SIZE * 2 * 0.65f) &&
                         mainRoom[entryRoom].Length < (int)(MapDef.MAX_ROOM_SIZE * 2 * 0.65f)))
            {
                entryRoom = gameDevice.Random.Next(0, mainRoom.Count);
            }
            int exitRoom = gameDevice.Random.Next(0, mainRoom.Count);       //出口の部屋（添え字）
            //メインの部屋ではないと選択しなおし、入口の部屋とかぶると選択しなおし
            while ((exitRoom == entryRoom) &&
                       (mainRoom[exitRoom].Width < (int)(MapDef.MAX_ROOM_SIZE * 2 * 0.65f) &&
                        mainRoom[entryRoom].Length < (int)(MapDef.MAX_ROOM_SIZE * 2 * 0.65f)))
            {
                exitRoom = gameDevice.Random.Next(0, mainRoom.Count);
            }

            //マップのX, Y最小値   基準座標
            int minX = rooms[minXRoomIndex].MinX;
            int minZ = rooms[minZRoomIndex].MinZ;

            //部屋内の乱数座標を設定（壁とつながっていないマス）
            Point entryPoint = new Point(
                gameDevice.Random.Next(mainRoom[entryRoom].MinX - minX + 1, mainRoom[entryRoom].MaxX - minX),
                gameDevice.Random.Next(mainRoom[entryRoom].MinZ - minZ + 1, mainRoom[entryRoom].MaxZ - minZ));
            Point exitPoint = new Point(
                gameDevice.Random.Next(mainRoom[exitRoom].MinX - minX + 1, mainRoom[exitRoom].MaxX - minX),
                gameDevice.Random.Next(mainRoom[exitRoom].MinZ - minZ + 1, mainRoom[exitRoom].MaxZ - minZ));

            mapChip[entryPoint.Y, entryPoint.X] = (int)MapDef.BlockDef.Entry;
            mapChip[exitPoint.Y, exitPoint.X] = (int)MapDef.BlockDef.Exit;

            currentState = GenerateState.End;
        }

        /// <summary>
        /// 生成が終わっているか
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return currentState == GenerateState.End;
        }

        /// <summary>
        /// マップチップ
        /// </summary>
        public int[,] MapChip
        {
            get { return mapChip; }
        }

        /// <summary>
        /// Debug 表示
        /// </summary>
        public void Draw()
        {
            foreach (MapRoom r in rooms)
            {
                r.Draw();
            }
            foreach (MapRoom r in halls)
            {
                r.Draw();
            }
        }
    }
}
