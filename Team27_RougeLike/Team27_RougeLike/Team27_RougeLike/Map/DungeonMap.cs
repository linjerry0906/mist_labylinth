//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.19 ~ 2017.11.21
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input; //Debug
using Team27_RougeLike.Object;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    class DungeonMap
    {
        private GameDevice gameDevice;      //Debug可視化用

        private List<Cube> mapBlocks;       //モデルに変更可
        private int[,] mapChip;             //マップチップ

        private List<Cube> mapBlocksToDraw; //描画するマップ
        private Point position;             //描画中心座標
        private int radius = 10;            //描画半径

        public DungeonMap(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;

            mapChip = new int[1, 1];
            mapBlocks = new List<Cube>();
            mapBlocksToDraw = new List<Cube>();

            position = new Point(0, 0);
        }

        public DungeonMap(int[,] mapChip, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;

            this.mapChip = mapChip;
            mapBlocks = new List<Cube>();
            mapBlocksToDraw = new List<Cube>();

            position = new Point(0, 0);
        }

        /// <summary>
        /// マップのオブジェクトを生成
        /// </summary>
        public void Initialize()
        {
            if (mapBlocks.Count > 0)    //マップの状態があった場合は生成しない
                return;
            for (int i = 0; i < mapChip.GetLength(0); i++)          //マップのY軸
            {
                for (int j = 0; j < mapChip.GetLength(1); j++)      //マップのX軸
                {
                    if (mapChip[i, j] == 0)             //TODO：未定義ブロック　0：壁
                    {
                        Cube c = new Cube(
                            new Vector3(j * MapDef.TILE_SIZE, 0, i * MapDef.TILE_SIZE),
                            new Vector3(MapDef.TILE_SIZE / 2.0f, 4.0f, MapDef.TILE_SIZE / 2.0f),
                            gameDevice);
                        c.SetColor(Color.Chocolate);
                        mapBlocks.Add(c);
                    }
                    else if (mapChip[i, j] == 1)        //TODO：未定義ブロック　1：地面
                    {
                        Cube c = new Cube(
                            new Vector3(j * MapDef.TILE_SIZE, 0, i * MapDef.TILE_SIZE),
                            new Vector3(MapDef.TILE_SIZE / 2.0f, 0.1f, MapDef.TILE_SIZE / 2.0f),
                            gameDevice);
                        mapBlocks.Add(c);
                    }
                }
            }
        }

        /// <summary>
        /// 描画の中心位置を設定
        /// </summary>
        /// <param name="worldPosition"></param>
        public void FocusCenter(Vector3 worldPosition)
        {
            int x = (int)((MapDef.TILE_SIZE / 2.0f + worldPosition.X) / MapDef.TILE_SIZE);
            int z = (int)((MapDef.TILE_SIZE / 2.0f + worldPosition.Z) / MapDef.TILE_SIZE);

            position.X = x;
            position.Y = z;
            ClampFocusPoint();
        }

        /// <summary>
        /// 描画領域の更新
        /// </summary>
        public void Update()
        {
            mapBlocksToDraw.Clear();

            for (int y = position.Y - radius; y <= position.Y + radius; y++)        //Y軸
            {
                if (y < 0 || y > mapChip.GetLength(0) - 1)                          //マップのサイズ外はスキップ
                    continue;
                for (int x = position.X - radius; x <= position.X + radius; x++)    //X軸
                {
                    if (x < 0 || x > mapChip.GetLength(1) - 1)                      //マップのサイズ外はスキップ
                        continue;
                    int index = y * mapChip.GetLength(1) + x;                       //Add順でIndexの計算
                    mapBlocksToDraw.Add(mapBlocks[index]);                          //描画部分に追加
                }
            }

            ClampFocusPoint();      //配列を中に納める
        }

        /// <summary>
        /// Debug移動用
        /// </summary>
        private void Move()
        {
            if (gameDevice.InputState.GetKeyTrigger(Keys.Up))
            {
                position.Y--;
            }
            else if (gameDevice.InputState.GetKeyTrigger(Keys.Down))
            {
                position.Y++;
            }
            else if (gameDevice.InputState.GetKeyTrigger(Keys.Right))
            {
                position.X++;
            }
            else if (gameDevice.InputState.GetKeyTrigger(Keys.Left))
            {
                position.X--;
            }
        }

        /// <summary>
        /// 配列の長さを超えないように設定
        /// </summary>
        private void ClampFocusPoint()
        {
            position.Y = (position.Y < 0) ? 0 : position.Y;
            position.Y = (position.Y >= mapChip.GetLength(0)) ? mapChip.GetLength(0) - 1 : position.Y;
            position.X = (position.X < 0) ? 0 : position.X;
            position.X = (position.X >= mapChip.GetLength(1)) ? mapChip.GetLength(1) - 1 : position.X;
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
