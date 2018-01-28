//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29
// 内容  ：ダンジョンシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Map;
using Team27_RougeLike.Object;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.UI;
using Team27_RougeLike.Object.ParticleSystem;

namespace Team27_RougeLike.Scene
{
    class DungeonScene : IScene
    {
        private GameDevice gameDevice;          //Device系をまとめたクラス
        private Renderer renderer;              //レンダラー
        private GameManager gameManager;        //シーンの間に情報を渡す機能のクラス
        private StageManager stageManager;      //ステージ管理者
        private int currentFloor;               //現在のフロア（描画用）
        private CharacterManager characterManager;
        private bool endFlag;                   //終了フラグ
        private SceneType nextScene;            //次のシーン

        private DungeonMap map;                 //マップ
        private MapItemManager mapItemManager;  //マップ内に落ちているアイテムの管理者
        private FogBackground background;       //背景の霧

        private float angle = 0;                //カメラ回転角度

        private DungeonUI ui;                   //Popメッセージ

        private ParticleManager pManager;

        public DungeonScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
            stageManager = gameManager.StageManager;

            characterManager = new CharacterManager(gameDevice, gameManager);
            pManager = new ParticleManager(gameDevice);
        }

        public void Draw()
        {
            renderer.EffectManager.GetBloomEffect().WriteRendererTarget(renderer.FogManager.CurrentColor());

            map.Draw();                 //Mapの描画
            mapItemManager.Draw();      //アイテムの描画

            characterManager.Draw();
            pManager.Draw();

            renderer.EffectManager.GetBloomEffect().ReleaseRenderTarget();
            renderer.EffectManager.GetBloomEffect().Draw(renderer);

            background.Draw(renderer.FogManager.CurrentColor());

            map.DrawMiniMap();          //MiniMapの描画
            DrawUI();                   //UIを描画
        }

        /// <summary>
        /// UIの描画
        /// </summary>
        private void DrawUI()
        {
            renderer.Begin();

            stageManager.DrawLimitTime();       //残り時間を表示
            stageManager.DrawDungeonInfo(currentFloor);
            ui.Draw();                          //UIの描画
            gameManager.PlayerInfo.DrawUIStatue();

            renderer.End();
        }

        public void Initialize(SceneType lastScene)
        {
            endFlag = false;                        //終了フラグ初期化
            nextScene = SceneType.LoadMap;
            if (lastScene == SceneType.Pause)       //Pauseから来た場合は以下のもの初期化しない
                return;

            pManager.Initialize();
            gameDevice.MainProjector.SetRelativePosition(new Vector3(0, 6, 12f));
            currentFloor = stageManager.CurrentFloor();

            #region Map初期化
            map = gameManager.GetDungeonMap();      //生成したマップを取得
            if (map == null)                         //エラー対策　マップが正常に生成されてなかったらLoadingに戻る
            {
                nextScene = SceneType.LoadMap;
                endFlag = true;
                return;
            }
            map.Initialize(gameManager.BlockStyle);                       //マップを初期化
            map.SetExitColor(stageManager.ConstactColor());
            #endregion

            #region Item初期化
            mapItemManager = new MapItemManager(gameManager, gameDevice);
            mapItemManager.Initialize();
            int itemAmount = stageManager.CurrentFloor() / 10 + stageManager.StageSize() / 10;    //初期落ちているアイテムの数
            itemAmount = gameDevice.Random.Next(0, itemAmount + 1);
            for (int i = 0; i < itemAmount; i++)
            {
                Vector3 randomSpace = map.RandomSpace();
                if (randomSpace == Vector3.Zero)                    //Error対策
                    break;

                mapItemManager.AddItemByPossibility(randomSpace, 0.9f, 0.3f);
            }
            #endregion

            ///飯泉より　キャラクターマネージャのinitの前にずらしました
            ui = new DungeonUI(gameManager, gameDevice);

            Vector3 position = new Vector3(
                map.EntryPoint.X * MapDef.TILE_SIZE,
                MapDef.TILE_SIZE / 2,
                map.EntryPoint.Y * MapDef.TILE_SIZE);
            characterManager.Initialize(ui, mapItemManager);
            characterManager.AddPlayer(position, pManager, gameManager);
            //var d = new int[2];
            //d[0] = 3;
            //d[1] = 2;
            ////配列を渡せばその中からランダムで、ＩＤ単体を渡せばそれのみをスポーンさせます
            //characterManager.AddSpawner(new Spawner(500, characterManager.GetPlayer().Position, d, 10,1, characterManager));
            //characterManager.AddSpawner(new Spawner(500, characterManager.GetPlayer().Position, 1, 10, 1, characterManager));

            int spawnerAmount =
                stageManager.StageSize() / 3 +              //サイズ補正
                stageManager.CurrentFloor() / 5 +          //Floor補正
                stageManager.CurrentDungeonNum() / 2;   　  //ダンジョンの難易度補正

            GenerateSpawner(spawnerAmount);
            #region カメラ初期化
            angle = 0;
            gameDevice.MainProjector.Initialize(characterManager.GetPlayer().GetPosition);       //カメラを初期化
            #endregion


            background = new FogBackground(gameDevice);
        }

