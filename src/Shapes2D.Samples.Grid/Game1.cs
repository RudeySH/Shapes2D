using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shapes2D.Drawing;

namespace Shapes2D.Samples.Grid
{
    public enum GridType
    {
        Triangle = 3,
        Square = 4,
        Hexagon = 6
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private PrimitiveBatch primitiveBatch;

        private const float TriangleRatio = 0.86602540378443864676372317075294f;
        private MouseState prevMouseState;
        private float paintDistanceSquared;

        // Change these variables to modify the generated grid.
        public GridType GridType = GridType.Hexagon;
        private Vector2 scale = new Vector2(40);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true, // Only works in DesktopGL.
                SynchronizeWithVerticalRetrace = false // Unlocks framerate.
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            paintDistanceSquared = scale.LengthSquared();
        }

        protected override void Initialize()
        {
            // Create a new PrimitiveBatch, which can be used to draw 2D lines and shapes.
            primitiveBatch = new PrimitiveBatch(this);

            var ratio = GridType == GridType.Square ? Vector2.One : new Vector2(TriangleRatio, 1);

            var p = GridType == GridType.Triangle ? 2 :
                    GridType == GridType.Hexagon ? 4f / 3 : 1;

            var height = graphics.PreferredBackBufferHeight / scale.Y * p + 1;
            var width = graphics.PreferredBackBufferWidth / (int)(ratio.X * scale.X);

            for (var y = 0; y <= height; y++)
            {
                var position = new Vector2(0, y * scale.Y / p);

                switch (GridType)
                {
                    case GridType.Triangle:
                        position.X += scale.X * ratio.X * 2 / 3;
                        if (y % 2 == 1) position.X += -scale.X * ratio.X / 3;
                        break;

                    case GridType.Square:
                        position += scale / 2;
                        break;

                    case GridType.Hexagon:
                        position.Y += -scale.Y / 4;
                        if (y % 2 == 1) position.X += scale.X * ratio.X / 2;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                for (var x = 0; x <= width + 1; x++)
                {
                    var even = x % 2 == y % 2;
                    primitiveBatch.Primitives.Add(new RegularPolygon((int)GridType)
                    {
                        Position = position,
                        Stretch = scale,
                        Rotation = GridType != GridType.Square ? (float)(Math.PI * (even ? -1 : +1) / 2f) : 0,
                        Fill = GetRandomGridColor()
                    });
                    position.X += scale.X * ratio.X * (GridType == GridType.Triangle ? (even ? 1 : 2) * 2f / 3 : 1);
                }
            }

            Components.Add(primitiveBatch);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("Font");

            Components.Add(new FrameRateCounter(this, spriteBatch, spriteFont));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mouseState = Mouse.GetState();
            
            var scrollWheelOffset = mouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;
            paintDistanceSquared += scrollWheelOffset / 2f;
            if (paintDistanceSquared < 0) paintDistanceSquared = 0;

            var toggleButtonPressed = mouseState.MiddleButton == ButtonState.Pressed && prevMouseState.MiddleButton != ButtonState.Pressed;

            if (scrollWheelOffset != 0 || toggleButtonPressed || prevMouseState.Position != mouseState.Position || mouseState.LeftButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed)
            {
                var mousePosition = mouseState.Position.ToVector2();
                bool? visible = null;

                for (var i = 0; i < primitiveBatch.Primitives.Count; i++)
                {
                    var primitive = primitiveBatch.Primitives[i];
                    var shape = primitive as Shape;

                    if (toggleButtonPressed && shape != null)
                    {
                        if (visible == null) visible = shape.Fill == Color.Transparent;
                        shape.Fill = visible.Value ? GetRandomGridColor() : Color.Transparent;
                    }

                    if (Vector2.DistanceSquared(primitive.Position, mousePosition) < paintDistanceSquared)
                    {
                        if (shape != null)
                        {
                            if (mouseState.LeftButton == ButtonState.Pressed) shape.Fill = GetRandomGridColor();
                            else if (mouseState.RightButton == ButtonState.Pressed) shape.Fill = Color.Transparent;
                        }

                        primitive.Stroke = Color.Red;
                    }
                    else
                    {
                        primitive.Stroke = Color.Transparent;
                    }
                }
            }

            base.Update(gameTime);

            prevMouseState = mouseState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Returns a random color used for the grid demo.
        /// </summary>
        private static Color GetRandomGridColor()
        {
            return Color.SaddleBrown.Darken(0, 0.75f).Lighten(0, 0.25f);
        }
    }
}
