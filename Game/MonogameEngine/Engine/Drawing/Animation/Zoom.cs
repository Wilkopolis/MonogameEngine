using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class Zoom : Animation
        {
            float Duration;
            float TargetScale;
            float StartingScale;

            public Zoom(Element target, int duration, float targetScale, Action callback = null)
            {
                this.Target = target;
                this.Duration = duration;
                this.TargetScale = targetScale;

                this.Callback = callback;

                target.Animations.Remove(this);

                this.Begin();
            }

            public override void Begin()
            {
                this.StartingScale = this.Target.Scale;

                base.Begin();
            }

            public override void Tick()
            {
                double dt = (MsEllapsed - this.LastFrame);
                this.Elapsed += dt;
                
                float percentComplete = (float)this.Elapsed / this.Duration;

                this.Target.Scale = percentComplete * (this.TargetScale - this.StartingScale) + this.StartingScale;

                if (percentComplete >= 1)
                    this.Complete();
            }

            public override void Complete()
            {
                this.Target.Scale = this.TargetScale;

                this.Over = true;
                this.Running = false;
                this.Target.Animations.Remove(this);
            }
        }
    }
}
