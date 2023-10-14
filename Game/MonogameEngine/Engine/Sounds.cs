using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();

        void LoadSounds()
        {
        }

        public class Sound
        {
            public List<SoundEffect> Effects = new List<SoundEffect>();
            public bool Song = false;
            public SoundEffectInstance Instance;
            public bool Loops = false;
            public float Volume;
            public float Pitch = 0;
            public float PitchRange = .5f;
            public Sound Next = null;
            
            public Sound(SoundEffect effect, float volume)
            {
                this.Effects.Add(effect);
                this.Volume = volume;
            }

            public Sound(List<SoundEffect> effects, float volume)
            {
                this.Effects = effects;
                this.Volume = volume;
            }

            public Sound Clone()
            {
                Sound result = null;

                result = new Sound(this.Effects, this.Volume);
                result.Song = this.Song;
                result.Pitch = this.Pitch;
                result.PitchRange = this.PitchRange;

                return result;
            }

            public void Play()
            {
                // if its a song
                if (this.Song)
                {
                    float volume = this.Volume * Setting.MUSICVOLUME;

                    SoundEffect chosen = this.Effects[Random.Next(this.Effects.Count)];
                    this.Instance = chosen.CreateInstance();
                    this.Instance.Volume = volume;
                    this.Instance.IsLooped = this.Loops;
                    this.Instance.Play();
                }
                // if its a sound effect
                else
                {
                    float volume = this.Volume * Setting.SFXVOLUME;

                    SoundEffect chosen = this.Effects[Random.Next(this.Effects.Count)];
                    this.Instance = chosen.CreateInstance();
                    this.Instance.Pitch = this.Pitch;
                    this.Instance.Pitch += (float)Random.NextDouble() * this.PitchRange - this.PitchRange / 2f;
                    this.Instance.Volume = volume;
                    this.Instance.Play();
                }
            }

            public void Stop()
            {
                // stop the instance
                if (this.Instance != null)
                {
                    this.Instance.Stop(true);
                }
            }
        }

        public class VolumeFade : Animation
        {
            float Duration;
            float TargetVolume;
            float StartingVolume;
            Sound Sound;

            public VolumeFade(Sound sound, int duration, float targetVolume, Action callback = null)
            {
                this.Sound = sound;
                this.Duration = duration;
                this.TargetVolume = targetVolume;

                this.Callback = callback;

                this.Begin();

                this.StartingVolume = 0f;
                this.TargetVolume = 0f;
            }

            public override void Begin()
            {
                this.StartingVolume = this.Sound.Volume;
                
                base.Begin();
            }

            public override void Tick()
            {
                double dt = (MsEllapsed - this.LastFrame);
                this.Elapsed += dt;
                
                float percentComplete = (float)this.Elapsed / this.Duration;

                if (percentComplete >= 1)
                {
                    this.Complete();
                    return;
                }

                this.Sound.Volume = percentComplete * (this.TargetVolume - this.StartingVolume) + this.StartingVolume;
                if (this.Sound.Instance != null)
                    this.Sound.Instance.Volume = this.Sound.Volume;
            }

            public override void Complete()
            {
                this.Sound.Volume = this.TargetVolume;
                if (this.Sound.Instance != null)
                    this.Sound.Instance.Volume = this.Sound.Volume;

                this.Over = true;
                this.Running = false;
            }
        }
    }
}
