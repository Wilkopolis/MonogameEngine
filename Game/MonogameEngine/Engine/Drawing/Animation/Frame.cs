using System;
using System.Collections.Generic;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class FrameAnimation : Animation
        {
            public int CurrentStageIndex = 0;
            public List<AnimationStage> Stages;
            public bool Loop = false;
            public int LoopIndex = 0;
            public bool CorrectFlipped = true;
            public bool AddOffset = false;
            
            public FrameAnimation() { }
            public FrameAnimation(Sprite target, List<AnimationStage> stages, Sound sound = null, Action callback = null, bool pauses = false)
            {
                this.Target = target;

                this.Stages = stages;

                this.Callback = callback;

                this.Sound = sound;

                target.Animations.Add(this);

                this.Begin();
            }

            public override void Begin()
            {
                base.Begin();
            }

            public override void Tick()
            {
                // get the delta since last tick call
                double dt = (MsEllapsed - this.LastFrame);

                // get our current animation stage and add the delta
                AnimationStage CurrentStage = this.Stages[this.CurrentStageIndex];
                CurrentStage.Elapsed += dt;

                // if we finished our stage, move the sprite to the correct position
                // and make the next stage our new stage
                while (CurrentStage.Elapsed > CurrentStage.Duration)
                {
                    dt = CurrentStage.Elapsed - CurrentStage.Duration;
                    // get the new animation stage
                    CurrentStageIndex++;

                    if (CurrentStageIndex >= this.Stages.Count)
                    {
                        if (this.Loop)
                        {
                            CurrentStageIndex = this.LoopIndex;
                        }                            
                        else
                        {
                            this.Complete();
                            return;
                        }                            
                    }

                    // reset this if we come back
                    CurrentStage.Elapsed = dt;
                }

                // set the texture
                if (this.Target.Texture != Textures[CurrentStage.SpriteKey].Texture)
                {
                    this.Target.Texture = Textures[CurrentStage.SpriteKey].Texture;
                    this.Target.Resize();
                }

                this.Target.FlipHorizontal = this.Flipped;

                // set its offset from the idle animation
                this.Offset = CurrentStage.IdleOffset + CurrentStage.FrameOffset;
                if (this.Flipped && this.CorrectFlipped)
                {
                    this.Offset.X = (CurrentStage.SpriteDimensions.X - this.Target.Texture.Width) * this.Target.Scale - this.Offset.X;
                }
            }

            public override void Complete()
            {
                // set us to the final frame
                AnimationStage CurrentStage = this.Stages[this.Stages.Count - 1];
                if (this.Target.Texture != Textures[CurrentStage.SpriteKey].Texture)
                {
                    this.Target.Texture = Textures[CurrentStage.SpriteKey].Texture;
                    this.Target.Resize();
                }

                base.Complete();

                // add the death animation offset
                // if (this.AddOffset)
                //     this.Target.Position += this.Offset;

                this.Over = true;
                this.Running = false;
                this.Target.Animations.Remove(this);
            }
        }
    }
}
