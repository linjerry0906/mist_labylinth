//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.12
// 内容  ：マップブロックのテクスチャーを定義するクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Map
{
    class BlockStyle
    {
        private Dictionary<MapDef.BlockDef, string> blockStyle;

        public BlockStyle()
        {
            blockStyle = new Dictionary<MapDef.BlockDef, string>();
        }

        /// <summary>
        /// 資料をクリア
        /// </summary>
        public void Clear()
        {
            blockStyle.Clear();
        }

        /// <summary>
        /// Style追加
        /// </summary>
        /// <param name="blockType">ブロックのタイプ</param>
        /// <param name="textureName">texture名</param>
        public void Add(MapDef.BlockDef blockType, string textureName)
        {
            if (blockStyle.ContainsKey(blockType))
                return;

            blockStyle.Add(blockType, textureName);
        }

        /// <summary>
        /// 定義内容を取得
        /// </summary>
        /// <returns></returns>
        public Dictionary<MapDef.BlockDef, string> GetDefination()
        {
            return blockStyle;
        }
    }
}
