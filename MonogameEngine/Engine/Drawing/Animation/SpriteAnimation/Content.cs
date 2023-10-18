using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MonogameEngine
{
    partial class MonogameEngine
    {
        public class TextureEntry
        {
            public string Path = "";
            public Texture2D Texture;
            public bool Loaded = false;
            public bool Disposable = false;
            public bool Missing = false;

            public TextureEntry(string path, bool disposable = false)
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
                        this.Texture = content.Load<Texture2D>(this.Path);
                    }
                    catch (Exception e)
                    {
                        LOG("Missing: " + this.Path);
                        this.Texture = Textures["core/missing"].Texture;
                        this.Missing = true;
                    }                        
                }
                    
                this.Loaded = true;
            }

            public void Unload()
            {
                this.Texture.Dispose();

                this.Loaded = false;
            }
        }

        public static Dictionary<string, TextureEntry> Textures = new Dictionary<string, TextureEntry>();

        void LoadStaticResources()
        {
            // shaders
            LoadEffects();

            // fonts
            LoadFonts();

            // sounds
            LoadSounds();

            LoadAnimationSystems();

            // vids
            //LoadVids();

            // set up all our texture definitions
            LoadTextures();

            List<string> staticAssets = new List<string>
            {

            };

            // load missing first
            Textures["core/missing"].Load();

            // just load everything for now - very easy to adjust this later!
            foreach (string key in Textures.Keys)
                Textures[key].Load();
        }

        void LoadEffects()
        {
            string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\Content\shaders\"));

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
                    if (fileExtension == "fx")
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
                int keyIndx = f.FullName.IndexOf("Content") + 16;
                int lastDot = f.Name.LastIndexOf('.');
                int fileExtension = f.Name.Substring(lastDot + 1, f.Name.Length - lastDot - 1).Length + 1;
                string localPath = f.FullName.Substring(indx, f.FullName.Length - indx - fileExtension);
                string key = f.FullName.Substring(keyIndx, f.FullName.Length - keyIndx - fileExtension);
                key = key.Replace(@"\", "/");
                Effects[key] = content.Load<Effect>(localPath);
            }
        }

        void LoadTextures()
        {
            string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\Content\sprites\"));

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
                    if (fileExtension == "png" || fileExtension == "jpg" || fileExtension == "jpeg")
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
                int keyIndx = f.FullName.IndexOf("Content") + 16;
                int lastDot = f.Name.LastIndexOf('.');
                int fileExtension = f.Name.Substring(lastDot + 1, f.Name.Length - lastDot - 1).Length + 1;
                string localPath = f.FullName.Substring(indx, f.FullName.Length - indx - fileExtension);
                string key = f.FullName.Substring(keyIndx, f.FullName.Length - keyIndx - fileExtension);
                key = key.Replace(@"\","/");
                Textures[key] = new TextureEntry(localPath);
            }
        }
    }
}
