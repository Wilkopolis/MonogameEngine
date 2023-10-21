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
using BracketHouse.FontExtension;
using System.Xml.Linq;
using System.Security.AccessControl;

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
            public int FontSize;
            public float StrokeSize = 0;
            public Vector2 Offset = Vector2.Zero;

            public Phrase(string text, Font font, int fontSize) 
            {
                this.Text = text;
                this.Font = font;
                this.FontSize = fontSize;
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

                List<string> words = phrase.Text.Split(' ').ToList();
                if (phrase.Text.StartsWith(" "))
                    words.Insert(0, " ");

                for (int j = 0; j < words.Count; j++)
                {
                    string word = words[j];
                    if (word == "")
                        continue;

                    // add space if we arent the last word of the last phrase
                    if (j != words.Count - 1 && word != " ")
                        word += ' ';

                    Element text = new Text(x, y, word, phrase.Font, phrase.FontSize) { StrokeSize = phrase.StrokeSize };
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

        void LoadFonts()
        {
            string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @"Content\fonts\"));

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
                int keyIndx = f.FullName.IndexOf("Content") + 14;
                int lastDot = f.Name.LastIndexOf('.');
                int fileExtension = f.Name.Substring(lastDot + 1, f.Name.Length - lastDot - 1).Length + 1;
                string localPath = f.FullName.Substring(indx, f.FullName.Length - indx - fileExtension);
                string key = f.FullName.Substring(keyIndx, f.FullName.Length - keyIndx - fileExtension);
                key = key.Replace(@"\", "/");

                // translate the file name to our nice name
                string fieldName = Fonts.FontDefintions[key];

                // create the text renderer for that font
                FieldFont fieldFont = content.Load<FieldFont>(localPath);
                Font font = new Font(fieldFont, new TextRenderer(fieldFont, graphics, Effects["FieldFontEffect"]));
                foreach (FieldInfo fieldInfo in typeof(Fonts).GetFields())
                {
                    if (fieldInfo.Name == fieldName)
                    {
                        fieldInfo.SetValue(fieldInfo, font);
                    }
                }
            }
        }

        public class Font
        {
            public FieldFont FieldFont;
            public TextRenderer TextRenderer;
            public Color Color = Col(143, 140, 140);

            public Font(FieldFont fieldFont, TextRenderer textRenderer)
            {
                this.FieldFont = fieldFont;
                this.TextRenderer = textRenderer;
            }

            public Font Hue(Color color)
            {
                return new Font(this.FieldFont, this.TextRenderer) { Color = color };
            }
        }

        public static class Fonts
        {
            public static Font K2D;
            public static Font Acme;
            public static Font Arial;
            public static Font OpenSans;
            public static Font Catamaran;
            public static Font RobotoSlab;
            public static Font RobotoMono;
            public static Font Bahnschrift;
            public static Font Merriweather;

            public static Dictionary<string, string> FontDefintions = new Dictionary<string, string>
            {
                { "K2D Medium", "K2D" },
                { "Acme", "Acme" },
                { "Arial", "Arial" },
                { "Open Sans SemiBold", "OpenSans" },
                { "Catamaran-Regular", "Catamaran" },
                { "RobotoSlabRegular", "RobotoSlab" },
                { "RobotoMono", "RobotoMono" },
                { "Bahnschrift", "Bahnschrift" },
                { "Merriweather", "Merriweather" }
            };
        }

        public class Text : Element
        {
            public Font Font;
            public int FontSize = 12;

            public Color StrokeColor = Color.Black;
            // 0 to 1 of how much stroke to draw
            public float StrokeSize = 0;

            public string String = "";

            public static RenderTarget2D TextCanvas;

            public Text(string text = "", Font font = null, int fontSize = 20, float x = 0, float y = 0, float zIndex = 1f)
            {
                if (font == null)
                    font = Fonts.K2D;

                this.Position = new Vector2(x, y);
                this.String = text;
                this.Font = font;
                this.FontSize = fontSize;
                this.Color = font.Color;
                this.zIndex = zIndex;

                this.BlendState = BlendState.AlphaBlend;

                this.Resize();

                this.RenderPeriod = -1;
            }

            public Text(float x = 0, float y = 0, string text = "", Font font = null, int fontSize = 20, float zIndex = 1f)
            {
                if (font == null)
                    font = Fonts.K2D;

                this.Position = new Vector2(x, y);
                this.String = text;
                this.Font = font;
                this.FontSize = fontSize;
                this.Color = font.Color;
                this.zIndex = zIndex;

                this.RenderPeriod = -1;

                this.BlendState = BlendState.AlphaBlend;

                this.Resize();

                this.RenderPeriod = -1;
            }

            public override void Fit(int dimension, bool expand = false)
            {
                if (expand)
                {
                    // bring it up to the limit
                    while (this.Width <= dimension)
                    {
                        this.FontSize++;
                        this.Resize();

                        // if we crossed the line, go back one
                        if (this.Width > dimension)
                        {
                            this.FontSize--;
                            this.Resize();
                        }
                    }
                }
                else
                {
                    while (this.Width > dimension)
                    {
                        this.FontSize--;
                        this.Resize();
                    }
                }
            }

            public Text Clone()
            {
                Text result = new Text(this.String, this.Font, this.FontSize, this.Position.X, this.Position.Y, this.zIndex);
                return result;
            }

            public override void Resize()
            {
                Vector2 dimensions = this.Font.FieldFont.MeasureString(this.String, false) * this.FontSize;
                this.Width = (int)Math.Ceiling(dimensions.X);
                this.Height = (int)Math.Ceiling(dimensions.Y);
            }

            public override void Render(bool direct = false)
            {
                if (this.Texture != null && !direct)
                {
                    if (this.RenderPeriod == -1)
                        return;

                    if (MsEllapsed - this.LastRenderTime < this.RenderPeriod)
                        return;
                }

                this.Unload();

                // render our the text to a texture
                graphics.SetRenderTarget(Text.TextCanvas);
                graphics.Clear(this.ClearColor);

                this.Font.TextRenderer.ResetLayout();
                this.Font.TextRenderer.OptimizeForTinyText = this.FontSize <= 20;

                if (this.StrokeSize > 0)
                {
                    this.Font.TextRenderer.StrokeSize = this.StrokeSize * 1.5f * this.FontSize / 64.0f;
                    this.Font.TextRenderer.LayoutText(this.String, Vector2.Zero, this.Color, this.StrokeColor, this.FontSize, 0, Vector2.Zero);
                    this.Font.TextRenderer.RenderStrokedText();
                }
                else
                {
                    this.Font.TextRenderer.SimpleLayoutText(this.String, Vector2.Zero, this.Color, Color.Black, this.FontSize);
                    this.Font.TextRenderer.RenderText();
                }

                spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);
                this.Font.TextRenderer.DrawSprites(spriteBatch);
                spriteBatch.End();

                // make us a little fatter if we are stroking
                int Border = 0;
                if (this.EffectAgents.Any(effect => effect.EffectType == EffectType.Stroke))
                    Border = 10;

                this.Texture = new RenderTarget2D(graphics, this.Width + 2 * Border, this.Height + 2 * Border);

                graphics.SetRenderTarget((RenderTarget2D)this.Texture);
                graphics.Clear(this.ClearColor);

                spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);
                spriteBatch.Draw(Text.TextCanvas, new Vector2(Border, Border), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.End();

                graphics.SetRenderTarget(null);

                // do the shader stuff
                if (this.EffectAgents.Count > 10)
                    Oops();

                int adjW = this.Texture.Width;
                int adjH = this.Texture.Height;
                if (this.RenderTarget2D == null || this.RenderTarget2D.Width != adjW || this.RenderTarget2D.Height != adjH)
                {
                    if (this.RenderTarget2D != null)
                    {
                        this.RenderTarget2D.Dispose();
                        this.RenderTarget2D = null;
                    }

                    this.RenderTarget2D = new RenderTarget2D(graphics, adjW, adjH);
                }

                // draw the texture out
                graphics.SetRenderTarget(this.RenderTarget2D);

                graphics.Clear(this.ClearColor);

                spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);

                spriteBatch.Draw(this.Texture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                if (this.EffectAgents.Count > 0)
                {
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
                if (this.Texture != null)
                    this.Texture.Dispose();

                this.RenderTarget2D = null;
                this.Canvas = null;
                this.Texture = null;
            }

            public override void Draw()
            {
                if (this.String == "")
                    return;

                if (!this.IsVisible())
                    return;

                Vector2 pos = this.Pos();

                // due to stroking, we make the texture bigger and draw it offset. Correct it here
                if (this.EffectAgents.Any(effect => effect.EffectType == EffectType.Stroke))
                    pos -= new Vector2(10, 10);

                spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);
                spriteBatch.Draw(this.GetTexture(), pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.End();
            }
        }
    }
}
