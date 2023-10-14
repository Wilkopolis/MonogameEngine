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
                if (content == null)
                {
                    content = new Microsoft.Xna.Framework.Content.ContentManager(_content.ServiceProvider);
                }

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
            DefineTextures();

            List<string> staticAssets = new List<string>
            {

            };

            // load missing first
            Textures["core/missing"].Load();

            // just load everything for now - very easy to adjust this later!
            foreach (string key in Textures.Keys)
                Textures[key].Load();
        }

        void LoadFonts()
        {
            CreateFonts();

            // right now we're going to load everything
            // eventually we will whittle it down to just the essentials
            foreach (Dictionary<int, Font> dictionary in Fonts.Library.Values)
            {
                foreach (Font font in dictionary.Values)
                {
                    font.Load();
                }
            }
        }

        void LoadEffects()
        {
            Effects["ColorShift"] = content.Load<Effect>("shaders/color_shift");

            // effects (shaders)
            //Effects["borderRadiusEffect"] = content.Load<Effect>("shaders/border_radius");
            //Effects["borderOnlyEffect"] = content.Load<Effect>("shaders/border_only");
            //Effects["smallRadialSmoke"] = content.Load<Effect>("shaders/radialSmoke");
            //Effects["fightButtonEffect"] = content.Load<Effect>("shaders/fightButton");
            //Effects["gradientEffect2"] = content.Load<Effect>("shaders/gradient2");
            //Effects["gradientEffect"] = content.Load<Effect>("shaders/gradient");
            //Effects["castingEffect"] = content.Load<Effect>("shaders/casting");
            //Effects["iterativeMask"] = content.Load<Effect>("shaders/iterative_mask");
            //Effects["rectangleMask"] = content.Load<Effect>("shaders/rectangular_mask");
            //Effects["portalEffect2"] = content.Load<Effect>("shaders/portal_glow2");
            //Effects["portalEffect"] = content.Load<Effect>("shaders/portal_glow");
            //Effects["radialEffect"] = content.Load<Effect>("shaders/radial_mask");
            //Effects["polygonMask"] = content.Load<Effect>("shaders/polygon_mask");
            //Effects["inverseMask"] = content.Load<Effect>("shaders/inverse_mask");
            //Effects["colorSmoke2"] = content.Load<Effect>("shaders/colorRectangularSmoke");
            //Effects["colorSmoke"] = content.Load<Effect>("shaders/colorRadialSmoke");
            //Effects["desaturate"] = content.Load<Effect>("shaders/desaturate");
            //Effects["colorShift"] = content.Load<Effect>("shaders/color_shift");
            //Effects["runeEffect"] = content.Load<Effect>("shaders/rune_effect");
            //Effects["colorNoise"] = content.Load<Effect>("shaders/color_shift_noise");
            //Effects["zapDistort"] = content.Load<Effect>("shaders/zapDistort");
            //Effects["indicator"] = content.Load<Effect>("shaders/indicator");
            //Effects["roundMask"] = content.Load<Effect>("shaders/roundMask");
            //Effects["blueshift"] = content.Load<Effect>("shaders/blueshift");
            //Effects["lightBall"] = content.Load<Effect>("shaders/lightBall");
            //Effects["bookPage2"] = content.Load<Effect>("shaders/bookPage2");
            //Effects["bookPage"] = content.Load<Effect>("shaders/bookPage");
            //Effects["pixelate"] = content.Load<Effect>("shaders/pixelate");
            //Effects["lootBeam"] = content.Load<Effect>("shaders/lootBeam");
            //Effects["missile1"] = content.Load<Effect>("shaders/missile1");
            //Effects["missile2"] = content.Load<Effect>("shaders/missile1");
            //Effects["missile3"] = content.Load<Effect>("shaders/missile1");
            //Effects["stealth"] = content.Load<Effect>("shaders/stealth");
            //Effects["bwMask"] = content.Load<Effect>("shaders/bw_mask");
            //Effects["sphere"] = content.Load<Effect>("shaders/sphere");
            //Effects["shield"] = content.Load<Effect>("shaders/shield");
            //Effects["light1"] = content.Load<Effect>("shaders/light1");
            //Effects["lazer"] = content.Load<Effect>("shaders/lazer");
            //Effects["pulse"] = content.Load<Effect>("shaders/pulse");
            //Effects["storm"] = content.Load<Effect>("shaders/storm");
            //Effects["shell"] = content.Load<Effect>("shaders/shell");
            //Effects["mimic"] = content.Load<Effect>("shaders/mimic");
            //Effects["smoke"] = content.Load<Effect>("shaders/smoke");
            //Effects["beam2"] = content.Load<Effect>("shaders/beam2");
            //Effects["beam3"] = content.Load<Effect>("shaders/beam3");
            //Effects["beam4"] = content.Load<Effect>("shaders/beam4");
            //Effects["beam5"] = content.Load<Effect>("shaders/beam5");
            //Effects["beam6"] = content.Load<Effect>("shaders/beam6");
            //Effects["beam7"] = content.Load<Effect>("shaders/beam7");
            //Effects["beam8"] = content.Load<Effect>("shaders/beam8");
            //Effects["blur2"] = content.Load<Effect>("shaders/blur2");
            //Effects["blur"] = content.Load<Effect>("shaders/blur");
            //Effects["beam"] = content.Load<Effect>("shaders/beam");
            //Effects["fog1"] = content.Load<Effect>("shaders/fog1");
            //Effects["fog2"] = content.Load<Effect>("shaders/fog2");
            //Effects["zap"] = content.Load<Effect>("shaders/zap");
            //Effects["ray"] = content.Load<Effect>("shaders/ray");
            //Effects["fow"] = content.Load<Effect>("shaders/fow");

            //// strokes
            //Effects["stroke"] = content.Load<Effect>("shaders/stroke");
            //Effects["stroke2"] = content.Load<Effect>("shaders/stroke2");
            //Effects["stroke2_5"] = content.Load<Effect>("shaders/stroke2_5");
            //Effects["stroke2_8"] = content.Load<Effect>("shaders/stroke2_8");
            //Effects["stroke2_12"] = content.Load<Effect>("shaders/stroke2_12");
            //Effects["stroke2_16"] = content.Load<Effect>("shaders/stroke2_16");
            //Effects["stroke2_20"] = content.Load<Effect>("shaders/stroke2_20");
            //Effects["strokeMask"] = content.Load<Effect>("shaders/strokeMask");
        }

        void DefineTextures()
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
