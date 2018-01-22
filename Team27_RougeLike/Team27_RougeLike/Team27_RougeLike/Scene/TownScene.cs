//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.06
// 内容  ：村シーン
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Device;
using Team27_RougeLike.UI;
using Team27_RougeLike.Effects;
using Team27_RougeLike.Utility;

namespace Team27_RougeLike.Scene
{
    class TownScene : IScene
    {
        private enum ButtonEnum
        {
            Dungeonbutton = 0,
            Guildtbutton,
            Shopbutton,
            Upgradebutton,
            Depotbutton,
            NULL,
        }

        private GameDevice gameDevice;
        private InputState input;
        private Renderer renderer;
        private GameManager gameManager;

        private TownEffect townEffect;      //Effect
        private Timer effectTimer;          //Effectを制御するタイマー

        private bool endFlag;

        private SceneType nextScene;

        private Button[] buttons;           //メインボタン
        private ButtonEnum onButton;        //どのボタンにある
        private Vector2 hintPos;            //Hint描画位置
        private DungeonHintUI hintUI;       //HintUI
        private string[] hint;              //Hint文字

        public TownScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            this.gameManager = gameManager;
        }

        public void Draw()
        {
            townEffect.WriteRenderTarget(Color.White);

            renderer.Begin();
            renderer.DrawTexture("town", Vector2.Zero);
            renderer.End();

            DrawUI();

            townEffect.ReleaseRenderTarget();
            townEffect.Draw();
        }

        private void DrawUI()
        {
            renderer.Begin();

            for(int i = 0; i < buttons.Length; i++)
            {
                Vector2 position = new Vector2(
                    buttons[i].Position().X,
                    buttons[i].Position().Y);
                Vector2 size = new Vector2(1, 1);
                if (i == (int)onButton)
                    size = new Vector2(1.2f, 1.2f);

                renderer.DrawTexture(
                    ((ButtonEnum)i).ToString(),
                    position,
                    size);
            }

            renderer.DrawTexture(
                "fade", new Vector2(0, Def.WindowDef.WINDOW_HEIGHT - 45),
                new Vector2(Def.WindowDef.WINDOW_WIDTH, 32), hintUI.CurrentAlpha() * 0.6f);
            hintUI.Draw();

            renderer.End();
        }

        public void Initialize(SceneType scene)
        {
            nextScene = SceneType.DungeonSelect;
            endFlag = false;

            gameManager.PlayerInfo.Initialize();       //レベルなどの初期化処理
            gameManager.Save();
            if (scene == SceneType.Pause ||
                scene == SceneType.ItemShop ||
                scene == SceneType.DungeonSelect ||
                scene == SceneType.Depot ||
                scene == SceneType.Quest || 
                scene == SceneType.UpgradeStore)
                return;

            InitEffect();
            InitButton();
            InitHint();

            gameManager.PlayerItem.RemoveTemp();       //一時的なアイテムを削除
        }

        /// <summary>
        /// Effectを初期化
        /// </summary>
        private void InitEffect()
        {
            townEffect = new TownEffect(
                new Vector2(Def.WindowDef.WINDOW_WIDTH / 2 + 300, 300),
                gameDevice);
            townEffect.Initialize();

            effectTimer = new Timer(1.2f);
            effectTimer.Initialize();

            townEffect.SetScaleRate(effectTimer.Rate() * 1.5f + 1);
        }

        /// <summary>
        /// ボタンを初期化
        /// </summary>
        private void InitButton()
        {
            buttons = new Button[(int)ButtonEnum.NULL];
            Vector2 offset = new Vector2(20, 20);
            Vector2 height = new Vector2(0, 80);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new Button(offset + i * height, 300, 60);
            }

            onButton = ButtonEnum.NULL;
        }

        /// <summary>
        /// ヒントUI、メッセージを初期化
        /// </summary>
        private void InitHint()
        {
            hintPos = new Vector2(30, Def.WindowDef.WINDOW_HEIGHT - 30);
            hintUI = new DungeonHintUI(gameDevice);
            hintUI.SetPosition(hintPos);
            hintUI.Switch(false);
            hintUI.SetSpeed(0.07f);

            hint = new string[(int)ButtonEnum.NULL];
            hint[(int)ButtonEnum.Dungeonbutton] = "ダンジョンへ冒険する";
            hint[(int)ButtonEnum.Guildtbutton] = "ギルトで依頼を受ける";
            hint[(int)ButtonEnum.Shopbutton] = "ショップはアイテムを売買できる";
            hint[(int)ButtonEnum.Upgradebutton] = "鍛冶屋は武器や防具の強化ができる";
            hint[(int)ButtonEnum.Depotbutton] = "倉庫にアイテムを保存できる";
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
        }

        public void Update(GameTime gameTime)
        {
            gameDevice.Sound.PlayBGM("Voyage_SE");

            UpdateEffect();
            if (!effectTimer.IsTime())      //Effect中は他の操作できない
                return;

            CheckButton();

            UpdateHint();

            CheckIsEnd();
        }

        /// <summary>
        /// Effectを更新
        /// </summary>
        private void UpdateEffect()
        {
            effectTimer.Update();
            townEffect.SetScaleRate(effectTimer.Rate() * 1.5f + 1);
        }

        /// <summary>
        /// クリックされたかをチェック
        /// </summary>
        private void CheckButton()
        {
            onButton = ButtonEnum.NULL;
            Point mousePos = new Point(
                (int)input.GetMousePosition().X,
                (int)input.GetMousePosition().Y);

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].IsClick(mousePos))
                {
                    onButton = (ButtonEnum)i;
                    break;
                }
            }
        }

        /// <summary>
        /// ヒントUIの更新
        /// </summary>
        private void UpdateHint()
        {
            hintUI.Update();
            Vector2 pos = Vector2.Lerp(hintPos + new Vector2(200, 0), hintPos, hintUI.CurrentAlpha());
            hintUI.SetPosition(pos);             //文字位置調整

            if (onButton == ButtonEnum.NULL)     //マウスがボタン上でない場合
            {
                hintUI.Switch(false);            //表示しない
                return;
            }

            hintUI.Switch(true);                        //表示する
            hintUI.SetMessage(hint[(int)onButton]);     //Hint文字設定
        }

        /// <summary>
        /// 終了するかをチェック
        /// </summary>
        private void CheckIsEnd()
        {
            if (input.GetKeyTrigger(Keys.P))
            {
                nextScene = SceneType.Pause;
                endFlag = true;
                return;
            }
  
            //カーソルがボタン上、クリックした
            if(onButton != ButtonEnum.NULL &&
                input.IsLeftClick())
            {
                SetNextScene();
                endFlag = true;
                return;
            }
        }

        /// <summary>
        /// 次のシーンを決定
        /// </summary>
        private void SetNextScene()
        {
            switch (onButton)
            {
                case ButtonEnum.Dungeonbutton:
                    nextScene = SceneType.DungeonSelect;
                    break;
                case ButtonEnum.Guildtbutton:
                    nextScene = SceneType.Quest;
                    break;
                case ButtonEnum.Shopbutton:
                    nextScene = SceneType.ItemShop;
                    break;
                case ButtonEnum.Upgradebutton:
                    nextScene = SceneType.UpgradeStore;
                    break;
                case ButtonEnum.Depotbutton:
                    nextScene = SceneType.Depot;
                    break;
            }
        }
    }
}
