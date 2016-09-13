using System;
using Microsoft.Xna.Framework;

namespace Shapes2D
{
    /// <summary>
    /// A regular polygon is a polygon that is equiangular (all angles are equal in measure) and equilateral (all sides have the same length).
    /// </summary>
    public class RegularPolygon : ConvexPolygon
    {
        public RegularPolygon(int edges, VertexScaling shapeScaling = VertexScaling.AutoHeight) : base(new Vector2[edges])
        {
            var angle = Math.PI * 2 / edges;

            var min = Vector2.Zero;
            var max = Vector2.Zero;

            for (var i = 0; i < edges; i++)
            {
                var radians = (float)(angle * (i + (edges % 2 == 0 ? 0.5f : 0f)));
                var vertex = Vertices[i] = Vector2.Transform(-Vector2.UnitY / 2, Matrix.CreateRotationZ(radians)); // TODO: simplify
                if (min.X > vertex.X) min.X = vertex.X; else if (max.X < vertex.X) max.X = vertex.X;
                if (min.Y > vertex.Y) min.Y = vertex.Y; else if (max.Y < vertex.Y) max.Y = vertex.Y;
            }

            if (shapeScaling != VertexScaling.Circumcircle)
            {
                var scale = max - min;
                scale.X = 1 / scale.X;
                scale.Y = 1 / scale.Y;

                switch (shapeScaling)
                {
                    case VertexScaling.AutoWidth:
                        scale.X = scale.Y;
                        break;

                    case VertexScaling.AutoHeight:
                        scale.Y = scale.X;
                        break;
                }

                for (var i = 0; i < Vertices.Count; i++)
                {
                    Vertices[i] *= scale;
                }
            }
        }
    }
}
