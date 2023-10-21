using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#pragma warning disable IDE0017 // Simplify object initialization

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class Box : Element
        {
            public Box(int width = 100, int height = 100, Color color = new Color(), float x = 0, float y = 0, float zIndex = 1f)
            {
                this.Position = new Vector2(x, y);
                this.Color = color;
                this.zIndex = zIndex;

                this.Width = width;
                this.Height = height;

                this.Resize();
            }

            public Box(int width = 100, int height = 100, float x = 0, float y = 0, Color color = new Color(), float zIndex = 1f)
            {
                this.Position = new Vector2(x, y);
                this.Color = color;
                this.zIndex = zIndex;

                this.Width = width;
                this.Height = height;

                this.Resize();
            }

            public override void Unload()
            {
                if (this.Texture != null)
                    this.Texture.Dispose();

                this.Texture = null;

                if (this.RenderTarget2D != null)
                    this.RenderTarget2D.Dispose();
                if (this.Canvas != null)
                    this.Canvas.Dispose();

                this.RenderTarget2D = null;
                this.Canvas = null;
            }

            public override void Resize()
            {
                this.RecalcTexture();
            }

            public void RecalcTexture()
            {
                this.Unload();

                if (this.Width == 0 || this.Height == 0)
                    return;

                RenderTarget2D renderTarget = new RenderTarget2D(graphics, this.Width, this.Height);

                graphics.SetRenderTarget(renderTarget);

                graphics.Clear(this.Color);

                graphics.SetRenderTarget(null);

                this.Texture = renderTarget;
            }

            public override void Render(bool direct = false)
            {
                if (MsEllapsed - this.LastRenderTime < this.RenderPeriod)
                    return;

                if (this.EffectAgents.Count < 1)
                    return;

                if (this.Width == 0 || this.Height == 0 || this.Texture == null || !this.IsVisible())
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

            public override Texture2D GetTexture()
            {
                if (this.EffectAgents.Count > 0)
                {
                    if (this.EffectAgents.Count % 2 == 1)
                        return this.Canvas;
                    else
                        return this.RenderTarget2D;
                }
                else
                    return this.Texture;
            }

            public override void Draw()
            {
                if (!this.IsVisible())
                    return;

                Vector2 position = this.Pos();

                if (this.RotationStyle == RotationStyle.Fixed)
                    position += this.Origin;
                else if (this.RotationStyle == RotationStyle.InPlace)
                    position += this.Origin * this.Scale;

                SpriteEffects spriteEffect = SpriteEffects.None;
                if (this.FlipHorizontal)
                    spriteEffect = SpriteEffects.FlipHorizontally;

                spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);

                if (this.ScaleVector.X != 1 || this.ScaleVector.Y != 1)
                    spriteBatch.Draw(this.GetTexture(), position, null, Color.White * this.Alpha, this.Rotation, this.Origin, this.ScaleVector, spriteEffect, 1f);
                else
                    spriteBatch.Draw(this.GetTexture(), position, null, Color.White * this.Alpha, this.Rotation, this.Origin, this.Scale, spriteEffect, 1f);

                spriteBatch.End();
            }

            public Box Clone()
            {
                Box result = new Box(0, 0, this.Width, this.Height, this.Color, this.zIndex);
                result.Rotation = this.Rotation;
                result.Origin = this.Origin;
                result.Position = this.Position;
                foreach (EffectAgent ea in this.EffectAgents)
                {
                    result.EffectAgents.Add(ea.Clone());
                }

                result.Target = this.Target;

                return result;
            }
        }
    }
}
