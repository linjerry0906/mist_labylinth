//--------------------------------------------------------------------------------------------------
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

namespace Team27_RougeLike.Scene
{
    class BossScene : IScene
    {
        private GameDevice gameDevice;
        private GameManager gameManager;
        private Renderer renderer;
        private InputState input;

        private bool endFlag;
        private SceneType nextScene;

        private DungeonMap map;                 //マップ
        private MapItemManager mapItemManager;  //マップ内に落ちているアイテムの管理者

        private CharacterManager characterManager;
        private ParticleManager pManager;
        private float angle;

        private DungeonUI ui;             //Popメッセージ


        public BossScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;

            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
        }

        public void Draw()
        {
            map.Draw();
            mapItemManager.Draw();      //アイテムの描画

            characterManager.Draw();
            pManager.Draw();

            DrawUI();
        }

        public void DrawUI()
        {
            renderer.Begin();

            renderer.DrawString("Boss Scene\n P Key:Pause\n T Key: Back to Town", Vector2.Zero, new Vector2(1, 1), new Color(1, 1, 1));

            ui.Draw();

            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            endFlag = false;
            nextScene = SceneType.LoadTown;

            if (scene == SceneType.Pause)
                return;

            #region Map初期化
            map = gameManager.GetDungeonMap();      //生成したマップを取得
            if (map == null)                         //エラー対策　マップが正常に生成されてなかったらLoadingに戻る
            {
                nextScene = SceneType.LoadMap;
                endFlag = true;
                return;
            }

            map.Initialize();                       //マップを初期化
            #endregion

            #region MapItemの初期化処理
            mapItemManager = new MapItemManager(gameManager, gameDevice);
            mapItemManager.Initialize();
            #endregion

            Vector3 position = new Vector3(
               map.EntryPoint.X * MapDef.TILE_SIZE,
               MapDef.TILE_SIZE,
               map.EntryPoint.Y * MapDef.TILE_SIZE);

            Vector3 bossPosition = new Vector3(
               map.EndPoint.X * MapDef.TILE_SIZE,
               MapDef.TILE_SIZE,
               map.EndPoint.Y * MapDef.TILE_SIZE);

            pManager = new ParticleManager(gameDevice);
            pManager.Initialize();

            characterManager = new CharacterManager(gameDevice);
            characterManager.Initialize(position);
            characterManager.AddPlayer(position, pManager);
            characterManager.AddCharacter(characterManager.Enemys()[4].Clone(bossPosition));

            #region カメラ初期化
            angle = 0;
            gameDevice.MainProjector.Initialize(characterManager.GetPlayer().Position);       //カメラを初期化
            #endregion

            ui = new DungeonUI(gameManager, gameDevice);
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return nextScene;
        }

        public void Shutdown()
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

            ui = null;
        }

        public void Update(GameTime gameTime)
        {
            ui.Update();
            if (ui.IsPop())                   //メッセージ表示中は以下Updateしない
                return;

            RotateCamera();

            //Chara処理
            characterManager.Update(gameTime);

            pManager.Update(gameTime);

            //マップ処理
            map.MapCollision(gameDevice.Renderer.MainProjector);
            map.FocusCenter(characterManager.GetPlayer().Position);
            map.Update();
            map.MapCollision(characterManager.GetPlayer());
            map.MapCollision(characterManager.GetCharacters());

            //アイテム処理
            mapItemManager.ItemCollision(characterManager.GetPlayer(), ui);

            //Debug 村シーンへ
            if (input.GetKeyTrigger(Keys.T))
            {
                endFlag = true;

                gameManager.UpdateDungeonProcess();     //攻略状況更新
                gameManager.Save();
                return;
            }

            //Pause機能
            if (input.GetKeyTrigger(Keys.P))
            {
                nextScene = SceneType.Pause;
                endFlag = true;
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
                angle += 1;
                angle = (angle > 360) ? angle - 360 : angle;
            }
            else if (gameDevice.InputState.GetKeyState(Keys.E))
            {
                angle -= 1;
                angle = (angle < 0) ? angle + 360 : angle;
            }
            gameDevice.MainProjector.Rotate(angle);
        }
    }
}
