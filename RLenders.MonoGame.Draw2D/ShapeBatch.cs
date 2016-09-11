using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RLenders.MonoGame.Draw2D.Shapes;

namespace RLenders.MonoGame.Draw2D
{
    public class ShapeBatch : DrawableGameComponent
    {
        private BasicEffect basicEffect;

        /// <summary>
        /// Shapes in this collection will be drawed each update, as long as a shape's Visible property is set to true.
        /// </summary>
        public Collection<Shape> Shapes { get; } = new Collection<Shape>();

        public ShapeBatch(Game game) : base(game) { }

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

            var visible = false; // indicates if at least one shape is visible
            var triangeList = new Collection<VertexPositionColor>();
            var lineList = new Collection<VertexPositionColor>();

            // collect all fill and stroke data from all shapes
            for (var i = 0; i < Shapes.Count; i++)
            {
                var shape = Shapes[i];

                if (shape.Visible)
                {
                    visible = true;

                    var polygon = shape as Polygon;

                    if (polygon != null)
                    {
                        if (polygon.Fill != Color.Transparent)
                        {
                            for (var j = 0; j < polygon.TriangleList.Length; j++)
                            {
                                var position = polygon.TriangleList[j] * polygon.Size;
                                if (polygon.Rotation != 0) position = Vector2.Transform(position, Matrix.CreateRotationZ(polygon.Rotation));
                                position = position * polygon.Scale + polygon.Position;

                                triangeList.Add(new VertexPositionColor(new Vector3(position, 0), polygon.Fill));
                            }
                        }

                        if (polygon.Stroke != Color.Transparent)
                        {
                            for (var j = 0; j < polygon.Vertices.Length * 2; j++)
                            {
                                var position = polygon.Vertices[(j + 1) / 2 % polygon.Vertices.Length] * polygon.Size;
                                if (polygon.Rotation != 0) position = Vector2.Transform(position, Matrix.CreateRotationZ(polygon.Rotation));
                                position = position * polygon.Scale + polygon.Position;

                                lineList.Add(new VertexPositionColor(new Vector3(position, 0), polygon.Stroke));
                            }
                        }
                    }
                }
            }

            if (visible)
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
