using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RLenders.MonoGame.Draw2D.Shapes
{
    public class Polygon : Shape
    {
        public override Color Fill
        {
            get { return base.Fill; }
            set
            {
                base.Fill = value;
                RefreshTriangulation();
            }
        }

        /// <summary>
        /// Clockwise collection of coordinates.
        /// </summary>
        public Vector2[] Vertices
        {
            get { return vertices; }
            protected set
            {
                vertices = value;
                TriangleList = null;
                RefreshTriangulation();
            }
        }

        private Vector2[] vertices;

        /// <summary>
        /// Vertices from triangles inside polygon.
        /// </summary>
        public Vector2[] TriangleList { get; protected set; }

        protected Polygon(ShapeBatch shapeBatch, Vector2[] vertices) : base(shapeBatch)
        {
            Vertices = vertices;
        }

        public override void Initialize()
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
        /// Triangulates the polygon, splitting the shape into triangles.
        /// </summary>
        protected virtual void Triangulate()
        {
            throw new NotImplementedException();
        }
    }
}
