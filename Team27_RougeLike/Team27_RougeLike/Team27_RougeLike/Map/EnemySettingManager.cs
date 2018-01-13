//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.13
// 内容  ：Enemyの配置を持つクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    public struct EnemySetting
    {
        public int[] ids;               //生成する敵ID
        public int rate;                //間隔
        public int max;                 //最大数
        public int amountPerSpawn;      //一回ごとの数量

        public bool isBoss;             //ボス部屋か
    }

    class EnemySettingManager
    {
        private GameDevice gameDevice;
        private static List<EnemySetting> settings = new List<EnemySetting>();

        public EnemySettingManager(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
        }

        /// <summary>
        /// 既存の配置設定を削除
        /// </summary>
        public void Clear()
        {
            settings.Clear();
        }

        /// <summary>
        /// 配置追加
        /// </summary>
        /// <param name="setting">配置設定</param>
        public void Add(EnemySetting setting)
        {
            settings.Add(setting);
        }

        /// <summary>
        /// 配置を取得
        /// </summary>
        /// <returns></returns>
        public EnemySetting RandomSpawnerSetting()
        {
            int index = 0;
            if (settings.Count > 1)
            {
                index = gameDevice.Random.Next(0, settings.Count);
            }

            return settings[index];
        }

        /// <summary>
        /// Bossの設定を取得
        /// </summary>
        public EnemySetting BossSetting
        {
            get { return settings[0]; }
        }
    }
}
