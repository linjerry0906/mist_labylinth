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
using Team27_RougeLike.Def;
using Team27_RougeLike.Object;
using Team27_RougeLike.Map;
using Team27_RougeLike.Object.Actor;
using Team27_RougeLike.Utility;

namespace Team27_RougeLike
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private GameDevice gameDevice;

        private MapGenerator mapGenerator;
        private DungeonMap map;

        private Player player;

        private float angle = 0;

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
            gameDevice = new GameDevice(Content, GraphicsDevice);
            mapGenerator = new MapGenerator(gameDevice);
            map = new DungeonMap(gameDevice);
            player = new Player(new Vector3(0, 1.5f, 0), gameDevice);

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
            gameDevice.Renderer.LoadTexture("test");
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
            player.Update(gameTime);

            //map ÇÃê∂ê¨Ç©ÇÁÉQÅ[ÉÄÇ‹Ç≈
            if (!mapGenerator.IsEnd())
            {
                player.Position = new Vector3(0, 40, 40);
                mapGenerator.Update();
            }
            else if(map.MapChip.Length < 2)
            {
                map = new DungeonMap(mapGenerator.MapChip, gameDevice);
                map.Initialize();

                player.Position = new Vector3(
                    map.EntryPoint.X * MapDef.TILE_SIZE,
                    MapDef.TILE_SIZE + 5,
                    map.EntryPoint.Y * MapDef.TILE_SIZE);
            }
            else
            {
                map.FocusCenter(player.Position);
                map.Update();
                map.MapCollision(player);
            }

            //Rotate Test
            if (gameDevice.InputState.GetKeyTrigger(Keys.Q))
            {
                angle += 45;
                angle = (angle > 360) ? angle - 360 : angle;
                gameDevice.MainProjector.Rotate(angle);
            }
            else if (gameDevice.InputState.GetKeyTrigger(Keys.E))
            {
                angle -= 45;
                angle = (angle < 0) ? angle + 360 : angle;
                gameDevice.MainProjector.Rotate(angle);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            mapGenerator.Draw();

            map.Draw();

            player.Draw();

            base.Draw(gameTime);
        }
    }
}
