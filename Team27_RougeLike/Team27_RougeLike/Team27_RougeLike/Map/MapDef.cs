//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17 ~ 2017.11.27
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Map
{
    static class MapDef
    {
        //マップチップ一つのサイズ
        public static readonly float TILE_SIZE = 20;

        //部屋生成する時にランダムサイズの最大最小値
        public static readonly int MAX_ROOM_SIZE = 3;
        public static readonly int MIN_ROOM_SIZE = 1;

        //ブロックの定義
        public enum BlockDef
        {
            Wall = 0,
            Space = 1,
            Entry = 2,
            Exit = 3,
        }
    }
}
