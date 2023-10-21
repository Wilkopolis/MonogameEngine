using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static MonogameEngine.MonogameEngine;

#pragma warning disable IDE0017 // Simplify object initialization

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class Sprite : Element
        {
            public bool TextureDisposable = false;

            public Sprite(float x = 0, float y = 0, TextureEntry t = null, float scale = 1f, float zIndex = 1f)
            {
                t.Load();
                this.Texture = t.Texture;
                this.TextureDisposable = t.Disposable;
                this.Scale = scale;
                this.Position = new Vector2(x, y);
                this.zIndex = zIndex;
               
                this.Resize();
            }

            public Sprite(TextureEntry t = null, float x = 0, float y = 0, float scale = 1f, float zIndex = 1f)
            {
                t.Load();
                this.Texture = t.Texture;
                this.TextureDisposable = t.Disposable;
                this.Scale = scale;
                this.Position = new Vector2(x, y);
                this.zIndex = zIndex;
               
                this.Resize();
            }

            public Sprite(float x = 0, float y = 0, Texture2D t = null, float scale = 1f, float zIndex = 1f)
            {
                this.Texture = t;
                this.Scale = scale;
                this.Position = new Vector2(x, y);
                this.zIndex = zIndex;
               
                this.Resize();
            }

            public Sprite(Texture2D t = null, float x = 0, float y = 0, float scale = 1f, float zIndex = 1f)
            {
                this.Texture = t;
                this.Scale = scale;
                this.Position = new Vector2(x, y);
                this.zIndex = zIndex;
               
                this.Resize();
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

            public override void Render(bool cursor = false)
            {
                if (MsEllapsed - this.LastRenderTime < this.RenderPeriod)
                    return;

                if (this.EffectAgents.Count > 10)
                    Oops();

                int adjW = (int)Math.Round(this.Width / this.Scale);
                int adjH = (int)Math.Round(this.Height / this.Scale);
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

            public override void Draw()
            {
                if (this.IsVisible())
                {
                    Vector2 position = this.Pos();

                    if (this.RotationStyle == RotationStyle.Fixed)
                        position += this.Origin;
                    else if(this.RotationStyle == RotationStyle.InPlace)
                        position += this.Origin * this.Scale;

                    SpriteEffects spriteEffect = SpriteEffects.None;
                    if (this.FlipHorizontal)
                        spriteEffect = SpriteEffects.FlipHorizontally;

                    spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);

                    spriteBatch.Draw(this.GetTexture(), position, null, Color.White * this.Alpha, this.Rotation, this.Origin, this.Scale, spriteEffect, 1f);

                    spriteBatch.End();
                }
            }
            
            public override void Resize()
            {
                this.Width = (int)Math.Round(this.Texture.Width * this.Scale);
                this.Height = (int)Math.Round(this.Texture.Height * this.Scale);
            }

            public Sprite Clone()
            {
                Sprite result = new Sprite(0, 0, this.Texture, this.Scale, this.zIndex);
                result.Alpha = this.Alpha;
                result.Rotation = this.Rotation;
                result.Origin = this.Origin;
                result.Position = this.Position;
                return result;
            }
        }
    }
}
