using System;
using Microsoft.Xna.Framework;
using Shapes2D.Drawing;
using Shapes2D.Samples.Shared;

namespace Shapes2D.Samples.Animation
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class AnimationSample : SampleGame
    {
        private readonly PrimitiveBatch primitiveBatch;
        private Polygon water;

        private const uint WaveSegments = 10;
        private const float WaveLength = 100;
        private const float WaveHeight = 10;
        private const float WaveSpeed = 10;

        public AnimationSample()
        {
            // Create a new PrimitiveBatch, which can be used to draw 2D lines and shapes.
            primitiveBatch = new PrimitiveBatch(this);
            Components.Add(primitiveBatch);
        }

        protected override void Initialize()
        {
            base.Initialize();

            var offset = new Vector2(10);
            var width = Graphics.PreferredBackBufferWidth - offset.X * 2;
            var height = Graphics.PreferredBackBufferHeight / 2f - offset.Y;

            var vertices = new Vector2[WaveSegments + 3];
            var segmentLength = width / WaveSegments;

            for (var i = 0; i <= WaveSegments; i++)
            {
                vertices[i] = new Vector2(i * segmentLength, 0);
            }

            vertices[WaveSegments + 1] = new Vector2(width, height); // bottom-right
            vertices[WaveSegments + 2] = new Vector2(0, height); // bottom-left

            water = new Polygon(vertices) // Don't use ConvexPolygon because the animation will make it concave.
            {
                Position = new Vector2(offset.X, Graphics.PreferredBackBufferHeight - height - offset.Y),
                Fill = Color.Blue
            };

            primitiveBatch.Primitives.Add(water);
        }

        protected override void Update(GameTime gameTime)
        {
            var waveOffset = gameTime.TotalGameTime.TotalSeconds * WaveSpeed;

            for (var i = 0; i < water.Vertices.Count - 2; i++)
            {
                var vertex = water.Vertices[i];
                vertex.Y = (float)Math.Sin(vertex.X / WaveLength + waveOffset) * WaveHeight;
                water.Vertices[i] = vertex;
            }

            base.Update(gameTime);
        }
    }
}
