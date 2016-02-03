using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GameEngineProgramming.Engine.Engines
{
    public class FrameRateCounter : DrawableGameComponent
    {
        static int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public static int FrameRate { get {return frameRate; } }

        public FrameRateCounter(Game _game): base (_game)
        {
          
            _game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
            base.Draw(gameTime);
        }

    }
}