        /// <summary>
        /// Spawnerを初期化
        /// </summary>
        /// <param name="amount">量</param>
        private void GenerateSpawner(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector3 randomSpace = map.RandomSpace();
                if (randomSpace == Vector3.Zero)                    //Error対策
                    break;

                EnemySetting setting = gameManager.EnemySetting.RandomSpawnerSetting();
                if (setting.ids.Length > 1)                         //配列の場合
                {
                    Spawner spawner = new Spawner(
                            setting.rate, randomSpace, setting.ids,
                            setting.max, setting.amountPerSpawn, characterManager);
                    spawner.InitCurrentTime();
                    characterManager.AddSpawner(spawner);
                }
                else
                {
                    Spawner spawner = new Spawner(
                            setting.rate, randomSpace, setting.ids[0],
                            setting.max, setting.amountPerSpawn, characterManager);
                    spawner.InitCurrentTime();
                    characterManager.AddSpawner(spawner);
                }
            }
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return nextScene;
        }

        public void ShutDown()
        {
            if (nextScene == SceneType.Pause)       //次のシーンがPauseだったら以下のものShutdownしない
                return;

            map.Clear();                            //マップ解放
            map = null;
            gameManager.ReleaseMap();

            mapItemManager.Initialize();            //Item解放
            mapItemManager = null;

            gameManager.EnemySetting.Clear();       //SpawnSettingを削除

            ui = null;
        }

        public void Update(GameTime gameTime)
        {
            if (endFlag)
                return;

            ui.Update();
            if (ui.IsPop())                   //メッセージ表示中は以下Updateしない
                return;

            background.Update();
            RotateCamera();

            //Chara処理
            characterManager.Update(gameTime);

            pManager.Update(gameTime);

            //マップ処理
            map.MapCollision(gameDevice.Renderer.MainProjector);
            map.FocusCenter(characterManager.GetPlayer().GetPosition);
            map.Update();
            map.MapCollision(characterManager.GetPlayer());
            map.MapCollision(characterManager.GetCharacters());
            map.MapCollision(characterManager.GetHitBoxs());

            //アイテム処理
            mapItemManager.ItemCollision(characterManager.GetPlayer(), ui);

            //時間やFog処理の更新
            stageManager.Update();

            AddParticle();

            //Camera Shake仮実装 ToDo:Class化
            if (gameDevice.InputState.IsLeftClick())
            {
                Vector3 offset = new Vector3(
                    gameDevice.Random.Next(-10, 10) / 50.0f,
                    gameDevice.Random.Next(-10, 10) / 50.0f,
                    gameDevice.Random.Next(-10, 10) / 50.0f);
                gameDevice.MainProjector.Collision.Position += offset;
            }

            CheckEnd();                         //プレイ終了をチェック
        }

        private void AddParticle()
        {
            //出口の粒子
            int tileSize = (int)MapDef.TILE_SIZE;
            for (int i = 0; i < 1; i++)
            {
                Vector3 position = new Vector3(
                    map.EndPoint.X * tileSize, 
                    0, 
                    map.EndPoint.Y * tileSize);
                position += new Vector3(
                    gameDevice.Random.Next(-tileSize * 45, tileSize * 45 + 1) / 100.0f,
                    0,
                    gameDevice.Random.Next(-tileSize * 45, tileSize * 45 + 1) / 100.0f);
                position.Y = tileSize / 2;
                pManager.AddParticle(new TransportParticle(position, stageManager.ConstactColor(), gameDevice));
            }

            //以下は浮遊粒子
            if (!stageManager.UseParticle())
                return;

            if (pManager.Count() < 2000)
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector3 position = characterManager.GetPlayer().GetPosition;
                    position += new Vector3(
                        gameDevice.Random.Next(-30000, 30001) / 100.0f,
                        0,
                        gameDevice.Random.Next(-30000, 30001) / 100.0f);
                    position.Y = tileSize / 2;
                    pManager.AddParticle(new SphereParticle(position, stageManager.ConstactColor(), gameDevice));
                }
            }
        }

        /// <summary>
        /// カメラの回転
        /// </summary>
        private void RotateCamera()
        {
            if (gameDevice.InputState.GetKeyState(Keys.Q))
            {
                angle += 1.5f;
                angle = (angle > 360) ? angle - 360 : angle;
            }
            else if (gameDevice.InputState.GetKeyState(Keys.E))
            {
                angle -= 1.5f;
                angle = (angle < 0) ? angle + 360 : angle;
            }
            gameDevice.MainProjector.Rotate(angle);
        }

        /// <summary>
        /// シーンを変えるかのチェック
        /// </summary>
        private void CheckEnd()
        {
#if DEBUG
            if (gameDevice.InputState.GetKeyTrigger(Keys.N))
            {
                endFlag = true;
                nextScene = SceneType.LoadMap;
                gameManager.UpdateDungeonProcess();     //攻略状況更新
                stageManager.NextFloor();
                if (stageManager.IsBoss())
                    nextScene = SceneType.LoadBoss;
                return;
            }
#endif

            //Pause機能
            if (gameDevice.InputState.GetKeyTrigger(Keys.P))
            {
                endFlag = true;
                nextScene = SceneType.Pause;
            }

            //死んだ時
            if (characterManager.GetPlayer().IsDead())
            {
                gameManager.PlayerItem.RemoveAll();
                endFlag = true;
                nextScene = SceneType.LoadTown;
            }

            //時間になったら村に戻される
            if (stageManager.IsTime())
            {
                gameManager.PlayerItem.RemoveTempItem();
                endFlag = true;
                nextScene = SceneType.LoadTown;
                gameManager.Save();
                return;
            }

            //階段にたどり着いた場合
            if (map.WorldToMap(characterManager.GetPlayer().GetPosition) == map.EndPoint)
            {
                //ヒント文字を出す
                ui.HintUI.Switch(true);
                ui.HintUI.SetMessage("次へ：Space");
                if (!ui.HintUI.IsPush(Keys.Space))
                    return;

                //次へ行く処理
                endFlag = true;
                nextScene = SceneType.LoadMap;
                gameManager.UpdateDungeonProcess();     //攻略状況更新
                stageManager.NextFloor();
                if (stageManager.IsBoss())
                    nextScene = SceneType.LoadBoss;
                return;
            }
        }
    }
}