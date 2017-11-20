//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.19
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
    class DungeonMap
    {
        private GameDevice gameDevice;

        private List<Cube> mapBlocks;       //モデルに変更可
        private int[,] mapChip;             //マップチップ

        public DungeonMap(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;

            mapChip = new int[1, 1];
            mapBlocks = new List<Cube>();
        }

        public DungeonMap(int[,] mapChip, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;

            this.mapChip = mapChip;
            mapBlocks = new List<Cube>();
        }

        public void Initialize()
        {
            if (mapBlocks.Count > 0)
                return;
            for (int i = 0; i < mapChip.GetLength(0); i++)
            {
                for (int j = 0; j < mapChip.GetLength(1); j++)
                {
                    if (mapChip[i, j] == 0)
                    {
                        Cube c = new Cube(
                            new Vector3(j * MapDef.TILE_SIZE, 0, i * MapDef.TILE_SIZE),
                            new Vector3(MapDef.TILE_SIZE / 2.0f, 2.0f, MapDef.TILE_SIZE / 2.0f),
                            gameDevice);
                        c.SetColor(Color.Chocolate);
                        mapBlocks.Add(c);
                    }
                    else if (mapChip[i, j] == 1)
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
        }

        /// <summary>
        /// マップを描画
        /// </summary>
        public void Draw()
        {
            foreach (Cube c in mapBlocks)
            {
                c.Draw();
            }
        }
    }
}
