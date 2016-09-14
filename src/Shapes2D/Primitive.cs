using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Shapes2D
{
    public class Primitive
    {
        /// <summary>
        /// Collection of coordinates.
        /// </summary>
        public ObservableCollection<Vector2> Vertices { get; private set; }

        /// <summary>
        /// Translation relative to the top-left corner of the screen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Two-dimensional scale multiplier, applied after rotation.
        /// </summary>
        public Vector2 Stretch { get; set; } = Vector2.One;

        /// <summary>
        /// Two-dimensional scale multiplier, applied before rotation.
        /// </summary>
        public Vector2 Size { get; set; } = Vector2.One;

        /// <summary>
        /// Amount of rotation of the Z-axis, in radians.
        /// </summary>
        public float Rotation { get; set; } = 0;

        /// <summary>
        /// Color of the (out)line.
        /// </summary>
        public Color Stroke { get; set; } = Color.White;

        protected Primitive(IEnumerable<Vector2> vertices)
        {
            Vertices = new ObservableCollection<Vector2>(vertices);
        }
    }
}
