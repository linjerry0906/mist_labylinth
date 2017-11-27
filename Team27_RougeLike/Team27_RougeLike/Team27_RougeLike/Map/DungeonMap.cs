//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.19 ~ 2017.11.27
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input; //Debug
using Team27_RougeLike.Object;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Actor;

namespace Team27_RougeLike.Map
{
    class DungeonMap
    {
        private GameDevice gameDevice;      //Debug可視化用

        private List<Cube> mapBlocks;       //モデルに変更可
        private int[,] mapChip;             //マップチップ

        private List<Cube> mapBlocksToDraw; //描画するマップ
        private Point previousPosition;     //前フレームの描画中心座標
        private Point currentPosition;      //今のフレームの描画中心座標
        private Point entryPoint;           //入口
        private int radius = 20;            //描画半径

        public DungeonMap(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;

            mapChip = new int[1, 1];
            mapBlocks = new List<Cube>();
            mapBlocksToDraw = new List<Cube>();

            currentPosition = new Point(0, 0);
            previousPosition = currentPosition;
            entryPoint = new Point(0, 0);
        }

        public DungeonMap(int[,] mapChip, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;

            this.mapChip = mapChip;
            mapBlocks = new List<Cube>();
            mapBlocksToDraw = new List<Cube>();

            currentPosition = new Point(0, 0);
            previousPosition = currentPosition;
            entryPoint = new Point(0, 0);
        }

        /// <summary>
        /// マップのオブジェクトを生成
        /// </summary>
        public void Initialize()
        {
            if (mapBlocks.Count > 0)    //マップの状態があった場合は生成しない
                return;
            for (int y = 0; y < mapChip.GetLength(0); y++)          //マップのY軸
            {
                for (int x = 0; x < mapChip.GetLength(1); x++)      //マップのX軸
                {
                    Cube c;
                    switch (mapChip[y, x])
                    {
                        case (int)MapDef.BlockDef.Wall:
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 2, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, 2.0f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetColor(new Color(80, 40, 10));
                            mapBlocks.Add(c);
                            break;
                        case (int)MapDef.BlockDef.Space:
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, 0.1f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetColor(new Color(10, 10, 10));
                            mapBlocks.Add(c);
                            break;
                        case (int)MapDef.BlockDef.Entry:
                            entryPoint = new Point(x, y);
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, 0.1f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetColor(new Color(0, 60, 60));
                            mapBlocks.Add(c);
                            break;
                        case (int)MapDef.BlockDef.Exit:
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, 1.5f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetColor(new Color(60, 0, 0));
                            mapBlocks.Add(c);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 入口の配列座標
        /// </summary>
        public Point EntryPoint
        {
            get { return entryPoint; }
        }

        /// <summary>
        /// 描画の中心位置を設定
        /// </summary>
        /// <param name="worldPosition"></param>
        public void FocusCenter(Vector3 worldPosition)
        {
            int x = (int)((MapDef.TILE_SIZE / 2.0f + worldPosition.X) / MapDef.TILE_SIZE);
            int z = (int)((MapDef.TILE_SIZE / 2.0f + worldPosition.Z) / MapDef.TILE_SIZE);

            currentPosition.X = x;
            currentPosition.Y = z;
            ClampPoint(ref currentPosition.X, ref currentPosition.Y);          //エラーがないように配列の中に納める
        }

        /// <summary>
        /// 描画領域の更新
        /// </summary>
        public void Update()
        {
            if (previousPosition == currentPosition)    //描画領域変わってなかったら更新する必要がない
                return;

            previousPosition = currentPosition;         //前フレームのPositionを設定
            mapBlocksToDraw.Clear();

            ClampPoint(ref currentPosition.X, ref currentPosition.Y);                       //エラーがないように配列の中に納める
            for (int y = currentPosition.Y - radius; y <= currentPosition.Y + radius; y++)        //Y軸
            {
                if (y < 0 || y > mapChip.GetLength(0) - 1)                          //マップのサイズ外はスキップ
                    continue;
                for (int x = currentPosition.X - radius; x <= currentPosition.X + radius; x++)    //X軸
                {
                    if (x < 0 || x > mapChip.GetLength(1) - 1)                      //マップのサイズ外はスキップ
                        continue;
                    int index = y * mapChip.GetLength(1) + x;                       //Add順でIndexの計算
                    mapBlocksToDraw.Add(mapBlocks[index]);                          //描画部分に追加
                }
            }
        }

        /// <summary>
        /// 配列の長さを超えないように設定
        /// </summary>
        /// <param name="x">X ユニット</param>
        /// <param name="y">Y ユニット</param>
        private void ClampPoint(ref int x, ref int y)
        {
            y = (y < 0) ? 0 : y;
            y = (y >= mapChip.GetLength(0)) ? mapChip.GetLength(0) - 1 : y;
            x = (x < 0) ? 0 : x;
            x = (x >= mapChip.GetLength(1)) ? mapChip.GetLength(1) - 1 : x;
        }

        /// <summary>
        /// マップチップ
        /// </summary>
        public int[,] MapChip
        {
            get { return mapChip; }
        }

        /// <summary>
        /// マップオブジェクトを消す
        /// </summary>
        public void Clear()
        {
            mapBlocks.Clear();
            mapBlocksToDraw.Clear();
        }

        /// <summary>
        /// Debug暫定　Todo：PlayerをCharacterに置き換える
        /// </summary>
        /// <param name="player"></param>
        public void MapCollision(Player player)
        {
            int x = (int)((MapDef.TILE_SIZE / 2.0f + player.Position.X) / MapDef.TILE_SIZE);
            int z = (int)((MapDef.TILE_SIZE / 2.0f + player.Position.Z) / MapDef.TILE_SIZE);

            ClampPoint(ref x, ref z);

            for (int mapchipZ = z - 1; mapchipZ <= z + 1; mapchipZ++)
            {
                if (mapchipZ < 0 || mapchipZ > mapChip.GetLength(0) - 1)
                    continue;
                for (int mapchipX = x - 1; mapchipX <= x + 1; mapchipX++)
                {
                    if (mapchipX < 0 || mapchipX > mapChip.GetLength(1) - 1)
                        continue;
                    int index = mapchipZ * mapChip.GetLength(1) + mapchipX;
                    if (player.Collision.IsCollision(mapBlocks[index].Collision))
                    {
                        player.Collision.Hit(mapBlocks[index].Collision);
                    }
                }
            }
        }

        /// <summary>
        /// マップを描画
        /// </summary>
        public void Draw()
        {
            foreach (Cube c in mapBlocksToDraw)     //描画領域しか描画しない
            {
                c.Draw();
            }
        }
    }
}
