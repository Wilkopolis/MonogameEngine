using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using BracketHouse.FontExtension;

namespace MonogameEngine
{
    public partial class MonogameEngine : Game
    {
        public static GraphicsDevice graphics;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphicsManager;
        public static ContentManager content;

        public static GameTime Time = new GameTime();
        public static long MsEllapsed = 0;
        public static bool isActive = true;

        public static string Version = "0.0.1.0";

        public MonogameEngine()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            content = new ContentManager(Content.ServiceProvider);
            content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics = GraphicsDevice;
            TextRenderer.Initialize(graphicsManager, Window, content);
            Text.TextCanvas = new RenderTarget2D(graphics, 1920, 1080);

            this.Activated += ActivateMyGame;
            this.Deactivated += DeactivateMyGame;

            // set the initial keyboard states
            PreviousKeyboardState = Keyboard.GetState();
            CurrentKeyboardState = Keyboard.GetState();

            // load the config/key bindings
            LoadSettingsAndKeys();
            ApplySettings();

            Setting.MUSICVOLUME = 0f;
            Setting.SFXVOLUME = 0f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // unlocks/tutorial etc.
            LoadProgress();

            // load effects, fonts, and initial sprites
            LoadStaticResources();

            InitCursors();

            SaveProgress();

            if (Test())
                return;
        }

        protected override void UnloadContent()
        {

        }

        public static KeyboardState PreviousKeyboardState;
        public static KeyboardState CurrentKeyboardState;
        public static MState PreviousMouseState = new MState(Mouse.GetState());
        public static MState CurrentMouseState = new MState(Mouse.GetState());
        public static float GameSpeed = 1f;
        protected override void Update(GameTime gameTime)
        {
            Time = gameTime;
            float delta = gameTime.ElapsedGameTime.Milliseconds * GameSpeed;
            MsEllapsed += (long)delta;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = new MState(Mouse.GetState());

            foreach (Screen screen in Screens.Values)
                screen.Update(delta);

            // check input interactions
            KeyboardHandler();
            MouseHandler();

            if (GetFlag("InGame"))
                Game_Update(delta);
            if (GetFlag("InEditor"))
                Editor_Update(delta);

            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;

            Mouse.SetCursor(Cursors[TargetCursor]);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float totalTime = (float)gameTime.TotalGameTime.TotalSeconds;

            List<Screen> screens = Screens.Values.ToList();
            screens.OrderBy(@screen => @screen.zIndex);

            // render all our screens
            foreach (Screen screen in screens)
                screen.Render();

            // draw all our screens
            foreach (Screen screen in screens)
                screen.Draw();

            foreach (Element element in DebugElements.Values)
            {
                element.Render();
                element.Draw();
            }

            base.Draw(gameTime);
        }

        public void OnResize(Object sender, EventArgs e)
        {
        }

        public void ActivateMyGame(object sendet, EventArgs args)
        {
            MonogameEngine.isActive = true;
        }

        public void DeactivateMyGame(object sendet, EventArgs args)
        {
            MonogameEngine.isActive = false;
        }
    }
}
