using Microsoft.Xna.Framework;

namespace RLenders.MonoGame.Draw2D.Shapes
{
    public abstract class Shape : DrawableGameComponent
    {
        private readonly ShapeBatch shapeBatch;

        /// <summary>
        /// The position of the shape relative to the top-left corner of the screen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Two-dimensional size multiplier, applied after rotation.
        /// </summary>
        public Vector2 Scale { get; set; } = Vector2.One;

        /// <summary>
        /// Two-dimensional size multiplier, applied before rotation.
        /// </summary>
        public Vector2 Size { get; set; } = Vector2.One;

        /// <summary>
        /// Amount of rotation of the Z-axis, in radians.
        /// </summary>
        public float Rotation { get; set; } = 0;

        /// <summary>
        /// The color of the surface of the shape.
        /// </summary>
        public virtual Color Fill { get; set; } = Color.White;

        /// <summary>
        /// The color of the outline of the shape.
        /// </summary>
        public virtual Color Stroke { get; set; }

        protected Shape(ShapeBatch shapeBatch) : base(shapeBatch.Game)
        {
            this.shapeBatch = shapeBatch;
        }
    }
}
