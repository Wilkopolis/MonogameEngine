using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum Direction { N, S, E, W }

        public class WalkAnimation : FrameAnimation
        {
            public Direction Direction;
            public Vector2 ElapsedOffset;

            public bool Flipped = false;

            // hack for moving
            public double TotalDuration = 0;
            public Action HalfTimeCallback;
            public bool PastHalf = false;

            public WalkAnimation(Element target, Direction direction, float percentX, float percentY, List<AnimationStage> stages, Sound sound, Action handler = null)
            {
                this.Target = target;
                this.GoalOffset = new Vector2(percentX, percentY);

                this.Direction = direction;

                this.Stages = stages;
                // calculate the total duration of all stages
                foreach (AnimationStage s in this.Stages)
                    this.TotalDuration += s.Duration;

                this.Sound = sound;

                this.Callback = handler;

                target.Animations.Add(this);

                this.Begin();
            }

            public override void Tick()
            {
                // get the delta since last tick call
                double dt = MsEllapsed - this.LastFrame;
                this.Elapsed += dt;

                // get our current animation stage and add the delta
                AnimationStage CurrentStage = this.Stages[this.CurrentStageIndex];
                CurrentStage.Elapsed += dt;

                // if we finished our stage, move the sprite to the correct position
                // and make the next stage our new stage
                while (CurrentStage.Elapsed >= CurrentStage.Duration)
                {
                    dt = CurrentStage.Elapsed - CurrentStage.Duration;

                    // add the max offset to the elapsed offset
                    this.ElapsedOffset += CurrentStage.FrameOffset;

                    // get the new animation stage
                    CurrentStageIndex = (CurrentStageIndex + 1) % this.Stages.Count;

                    // reset this if we come back
                    CurrentStage.Elapsed = dt;
                }

                // set the texture
                if (this.Target.Texture != Textures[CurrentStage.SpriteKey].Texture)
                {
                    this.Target.Texture = Textures[CurrentStage.SpriteKey].Texture;
                    this.Target.Resize();

                    this.Target.FlipHorizontal = this.Flipped;
                }
                
                // get the final current offset by adding the elapsed offset to the 
                // current stage offset
                Vector2 CurrentOffset = CurrentStage.GetOffset(CurrentStage);
                Vector2 TotalOffset = this.ElapsedOffset + CurrentOffset;

                this.Offset = TotalOffset + CurrentStage.FrameOffset;
                if (this.Flipped)
                {
                    this.Offset.X = (CurrentStage.SpriteDimensions.X - this.Target.Texture.Width) * this.Target.Scale + TotalOffset.X - CurrentStage.FrameOffset.X;
                }

                CurrentStage.Elapsed += MsEllapsed - this.LastFrame;
                Vector2 nextFrameOffset = this.ElapsedOffset + CurrentStage.GetOffset(CurrentStage);
                CurrentStage.Elapsed -= MsEllapsed - this.LastFrame;

                double percentComplete = 0;
                switch(this.Direction)
                {
                    case Direction.N:

                        percentComplete = nextFrameOffset.Y / this.GoalOffset.Y;

                        break;
                    case Direction.S:

                        percentComplete = nextFrameOffset.Y / this.GoalOffset.Y;

                        break;
                    case Direction.E:

                        percentComplete = nextFrameOffset.X / this.GoalOffset.X;

                        break;
                    case Direction.W:

                        percentComplete = nextFrameOffset.X / this.GoalOffset.X;

                        break;
                }

                // if we cross half, do any half time animation
                if (this.HalfTimeCallback != null && !this.PastHalf && percentComplete >= .5)
                {                    
                    this.HalfTimeCallback?.Invoke();
                    this.PastHalf = true;
                }

                // if we finish, complete
                if (percentComplete >= 1)
                    this.Complete();
            }

            public override void Begin()
            {
                base.Begin();
            }

            public override void Complete()
            {
                base.Complete();

                this.Target.Position += this.GoalOffset;
                this.Over = true;
                this.Running = false;
                this.Target.Animations.Remove(this);
            }
        }
    }
}
