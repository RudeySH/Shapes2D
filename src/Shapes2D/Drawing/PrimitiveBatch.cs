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
            
            var triangeList = new Collection<VertexPositionColor>();
            var lineList = new Collection<VertexPositionColor>();

            // collect all fill and stroke data from all primitives
            for (var i = 0; i < Primitives.Count; i++)
            {
                var primitive = Primitives[i];
                
                var shape = primitive as Shape;

                if (shape != null && shape.Fill != Color.Transparent)
                {
                    for (var j = 0; j < shape.TriangleList.Length; j++)
                    {
                        var position = shape.TriangleList[j] * primitive.Size;
                        if (primitive.Rotation != 0) position = Vector2.Transform(position, Matrix.CreateRotationZ(primitive.Rotation));
                        position = position * primitive.Stretch + primitive.Position;

                        triangeList.Add(new VertexPositionColor(new Vector3(position, 0), shape.Fill));
                    }
                }

                if (primitive.Stroke != Color.Transparent)
                {
                    for (var j = 0; j < primitive.Vertices.Length * 2; j++)
                    {
                        var position = primitive.Vertices[(j + 1) / 2 % primitive.Vertices.Length] * primitive.Size;
                        if (primitive.Rotation != 0) position = Vector2.Transform(position, Matrix.CreateRotationZ(primitive.Rotation));
                        position = position * primitive.Stretch + primitive.Position;

                        lineList.Add(new VertexPositionColor(new Vector3(position, 0), primitive.Stroke));
                    }
                }
            }

            if (triangeList.Count != 0 || lineList.Count != 0)
            {
                var triangleListData = triangeList.ToArray();
                var lineListData = lineList.ToArray();

                for (var i = 0; i < basicEffect.CurrentTechnique.Passes.Count; i++)
                {
                    basicEffect.CurrentTechnique.Passes[i].Apply();

                    if (triangleListData.Length != 0)
                    {
                        GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleListData, 0, triangleListData.Length / 3);
                    }

                    if (lineListData.Length != 0)
                    {
                        GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, lineListData, 0, lineListData.Length / 2);
                    }
                }
            }
        }
    }
}
