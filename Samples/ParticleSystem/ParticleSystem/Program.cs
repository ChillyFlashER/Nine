using System;

namespace ParticleSystem
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ParticleSystemGame game = new ParticleSystemGame())
            {
                game.Run();
            }
        }
    }
#endif
}

