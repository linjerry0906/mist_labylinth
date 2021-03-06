using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;

namespace Team27_RougeLike
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private GameDevice gameDevice;

        private GameManager gameManager;
        private SceneManager sceneManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Def.WindowDef.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Def.WindowDef.WINDOW_HEIGHT;
            Window.Title = Def.WindowDef.WINDOW_NAME;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = false;
            gameDevice = new GameDevice(Content, GraphicsDevice);
            gameManager = new GameManager(gameDevice);
            
            sceneManager = new SceneManager(gameDevice);
            IScene dungeon = new DungeonScene(gameManager, gameDevice);
            IScene boss = new BossScene(gameManager, gameDevice);
            IScene town = new TownScene(gameManager, gameDevice);
            sceneManager.AddScene(SceneType.Load, new Load(gameDevice));
            sceneManager.AddScene(SceneType.Logo, new LogoScene(gameDevice));
            sceneManager.AddScene(SceneType.Title, new SceneFader(new Title(gameDevice), gameDevice));
            sceneManager.AddScene(SceneType.LoadTown, new LoadTown(gameManager, gameDevice));
            sceneManager.AddScene(SceneType.Town, new SceneFader(town, gameDevice));
            sceneManager.AddScene(SceneType.UpgradeStore, new UpgradeStore(town, gameManager, gameDevice));
            sceneManager.AddScene(SceneType.LoadShop, new LoadShop(town, gameManager, gameDevice));
            sceneManager.AddScene(SceneType.ItemShop, new ItemShop(town, gameManager, gameDevice));
            sceneManager.AddScene(SceneType.Quest, new GuildScene(town, gameManager, gameDevice));
            sceneManager.AddScene(SceneType.Depot, new Depot(town, gameManager, gameDevice));
            sceneManager.AddScene(SceneType.DungeonSelect, new SceneFader(new DungeonSelect(town, gameManager, gameDevice), gameDevice));
            sceneManager.AddScene(SceneType.LoadMap, new LoadMap(gameManager, gameDevice));
            sceneManager.AddScene(SceneType.Dungeon, new SceneFader(dungeon, gameDevice));
            sceneManager.AddScene(SceneType.LoadBoss, new LoadBossScene(gameManager, gameDevice));
            sceneManager.AddScene(SceneType.Boss, new SceneFader(boss, gameDevice));
            sceneManager.AddScene(SceneType.Pause, new PauseScene(dungeon, boss, town, gameManager, gameDevice));
            sceneManager.Change(SceneType.Load);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
            gameDevice.Renderer.LoadFont("basicFont", "./Font/");
            gameDevice.Renderer.LoadFont("timerFont", "./Font/");
            gameDevice.Renderer.LoadModel("ItemModel", "./Model/");
            gameDevice.Renderer.LoadModel("map_block", "./Model/");
            gameDevice.Renderer.LoadModel("B_01", "./Model/");
            gameDevice.Renderer.LoadModel("magic_circle", "./Model/");

            //1ピクセル画像の生成
            Texture2D fade = new Texture2D(GraphicsDevice, 1, 1);
            Color[] date = new Color[1 * 1];
            date[0] = new Color(0, 0, 0);
            fade.SetData(date);
            gameDevice.Renderer.LoadTexture("fade", fade);

            //白：1ピクセル
            Texture2D white = new Texture2D(GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            data[0] = new Color(1.0f, 1.0f, 1.0f);
            white.SetData(data);
            gameDevice.Renderer.LoadTexture("white", white);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            gameDevice.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            // TODO: Add your update logic here
            gameDevice.Update();
            sceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(gameDevice.Renderer.FogManager.CurrentColor());

            // TODO: Add your drawing code here
            sceneManager.Draw();
            gameDevice.DrawCursor();

            base.Draw(gameTime);
        }
    }
}
