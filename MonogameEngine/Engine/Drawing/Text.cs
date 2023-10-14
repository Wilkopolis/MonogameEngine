using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Data;
using System.Reflection;
using static MonogameEngine.MonogameEngine;

#pragma warning disable IDE0017 // Simplify object initialization
#pragma warning disable IDE0028 // 

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class Phrase
        {
            public string Text = "";
            public Font Font;
            public Vector2 Offset = Vector2.Zero;

            public Phrase(string text, Font font) 
            {
                this.Text = text;
                this.Font = font;
            }
        }

        public class Paragraph
        {
            public List<Phrase> Phrases;
            public int MaxWidth = 100;

            public Paragraph(List<Phrase> phrases, int maxWidth)
            {
                this.Phrases = phrases;
                this.MaxWidth = maxWidth;
            }
        }

        public Screen MakeParagraph(Paragraph paragraph)
        {
            int x = 0;
            int y = 0;

            int height = 0;
            int width = 0;

            List<Element> elements = new List<Element>();

            // parse each word, color and parse any ratios
            for (int i = 0; i < paragraph.Phrases.Count; i++)
            {
                Phrase phrase = paragraph.Phrases[i];

                string[] words = phrase.Text.Split(' ');
                for (int j = 0; j < words.Length; j++)
                {
                    string word = words[j];

                    // add space if we arent the last word of the last phrase
                    if (j != words.Length - 1)
                        word += ' ';

                    Element text = new Text(x, y, word, phrase.Font);
                    text.Position += phrase.Offset;
                    x += text.Width;

                    // set this if its unset
                    if (height == 0)
                        height = text.Height;

                    if (x > paragraph.MaxWidth)
                    {
                        y += text.Height;
                        height = y + text.Height;
                        text.Position.X = 0;
                        text.Position.Y = y;

                        x = text.Width;
                    }

                    width = x > width ? x : width;

                    elements.Add(text);
                }
            }

            Screen result = new Screen(width, height);

            for (int i = 0; i < elements.Count; i++)
            {
                Element element = elements[i];
                element.Key = "text" + i;
                result.Elements[element.Key] = element;
            }

            result.RenderPeriod = -1;
            result.Render(true);

            return result;
        }

        public static class FontFamily
        {
            public static string K2D = "K2D Medium";
            public static string Acme = "Acme";
            public static string OpenSans = "Open Sans SemiBold";
            public static string Catamaran = "Catamaran-Regular";
            public static string RobotoSlab = "RobotoSlabRegular";
            public static string RobotoMono = "Roboto Mono";
            public static string Bahnschrift = "Bahnschrift";
            public static string Merriweather = "Merriweather";
        }

        public static List<int> FontSizes = new List<int>
        {
            8, 10, 12, 14, 16, 18, 20, 22, 24, 28, 32, 36, 40, 44, 48, 54, 60, 64, 72, 80, 90, 100, 120
        };

        static void CreateFonts()
        {
            bool createFonts = true;
            if (createFonts)
            {
                string path = Directory.GetCurrentDirectory();
                string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\Content\fonts\"));

                Type t = typeof(FontFamily);
                FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);

                List<string> fontFamilies = new List<string>();

                foreach (FieldInfo fieldInfo in fields)
                    fontFamilies.Add((string)fieldInfo.GetValue(null));

                foreach (string family in fontFamilies)
                {
                    Fonts.Library[family] = new Dictionary<int, Font>();

                    foreach (int size in FontSizes)
                    {
                        // create the file name path
                        string fullPath = newPath + family + size + ".spritefont";
                        
                        // check for existing of file
                        if (!File.Exists(fullPath))
                        {
                            string contents = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
                            contents += "<XnaContent xmlns:Graphics=\"Microsoft.Xna.Framework.Content.Pipeline.Graphics\">\r\n";
                            contents += "<Asset Type=\"Graphics:FontDescription\">\r\n";
                            contents += "<FontName>" + family + "</FontName>\r\n";
                            contents += "<Size>" + size + "</Size>\r\n";
                            contents += "<Spacing>0</Spacing>\r\n";
                            contents += "<UseKerning>true</UseKerning>\r\n";
                            contents += "<Style>Regular</Style>\r\n";
                            contents += "<CharacterRegions>\r\n";
                            contents += "<CharacterRegion>\r\n";
                            contents += "<Start>&#32;</Start>\r\n";
                            contents += "<End>&#126;</End>\r\n";
                            contents += "</CharacterRegion>\r\n";
                            contents += "</CharacterRegions>\r\n";
                            contents += "</Asset>\r\n";
                            contents += "</XnaContent>";

                            File.WriteAllText(fullPath, contents);
                        }

                        Fonts.Library[family][size] = new Font("fonts\\" + family + size) { Family = family };
                    }
                }
            }
            else
            {
                // grep or something to get only the fonts we actually use
                List<string> definedFonts = new List<string>
                {
                    "Acme8", "Acme10"
                };

                foreach (string path in definedFonts)
                {
                    // get the name and size from some string parsing
                    string family = "";
                    int size = 14;
                    Fonts.Library[family][size] = new Font("fonts\\" + family + size);
                }
            }
        }

        public class Font
        {
            private string Path;
            public SpriteFont SpriteFont;
            public Color Color = Color.White;
            public int VerticleSpacing = 0;
            public string Family;

            public Font(string path)
            {
                this.Path = path;
            }

            public void Load()
            {
                this.SpriteFont = content.Load<SpriteFont>(this.Path);
            }

            public Font Hue(Color color)
            {
                Font result = new Font(this.Path);
                result.SpriteFont = this.SpriteFont;
                result.Color = color;
                result.VerticleSpacing = this.VerticleSpacing;
                return result;
            }
        }

        public static class Fonts
        {
            public static Dictionary<string, Dictionary<int, Font>> Library = new Dictionary<string, Dictionary<int, Font>>();
        }

        public class Text : Element
        {
            public Font _Font;
            public SpriteFont Font;

            public string String = "";
            
            public Text(string text = "", Font font = null, float x = 0, float y = 0, float zIndex = 1f)
            {
                this.Position = new Vector2(x, y);
                this.String = text;
                this.Font = font.SpriteFont;
                this._Font = font;
                this.Color = font.Color;
                this.zIndex = zIndex;

                this.BlendState = BlendState.AlphaBlend;

                this.Resize();
            }

            public Text(float x = 0, float y = 0, string text = "", Font font = null, float zIndex = 1f)
            {
                this.Position = new Vector2(x, y);
                this.String = text;
                this.Font = font.SpriteFont;
                this._Font = font;
                this.Color = font.Color;
                this.zIndex = zIndex;

                this.BlendState = BlendState.AlphaBlend;

                this.Resize();
            }

            public override void Fit(int dimension, bool expand = false)
            {
                List<int> fontSizes = new List<int>(FontSizes);

                // set us to max size
                if (expand)
                {
                    int size = fontSizes[fontSizes.Count - 1];
                    fontSizes.Remove(size);

                    this._Font = Fonts.Library[this._Font.Family][size];
                    this.Font = this._Font.SpriteFont;
                    this.Resize();
                }

                // then try to fit us
                while (this.Width > dimension && fontSizes.Count > 0)
                {
                    int size = fontSizes[fontSizes.Count - 1];
                    fontSizes.Remove(size);

                    this._Font = Fonts.Library[this._Font.Family][size];
                    this.Font = this._Font.SpriteFont;
                    this.Resize();
                }
            }

            public Text Clone()
            {
                Text result = new Text(this.String, this._Font, this.Position.X, this.Position.Y, this.zIndex);
                return result;
            }

            public override void Render(bool cursor = false)
            {
                if (MsEllapsed - this.LastRenderTime < this.RenderPeriod)
                    return;

                if (this.String == "" || this.EffectAgents.Count < 1)
                {
                    this.RenderTarget2D = null;
                    this.Canvas = null;
                    return;
                }

                if (this.EffectAgents.Count > 10)
                    Oops();

                int adjW = (int)Math.Round(this.Width / this.Scale) + 40;
                int adjH = (int)Math.Round(this.Height / this.Scale) + 40;
                if (this.RenderTarget2D == null || this.RenderTarget2D.Width != adjW || this.RenderTarget2D.Height != adjH)
                {
                    if (this.RenderTarget2D != null)
                    {
                        this.RenderTarget2D.Dispose();
                        this.RenderTarget2D = null;
                    }

                    this.RenderTarget2D = new RenderTarget2D(graphics, adjW, adjH);
                }

                // draw the text out first to a texture
                graphics.SetRenderTarget(this.RenderTarget2D);

                graphics.Clear(this.ClearColor);

                spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);

                spriteBatch.DrawString(this.Font, this.String, new Vector2(20, 20), this.Color * this.Alpha, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);

                // make a second render target
                if (this.Canvas == null || this.Canvas.Width != adjW || this.Canvas.Height != adjH)
                {
                    if (this.Canvas != null)
                    {
                        this.Canvas.Dispose();
                        this.Canvas = null;
                    }

                    this.Canvas = new RenderTarget2D(graphics, adjW, adjH);
                }

                this.EffectAgents.Sort(delegate (EffectAgent a, EffectAgent b)
                {
                    return a.Priority.CompareTo(b.Priority);
                });

                // draw it per each effect
                for (int i = 0; i < this.EffectAgents.Count; i++)
                {
                    if (i % 2 == 1)
                    {
                        graphics.SetRenderTarget(this.RenderTarget2D);
                        graphics.Clear(this.ClearColor);

                        EffectAgent effect = this.EffectAgents[i];

                        effect.Apply(this);

                        spriteBatch.Draw(this.Canvas, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        graphics.SetRenderTarget(this.Canvas);
                        graphics.Clear(this.ClearColor);

                        EffectAgent effect = this.EffectAgents[i];

                        effect.Apply(this);

                        spriteBatch.Draw(this.RenderTarget2D, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }
                }

                spriteBatch.End();

                // set our screen back to normal
                graphics.SetRenderTarget(null);

                this.LastRenderTime = MsEllapsed;
            }

            public override void Unload()
            {
                if (this.RenderTarget2D != null)
                    this.RenderTarget2D.Dispose();
                if (this.Canvas != null)
                    this.Canvas.Dispose();

                this.RenderTarget2D = null;
                this.Canvas = null;
            }

            public override void Draw()
            {
                if (this.String == "")
                    return;

                if (this.IsVisible())
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);

                    Vector2 position = this.ScreenPos();

                    // if we only have 1 effect, spare the render target
                    if (this.RenderTarget2D != null)
                        spriteBatch.Draw(this.GetTexture(), position - new Vector2(20, 20), null, Color.White * this.Alpha, this.Rotation, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
                    else
                        spriteBatch.DrawString(this.Font, this.String, position, this.Color * this.Alpha, this.Rotation, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);

                    spriteBatch.End();
                }
            }

            public override void Resize()
            {
                this.Width = (int)Math.Round(this.Font.MeasureString(this.String).X * this.Scale);
                
                // maybe once they fix this !
                if (this._Font.VerticleSpacing != 0)
                    this.Height = this._Font.VerticleSpacing;
                else
                    this.Height = (int)Math.Round(this.Font.MeasureString(this.String).Y * this.Scale);
            }

            public override Texture2D GetTexture()
            {
                if (this.EffectAgents.Count % 2 == 1)
                    return this.Canvas;
                else
                    return this.RenderTarget2D;
            }
        }
    }
}
