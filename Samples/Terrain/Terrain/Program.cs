
namespace TerrainSample
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TerrainGame game = new TerrainGame())
            {
                game.Run();
            }
        }
    }
#endif
}

