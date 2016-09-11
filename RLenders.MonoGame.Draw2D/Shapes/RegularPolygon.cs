using System;
using Microsoft.Xna.Framework;

namespace RLenders.MonoGame.Draw2D.Shapes
{
    /// <summary>
    /// A regular polygon is a polygon that is equiangular (all angles are equal in measure) and equilateral (all sides have the same length).
    /// </summary>
    public class RegularPolygon : ConvexPolygon
    {
        public RegularPolygon(ShapeBatch shapeBatch, int sides, ShapeSizing shapeSizing = ShapeSizing.AutoHeight) : base(shapeBatch, new Vector2[sides])
        {
            var angle = Math.PI * 2 / sides;

            var min = Vector2.Zero;
            var max = Vector2.Zero;

            for (var i = 0; i < sides; i++)
            {
                var radians = (float)(angle * (i + (sides % 2 == 0 ? 0.5f : 0f)));
                var vertex = Vertices[i] = Vector2.Transform(-Vector2.UnitY / 2, Matrix.CreateRotationZ(radians));
                if (min.X > vertex.X) min.X = vertex.X; else if (max.X < vertex.X) max.X = vertex.X;
                if (min.Y > vertex.Y) min.Y = vertex.Y; else if (max.Y < vertex.Y) max.Y = vertex.Y;
            }

            if (shapeSizing != ShapeSizing.Circumcircle)
            {
                var scale = max - min;
                scale.X = 1 / scale.X;
                scale.Y = 1 / scale.Y;

                switch (shapeSizing)
                {
                    case ShapeSizing.AutoWidth:
                        scale.X = scale.Y;
                        break;

                    case ShapeSizing.AutoHeight:
                        scale.Y = scale.X;
                        break;
                }

                for (var i = 0; i < Vertices.Length; i++)
                {
                    Vertices[i] *= scale;
                }
            }
        }
    }
}
