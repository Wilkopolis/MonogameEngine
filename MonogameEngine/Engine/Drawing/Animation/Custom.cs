using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class CustomAnimation : Animation
        {
            float Duration;
            Action<float> TickHandler;
            
            public double LastRun = 0;

            public CustomAnimation(int duration, Action<float> tick = null, Action callback = null)
            {
                this.Duration = duration;
                this.TickHandler = tick;

                this.Callback = callback;

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
                
                if (this.Duration == 0)
                {
                    this.TickHandler(0);
                }
                else
                {
                    float percentComplete = (float)this.Elapsed / this.Duration;

                    float factor = EasingFunc(percentComplete, this.Easing);

                    this.TickHandler(factor);

                    if (percentComplete >= 1)
                        this.Complete();
                }
            }

            public override void Complete()
            {
                this.Over = true;
                this.Running = false;
            }
        }
    }
}
