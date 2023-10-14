using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum CursorType
        {
            Arrow, Pointer, Grab, Holding, Glass, Book, Return, Hidden
        }

        Dictionary<CursorType, MouseCursor> Cursors = new Dictionary<CursorType, MouseCursor>();
        public static CursorType TargetCursor = CursorType.Arrow;

        public void InitCursors()
        {
            Cursors[CursorType.Arrow] = MouseCursor.FromTexture2D(Textures["core/cursors/standard"].Texture, 2, 2);
            Cursors[CursorType.Glass] = MouseCursor.FromTexture2D(Textures["core/cursors/glass"].Texture, 24, 21);
            Cursors[CursorType.Book] = MouseCursor.FromTexture2D(Textures["core/cursors/book"].Texture, 4, 4);
            Cursors[CursorType.Pointer] = MouseCursor.FromTexture2D(Textures["core/cursors/hand1"].Texture, 2, 8);
            Cursors[CursorType.Grab] = MouseCursor.FromTexture2D(Textures["core/cursors/hand2"].Texture, 12, 12);
            Cursors[CursorType.Holding] = MouseCursor.FromTexture2D(Textures["core/cursors/hand3"].Texture, 12, 12);
            Cursors[CursorType.Return] = MouseCursor.FromTexture2D(Textures["core/cursors/return"].Texture, 4, 4);
            Cursors[CursorType.Hidden] = MouseCursor.FromTexture2D(Textures["core/empty"].Texture, 0, 0);
        }
    }
}
