using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Shapes2D
{
    public class Shape : Primitive
    {
        /// <summary>
        /// Triangulated vertices.
        /// </summary>
        public Vector2[] TriangleList { get; protected set; }

        /// <summary>
        /// Color of the surface.
        /// </summary>
        public Color Fill
        {
            get { return fill; }
            set
            {
                fill = value;
                RefreshTriangulation();
            }
        }

        private Color fill = Color.White;

        public Shape(Vector2[] vertices) : base(vertices)
        {
            RefreshTriangulation();
        }

        /// <summary>
        /// Triangulates the polygon if needed.
        /// </summary>
        protected void RefreshTriangulation()
        {
            if (Fill != Color.Transparent && Vertices.Length > 2 && TriangleList == null && Vertices.Any(v => v != Vector2.Zero))
            {
                Triangulate();
            }
        }

        /// <summary>
        /// Triangulates the shape, splitting it into triangles.
        /// </summary>
        protected virtual void Triangulate()
        {
            throw new NotImplementedException();
        }
    }
}
