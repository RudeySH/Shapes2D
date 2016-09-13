using Microsoft.Xna.Framework;

namespace Shapes2D
{
    public class Polygon : Shape
    {
        public int Edges { get; private set; }

        protected Polygon(Vector2[] vertices) : base(vertices)
        {
            Edges = vertices.Length;
        }
    }
}
