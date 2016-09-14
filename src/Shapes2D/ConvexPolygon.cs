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

        protected override int[] GetTriangleIndices()
        {
            var indices = new int[(Vertices.Count - 2) * 3];

            var verticesToTriangulate = new List<VertexPositionColorIndex>();
            for (var i = 0; i < Vertices.Count; i++)
            {
                verticesToTriangulate.Add(new VertexPositionColorIndex { Position = Vertices[i], Index = i });
            }

            var triangleIndex = 0;
            var cycleIndex = 0;

            while (verticesToTriangulate.Count >= 3)
            {
                // take first vertex
                indices[triangleIndex++] = verticesToTriangulate[cycleIndex].Index;

                // cycle to next vertex
                if (++cycleIndex == verticesToTriangulate.Count) cycleIndex = 0;

                // take second vertex
                indices[triangleIndex++] = verticesToTriangulate[cycleIndex].Index;

                // remove ear vertex and cycle to next vertex
                verticesToTriangulate.RemoveAt(cycleIndex--);
                if (++cycleIndex == verticesToTriangulate.Count) cycleIndex = 0;

                // take third vertex
                indices[triangleIndex++] = verticesToTriangulate[cycleIndex].Index;
            }

            return indices;
        }
    }
}
