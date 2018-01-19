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

        private bool endFlag;

        private SceneType nextScene;

        private Button[] buttons;
        private ButtonEnum onButton;
        private Vector2 hintPos;
        private DungeonHintUI hintUI;
        private string[] hint;

        public TownScene(GameManager gameManager, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;
            this.gameManager = gameManager;
        }

        public void Draw()
        {
            renderer.Begin();
            renderer.DrawTexture("town", Vector2.Zero);
            renderer.End();

            DrawUI();
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

            gameManager.PlayerInfo.Initialize();
            gameManager.Save();
            if (scene == SceneType.Pause ||
                scene == SceneType.ItemShop ||
                scene == SceneType.DungeonSelect ||
                scene == SceneType.Depot ||
                scene == SceneType.Quest)
                return;

            InitButton();

            InitHint();

            gameManager.PlayerItem.RemoveTemp();       //一時的なアイテムを削除
            gameManager.PlayerInfo.Initialize();       //レベルなどの初期化処理
        }

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
            CheckButton();

            UpdateHint();

            CheckIsEnd();
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

        private void UpdateHint()
        {
            hintUI.Update();
            Vector2 pos = Vector2.Lerp(hintPos + new Vector2(200, 0), hintPos, hintUI.CurrentAlpha());
            hintUI.SetPosition(pos);

            if (onButton == ButtonEnum.NULL)
            {
                hintUI.Switch(false);
                return;
            }

            hintUI.Switch(true);
            hintUI.SetMessage(hint[(int)onButton]);
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
