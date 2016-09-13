using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Shapes2D
{
    /// <summary>
    /// Convex: any line drawn through the polygon (and not tangent to an edge or corner) meets its boundary exactly twice.
    /// As a consequence, all its interior angles are less than 180°.
    /// Equivalently, any line segment with endpoints on the boundary passes through only interior points between its endpoints.
    /// </summary>
    public class ConvexPolygon : Polygon
    {
        public ConvexPolygon(IEnumerable<Vector2> vertices) : base(vertices) { }

        public override void Triangulate()
        {
            TriangleList = new Vector2[(Vertices.Count - 2) * 3];

            var vertices = new List<Vector2>(Vertices);
            var triangleListIndex = 0;
            var vertexIndex = 0;

            while (vertices.Count >= 3)
            {
                // take first vertex
                TriangleList[triangleListIndex++] = vertices[vertexIndex];

                // cycle to next vertex
                if (++vertexIndex == vertices.Count) vertexIndex = 0; 

                // take second vertex
                TriangleList[triangleListIndex++] = vertices[vertexIndex];

                // remove ear vertex and cycle to next vertex
                vertices.RemoveAt(vertexIndex--);
                if (++vertexIndex == vertices.Count) vertexIndex = 0;

                // take third vertex
                TriangleList[triangleListIndex++] = vertices[vertexIndex];
            }
        }
    }
}
