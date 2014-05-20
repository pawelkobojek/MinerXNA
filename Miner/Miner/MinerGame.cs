using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Miner
{
    /// <summary>
    /// Główna klasa gry.
    /// </summary>
    public class MinerGame : Game
    {
        private const string SETTINGS_PATH = "settings.xml";
        private const string HIGHSCORE_PATH = "scores.xml";

        /// <summary>
        /// Pole przechowujące stan gry.
        /// </summary>
        public GameState GameState { get; set; }

        /// <summary>
        /// Pole służące do zmiany aktualnie wyświetlanego menu.
        /// </summary>
        private MenuManager menuManager = MenuManager.GetInstance();

        /// <summary>
        /// Pole stanowiące referencje do obiektu ustawień.
        /// </summary>
        public Settings Settings { get; set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public MinerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1440;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            Settings = Settings.GetInstance();
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
            base.Initialize();
            Field.FieldHeight = this.GraphicsDevice.Viewport.Bounds.Height / GameMap.DEF_HEIGHT;
            Field.FieldWidth = this.GraphicsDevice.Viewport.Bounds.Width / GameMap.DEF_WIDTH;
            this.GameState = new GameState();
            this.GameState.Miner = new Miner(this);
            this.GameState.CurrentLevel = 1;

            //this.menuManager.CurrentMenu = new StartMenu(this);
            this.menuManager.CurrentMenu = new MainMenu(this);
            if (File.Exists(SETTINGS_PATH))
            {
                using (FileStream file = new FileStream(SETTINGS_PATH, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(Settings));
                    this.Settings = (Settings)xml.Deserialize(file);
                }
            }

            if (File.Exists(HIGHSCORE_PATH))
            {
                using (FileStream file = new FileStream(HIGHSCORE_PATH, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(HighScores));
                    HighScores hs = HighScores.GetInstance();
                    hs = (HighScores)xml.Deserialize(file);
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.Content.Load<SpriteFont>("general");
            this.Content.Load<Texture2D>(Kosmojopek.ASSET_NAME);
            this.Content.Load<Texture2D>(Ufolowca.ASSET_NAME);
            this.Content.Load<Texture2D>(WladcaLaserowejDzidy.ASSET_NAME);
            //this.Content.Load<Texture2D>(PoteznySultan.ASSET_NAME);
            this.Content.Load<Texture2D>(Miner.ASSET_NAME);
            //this.Content.Load<Texture2D>(CosmicMatter.ASSET_NAME);
            this.Content.Load<Texture2D>(DoubleJump.ASSET_NAME);
            this.Content.Load<Texture2D>(Field.ASSET_NAME);
            this.Content.Load<Texture2D>(Fuel.ASSET_NAME);
            this.Content.Load<Texture2D>(Indestructibility.ASSET_NAME);
            this.Content.Load<Texture2D>(Key.ASSET_NAME);
            this.Content.Load<Texture2D>(KeyLocalizer.ASSET_NAME);
            this.Content.Load<Texture2D>(Laser.ASSET_NAME);
            this.Content.Load<Texture2D>(MoreEnemies.ASSET_NAME);
            this.Content.Load<Texture2D>(SolidRock.ASSET_NAME);
            this.Content.Load<Texture2D>(SpaceGate.ASSET_NAME);
            this.Content.Load<Texture2D>(TitanRock.ASSET_NAME);
            this.Content.Load<Texture2D>(Bullet.ASSET_NAME);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.ExitGame();

            this.menuManager.CurrentMenu.Update(gameTime);

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
            spriteBatch.Begin();

            this.menuManager.CurrentMenu.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Wychodzi z gry zapisując uprzednio ustawienia gracza.
        /// </summary>
        public void ExitGame()
        {
            using (FileStream fs = new FileStream(SETTINGS_PATH, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(Settings));
                xml.Serialize(fs, this.Settings);
            }

            if (this.GameState.User != null && this.GameState.User.Score > 0)
            {
                Result result = new Result(this.GameState.User);
                HighScores hs = HighScores.GetInstance();
                hs.AddResult(result);

                using (FileStream fs = new FileStream(HIGHSCORE_PATH, FileMode.Create))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(HighScores));
                    xml.Serialize(fs, hs);
                }
            }
            this.Exit();
        }
    }
}
