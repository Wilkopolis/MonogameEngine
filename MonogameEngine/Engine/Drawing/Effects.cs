using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        Element MakeDropShadow(Element element, float strokeRadius = 1f, float blurRadius = 3, Color color = new Color())
        {
            // if the element hasn't been rendered yet, render it out
            if (element.GetTexture() == null)
                element.Render(true);

            // 1. Make a RT2D of original texture + some space
            RenderTarget2D rt2d = new RenderTarget2D(graphics, element.Width + 36, element.Height + 36);

            Sprite result = new Sprite(rt2d);
            result.BlendState = BlendState.AlphaBlend;

            // 2. Render out original texture to the target color
            graphics.SetRenderTarget(rt2d);
            graphics.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            Vector2 pos = new Vector2(rt2d.Width / 2f, rt2d.Height / 2f) - new Vector2(element.Width / 2f, element.Height / 2f);

            EffectAgent darken = new EffectAgent();
            darken.Effect = Effects["color_in"];
            darken.Parameters["R"] = color.R;
            darken.Parameters["G"] = color.G;
            darken.Parameters["B"] = color.B;
            darken.Parameters["forceAlpha"] = 0;
            darken.Type = EffectAgentType.Color;
            darken.Apply(result);

            spriteBatch.Draw(element.GetTexture(), pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            spriteBatch.End();
            graphics.SetRenderTarget(null);

            result.Texture = rt2d;
            result.Resize();

            // 3. Render out with a stroke
            result.AddStroke(EffectType.Stroke, strokeRadius);
            // 4. Blur the result
            result.AddBlur(EffectType.Blur, blurRadius);
            result.Render(true);
            result.RenderPeriod = -1;

            result.Center(element.GetCenter());
            return result;
        }

        Element MakeSpriteGlow(Element element, float strokeRadius = 1f, float blurRadius = 3, Color color = new Color())
        {
            // if the element hasn't been rendered yet, render it out
            if (element.GetTexture() == null)
                element.Render(true);

            // 1. Make a RT2D of original texture + some space
            RenderTarget2D rt2d = new RenderTarget2D(graphics, element.Width + 36, element.Height + 36);

            Sprite result = new Sprite(rt2d);

            // 2. Render out original texture to the target color
            graphics.SetRenderTarget(rt2d);
            graphics.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            Vector2 pos = new Vector2(rt2d.Width / 2f, rt2d.Height / 2f) - new Vector2(element.Width / 2f, element.Height / 2f);

            EffectAgent darken = new EffectAgent();
            darken.Effect = Effects["color_in"];
            darken.Parameters["R"] = 1;
            darken.Parameters["G"] = 1;
            darken.Parameters["B"] = 1;
            darken.Parameters["forceAlpha"] = 1;
            darken.Type = EffectAgentType.Color;
            darken.Apply(result);

            spriteBatch.Draw(element.GetTexture(), pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            spriteBatch.End();
            graphics.SetRenderTarget(null);

            result.Texture = rt2d;
            result.Resize();

            // 3. Render out with a stroke
            result.AddStroke(EffectType.Stroke, strokeRadius, 1, 1, 1);
            // 4. Blur the result
            result.AddBlur(EffectType.Blur, blurRadius);
            result.Render(true);
            //return result;

            // draw the blurred stroked texture onto a white background to make a final mask
            RenderTarget2D renderTarget2D = new RenderTarget2D(graphics, result.Width, result.Height);
            graphics.SetRenderTarget(renderTarget2D);
            graphics.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(result.GetTexture(), Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();
            graphics.SetRenderTarget(null);

            Box outline = new Box(result.Width, result.Height, Color.Black);
            EffectAgent glow = outline.AddEffect(EffectType.GlowSmoke);
            glow.Parameters["Radius"] = 10;
            glow.Parameters["Power"] = 0;
            glow.Parameters["R"] = color.R / 255f;
            glow.Parameters["G"] = color.G / 255f;
            glow.Parameters["B"] = color.B / 255f;
            outline.AddMask(EffectType.KeyMask, texture: renderTarget2D);
            outline.Center(element.GetCenter());
            outline.zIndex = element.GetzIndex() - .001f;

            return outline;
        }

        void AnimateSparkle(Element element, float intensity = 1f)
        {
            float sparkleDensity = 100f / element.Width * 300;
            sparkleDensity /= intensity;

            element.Flags["sparkleIntensity"] = sparkleDensity;

            float lastSparkle = MsEllapsed;

            Screen screen = element.GetScreen();

            screen.Animations["elementSparkle" + Hash++] = new CustomAnimation(0, delegate (float percentComplete)
            {
                // spawn one
                if (MsEllapsed - lastSparkle > element.Flags["sparkleIntensity"])
                {
                    float xMin = element.Pos().X - 5;
                    float xMax = element.Right() + 5;

                    float xPos = xMin + (float)Random.NextDouble() * (xMax - xMin);

                    float yMin = element.Pos().Y + 45;
                    float yMax = element.ScreenBot() + 15;

                    float yPos = yMin + (float)Random.NextDouble() * (yMax - yMin);

                    bool below = Random.Next(3) == 0;

                    Sprite sparkle = new Sprite(0, 0, Textures["core/sparkle/1"], 1f, element.zIndex);
                    sparkle.zIndex += below ? -.001f : .001f;
                    sparkle.Key = "sparkle" + Hash++;
                    screen.Elements[sparkle.Key] = sparkle;

                    sparkle.Center(new Vector2(xPos, yPos));

                    List<AnimationStage> stages = new List<AnimationStage>();

                    List<Option> Probablities = new List<Option>()
                    {
                        new Option { Choice = "0", Weight = 10f },
                        new Option { Choice = "1", Weight = 8f  },
                        new Option { Choice = "2", Weight = 3f  }
                    };

                    string outcome = (string)PickWeighted(Probablities);

                    float speed = (float)Random.NextDouble() * .2f + 1f;
                    float height = (float)Random.NextDouble() * .6f + .5f;

                    sparkle.Alpha = Math.Min(1, (float)Random.NextDouble() * -.3f + .9f);
                    sparkle.Scale = (float)Random.NextDouble() * .3f + .85f;

                    switch (outcome)
                    {
                        case "0":
                            stages.Add(new AnimationStage("core/sparkle/1", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/4", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/1", 150));
                            break;
                        case "1":
                            stages.Add(new AnimationStage("core/sparkle/1", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/4", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/4", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/1", 150));

                            sparkle.Position.Y += 20;
                            sparkle.Position.Y = Math.Min(sparkle.Position.Y, yMax);
                            break;
                        case "2":
                            stages.Add(new AnimationStage("core/sparkle/1", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/4", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/4", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/4", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/3", 150));
                            stages.Add(new AnimationStage("core/sparkle/2", 150));
                            stages.Add(new AnimationStage("core/sparkle/1", 150));

                            sparkle.Position.Y += 30;
                            sparkle.Position.Y = Math.Min(sparkle.Position.Y, yMax);
                            break;
                    }

                    foreach (AnimationStage stage in stages)
                    {
                        stage.Duration *= speed;
                        stage.FrameOffset *= height;
                    }

                    screen.Animations[sparkle.Key] = new FrameAnimation(sparkle, stages, callback: delegate
                    {
                        screen.Remove(sparkle.Key);
                    });
                    screen.Animations[sparkle.Key + 1] = new SlideAnimation(sparkle, (stages.Count + 2) * 150, 0, (stages.Count + 2) * -10) { Easing = Easing.Linear };

                    lastSparkle = MsEllapsed;
                }
            });
        }
    }
}
