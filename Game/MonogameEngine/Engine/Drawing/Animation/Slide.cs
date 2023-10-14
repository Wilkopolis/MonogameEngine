using Microsoft.Xna.Framework;
using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class SlideAnimation : Animation
        {
            public float Duration;

            public SlideAnimation(Element target, float duration, float percentX, float percentY, Action callback = null)
            {
                this.Target = target;
                this.GoalOffset = new Vector2(percentX, percentY);
                this.Duration = duration;

                this.Callback = callback;

                target.Animations.Add(this);

                this.Begin();
            }

            public override void Tick()
            {
                double dt = (MsEllapsed - this.LastFrame);
                this.Elapsed += dt;
                
                float percentComplete = (float)this.Elapsed / this.Duration;

                if (percentComplete >= 1)
                {
                    this.Complete();
                    return;
                }

                // Linear
                float factor = EasingFunc(percentComplete, this.Easing);

                Vector2 adjustedOffset = this.GoalOffset * factor;
                this.Offset = adjustedOffset;
            }

            public override void Complete()
            {
                this.Target.Position += this.GoalOffset;
                this.Over = true;
                this.Running = false;
                this.Target.Animations.Remove(this);
            }
        }
    }
}
