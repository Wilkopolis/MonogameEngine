using Microsoft.Xna.Framework;
using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class ItemThrowAnimation : Animation
        {
            // target is a character
            Element Victim;
            // target is a tile
            Vector2 Destination;
            // scales based on distance
            public float Duration = 0;
            public double LastRun = 0;
            // scales 
            public float Height = -.05f;
            public float RotStep = -.4f;

            public ItemThrowAnimation(Element target, Element victim, Action handler = null)
            {
                this.Target = target;
                this.Victim = victim;

                this.Callback = handler;

                target.Animations.Add(this);

                // make this scale based on distance
                this.Duration = 1000;

                this.Begin();
            }

            public ItemThrowAnimation(Element target, Vector2 destination, Action handler = null)
            {
                this.Target = target;
                this.Destination = destination;

                this.Callback = handler;

                target.Animations.Add(this);

                // make this scale based on distance
                this.Duration = 1000;

                this.Begin();
            }

            public override void Begin()
            {
                base.Begin();

                this.LastRun = this.StartTime;
            }

            public override void Tick()
            {
                double dt = (MsEllapsed - this.LastFrame);
                this.Elapsed += dt;
                
                float percentComplete = (float)this.Elapsed / this.Duration;

                // handle roation
                this.Target.Rotation += this.RotStep;

                // handle position
                Vector2 dest = this.Destination;

                // set the offset as a fraction of the cast time
                float xOffset = percentComplete * (dest.X - this.Target.Position.X - this.Target.Width / 2);
                float yOffset = this.Height * (float)Math.Sin(percentComplete * Math.PI) + percentComplete * (dest.Y - this.Target.Position.Y);
                this.Offset = new Vector2(xOffset, yOffset);

                if (percentComplete >= 1)
                    this.Complete();
            }

            public override void Complete()
            {
                this.Over = true;
                this.Running = false;
                this.Target.Position += this.Offset;
                this.Target.Animations.Remove(this);
            }
        }
    }
}
