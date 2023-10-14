using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class Rotation : Animation
        {
            float Duration;
            float TargetRotation;
            float StartingRotation;

            float Delta;

            public Rotation(Element target, int duration, float targetRotation, Action callback = null)
            {
                this.Target = target;
                this.Duration = duration;
                this.TargetRotation = targetRotation;

                this.Callback = callback;

                target.Animations.Add(this);

                this.Begin();
            }

            public Rotation(Element target, float delta, Action callback = null)
            {
                this.Target = target;
                this.Delta = delta;

                this.Callback = callback;

                target.Animations.Add(this);

                this.Begin();
            }

            public override void Begin()
            {
                this.StartingRotation = this.Target.Rotation;

                base.Begin();
            }

            public override void Tick()
            {
                double dt = (MsEllapsed - this.LastFrame);
                this.Elapsed += dt;
                
                if (this.Duration > 0)
                {
                    float percentComplete = (float)this.Elapsed / this.Duration;

                    this.Target.Rotation = percentComplete * (this.TargetRotation - this.StartingRotation) + this.StartingRotation;

                    if (percentComplete >= 1)
                        this.Complete();
                }
                else
                {
                    this.Target.Rotation += this.Delta;
                }
            }

            public override void Complete()
            {
                if (this.Duration > 0)
                    this.Target.Rotation = this.TargetRotation;

                this.Over = true;
                this.Running = false;
                this.Target.Animations.Remove(this);
            }
        }
    }
}
