using Microsoft.Xna.Framework;
using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public static Color Col(int r, int g, int b)
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }

        public static class COLORS
        {
            public static Color blank = new Color(0,0,0,0);
            // ui
            public static Color text_bright_1 = new Color(244 / 255f, 244 / 255f, 244 / 255f);
            public static Color text_bright_2 = new Color(.7f, .7f, .7f);
            public static Color text_bright_3 = new Color(171 / 255f, 171 / 255f, 171 / 255f);
            public static Color text_bright_4 = new Color(.8f, .8f, .8f);
        }
    }
}
