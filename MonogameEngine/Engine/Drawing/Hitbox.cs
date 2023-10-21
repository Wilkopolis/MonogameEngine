using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum HitBoxType
        {
            Texture, Rect, Circle, Polygon, Compound
        }

        public abstract partial class Element
        {
            public bool MouseOverCheck = false;

            // for mouse over
            public HitBoxType HitBoxType = HitBoxType.Texture;

            // for circle hit boxes
            public float Radius = 0;

            public bool IsMouseOver()
            {
                // if this is turned off, do not check this
                if (this is not Screen && !this.MouseOverCheck)
                    return false;

                // if our screen is uninteractive, then do not check
                if (!this.IsVisible())
                    return false;

                // check if we are within the screen
                if (this is not Screen && !this.GetScreen().IsMouseOver())
                    return false;

                switch (this.HitBoxType)
                {
                    case HitBoxType.Texture:

                        Vector2 pos = this.AbsPos();

                        Vector2 p = new Vector2(pos.X, pos.Y);

                        Vector2 p1 = p;
                        Vector2 p2 = p + new Vector2(this.Width, 0);
                        Vector2 p3 = p + new Vector2(this.Width, this.Height);
                        Vector2 p4 = p + new Vector2(0, this.Height);

                        if (this.Rotation != 0)
                        {
                            if (this.RotationStyle == RotationStyle.Remote)
                            {
                                p1 = p;
                                p2 = Rotate(p, p + new Vector2(this.Width, 0), this.Rotation);
                                p3 = Rotate(p, p + new Vector2(this.Width, this.Height), this.Rotation);
                                p4 = Rotate(p, p + new Vector2(0, this.Height), this.Rotation);
                            }
                            else if (this.RotationStyle == RotationStyle.Fixed)
                            {
                                p1 = Rotate(p + this.Origin, p, this.Rotation);
                                p2 = Rotate(p + this.Origin, p + new Vector2(this.Width, 0), this.Rotation);
                                p3 = Rotate(p + this.Origin, p + new Vector2(this.Width, this.Height), this.Rotation);
                                p4 = Rotate(p + this.Origin, p + new Vector2(0, this.Height), this.Rotation);
                            }
                            else if (this.RotationStyle == RotationStyle.InPlace)
                            {
                                p1 = Rotate(p + this.Origin * this.Scale, p, this.Rotation);
                                p2 = Rotate(p + this.Origin * this.Scale, p + new Vector2(this.Width, 0), this.Rotation);
                                p3 = Rotate(p + this.Origin * this.Scale, p + new Vector2(this.Width, this.Height), this.Rotation);
                                p4 = Rotate(p + this.Origin * this.Scale, p + new Vector2(0, this.Height), this.Rotation);
                            }
                        }

                        double x1 = p1.X;
                        double x2 = p2.X;
                        double x3 = p3.X;
                        double x4 = p4.X;

                        double y1 = p1.Y;
                        double y2 = p2.Y;
                        double y3 = p3.Y;
                        double y4 = p4.Y;

                        double a1 = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
                        double a2 = Math.Sqrt((x2 - x3) * (x2 - x3) + (y2 - y3) * (y2 - y3));
                        double a3 = Math.Sqrt((x3 - x4) * (x3 - x4) + (y3 - y4) * (y3 - y4));
                        double a4 = Math.Sqrt((x4 - x1) * (x4 - x1) + (y4 - y1) * (y4 - y1));

                        double b1 = Math.Sqrt((x1 - CurrentMouseState.X) * (x1 - CurrentMouseState.X) + (y1 - CurrentMouseState.Y) * (y1 - CurrentMouseState.Y));
                        double b2 = Math.Sqrt((x2 - CurrentMouseState.X) * (x2 - CurrentMouseState.X) + (y2 - CurrentMouseState.Y) * (y2 - CurrentMouseState.Y));
                        double b3 = Math.Sqrt((x3 - CurrentMouseState.X) * (x3 - CurrentMouseState.X) + (y3 - CurrentMouseState.Y) * (y3 - CurrentMouseState.Y));
                        double b4 = Math.Sqrt((x4 - CurrentMouseState.X) * (x4 - CurrentMouseState.X) + (y4 - CurrentMouseState.Y) * (y4 - CurrentMouseState.Y));

                        double u1 = (a1 + b1 + b2) / 2;
                        double u2 = (a2 + b2 + b3) / 2;
                        double u3 = (a3 + b3 + b4) / 2;
                        double u4 = (a4 + b4 + b1) / 2;

                        double A1 = Math.Sqrt(u1 * (u1 - a1) * (u1 - b1) * (u1 - b2));
                        double A2 = Math.Sqrt(u2 * (u2 - a2) * (u2 - b2) * (u2 - b3));
                        double A3 = Math.Sqrt(u3 * (u3 - a3) * (u3 - b3) * (u3 - b4));
                        double A4 = Math.Sqrt(u4 * (u4 - a4) * (u4 - b4) * (u4 - b1));

                        double difference = A1 + A2 + A3 + A4 - a1 * a2;
                        return difference < 1;

                    case HitBoxType.Circle:

                    return true;

                    case HitBoxType.Polygon:

                    return true;

                    case HitBoxType.Compound:

                        if (this is CompoundElement combo)
                        {
                            foreach (Element e in combo.GetElements())
                            {
                                if (e.IsMouseOver())
                                    return true;
                            }
                        }

                    return false;
                }

                return false;
            }
        }
    }
}
