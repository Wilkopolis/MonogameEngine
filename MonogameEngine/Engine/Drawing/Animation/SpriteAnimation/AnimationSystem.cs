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
            public AnimationSequence IdleLeft;
            public AnimationSequence IdleRight;
            public AnimationSequence AttackLeft;
            public AnimationSequence AttackRight;
            public AnimationSequence AttackUp;
            public AnimationSequence AttackDown;
            public AnimationSequence CastLeft;
            public AnimationSequence CastRight;
            public AnimationSequence WalkLeft;
            public AnimationSequence WalkRight;
            public AnimationSequence WalkUp;
            public AnimationSequence WalkDown;
            public AnimationSequence DieLeft;
            public AnimationSequence DieRight;
            public AnimationSequence DamageLeft;
            public AnimationSequence DamageRight;
            // boss specific
            public AnimationSequence Cast;
            public AnimationSequence Grab;

            // sprite specific offset for damage numbers
            public Dictionary<string, Vector2> Offsets = new Dictionary<string, Vector2>();

            public AnimationSystem Clone()
            {
                AnimationSystem result = new AnimationSystem();
                
                result.IdleLeft = this.IdleLeft.Clone();
                result.IdleRight = this.IdleRight.Clone();
                result.AttackLeft = this.AttackLeft.Clone();
                result.AttackRight = this.AttackRight.Clone();
                if (this.AttackUp != null)
                    result.AttackUp = this.AttackUp.Clone();
                if (this.AttackDown != null)
                    result.AttackDown = this.AttackDown.Clone();
                if (this.CastLeft != null)
                    result.CastLeft = this.CastLeft.Clone();
                if (this.CastRight != null)
                    result.CastRight = this.CastRight.Clone();
                result.WalkLeft = this.WalkLeft.Clone();
                result.WalkRight = this.WalkRight.Clone();
                result.WalkUp = this.WalkUp.Clone();
                result.WalkDown = this.WalkDown.Clone();
                result.DieLeft = this.DieLeft.Clone();
                result.DieRight = this.DieRight.Clone();
                if (this.DamageLeft != null)
                    result.DamageLeft = this.DamageLeft.Clone();
                if (this.DamageRight != null)
                    result.DamageRight = this.DamageRight.Clone();

                result.Offsets = new Dictionary<string, Vector2>(this.Offsets);

                return result;
            }

            public void SetSource(Character character)
            {
                if (this.IdleLeft != null)
                    this.IdleLeft.SpriteTarget = character.Element;
                if (this.IdleRight != null)
                    this.IdleRight.SpriteTarget = character.Element;
                if (this.AttackLeft != null)
                    this.AttackLeft.SpriteTarget = character.Element;
                if (this.AttackRight != null)
                    this.AttackRight.SpriteTarget = character.Element;
                if (this.AttackUp != null)
                    this.AttackUp.SpriteTarget = character.Element;
                if (this.AttackDown != null)
                    this.AttackDown.SpriteTarget = character.Element;
                if (this.CastLeft != null)
                    this.CastLeft.SpriteTarget = character.Element;
                if (this.CastRight != null)
                    this.CastRight.SpriteTarget = character.Element;
                if (this.WalkLeft != null)
                    this.WalkLeft.SpriteTarget = character.Element;
                if (this.WalkRight != null)
                    this.WalkRight.SpriteTarget = character.Element;
                if (this.WalkUp != null)
                    this.WalkUp.SpriteTarget = character.Element;
                if (this.WalkDown != null)
                    this.WalkDown.SpriteTarget = character.Element;
                if (this.DieLeft != null)
                    this.DieLeft.SpriteTarget = character.Element;
                if (this.DieRight != null)
                    this.DieRight.SpriteTarget = character.Element;
                if (this.DamageLeft != null)
                    this.DamageLeft.SpriteTarget = character.Element;
                if (this.DamageRight != null)
                    this.DamageRight.SpriteTarget = character.Element;
            }

            public void Load()
            {
                foreach (AnimationFrame frame in this.IdleLeft.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.IdleRight.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.AttackLeft.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.AttackRight.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.WalkLeft.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.WalkRight.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.WalkUp.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.WalkDown.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.DieLeft.Frames)
                    Textures[frame.FrameKey].Load();
                foreach (AnimationFrame frame in this.DieRight.Frames)
                    Textures[frame.FrameKey].Load();

                if (this.AttackUp != null)
                {
                    foreach (AnimationFrame frame in this.AttackUp.Frames)
                        Textures[frame.FrameKey].Load();
                }
                if (this.AttackDown != null)
                {
                    foreach (AnimationFrame frame in this.AttackDown.Frames)
                        Textures[frame.FrameKey].Load();
                }
                if (this.CastLeft != null)
                {
                    foreach (AnimationFrame frame in this.CastLeft.Frames)
                        Textures[frame.FrameKey].Load();
                }
                if (this.CastRight != null)
                {
                    foreach (AnimationFrame frame in this.CastRight.Frames)
                        Textures[frame.FrameKey].Load();
                }
                if (this.DamageLeft != null)
                {
                    foreach (AnimationFrame frame in this.DamageLeft.Frames)
                        Textures[frame.FrameKey].Load();
                }
                if (this.DamageRight != null)
                {
                    foreach (AnimationFrame frame in this.DamageRight.Frames)
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

            public virtual Animation Play(Screen screen, float speed = 1, Action callback = null, Func<Animation, bool> checkComplete = null) 
            {
                return null;
            }

            public virtual Animation Play(Screen screen, float delta, float speed = 1, Action callback = null, Func<Animation, bool> checkComplete = null)
            {
                return null;
            }

            public virtual AnimationSequence Clone()
            {
                return null;
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
        }

        public class StaticAnimationSequence : AnimationSequence
        {
            public override AnimationSequence Clone()
            {
                StaticAnimationSequence result = new StaticAnimationSequence();
                if (this.Sound != null)
                    result.Sound = this.Sound.Clone();
                result.Direction = this.Direction;
                result.Frames = this.Frames;
                result.Loop = this.Loop;
                result.Flipped = this.Flipped;
                return result;
            }

            public override Animation Play(Screen screen, float speed, Action callback = null, Func<Animation, bool> checkComplete = null)
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

            public override Animation Play(Screen screen, float delta, float speed, Action callback = null, Func<Animation, bool> checkComplete = null)
            {
                throw new NotImplementedException();
            }
        }

        public class MovingAnimationSequence : AnimationSequence
        {
            public override AnimationSequence Clone()
            {
                MovingAnimationSequence result = new MovingAnimationSequence();
                if (this.Sound != null)
                    result.Sound = this.Sound.Clone();
                result.Frames = this.Frames;
                result.Direction = this.Direction;
                result.Loop = this.Loop;
                result.Flipped = this.Flipped;
                return result;
            }

            public override Animation Play(Screen screen, float speed, Action callback = null, Func<Animation, bool> checkComplete = null)
            {
                if (this.SpriteTarget == null)
                {
                    callback?.Invoke();
                    return null;
                }

                List<AnimationStage> stages = new List<AnimationStage>();

                float scale = this.SpriteTarget.Scale;

                Vector2 stage1Func(AnimationStage a)
                {
                    return new Vector2((float)(a.Elapsed / a.Duration) * a.FrameOffset.X, (float)(a.Elapsed / a.Duration) * a.FrameOffset.Y);
                }

                Vector2 dimensions = new Vector2(this.SpriteTarget.Texture.Width, this.SpriteTarget.Texture.Height);

                foreach (AnimationFrame key in this.Frames)
                    stages.Add(new AnimationStage(key.FrameKey, key.Length / speed, key.FrameOffset * scale, dimensions, stage1Func));

                Animation result = null;
                switch (this.Direction)
                {
                    case Direction.N:
                        result = new FrameAnimation(this.SpriteTarget, stages, this.Sound, callback) { Loop = true, Flipped = this.Flipped };
                        break;
                    case Direction.S:
                        result = new FrameAnimation(this.SpriteTarget, stages, this.Sound, callback) { Loop = true, Flipped = this.Flipped };
                        break;
                    case Direction.E:
                        result = new FrameAnimation(this.SpriteTarget, stages, this.Sound, callback) { Loop = true, Flipped = this.Flipped };
                        break;
                    case Direction.W:
                        result = new FrameAnimation(this.SpriteTarget, stages, this.Sound, callback) { Loop = true, Flipped = this.Flipped };
                        break;
                }

                screen.Animations[this.SpriteTarget.Key] = result;

                // do this once to set the sprite and resize it and add the offsets and stuff
                result.Tick();

                return result;
            }

            public override Animation Play(Screen screen, float delta, float speed, Action callback = null, Func<Animation, bool> checkComplete = null)
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

                // check to see if any frames match our current frame - start from there
                //int i = 0;
                //while (i < stages.Count)
                //{
                //    if (Textures[stages[i].SpriteKey].Texture == this.SpriteTarget.Texture)
                //        break;

                //    i++;
                //}

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
        }
    }
}
