using System;

namespace Shapes2D.Samples.Animation
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new AnimationSample())
                game.Run();
        }
    }
}
