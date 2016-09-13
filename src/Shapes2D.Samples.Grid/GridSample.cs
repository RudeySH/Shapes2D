using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shapes2D.Drawing;
using Shapes2D.Samples.Shared;

namespace Shapes2D.Samples.Grid
{
    public enum GridType
    {
        Triangle = 3,
        Square = 4,
        Hexagon = 6
    }
    
    public class GridSample : SampleGame
    {
        // Change these variables to modify the generated grid.
        public GridType GridType = GridType.Hexagon;
        private Vector2 scale = new Vector2(40);

        private readonly PrimitiveBatch primitiveBatch;
        private float paintDistanceSquared;

        public GridSample()
        {
            // Create a new PrimitiveBatch, which can be used to draw 2D lines and shapes.
            primitiveBatch = new PrimitiveBatch(this);
            Components.Add(primitiveBatch);

            paintDistanceSquared = scale.LengthSquared();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var ratio = GridType == GridType.Square ? Vector2.One : new Vector2(TriangleRatio, 1);

            var p = GridType == GridType.Triangle ? 2 :
                    GridType == GridType.Hexagon ? 4f / 3 : 1;

            var height = Graphics.PreferredBackBufferHeight / scale.Y * p + 1;
            var width = Graphics.PreferredBackBufferWidth / (int)(ratio.X * scale.X);

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
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var scrollWheelOffset = CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
            paintDistanceSquared += scrollWheelOffset / 2f;
            if (paintDistanceSquared < 0) paintDistanceSquared = 0;

            var toggleButtonPressed = CurrentMouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton != ButtonState.Pressed;

            if (scrollWheelOffset != 0 || toggleButtonPressed || PreviousMouseState.Position != CurrentMouseState.Position || CurrentMouseState.LeftButton == ButtonState.Pressed || CurrentMouseState.RightButton == ButtonState.Pressed)
            {
                var mousePosition = CurrentMouseState.Position.ToVector2();
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
                            if (CurrentMouseState.LeftButton == ButtonState.Pressed) shape.Fill = GetRandomGridColor();
                            else if (CurrentMouseState.RightButton == ButtonState.Pressed) shape.Fill = Color.Transparent;
                        }

                        primitive.Stroke = Color.Red;
                    }
                    else
                    {
                        primitive.Stroke = Color.Transparent;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a random color for each primitive in the grid.
        /// </summary>
        private static Color GetRandomGridColor()
        {
            return Color.SaddleBrown.Darken(0, 0.75f).Lighten(0, 0.25f);
        }
    }
}
