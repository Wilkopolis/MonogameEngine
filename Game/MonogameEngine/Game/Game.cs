using System;

using Microsoft.Xna.Framework;

#pragma warning disable IDE0017 // Simplify object initialization
#pragma warning disable IDE0019 // Use pattern matching
#pragma warning disable IDE0028 // 

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public static int Hash = 0;
        public static int Id = 0;
        public static Random Random = new Random();

        void Game_SwitchToGame()
        {

        }

        void Game_Update(float delta)
        {
        }

        void Game_KeyboardHandler()
        {
            if (GetFlag("InEditor"))
                Editor_KeyboardHandler();
        }
    }
}
