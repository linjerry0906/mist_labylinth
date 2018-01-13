//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.10
// 内容  ：攻略状況を記録するクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class DungeonProcess
    {
        private static Dictionary<int, int> dungeonProcess = new Dictionary<int, int>();

        public DungeonProcess()
        {
        }

        /// <summary>
        /// 記録更新
        /// </summary>
        /// <param name="dungeonNo">ダンジョンの番号</param>
        /// <param name="floor">階層</param>
        public void UpdateProcess(int dungeonNo, int floor)
        {
            if (!dungeonProcess.ContainsKey(dungeonNo))     //記録がなかったら追加
            {
                dungeonProcess.Add(dungeonNo, floor);
                return;
            }

            if(dungeonProcess[dungeonNo] < floor)           //記録があったら比較して追加
                dungeonProcess[dungeonNo] = floor;
        }

        /// <summary>
        /// 攻略状況を取得
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetProcess()
        {
            return dungeonProcess;
        }

        /// <summary>
        /// 攻略したことあるか
        /// </summary>
        /// <param name="dungeonNum"></param>
        /// <returns></returns>
        public bool HasKey(int dungeonNum)
        {
            return dungeonProcess.ContainsKey(dungeonNum);
        }

        /// <summary>
        /// セーブから読み込む
        /// </summary>
        /// <param name="save">セーブデータ</param>
        public void LoadSaveData(SaveData save)
        {
            dungeonProcess = save.GetClearDungen();
        }
    }
}
