//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29
// 内容  ：ステージマネージャー
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Utility;

namespace Team27_RougeLike.Scene
{
    class StageManager
    {
        private GameDevice gameDevice;
        private Renderer renderer;
        private FogManager fogManager;

        private string dungeonName;     //ダンジョンの名
        private int dungeonNum;         //ダンジョンの番号
        private Timer limitTime;        //制限時間
        private int floor;              //今の階層
        private int bossRange;          //何階ごとにボス

        private int stageSize;          //今のサイズ
        private int expandRate;         //拡大の比率

        private static readonly float FAREST_FOG = 100;
        private static readonly float NEAREST_FOG = -50;
        private static readonly float FOG_RANGE = 70;
        private float nearFog;

        public StageManager(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            renderer = gameDevice.Renderer;
            fogManager = renderer.FogManager;

            limitTime = new Timer(5 * 60);
            limitTime.Initialize();
            floor = 1;
            bossRange = 5;
            stageSize = 20;

            nearFog = FAREST_FOG;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="dungeonNum">ダンジョンの番号</param>
        /// <param name="dungeonName">ダンジョン名</param>
        /// <param name="limitSecond">攻略の制限時間</param>
        /// <param name="floor">階層目</param>
        /// <param name="totalFloor">総階層</param>
        /// <param name="bossRange">Boss出る階層</param>
        /// <param name="stageSize">ダンジョンのサイズ</param>
        /// <param name="expandRate">拡大の比率</param>
        /// <param name="fogColor">霧の色</param>
        public void Initialize(int dungeonNum, string dungeonName, 
            int limitSecond, int floor, int totalFloor, int bossRange, int stageSize, int expandRate,
            Vector3 fogColor)
        {
            this.dungeonName = dungeonName;
            this.dungeonNum = dungeonNum;
            limitTime = new Timer(limitSecond);
            limitTime.Initialize();
            this.floor = floor;
            this.stageSize = stageSize;
            this.bossRange = bossRange;
            this.expandRate = expandRate;
            fogManager.SetColor(fogColor);
        }

        /// <summary>
        /// 時間や霧の更新
        /// </summary>
        public void Update()
        {
            limitTime.Update();

            //Fog
            nearFog = limitTime.Rate() * (FAREST_FOG - NEAREST_FOG);
            fogManager.SetNear(nearFog);
            fogManager.SetFar(nearFog + FOG_RANGE);
            renderer.StartFog();
        }

        /// <summary>
        /// Bossシーン終わる時に晴れる
        /// </summary>
        public void RemoveFog()
        {
            if (nearFog >= FAREST_FOG)
                return;

            nearFog += 1;
            fogManager.SetNear(nearFog);
            fogManager.SetFar(nearFog + FOG_RANGE);
            renderer.StartFog();
        }

        /// <summary>
        /// 今の時間（UI描画や記録用）
        /// </summary>
        /// <returns></returns>
        public float CurrentTime()
        {
            return limitTime.Now() / 60.0f;
        }

        /// <summary>
        /// 残り時間を表示
        /// </summary>
        public void DrawLimitTime()
        {
            int min = (int)CurrentTime() / 60;
            int sec = (int)CurrentTime() - min * 60;
            string timeString = string.Format("{0:00} : {1:00}", min, sec);
            renderer.DrawString(
                timeString,
                new Vector2(Def.WindowDef.WINDOW_WIDTH / 2, 80),
                new Color(1, 0.2f, 0.2f),
                new Vector2(2, 2),
                0.8f, true, true);
        }

        /// <summary>
        /// ダンジョンの進行状況を描画
        /// </summary>
        public void DrawDungeonInfo()
        {
            renderer.DrawString(
                dungeonName,
                new Vector2(40, 60),
                Color.WhiteSmoke,
                new Vector2(1.4f, 1.4f),
                0.8f, false, true);

            renderer.DrawString(
                "地下" + floor + "階",
                new Vector2(40, 100),
                Color.WhiteSmoke,
                new Vector2(1.2f, 1.2f),
                0.8f, false, true);
        }

        /// <summary>
        /// ステージの時間切れか
        /// </summary>
        /// <returns></returns>
        public bool IsTime()
        {
            return limitTime.IsTime();
        }

        /// <summary>
        /// 現在の階層（UI描画や記録用）
        /// </summary>
        /// <returns></returns>
        public int CurrentFloor()
        {
            return floor;
        }

        /// <summary>
        /// ダンジョンの番号
        /// </summary>
        /// <returns></returns>
        public int CurrentDungeonNum()
        {
            return dungeonNum;
        }

        /// <summary>
        /// ダンジョンの名前
        /// </summary>
        /// <returns></returns>
        public string DungeonName()
        {
            return dungeonName;
        }

        /// <summary>
        /// Boss部屋か？
        /// </summary>
        /// <returns></returns>
        public bool IsBoss()
        {
            return floor % bossRange == 0;
        }

        /// <summary>
        /// 次の階層
        /// </summary>
        public void NextFloor()
        {
            floor++;
            stageSize += expandRate;         //Sizeを拡大
        }

        /// <summary>
        /// ステージ生成時用（マップの大きさ）
        /// </summary>
        /// <returns></returns>
        public int StageSize()
        {
            return stageSize;
        }
    }
}
