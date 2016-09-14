using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shapes2D.Drawing
{
    public class PrimitiveBatch : DrawableGameComponent
    {
        private BasicEffect basicEffect;

        /// <summary>
        /// Primitives in this collection will be drawed each update, as long as a primitive's Visible property is set to true.
        /// </summary>
        public Collection<Primitive> Primitives { get; } = new Collection<Primitive>();

        /// <summary>
        /// Draw 2D lines and shapes.
        /// </summary>
        public PrimitiveBatch(Game game) : base(game) { }

        public override void Initialize()
        {
            GraphicsDevice.RasterizerState = new RasterizerState
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None,
                MultiSampleAntiAlias = true
            };

            basicEffect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up),
                Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 1.0f, 1000.0f)
            };

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var visible = false;
            var lineVertices = new Collection<VertexPositionColor>();
            var lineIndices = new Collection<int>();
            var triangleVertices = new Collection<VertexPositionColor>();
            var triangleIndices = new Collection<int>();

            for (var i = 0; i < Primitives.Count; i++)
            {
                var primitive = Primitives[i];

                var shape = primitive as Shape;

                if (primitive.Stroke != Color.Transparent || shape != null && shape.Fill != Color.Transparent)
                {
                    visible = true;

                    var positions = new Vector3[primitive.Vertices.Count];
                    for (var j = 0; j < primitive.Vertices.Count; j++)
                    {
                        var position = primitive.Vertices[j] * primitive.Size;
                        if (primitive.Rotation != 0) position = Vector2.Transform(position, Matrix.CreateRotationZ(primitive.Rotation));
                        position = position * primitive.Stretch + primitive.Position;
                        positions[j] = new Vector3(position, 0);
                    }

                    if (primitive.Stroke != Color.Transparent)
                    {
                        var offset = lineVertices.Count;
                        for (var j = 0; j < positions.Length; j++)
                        {
                            lineVertices.Add(new VertexPositionColor(positions[j], primitive.Stroke));
                            lineIndices.Add(offset + j);
                            lineIndices.Add(offset + (j + 1) % primitive.Vertices.Count);
                        }
                    }

                    if (shape != null && shape.Fill != Color.Transparent)
                    {
                        var offset = triangleVertices.Count;
                        for (var j = 0; j < positions.Length; j++)
                        {
                            triangleVertices.Add(new VertexPositionColor(positions[j], shape.Fill));
                        }

                        if (shape.TriangleIndices == null)
                        {
                            shape.Triangulate();
                        }

                        for (var j = 0; j < shape.TriangleIndices.Length; j++)
                        {
                            triangleIndices.Add(offset + shape.TriangleIndices[j]);
                        }
                    }
                }
            }

            if (visible)
            {
                var triangleVertexData = triangleVertices.ToArray();
                var triangleIndexData = triangleIndices.ToArray();
                var lineVertexData = lineVertices.ToArray();
                var lineIndexData = lineIndices.ToArray();

                for (var i = 0; i < basicEffect.CurrentTechnique.Passes.Count; i++)
                {
                    basicEffect.CurrentTechnique.Passes[i].Apply();

                    if (triangleIndexData.Length != 0)
                    {
                        GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, triangleVertexData, 0, triangleVertexData.Length, triangleIndexData, 0, triangleIndexData.Length / 3);
                    }

                    if (lineIndexData.Length != 0)
                    {
                        GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, lineVertexData, 0, lineVertexData.Length, lineIndexData, 0, lineIndexData.Length / 2);
                    }
                }
            }
        }
    }
}
