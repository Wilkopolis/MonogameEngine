using Microsoft.Xna.Framework;
using System;

namespace MonogameEngine
{
    public partial class MonogameEngine
    { 
        public enum Easing { Linear, Exponential, Cubic, Quadradic, Quint, Sin, Ln2, Ln5 };

        public abstract class Animation
        {
            public Element Target;
            public Sound Sound;

            public Easing Easing = Easing.Sin;

            public bool Running = true;
            public bool Over = false;
            public bool Flipped = false;
            public int Stage = 0;
            public double StartTime = 0;
            public double Elapsed = 0;

            // for framerate stuff
            public double FrameLength = 0;
            public double LastFrame = 0;

            public Action Callback;

            public string Key = "";

            // instead of tracking animation offsets, 
            // we just pass up an offset
            public Vector2 Offset;
            // used for tracking how much we want to move something
            public Vector2 GoalOffset;

            public abstract void Tick();

            public void Pause()
            {
                this.Running = false;
            }

            public void Unpause()
            {
                this.Running = true;
            }

            public virtual void Complete()
            {
                Sound?.Stop();

                this.Over = true;
                this.Running = false;
                this.Target.Animations.Remove(this);
            }

            public virtual void Begin()
            {
                this.StartTime = MsEllapsed;
                this.Running = true;
                this.LastFrame = this.StartTime;
            }
        }

        public class TimeBuffer : Animation
        {
            float Duration = 0;

            public TimeBuffer(int duration, Action callback = null)
            {
                this.Duration = duration;
                this.Callback = callback;

                this.Begin();
            }
            
            public override void Tick()
            {
                double dt = (MsEllapsed - this.LastFrame);
                this.Elapsed += dt;
                
                float percentComplete = (float)this.Elapsed / this.Duration;

                if (percentComplete >= 1)
                    this.Complete();
            }

            public override void Complete()
            {
                this.Over = true;
                this.Running = false;
            }
        }

        // x has to be between 0 and 1
        public static float EasingFunc(float x, Easing style)
        {
            float factor = (float)Math.Min(1, Math.Max(x, 0));
            switch (style)
            {   
                case Easing.Linear:
                    factor = x;
                    break;
                case Easing.Exponential:
                    factor = x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2f;
                    break;
                case Easing.Cubic:
                    factor = x < 0.5 ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2f;
                    break;
                case Easing.Quadradic:
                    factor = x < 0.5 ? 8 * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 4) / 2f;
                    break;
                case Easing.Quint:
                    factor = x < 0.5 ? 16 * x * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 5) / 2f;
                    break;
                case Easing.Sin:
                    factor = (float)Math.Sin(x * Math.PI / 2);
                    break;
                case Easing.Ln2:
                    factor = 1 - (float)Math.Pow(1 - x, 2);
                    break;
                case Easing.Ln5:
                    factor = 1 - (float)Math.Pow(1 - x, 5);
                    break;
            }
            return factor;
        }
    }
}
