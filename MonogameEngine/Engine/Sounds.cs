using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        // tracks our library of sounds
        public static Dictionary<string, SoundEntry> SoundEntries = new Dictionary<string, SoundEntry>();
        // tracks current running sounds
        public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();

        void LoadSounds()
        {
            string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @"Content\sounds\"));

            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            List<FileInfo> files = new List<FileInfo>();

            Stack<DirectoryInfo> tempFolders = new Stack<DirectoryInfo>();
            tempFolders.Push(new DirectoryInfo(newPath));

            DirectoryInfo currentFolder = tempFolders.Pop();

            // populate MediaFileInfo with all files we find in the video directory
            while (true)
            {
                // add to the fileSet
                foreach (FileInfo f in currentFolder.GetFiles())
                {
                    int lastDot = f.Name.LastIndexOf('.');
                    string fileExtension = f.Name.Substring(lastDot + 1, f.Name.Length - lastDot - 1).ToLower();
                    if (fileExtension == "xnb")
                        files.Add(f);
                }

                // add to the folderSet
                foreach (DirectoryInfo d in currentFolder.GetDirectories())
                    tempFolders.Push(d);

                if (tempFolders.Count == 0)
                    break;

                currentFolder = tempFolders.Pop();
            }

            foreach (FileInfo f in files)
            {
                int indx = f.FullName.IndexOf("Content") + 8;
                int keyIndx = f.FullName.IndexOf("Content") + 15;
                int lastDot = f.Name.LastIndexOf('.');
                int fileExtension = f.Name.Substring(lastDot + 1, f.Name.Length - lastDot - 1).Length + 1;
                string localPath = f.FullName.Substring(indx, f.FullName.Length - indx - fileExtension);
                string key = f.FullName.Substring(keyIndx, f.FullName.Length - keyIndx - fileExtension);
                key = key.Replace(@"\", "/");
                SoundEntries[key] = new SoundEntry(localPath);
            }

            // for now we're loading them all
            foreach (SoundEntry sound in SoundEntries.Values)
                sound.Load();
        }
        public class SoundEntry
        {
            public string Path = "";
            public SoundEffect SoundEffect;
            public bool Loaded = false;
            public bool Disposable = false;
            public bool Missing = false;

            public SoundEntry(string path, bool disposable = false)
            {
                this.Path = path;
                this.Disposable = disposable;
            }

            public void Load()
            {
                //if (content == null)
                //{
                //    content = new Microsoft.Xna.Framework.Content.ContentManager(_content.ServiceProvider);
                //}

                if (!this.Loaded)
                {
                    try
                    {
                        this.SoundEffect = content.Load<SoundEffect>(this.Path);
                    }
                    catch (Exception e)
                    {
                        LOG("Missing: " + this.Path);
                        this.Missing = true;
                    }
                }

                this.Loaded = true;
            }

            public void Unload()
            {
                this.SoundEffect.Dispose();

                this.Loaded = false;
            }
        }

        public class Sound
        {
            public List<SoundEffect> Effects = new List<SoundEffect>();
            public bool Song = false;
            public SoundEffectInstance Instance;
            public bool Loops = false;
            public float Volume;
            public float Pitch = 0;
            public float PitchRange = 0f;
            public Sound Next = null;
            
            public Sound(SoundEffect effect, float volume)
            {
                this.Effects.Add(effect);
                this.Volume = volume;
            }
            public Sound(SoundEntry effect, float volume)
            {
                this.Effects.Add(effect.SoundEffect);
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
