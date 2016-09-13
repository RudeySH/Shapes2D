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
        public Vector2[] TriangleList { get; protected set; }

        /// <summary>
        /// Color of the surface.
        /// </summary>
        public Color Fill { get; set; } = Color.White;

        private static Tess tess;

        public Shape(IEnumerable<Vector2> vertices) : base(vertices)
        {
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

            TriangleList = null;
        }

        /// <summary>
        /// Triangulates the shape, splitting it into triangles.
        /// </summary>
        public virtual void Triangulate()
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

            TriangleList = new Vector2[tess.Elements.Length];

            for (var i = 0; i < TriangleList.Length; i++)
            {
                var position = tess.Vertices[tess.Elements[i]].Position;
                TriangleList[i] = new Vector2(position.X, position.Y);
            }
        }
    }
}
