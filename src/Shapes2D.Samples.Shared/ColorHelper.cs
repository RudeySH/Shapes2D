using System;
using Microsoft.Xna.Framework;

namespace Shapes2D.Samples.Shared
{
    /// <summary>
    /// Various static and extension methods for colors.
    /// </summary>
    public static class ColorHelper
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Returns a non-transparent random color.
        /// </summary>
        public static Color GetRandomColor()
        {
            var r = Random.Next(0, byte.MaxValue);
            var g = Random.Next(0, byte.MaxValue);
            var b = Random.Next(0, byte.MaxValue);

            return FromRgb(r, g, b);
        }

        /// <summary>
        /// Returns a new color using integral RGB values ranging from 0 to 255.
        /// </summary>
        public static Color FromRgb(int r, int g, int b)
        {
            const float maxValue = byte.MaxValue - 1;
            return FromRgb(r / maxValue,
                           g / maxValue,
                           b / maxValue);
        }

        /// <summary>
        /// Returns a new color using floating-point RGB values ranging from 0f to 1f.
        /// </summary>
        public static Color FromRgb(float r, float g, float b)
        {
            return new Color(new Vector3(r, g, b));
        }

        /// <summary>
        /// Decreases the luminosity of a color by a random amount.
        /// </summary>
        public static Color Darken(this Color color)
        {
            return Darken(color, 0, 1);
        }

        /// <summary>
        /// Decreases the luminosity of a color by a random amount that is between a specified range.
        /// </summary>
        public static Color Darken(this Color color, float minAmount, float maxAmount)
        {
            return Darken(color, (float)Random.NextDouble() * (maxAmount - minAmount) + minAmount);
        }

        /// <summary>
        /// Decreases the luminosity of a color by a specified amount.
        /// </summary>
        public static Color Darken(this Color color, float amount)
        {
            return new Color(color.ToVector3() * (1 - amount));
        }

        /// <summary>
        /// Increases the luminosity of a color by a random amount.
        /// </summary>
        public static Color Lighten(this Color color)
        {
            return Lighten(color, 0, 1);
        }

        /// <summary>
        /// Increases the luminosity of a color by a random amount that is between a specified range.
        /// </summary>
        public static Color Lighten(this Color color, float minAmount, float maxAmount)
        {
            return Lighten(color, (float)Random.NextDouble() * (maxAmount - minAmount) + minAmount);
        }

        /// <summary>
        /// Increases the luminosity of a color by a specified amount.
        /// </summary>
        public static Color Lighten(this Color color, float amount)
        {
            var vector = color.ToVector3();
            return new Color(vector + amount * (Vector3.One - vector));
        }
    }
}
