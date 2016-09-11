using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RLenders.MonoGame.Draw2D.Shapes;

namespace RLenders.MonoGame.Draw2D.Demo
{
    /// <summary>
    /// A grid generating demo showcasing how to use ShapeBatch.
    /// </summary>
    public class GridDemo : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private ShapeBatch shapeBatch;

        private const float TriangleRatio = 0.86602540378443864676372317075294f;
        private readonly GamePadState[] prevGamePadStates = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];
        private KeyboardState prevKeyboardState;
        private MouseState prevMouseState;
        private float paintDistanceSquared;

        // Change these variables to modify the generated grid.
        private readonly GridType? gridType = GridType.Hexagon;
        private Vector2 scale = new Vector2(40);

        private enum GridType
        {
            Triangle = 3,
            Square = 4,
            Hexagon = 6
        }

        public GridDemo()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true,
                SynchronizeWithVerticalRetrace = false // unlocks framerate
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            paintDistanceSquared = scale.LengthSquared();
        }

        protected override void Initialize()
        {
            // Create a new ShapeBatch, which can be used to draw shapes.
            shapeBatch = new ShapeBatch(this);

            if (gridType == null)
            {
                shapeBatch.Shapes.Add(new RegularPolygon(shapeBatch, 5)
                {
                    Position = new Vector2(100),
                    Size = new Vector2(100),
                    Fill = Color.Blue,
                    Stroke = Color.White
                });
            }
            else
            {
                var ratio = gridType == GridType.Square ? Vector2.One : new Vector2(TriangleRatio, 1);

                var p = gridType == GridType.Triangle ? 2 :
                        gridType == GridType.Hexagon ? 4f / 3 : 1;

                var height = graphics.PreferredBackBufferHeight / scale.Y * p + 1;
                var width = graphics.PreferredBackBufferWidth / (int)(ratio.X * scale.X);

                for (var y = 0; y <= height; y++)
                {
                    var position = new Vector2(0, y * scale.Y / p);

                    switch (gridType.Value)
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
                        shapeBatch.Shapes.Add(new RegularPolygon(shapeBatch, (int) gridType)
                        {
                            Position = position,
                            Scale = scale,
                            Rotation = gridType != GridType.Square ? (float) (Math.PI * (even ? -1 : +1) / 2f) : 0,
                            Fill = GetRandomGridColor()
                        });
                        position.X += scale.X * ratio.X * (gridType == GridType.Triangle ? (even ? 1 : 2) * 2f / 3 : 1);
                    }
                }
            }

            Components.Add(shapeBatch);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("Arial");

            Components.Add(new FrameRateCounter(this, spriteBatch, spriteFont));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (gamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (gridType != null)
            {
                var scrollWheelOffset = mouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;
                paintDistanceSquared += scrollWheelOffset / 2f;
                if (paintDistanceSquared < 0) paintDistanceSquared = 0;

                var toggleButtonPressed = mouseState.MiddleButton == ButtonState.Pressed && prevMouseState.MiddleButton != ButtonState.Pressed;

                if (scrollWheelOffset != 0 || toggleButtonPressed || prevMouseState.Position != mouseState.Position || mouseState.LeftButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed)
                {
                    var mousePosition = mouseState.Position.ToVector2();
                    bool? visible = null;

                    for (var i = 0; i < shapeBatch.Shapes.Count; i++)
                    {
                        var shape = shapeBatch.Shapes[i];

                        if (toggleButtonPressed)
                        {
                            if (visible == null) visible = shape.Fill == Color.Transparent;
                            shape.Fill = visible.Value ? GetRandomGridColor() : Color.Transparent;
                        }

                        if (Vector2.DistanceSquared(shape.Position, mousePosition) < paintDistanceSquared)
                        {
                            if (mouseState.LeftButton == ButtonState.Pressed) shape.Fill = GetRandomGridColor();
                            else if (mouseState.RightButton == ButtonState.Pressed) shape.Fill = Color.Transparent;

                            shape.Stroke = Color.Red;
                        }
                        else
                        {
                            shape.Stroke = Color.Transparent;
                        }
                    }
                }
            }

            base.Update(gameTime);

            prevGamePadStates[(int)PlayerIndex.One] = gamePadState;
            prevKeyboardState = keyboardState;
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
