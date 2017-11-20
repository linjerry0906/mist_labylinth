//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.17
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
        public static readonly float TILE_SIZE = 3;

        //部屋生成する時にランダムサイズの最大最小値
        public static readonly int MAX_ROOM_SIZE = 6;
        public static readonly int MIN_ROOM_SIZE = 1;
    }
}
