namespace RLenders.MonoGame.Draw2D
{
    /// <summary>
    /// An enumeration that defines various sizing modes for shapes.
    /// By default the shape sizing mode is set to AutoHeight.
    /// </summary>
    public enum ShapeSizing
    {
        /// <summary>
        /// The height of the shape is relative to the width of the shape.
        /// This maintains the aspect ratio of the shape.
        /// </summary>
        AutoHeight,

        /// <summary>
        /// The width of the shape is relative to the height of the shape.
        /// This maintains the aspect ratio of the shape.
        /// </summary>
        AutoWidth,

        /// <summary>
        /// The size of the shape is relative to the diameter of the circumscribed circle of the shape.
        /// This maintains the aspect ratio of the shape.
        /// </summary>
        Circumcircle,

        /// <summary>
        /// The size of the shape is stretched to fit a given size.
        /// This does not maintain the aspect ratio of the shape.
        /// </summary>
        Stretch
    }
}
