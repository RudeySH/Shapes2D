namespace Shapes2D
{
    /// <summary>
    /// An enumeration that defines various sizing modes for vertices.
    /// By default the vertex sizing mode is set to AutoHeight.
    /// </summary>
    public enum VertexScaling
    {
        /// <summary>
        /// The height of the vertices is relative to the width of the vertices.
        /// This maintains the aspect ratio of the vertices.
        /// </summary>
        AutoHeight,

        /// <summary>
        /// The width of the vertices is relative to the height of the vertices.
        /// This maintains the aspect ratio of the vertices.
        /// </summary>
        AutoWidth,

        /// <summary>
        /// The scale of the vertices is relative to the diameter of the circumscribed circle of the vertices.
        /// This maintains the aspect ratio of the vertices.
        /// </summary>
        Circumcircle,

        /// <summary>
        /// The scale of the vertices is stretched to fit a given size.
        /// This does not maintain the aspect ratio of the vertices.
        /// </summary>
        Stretch
    }
}
