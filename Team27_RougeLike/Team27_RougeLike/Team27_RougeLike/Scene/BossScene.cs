﻿//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.07
// 内容  ：Bossシーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.ParticleSystem;
using Team27_RougeLike.Map;
using Team27_RougeLike.UI;
using Team27_RougeLike.Object;

namespace Team27_RougeLike.Scene
{
    class BossScene : IScene
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private StageManager stageManager;
        private Renderer renderer;
        private InputState input;

        private bool endFlag;
        private SceneType nextScene;

        private DungeonMap map;                 //マップ
        private FogBackground background;       //背景の霧
        private MapItemManager mapItemManager;  //マップ内に落ちているアイテムの管理者

        private CharacterManager characterManager;
        private ParticleManager pManager;
        private float angle;

        private DungeonUI ui;             //Popメッセージ
        private int currentFloor;         //現在フロア（描画記録用）

        public BossScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;

            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
        }

        public void Draw()
        {
            renderer.EffectManager.GetBloomEffect().WriteRendererTarget(renderer.FogManager.CurrentColor());

            map.Draw();
            mapItemManager.Draw();      //アイテムの描画

            characterManager.Draw();
            pManager.Draw();

            renderer.EffectManager.GetBloomEffect().ReleaseRenderTarget();
            renderer.EffectManager.GetBloomEffect().Draw(renderer);

            background.Draw(renderer.FogManager.CurrentColor());

            DrawUI();
        }

        public void DrawUI()
        {
            renderer.Begin();
            stageManager.DrawDungeonInfo(currentFloor);
            ui.Draw();
            gameManager.PlayerInfo.DrawUIStatue();
            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            nextScene = SceneType.LoadTown;

            if (scene == SceneType.Pause)
                return;

            stageManager = gameManager.StageManager;
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
            map.SwitchDrawExit(false);
            #endregion

            #region MapItemの初期化処理
            mapItemManager = new MapItemManager(gameManager, gameDevice);
            mapItemManager.Initialize();
            #endregion

            Vector3 position = new Vector3(
               map.EntryPoint.X * MapDef.TILE_SIZE,
               MapDef.TILE_SIZE / 2,
               map.EntryPoint.Y * MapDef.TILE_SIZE);

            pManager = new ParticleManager(gameDevice);
            pManager.Initialize();

            characterManager = new CharacterManager(gameDevice, gameManager);

            ui = new DungeonUI(gameManager, gameDevice);

            characterManager.Initialize(ui, mapItemManager);
            characterManager.AddPlayer(position, pManager, gameManager);
            //characterManager.AddCharacter(characterManager.Enemys()[4].Clone(bossPosition));

            GeneratBoss();

            #region カメラ初期化
            angle = 0;
            gameDevice.MainProjector.Initialize(characterManager.GetPlayer().GetPosition);       //カメラを初期化
            #endregion

            background = new FogBackground(gameDevice);
        }

        /// <summary>
        /// 配置よりボスを生成
        /// </summary>
        private void GeneratBoss()
        {
            EnemySetting setting = gameManager.EnemySetting.BossSetting;

            for (int i = 0; i < setting.ids.Length; i++)
            {
                int id = setting.ids[i];
                if (id < 0)
                    continue;

                Vector3 bossPosition = map.BossPoint(i);
                characterManager.AddCharacterBossScene(characterManager.Enemys()[id].Clone(bossPosition));
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
            if (nextScene == SceneType.Pause)
                return;

            map.Clear();                            //マップ解放
            map = null;
            gameManager.ReleaseMap();

            mapItemManager.Initialize();            //Item解放
            mapItemManager = null;

            pManager.Clear();
            pManager = null;
            characterManager = null;

            gameManager.EnemySetting.Clear();

            ui = null;

            stageManager.ReleaseBossBGM();
        }

        public void Update(GameTime gameTime)
        {
            if (endFlag)
                return;

            if(!map.IsOver())
                stageManager.PlayBossBGM();

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

            //Particle追加
            AddSphereParticle();

            //アイテム処理
            mapItemManager.ItemCollision(characterManager.GetPlayer(), ui);

            //終わるかどうかをチェック
            CheckEnd();
        }

        private void AddSphereParticle()
        {
            if (!stageManager.UseParticle())
                return;

            float tileSize = MapDef.TILE_SIZE;
            if (pManager.Count() < 1500)
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector3 position = characterManager.GetPlayer().GetPosition;
                    position += new Vector3(
                        gameDevice.Random.Next(-30000, 30001) / 100.0f,
                        gameDevice.Random.Next(-30000, 30001) / 100.0f,
                        gameDevice.Random.Next(-30000, 30001) / 100.0f);
                    position.Y = 0;
                    pManager.AddParticle(new SphereParticle(position, stageManager.ConstactColor(), gameDevice));
                }
            }
        }

        private void AddTransportParticle()
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
        }

        private void CheckEnd()
        {
            //Pause機能
            if (input.GetKeyTrigger(Keys.P))
            {
                nextScene = SceneType.Pause;
                endFlag = true;
                return;
            }

            //Player死んだ場合
            if (characterManager.GetPlayer().IsDead())
            {
                gameManager.PlayerItem.RemoveAll();
                endFlag = true;
                nextScene = SceneType.LoadTown;
                return;
            }

            //Player以外に他のキャラがいる場合は終了しない
            if (characterManager.LiveCharacterCnt() > 1)
                return;

            //Boss倒したら出現
            map.SwitchDrawExit(true);
            AddTransportParticle();
            gameManager.StageManager.RemoveFog();
            gameDevice.Sound.PlayBGM("n17");

            //階段にたどり着いた場合
            if (map.WorldToMap(characterManager.GetPlayer().GetPosition) == map.EndPoint)
            {
                //ヒント文字を出す
                ui.HintUI.Switch(true);
                ui.HintUI.SetMessage("村へ戻る");
                if (!ui.HintUI.IsPush(Keys.Space))
                    return;

                //次へ行く処理
                endFlag = true;
                nextScene = SceneType.LoadTown;
                gameManager.UpdateDungeonProcess();     //攻略状況更新
                return;
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
    }
}
