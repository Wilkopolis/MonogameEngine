using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#pragma warning disable IDE0017 // Simplify object initialization

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class Button : CompoundElement
        {
            public Vector2 PushedOffset = new Vector2(1, 2);

            public Button()
            {
                this.HitBoxType = HitBoxType.Compound;
                this.Cursor = CursorType.Pointer;
                this.Clickable = true;
                this.MouseOverCheck = true;
            }

            public override void Draw()
            {
                if (this.IsPressed())
                    this.Offsets["pushed"] = this.PushedOffset;

                base.Draw();
                
                if (this.IsPressed())
                    this.Offsets["pushed"] = new Vector2();
            }
        }
    }
}
