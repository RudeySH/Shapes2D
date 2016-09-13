using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shapes2D.Samples.Shared
{
    /// <summary>
    /// Implementation by Shawn Hargreaves:
    /// https://blogs.msdn.microsoft.com/shawnhar/2007/06/08/displaying-the-framerate/
    /// </summary>
    public class FrameRateCounter : DrawableGameComponent
    {
        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont spriteFont;

        private int frameRate;
        private int frameCounter;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        public FrameRateCounter(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;

            game.IsFixedTimeStep = false;
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
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            var fps = $"fps: {frameRate}";

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);

            spriteBatch.End();
        }
    }
}
