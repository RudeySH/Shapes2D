using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shapes2D.Drawing;

namespace Shapes2D.Samples.Shared
{
    public abstract class SampleGame : Game
    {
        protected GraphicsDeviceManager Graphics { get; }
        protected SpriteBatch SpriteBatch { get; private set; }
        protected SpriteFont SpriteFont { get; private set; }
        protected PrimitiveBatch PrimitiveBatch { get; private set; }

        protected MouseState CurrentMouseState { get; private set; }
        protected MouseState PreviousMouseState { get; private set; }

        protected const float TriangleRatio = 0.86602540378443864676372317075294f;

        protected SampleGame()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true, // Only works in DesktopGL.
                SynchronizeWithVerticalRetrace = false // Unlocks framerate.
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Create a new PrimitiveBatch, which can be used to draw 2D lines and shapes.
            PrimitiveBatch = new PrimitiveBatch(this);
            Components.Add(PrimitiveBatch);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFont = Content.Load<SpriteFont>("Font");
            Components.Add(new FrameRateCounter(this, SpriteBatch, SpriteFont));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
