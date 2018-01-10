//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2018.1.10
// 内容　：ダンジョン選択のUI
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Scene.Town;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.UI
{

    class DungeonSelectUI
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private InputState input;
        private Renderer renderer;

        private Window backLayer;                       //背景レイヤー
        private List<Button> dungeons;
        private Button[] buttons;
        private List<StageInfo> stageInfo;

        private readonly float LIMIT_ALPHA = 0.5f;      //背景Alphaの最大値 

        public DungeonSelectUI(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;

            backLayer = new Window(
                gameDevice,
                new Vector2(Def.WindowDef.WINDOW_WIDTH / 2 - 400, Def.WindowDef.WINDOW_HEIGHT / 2 - 200),
                new Vector2(800, 400));
            backLayer.Initialize();         //初期化
            backLayer.SetAlphaLimit(LIMIT_ALPHA);
            backLayer.Switch();             //開く
        }

        /// <summary>
        /// UIの更新処理
        /// </summary>
        public void Update()
        {
            backLayer.Update();

            if (!input.IsLeftClick())
                return;

            Vector2 mosPos = input.GetMousePosition();
        }

        /// <summary>
        /// 終わっているか
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return backLayer.IsEnd();
        }

        /// <summary>
        /// UIを閉じる
        /// </summary>
        public void SwitchOff()
        {
            backLayer.Switch();
        }

        /// <summary>
        /// 描画する
        /// </summary>
        public void Draw()
        {
            float constractAlpha = 1.0f / LIMIT_ALPHA;
            backLayer.Draw();

            renderer.DrawString(
                "どのダンジョンへ冒険するか",
                backLayer.GetOffsetPosition() + new Vector2(5, 5),
                new Vector2(1.1f, 1.1f), Color.White, backLayer.CurrentAlpha() * constractAlpha);


            DrawDungeons(constractAlpha);
        }

        private void DrawDungeons(float constractAlpha)
        {
            renderer.DrawTexture("fade",
                backLayer.GetOffsetPosition() + new Vector2(5, 5),
                new Vector2(280, 390),
                backLayer.CurrentAlpha() * 0.5f);

            for (int i = 0; i < stageInfo.Count; i++)
            {
                renderer.DrawString(
                    stageInfo[i].name,
                    backLayer.GetOffsetPosition() + new Vector2(5, 25 * i + 30),
                    new Vector2(1.1f, 1.1f), Color.White, backLayer.CurrentAlpha() * constractAlpha);
            }

        }

        public void SetStageInfo(List<StageInfo> info)
        {
            this.stageInfo = info;
        }
    }
}
