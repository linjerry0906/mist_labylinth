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

        private Timer limitTime;
        private int floor;

        private int stageSize;

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
            stageSize = 20;

            nearFog = FAREST_FOG;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="limitSecond">制限時間（秒）</param>
        /// <param name="floor">階層</param>
        public void Initialize(int limitSecond, int floor, int stageSize)
        {
            limitTime = new Timer(limitSecond);
            limitTime.Initialize();
            this.floor = floor;
            this.stageSize = stageSize;
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

        public void DrawLimitTime()
        {
            int min = (int)CurrentTime() / 60;
            int sec = (int)CurrentTime() - min * 60;
            string timeString = string.Format("{0:00} : {1:00}", min, sec);
            renderer.Begin();
            renderer.DrawString(
                timeString,
                new Vector2(Def.WindowDef.WINDOW_WIDTH / 2, 80),
                new Color(1, 0.2f, 0.2f),
                new Vector2(2, 2),
                0.8f, true, true);
            renderer.End();
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
        /// 次の階層
        /// </summary>
        public void NextFloor()
        {
            floor++;
            stageSize += 5;
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
