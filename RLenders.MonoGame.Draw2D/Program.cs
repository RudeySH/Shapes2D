using System;
using RLenders.MonoGame.Draw2D.Demo;

namespace RLenders.MonoGame.Draw2D
{
#if WINDOWS || LINUX
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
            using (var game = new GridDemo())
                game.Run();
        }
    }
#endif
}
