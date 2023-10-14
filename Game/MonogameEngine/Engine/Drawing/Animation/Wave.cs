using Microsoft.Xna.Framework;
using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class WaveAnimation : Animation
        {
            float Period;

            public WaveAnimation(Element target, float period, Vector2 offset, Action callback = null)
            {
                this.Target = target;
                // convert pixels to %
                this.GoalOffset = offset;
                this.Period = period;

                this.Callback = callback;

                target.Animations.Add(this);

                this.Begin();
            }
            
            public override void Tick()
            {
                double dt = (MsEllapsed - this.LastFrame);
                this.Elapsed += dt;
                
                float percentComplete = (float)this.Elapsed / this.Period;

                this.Offset = this.GoalOffset * (float)Math.Sin(percentComplete * 2 * Math.PI);
            }

            public override void Complete()
            {                
                this.Over = true;
                this.Running = false;
                this.Target.Animations.Remove(this);
            }
        }
    }
}
