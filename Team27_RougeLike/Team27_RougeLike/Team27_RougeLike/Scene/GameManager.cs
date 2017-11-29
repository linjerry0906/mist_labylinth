//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29
// 内容  ：シーンの間にゲーム情報を伝えるクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Map;

namespace Team27_RougeLike.Scene
{
    class GameManager
    {
        //ToDo：
        //Player情報  --->Save情報
        //Save機能
        //攻略進捗    --->Save情報
        //Item関連の読み込み ---> excelか他のファイルからデータを読み込む

        private GameDevice gameDevice;

        private DungeonMap mapInstance;     //マップの実体

        /// <summary>
        /// シーンの間にゲーム情報を伝える仲介者
        /// </summary>
        /// <param name="gameDevice">ゲームディバイス</param>
        public GameManager(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            mapInstance = null;
        }

        #region Map関連
        /// <summary>
        /// マップの実体を生成
        /// </summary>
        /// <param name="mapChip">マップチップ</param>
        public void GenerateMapInstance(int[,] mapChip)
        {
            mapInstance = new DungeonMap(mapChip ,gameDevice);
        }

        /// <summary>
        /// マップを解放
        /// </summary>
        public void ReleaseMap()
        {
            mapInstance = null;
        }

        /// <summary>
        /// マップのインスタンス
        /// </summary>
        /// <returns></returns>
        public DungeonMap GetDungeonMap()
        {
            return mapInstance;
        }

        #endregion
    }
}
