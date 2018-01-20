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
        public enum DungeonSelectButtonEnum
        {
            村 = 0,
            ダンジョン,
            NULL,
        }

        private GameDevice gameDevice;
        private GameManager gameManager;
        private InputState input;
        private Renderer renderer;

        private Window backLayer;                       //背景レイヤー

        private List<Button> dungeons;                  //ダンジョンのボタン
        private int dungeonIndex;                       //選択したダンジョン
        private List<Button> floors;                    //選択したダンジョンのフロア
        private int floorIndex;                         //選択したフロア

        private Button[] buttons;                       //クリックできるボタン
        private DungeonSelectButtonEnum choose;         //クリックしたボタン
        private List<StageInfo> stageInfo;              //Stage情報

        private readonly float LIMIT_ALPHA = 0.5f;      //背景Alphaの最大値 
        private readonly Vector2 DUNGEON_OFFSET = 
            new Vector2(Def.WindowDef.WINDOW_WIDTH / 2 - 360, Def.WindowDef.WINDOW_HEIGHT / 2 - 180);
        private readonly Vector2 FLOOR_OFFSET =
           new Vector2(Def.WindowDef.WINDOW_WIDTH / 2 + 80, Def.WindowDef.WINDOW_HEIGHT / 2 - 180);

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

            buttons = new Button[(int)DungeonSelectButtonEnum.NULL];
            buttons[(int)DungeonSelectButtonEnum.村] = new Button(backLayer.GetCenterUnder() + new Vector2(-200, -35), 140, 30);
            buttons[(int)DungeonSelectButtonEnum.ダンジョン] = new Button(backLayer.GetCenterUnder() + new Vector2(60, -35), 140, 30);

            choose = DungeonSelectButtonEnum.NULL;
        }

        /// <summary>
        /// UIの更新処理
        /// </summary>
        public void Update()
        {
            backLayer.Update();

            if (!input.IsLeftClick() || !backLayer.CurrentState())
                return;

            Vector2 mosPos = input.GetMousePosition();
            CheckDungeonButton(mosPos);
            CheckFloorButton(mosPos);
            CheckButtons(mosPos);
        }

        /// <summary>
        /// ダンジョン名がクリックされているかをチェック
        /// </summary>
        /// <param name="mosPos">マウスの位置</param>
        private void CheckDungeonButton(Vector2 mosPos)
        {
            int index = 0;
            foreach (Button b in dungeons)
            {
                if (b.IsClick(new Point((int)mosPos.X, (int)mosPos.Y)))
                {
                    dungeonIndex = index;
                    InitFloor();
                    return;
                }
                index++;
            }
        }

        private void CheckFloorButton(Vector2 mosPos)
        {
            int index = 0;
            foreach (Button b in floors)
            {
                if (b.IsClick(new Point((int)mosPos.X, (int)mosPos.Y)))
                {
                    floorIndex = index;
                    return;
                }
                index++;
            }
        }

        /// <summary>
        /// ボタンが押されていたかをチェック
        /// </summary>
        /// <param name="mosPos">マウスの位置</param>
        private void CheckButtons(Vector2 mosPos)
        {
            DungeonSelectButtonEnum index = 0;
            foreach (Button b in buttons)
            {
                if (b.IsClick(new Point((int)mosPos.X, (int)mosPos.Y)))
                {
                    SwitchOff();
                    choose = index;
                    return;
                }
                index++;
            }
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

            DrawDungeons(constractAlpha);
            DrawFloors(constractAlpha);
            DrawButtons(constractAlpha);
        }

        /// <summary>
        /// ダンジョンイメージを背景に描画
        /// </summary>
        public void DrawBackGround()
        {
            float constractAlpha = 1.0f / LIMIT_ALPHA;
            renderer.DrawTexture(stageInfo[dungeonIndex].imageName, Vector2.Zero, backLayer.CurrentAlpha() * constractAlpha);
            renderer.DrawTexture(
                "white", 
                Vector2.Zero, 
                new Vector2(Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT), 
                backLayer.CurrentAlpha() * 0.5f);
        }

        /// <summary>
        /// ダンジョンの種類を表示
        /// </summary>
        /// <param name="constractAlpha">透明度</param>
        private void DrawDungeons(float constractAlpha)
        {
            renderer.DrawString(
                "どのダンジョンへ冒険するか",
                DUNGEON_OFFSET + new Vector2(10, 5),
                new Vector2(1.1f, 1.1f), Color.White, backLayer.CurrentAlpha() * constractAlpha);

            renderer.DrawTexture("fade",                                         //背景
                DUNGEON_OFFSET,
                new Vector2(280, 330),
                backLayer.CurrentAlpha() * 0.5f);

            for (int i = 0; i < stageInfo.Count; i++)                            //各ダンジョン
            {
                Vector2 buttonPos = new Vector2(dungeons[i].Position().X, dungeons[i].Position().Y);
                Color color = (i == dungeonIndex) ? Color.Gold : Color.White;    //選択の色

                renderer.DrawTexture(
                    "fade",
                    buttonPos,
                    dungeons[i].Size(),
                    backLayer.CurrentAlpha());

                renderer.DrawString(
                    stageInfo[i].name,
                    DUNGEON_OFFSET + new Vector2(10, 25 * i + 35),
                    new Vector2(1.1f, 1.1f), color, backLayer.CurrentAlpha() * constractAlpha);
            }
        }

        /// <summary>
        /// Buttonを描画
        /// </summary>
        /// <param name="constractAlpha">透明度</param>
        private void DrawButtons(float constractAlpha)
        {
            for (int i = 0; i < buttons.Length; i++)                             //各Button
            {
                Vector2 buttonPos = new Vector2(buttons[i].Position().X, buttons[i].Position().Y);

                renderer.DrawTexture(
                    "fade",
                    buttonPos,
                    buttons[i].Size(),
                    backLayer.CurrentAlpha());

                renderer.DrawString(
                    ((DungeonSelectButtonEnum)i).ToString() + " へ",
                    new Vector2(buttons[i].ButtonCenter().X, buttons[i].ButtonCenter().Y),
                    Color.White, new Vector2(1.1f, 1.1f), backLayer.CurrentAlpha() * constractAlpha,
                    true, true);
            }
        }

        private void DrawFloors(float constractAlpha)
        {
            renderer.DrawTexture("fade",                                       //背景
                FLOOR_OFFSET,
                new Vector2(280, 330),
                backLayer.CurrentAlpha() * 0.5f);

            StageInfo info = stageInfo[dungeonIndex];
            int chooseFloor = info.totalFloor / info.bossRange;

            for (int i = 0; i < floors.Count; i++)                             //各ダンジョン
            {
                Vector2 buttonPos = new Vector2(floors[i].Position().X, floors[i].Position().Y);
                Color color = (i == floorIndex) ? Color.Gold : Color.White;    //選択の色

                renderer.DrawTexture(
                    "fade",
                    buttonPos,
                    floors[i].Size(),
                    backLayer.CurrentAlpha());

                renderer.DrawString(
                    "地下 " + (1 + info.bossRange * i) + " 階",
                    FLOOR_OFFSET + new Vector2(10, 25 * i + 35),
                    new Vector2(1.1f, 1.1f), color, backLayer.CurrentAlpha() * constractAlpha);
            }
        }

        /// <summary>
        /// Stage情報を追加
        /// </summary>
        /// <param name="info">Stage情報</param>
        public void SetStageInfo(List<StageInfo> info)
        {
            this.stageInfo = info;

            dungeons = new List<Button>();
            for (int i = 0; i < info.Count; i++)        //Buttonを初期化
            {
                Vector2 position = DUNGEON_OFFSET + new Vector2(0, 25 * i + 35);
                Button button = new Button(position + new Vector2(0, 2), 280, 21);
                dungeons.Add(button);
            }

            dungeonIndex = 0;                           //指定のダンジョン
            InitFloor();
        }

        /// <summary>
        /// FloorUIを更新
        /// </summary>
        public void InitFloor()
        {
            floorIndex = 0;
            floors = new List<Button>();
            DungeonProcess process = gameManager.DungeonProcess;
            StageInfo info = stageInfo[dungeonIndex];
            int limit = 1;                                  //制限階層
            if (process.HasKey(info.dungeonNo))             //攻略したことがあれば進捗で更新
            {
                limit = process.GetProcess()[info.dungeonNo];
            }

            int chooseFloor = info.totalFloor / info.bossRange;

            for (int i = 0; i < chooseFloor; i++)           //Buttonを作る
            {
                if ((1 + info.bossRange * i) > limit + 1)   //到達した階層以外は選択できない
                    break;

                Vector2 position = FLOOR_OFFSET + new Vector2(0, 25 * i + 35);
                Button button = new Button(position + new Vector2(0, 2), 280, 21);
                floors.Add(button);
            }
        }

        /// <summary>
        /// 選択したボタン
        /// </summary>
        /// <returns></returns>
        public DungeonSelectButtonEnum Next()
        {
            return choose;
        }

        /// <summary>
        /// Stageを初期化する
        /// </summary>
        public void InitStage()
        {
            StageInfo nextStage = stageInfo[dungeonIndex];               //指定のダンジョン
            int chooseFloor = nextStage.bossRange * floorIndex + 1;      //指定のフロアを計算

            //Stage情報を入力
            gameManager.StageNum = nextStage.fileNum;
            gameManager.InitStage(
                nextStage.dungeonNo,
                nextStage.name,
                (int)nextStage.limitTime,
                chooseFloor,
                nextStage.totalFloor,
                nextStage.bossRange,
                nextStage.baseSize + (chooseFloor - 1) * nextStage.expandRate,
                nextStage.expandRate,
                nextStage.fogColor,
                nextStage.constractColor,
                nextStage.useParticle,
                nextStage.bgmName);

            //BlockStyle設定
            gameManager.ClearBlockStyle();
            gameManager.BlockStyle.Add(Map.MapDef.BlockDef.Entry, nextStage.groundTexture);
            gameManager.BlockStyle.Add(Map.MapDef.BlockDef.Exit, nextStage.groundTexture);
            gameManager.BlockStyle.Add(Map.MapDef.BlockDef.Space, nextStage.groundTexture);
            gameManager.BlockStyle.Add(Map.MapDef.BlockDef.Wall, nextStage.wallTexture);
        }
    }
}
