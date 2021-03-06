using System;

namespace ArenaXNA
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameClass game = new GameClass())
            {
                game.Run();
            }
        }
    }
#endif
}

