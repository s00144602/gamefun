using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine;
using Engine.Engines;
using GameEngineProgramming.Engine.Base;
using GameEngineProgramming.Engine.Engines;
using GameEngineProgramming.Scenes;

namespace GameEngineProgramming
{
    public class GameStart : Game
    {
        GraphicsDeviceManager graphics;
        ContentManager nonPeristentContent;
        GameEngine engine;
        public GameStart()
        {
            Window.Title = "";
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;

            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 768;
            }

            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(16);

            Content.RootDirectory = "Content";

            nonPeristentContent = new ContentManager(Content.ServiceProvider, "Content");
            Content.RootDirectory = "Content";
            engine = new GameEngine(this);
        }

        protected override void Initialize()
        {
            GameUtilities.GraphicsDevice = GraphicsDevice;
            GameUtilities.NonPersistentContent = nonPeristentContent;
            GameUtilities.PersistentContent = Content;
            GameUtilities.Random = new Random(DateTime.Now.Millisecond * DateTime.Now.Day);

            IsMouseVisible = false;
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            GameUtilities.DebugFont = Content.Load<SpriteFont>("debug");
            engine.LoadScene(new MazeScene("test", engine));
        }

        protected override void Update(GameTime gameTime)
        {
            GameUtilities.Time = gameTime;

            if (GameUtilities.IsDevelopmentMode)
            {
                if (InputEngine.IsKeyPressed(Keys.Escape) || InputEngine.IsButtonPressed(Buttons.Back))
                    this.Exit();
            }
         
            Window.Title = "FPS: " + FrameRateCounter.FrameRate; 
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
