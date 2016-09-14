using System.Collections.Generic;
using System.Collections.Specialized;
using LibTessDotNet;
using Microsoft.Xna.Framework;

namespace Shapes2D
{
    public class Shape : Primitive
    {
        /// <summary>
        /// Triangulated vertices.
        /// </summary>
        internal int[] TriangleIndices { get; private set; }

        /// <summary>
        /// Color of the surface.
        /// </summary>
        public Color Fill { get; set; } = Color.White;

        private static Tess tess;

        public Shape(IEnumerable<Vector2> vertices) : base(vertices)
        {
            Stroke = Color.Transparent;
            Vertices.CollectionChanged += Vertices_CollectionChanged;
        }

        private void Vertices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace && e.OldItems.Count == e.NewItems.Count)
            {
                var equal = true;

                for (var i = 0; i < e.OldItems.Count; i++)
                {
                    if ((Vector2)e.OldItems[i] != (Vector2)e.NewItems[i])
                    {
                        equal = false;
                        break;
                    }
                }

                if (equal)
                {
                    return;
                }
            }

            TriangleIndices = null;
        }

        /// <summary>
        /// Triangulates the shape, splitting it into triangles.
        /// </summary>
        internal void Triangulate()
        {
            TriangleIndices = GetTriangleIndices();
        }

        protected virtual int[] GetTriangleIndices()
        {
            if (tess == null) tess = new Tess();

            var contour = new ContourVertex[Vertices.Count];

            for (var i = 0; i < contour.Length; i++)
            {
                var vertex = Vertices[i];
                contour[i].Position = new Vec3 { X = vertex.X, Y = vertex.Y };
            }

            tess.AddContour(contour, ContourOrientation.Clockwise);
            tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3);

            var indices = new int[tess.Elements.Length];

            for (var i = 0; i < indices.Length; i++)
            {
                var position = tess.Vertices[tess.Elements[i]].Position;
                indices[i] = Vertices.IndexOf(new Vector2(position.X, position.Y));
            }

            return indices;
        }
    }
}
