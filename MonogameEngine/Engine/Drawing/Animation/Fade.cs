using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class Fade : Animation
        {
            float Duration;
            float TargetAlpha;
            float StartingAlpha;

            public Fade(Element target, int duration, float targetAlpha, Action callback = null)
            {
                this.Target = target;
                this.Duration = duration;
                this.TargetAlpha = targetAlpha;

                this.Callback = callback;

                this.Target.Animations.Add(this);

                this.Begin();
            }

            public override void Begin()
            {
                this.StartingAlpha = this.Target.Alpha;

                base.Begin();
            }

            public override void Tick()
            {
                double dt = (MsEllapsed - this.LastFrame);
                this.Elapsed += dt;

                float percentComplete = (float)this.Elapsed / this.Duration;
                
                float factor = EasingFunc(percentComplete, this.Easing);

                this.Target.Alpha = factor * (this.TargetAlpha - this.StartingAlpha) + this.StartingAlpha;

                if (percentComplete >= 1)
                    this.Complete();
            }

            public override void Complete()
            {
                this.Target.Alpha = this.TargetAlpha;

                this.Over = true;
                this.Running = false;
                this.Target.Animations.Remove(this);
            }
        }
    }
}
