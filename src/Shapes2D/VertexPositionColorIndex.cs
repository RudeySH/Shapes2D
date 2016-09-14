using Microsoft.Xna.Framework;

namespace Shapes2D
{
    public struct VertexPositionColorIndex
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public int Index { get; set; }

        public VertexPositionColorIndex(Vector2 position, Color color, int index)
        {
            Position = position;
            Color = color;
            Index = index;
        }
    }
}
