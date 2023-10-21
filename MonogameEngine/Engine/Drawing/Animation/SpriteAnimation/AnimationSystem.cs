using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#pragma warning disable IDE0017 // Simplify object initialization

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class AnimationSystem
        {
            // sprite specific offset for damage numbers
            public Dictionary<string, Vector2> Offsets = new Dictionary<string, Vector2>();
            public Dictionary<string, AnimationSequence> Sequences = new Dictionary<string, AnimationSequence>();

            public AnimationSystem Clone()
            {
                AnimationSystem result = new AnimationSystem();
                
                foreach (KeyValuePair<string, AnimationSequence> pair in this.Sequences)
                    result.Sequences[pair.Key] = pair.Value.Clone();

                result.Offsets = new Dictionary<string, Vector2>(this.Offsets);

                return result;
            }

            public void SetSource(Character character)
            {
                foreach (AnimationSequence sequence in this.Sequences.Values)
                    sequence.SpriteTarget = character.Sprite;
            }

            public void Load()
            {
                foreach (AnimationSequence sequence in this.Sequences.Values)
                {
                    foreach (AnimationFrame frame in sequence.Frames)
                        Textures[frame.FrameKey].Load();
                }
            }
        }

        public class AnimationSequence
        {
            public Sprite SpriteTarget;
            public Sound Sound;
            public List<AnimationFrame> Frames = new List<AnimationFrame>();
            // for certain moving animation
            public Direction Direction;
            // for idle animation
            public bool Loop;
            // flipped or not
            public bool Flipped = false;

            public virtual Animation PlayStatic(Screen screen, float speed = 1, Action callback = null, Func<Animation, bool> checkComplete = null)
            {
                if (this.SpriteTarget == null)
                {
                    callback?.Invoke();
                    return null;
                }

                List<AnimationStage> stages = new List<AnimationStage>();

                float scale = this.SpriteTarget.Scale;

                Vector2 dimensions = new Vector2(this.SpriteTarget.Texture.Width, this.SpriteTarget.Texture.Height);

                foreach (AnimationFrame key in this.Frames)
                    stages.Add(new AnimationStage(key.FrameKey, key.Length / speed, key.FrameOffset * scale, dimensions));

                Animation result = null;
                if (this.SpriteTarget != null)
                {
                    result = new FrameAnimation((Sprite)this.SpriteTarget, stages, this.Sound, callback)
                    {
                        Loop = this.Loop,
                        Flipped = this.Flipped
                    };
                    screen.Animations[this.SpriteTarget.Key] = result;

                    // do this once to set the sprite and resize it and add the offsets and stuff
                    result.Tick();
                }

                return result;
            }

            public virtual Animation PlayMove(Screen screen, float delta, float speed = 1, Action callback = null, Func<Animation, bool> checkComplete = null)
            {
                if (this.SpriteTarget == null)
                {
                    callback();
                    return null;
                }

                List<AnimationStage> stages = new List<AnimationStage>();

                float scale = this.SpriteTarget.Scale;

                Vector2 stage1Func(AnimationStage a)
                {
                    return new Vector2((float)(a.Elapsed / a.Duration) * a.FrameOffset.X, (float)(a.Elapsed / a.Duration) * a.FrameOffset.Y);
                }

                foreach (AnimationFrame key in this.Frames)
                    stages.Add(new AnimationStage(key.FrameKey, key.Length / speed, key.FrameOffset * scale, new Vector2(this.SpriteTarget.Width, this.SpriteTarget.Height), stage1Func));

                Animation result = null;
                switch (this.Direction)
                {
                    case Direction.N:
                        result = new WalkAnimation(this.SpriteTarget, this.Direction, 0, delta, stages, this.Sound, callback) { Flipped = this.Flipped };
                        break;
                    case Direction.S:
                        result = new WalkAnimation(this.SpriteTarget, this.Direction, 0, delta, stages, this.Sound, callback) { Flipped = this.Flipped };
                        break;
                    case Direction.E:
                        result = new WalkAnimation(this.SpriteTarget, this.Direction, delta, 0, stages, this.Sound, callback) { Flipped = this.Flipped };
                        break;
                    case Direction.W:
                        result = new WalkAnimation(this.SpriteTarget, this.Direction, delta, 0, stages, this.Sound, callback) { Flipped = this.Flipped };
                        break;
                }

                screen.Animations[this.SpriteTarget.Key] = result;

                // do this once to set the sprite and resize it and add the offsets and stuff
                result.Tick();

                return result;
            }

            public virtual AnimationSequence Clone()
            {
                AnimationSequence result = new AnimationSequence();

                result.Sound = this.Sound;

                foreach (AnimationFrame frame in this.Frames)
                    result.Frames.Add(frame.Clone());

                result.Loop = this.Loop;
                result.Flipped = this.Flipped;
                result.Direction = this.Direction;

                return result;
            }
        }

        public class AnimationStage
        {
            public string SpriteKey;
            public double Duration;
            public double Elapsed;
            public Vector2 FrameOffset;
            // for flipping
            public Vector2 SpriteDimensions;
            public Func<AnimationStage, Vector2> GetOffset;

            // for walking/running
            public AnimationStage(string spriteKey, double duration, Vector2 frameOffset = new Vector2(), Vector2 spriteDimensions = new Vector2(), Func<AnimationStage, Vector2> offsetFunction = null)
            {
                this.SpriteKey = spriteKey;
                this.Duration = duration;
                this.FrameOffset = frameOffset;
                this.SpriteDimensions = spriteDimensions;
                this.GetOffset = offsetFunction;
            }
        }

        public class AnimationFrame
        {
            public string FrameKey;
            // for walk/run animations
            public Vector2 FrameOffset;
            public int Length;

            // idle
            public AnimationFrame(string frameKey, int length)
            {
                this.FrameKey = frameKey;
                this.FrameOffset = Vector2.Zero;
                this.Length = length;
            }

            // walk/run
            [JsonConstructor]
            public AnimationFrame(string frameKey, Vector2 frameOffset, int length)
            {
                this.FrameKey = frameKey;
                this.FrameOffset = frameOffset;
                this.Length = length;
            }

            public AnimationFrame Clone()
            {
                AnimationFrame result = new AnimationFrame(this.FrameKey, this.FrameOffset, this.Length);
                return result;
            }
        }

        //public class StaticAnimationSequence : AnimationSequence
        //{
        //    public override AnimationSequence Clone()
        //    {
        //        StaticAnimationSequence result = new StaticAnimationSequence();
        //        if (this.Sound != null)
        //            result.Sound = this.Sound.Clone();
        //        result.Direction = this.Direction;
        //        result.Frames = this.Frames;
        //        result.Loop = this.Loop;
        //        result.Flipped = this.Flipped;
        //        return result;
        //    }

        //    public override Animation Play(Screen screen, float speed, Action callback = null, Func<Animation, bool> checkComplete = null)
        //    {
        //        if (this.SpriteTarget == null)
        //        {
        //            callback?.Invoke();
        //            return null;
        //        }

        //        List<AnimationStage> stages = new List<AnimationStage>();

        //        float scale = this.SpriteTarget.Scale;

        //        Vector2 dimensions = new Vector2(this.SpriteTarget.Texture.Width, this.SpriteTarget.Texture.Height);

        //        foreach (AnimationFrame key in this.Frames)
        //            stages.Add(new AnimationStage(key.FrameKey, key.Length / speed, key.FrameOffset * scale, dimensions));

        //        Animation result = null;
        //        if (this.SpriteTarget != null)
        //        {
        //            result = new FrameAnimation((Sprite)this.SpriteTarget, stages, this.Sound, callback)
        //            {
        //                Loop = this.Loop,
        //                Flipped = this.Flipped
        //            };
        //            screen.Animations[this.SpriteTarget.Key] = result;

        //            // do this once to set the sprite and resize it and add the offsets and stuff
        //            result.Tick();
        //        }

        //        return result;
        //    }

        //    public override Animation Play(Screen screen, float delta, float speed, Action callback = null, Func<Animation, bool> checkComplete = null)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public class MovingAnimationSequence : AnimationSequence
        //{
        //    public override AnimationSequence Clone()
        //    {
        //        MovingAnimationSequence result = new MovingAnimationSequence();
        //        if (this.Sound != null)
        //            result.Sound = this.Sound.Clone();
        //        result.Frames = this.Frames;
        //        result.Direction = this.Direction;
        //        result.Loop = this.Loop;
        //        result.Flipped = this.Flipped;
        //        return result;
        //    }

        //    public override Animation Play(Screen screen, float speed, Action callback = null, Func<Animation, bool> checkComplete = null)
        //    {
        //        if (this.SpriteTarget == null)
        //        {
        //            callback?.Invoke();
        //            return null;
        //        }

        //        List<AnimationStage> stages = new List<AnimationStage>();

        //        float scale = this.SpriteTarget.Scale;

        //        Vector2 stage1Func(AnimationStage a)
        //        {
        //            return new Vector2((float)(a.Elapsed / a.Duration) * a.FrameOffset.X, (float)(a.Elapsed / a.Duration) * a.FrameOffset.Y);
        //        }

        //        Vector2 dimensions = new Vector2(this.SpriteTarget.Texture.Width, this.SpriteTarget.Texture.Height);

        //        foreach (AnimationFrame key in this.Frames)
        //            stages.Add(new AnimationStage(key.FrameKey, key.Length / speed, key.FrameOffset * scale, dimensions, stage1Func));

        //        Animation result = null;
        //        switch (this.Direction)
        //        {
        //            case Direction.N:
        //                result = new FrameAnimation(this.SpriteTarget, stages, this.Sound, callback) { Loop = true, Flipped = this.Flipped };
        //                break;
        //            case Direction.S:
        //                result = new FrameAnimation(this.SpriteTarget, stages, this.Sound, callback) { Loop = true, Flipped = this.Flipped };
        //                break;
        //            case Direction.E:
        //                result = new FrameAnimation(this.SpriteTarget, stages, this.Sound, callback) { Loop = true, Flipped = this.Flipped };
        //                break;
        //            case Direction.W:
        //                result = new FrameAnimation(this.SpriteTarget, stages, this.Sound, callback) { Loop = true, Flipped = this.Flipped };
        //                break;
        //        }

        //        screen.Animations[this.SpriteTarget.Key] = result;

        //        // do this once to set the sprite and resize it and add the offsets and stuff
        //        result.Tick();

        //        return result;
        //    }

        //    public override Animation Play(Screen screen, float delta, float speed, Action callback = null, Func<Animation, bool> checkComplete = null)
        //    {
        //        if (this.SpriteTarget == null)
        //        {
        //            callback();
        //            return null;
        //        }

        //        List<AnimationStage> stages = new List<AnimationStage>();

        //        float scale = this.SpriteTarget.Scale;

        //        Vector2 stage1Func(AnimationStage a)
        //        {
        //            return new Vector2((float)(a.Elapsed / a.Duration) * a.FrameOffset.X, (float)(a.Elapsed / a.Duration) * a.FrameOffset.Y);
        //        }

        //        foreach (AnimationFrame key in this.Frames)
        //            stages.Add(new AnimationStage(key.FrameKey, key.Length / speed, key.FrameOffset * scale, new Vector2(this.SpriteTarget.Width, this.SpriteTarget.Height), stage1Func));

        //        // check to see if any frames match our current frame - start from there
        //        //int i = 0;
        //        //while (i < stages.Count)
        //        //{
        //        //    if (Textures[stages[i].SpriteKey].Texture == this.SpriteTarget.Texture)
        //        //        break;

        //        //    i++;
        //        //}

        //        Animation result = null;
        //        switch (this.Direction)
        //        {
        //            case Direction.N:
        //                result = new WalkAnimation(this.SpriteTarget, this.Direction, 0, delta, stages, this.Sound, callback) { Flipped = this.Flipped };
        //                break;
        //            case Direction.S:
        //                result = new WalkAnimation(this.SpriteTarget, this.Direction, 0, delta, stages, this.Sound, callback) { Flipped = this.Flipped };
        //                break;
        //            case Direction.E:
        //                result = new WalkAnimation(this.SpriteTarget, this.Direction, delta, 0, stages, this.Sound, callback) { Flipped = this.Flipped };
        //                break;
        //            case Direction.W:
        //                result = new WalkAnimation(this.SpriteTarget, this.Direction, delta, 0, stages, this.Sound, callback) { Flipped = this.Flipped };
        //                break;
        //        }
                
        //        screen.Animations[this.SpriteTarget.Key] = result;

        //        // do this once to set the sprite and resize it and add the offsets and stuff
        //        result.Tick();

        //        return result;
        //    }
        //}
    }
}
