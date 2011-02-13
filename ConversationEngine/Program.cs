using System;

namespace ConversationEngine
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ConversationEngine game = new ConversationEngine())
            {
                game.Run();
            }
        }
    }
#endif
}

