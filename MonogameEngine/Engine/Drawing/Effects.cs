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
                    float yMax = element.Bot() + 15;

                    float yPos = yMin + (float)Random.NextDouble() * (yMax - yMin);

                    bool below = Random.Next(3) == 0;

                    Sprite sparkle = new Sprite(0, 0, Textures["core/sparkle/1"], 1f, element.zIndex);
                    sparkle.zIndex += below ? -.001f : .001f;
                    sparkle.Key = "sparkle" + Hash++;
                    screen.Add(sparkle);

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

        Element AnimateZap(Screen screen, Vector2 source, Vector2 target, float r = 1, float g = .9f, float b = .4f)
        {
            float hypo = (source - target).Length();
            hypo *= source.X < target.X ? -1 : 1;
            float oppo = source.Y - target.Y;
            double angle = Math.Asin(oppo / hypo);

            int length =(int)Math.Round(Math.Abs(hypo) * 1.5f / 1f);

            Box zap = new Box(length, length / 2, Color.White, 1f);
            zap.BlendState = BlendState.Additive;
            EffectAgent effect = zap.AddEffect(EffectType.Zap);
            zap.AddEffect(EffectType.ZapDistort);

            effect.Parameters["R"] = r;
            effect.Parameters["G"] = g;
            effect.Parameters["B"] = b;

            zap.Origin = new Vector2(zap.Width / 2f, zap.Height / 2f);
            zap.RotationStyle = RotationStyle.Remote;
            zap.Rotation = (float)angle;
            zap.Position = source + (target - source) / 2f;
            zap.Key = "zap" + screen.Flags["hash"]++;

            screen.Add(zap);

            screen.Animations[zap.Key] = new Fade(zap, 400, 0, delegate
            {
                screen.Remove(zap.Key);
            }) { Easing = Easing.Sin };

            return zap;
        }

        void AddBuffEffect(Character character)
        {
            //Sprite sprite = character.Sprite;
            //Screen screen = sprite.GetScreen();

            //// Beam 1 //
            //Box front = new Box(126 * 2, 130 * 2, Color.Black, sprite.zIndex + .001f);
            //front.Center(sprite.Position + new Vector2(55, 40));
            //front.Origin = new Vector2(front.Width / 2f, front.Height / 2f);
            //front.RotationStyle = RotationStyle.InPlace;
            //front.Rotation = (float)Math.PI / 2f;
            //front.AddEffect(EffectType.LevelUp1Front).TimeScale = .002f;
            //front.Key = sprite.Key + "buff_1";
            //screen.Elements[front.Key] = front;

            //Box back = new Box(126 * 2, 130 * 2, front.Position.X, front.Position.Y, Color.Black, sprite.zIndex - .001f);
            //back.Origin = new Vector2(back.Width / 2f, back.Height / 2f);
            //back.RotationStyle = RotationStyle.InPlace;
            //back.Rotation = (float)Math.PI / 2f;
            //back.AddEffect(EffectType.LevelUp1Back).TimeScale = .002f;
            //back.Key = sprite.Key + "buff_2";
            //screen.Elements[back.Key] = back;

            //// Beam 2 //
            //Box front2 = new Box(126 * 2, 130 * 2, front.Position.X, front.Position.Y, Color.Black, sprite.zIndex + .001f);
            //front2.Origin = new Vector2(front2.Width / 2f, front2.Height / 2f);
            //front2.RotationStyle = RotationStyle.InPlace;
            //front2.Rotation = (float)Math.PI / 2f;
            //front2.AddEffect(EffectType.LevelUp4Front).TimeScale = .002f;
            //front2.Key = sprite.Key + "buff_3";
            //screen.Elements[front2.Key] = front2;

            //Box back2 = new Box(126 * 2, 130 * 2, front.Position.X, front.Position.Y, Color.Black, sprite.zIndex - .001f);
            //back2.Origin = new Vector2(back2.Width / 2f, back2.Height / 2f);
            //back2.RotationStyle = RotationStyle.InPlace;
            //back2.Rotation = (float)Math.PI / 2f;
            //back2.AddEffect(EffectType.LevelUp4Back).TimeScale = .002f;
            //back2.Key = sprite.Key + "buff_4";
            //screen.Elements[back2.Key] = back2;

            //// make level up gradient
            //Box gradient = new Box(200, 60, 0, sprite.Position.Y + 72, new Color(.9f, .6f, .4f), sprite.zIndex - .002f);
            //gradient.CenterX(sprite.Position.X + ToX(65));
            //gradient.BlendState = BlendState.Additive;
            //gradient.AddEffect(EffectType.Gradient);
            //gradient.Key = "combat_effect_buff_" + character.HashId + "_5";
            //screen.Elements[gradient.Key] = gradient;

            //front.Alpha = .8f;
            //front2.Alpha = .8f;
            //back.Alpha = .8f;
            //back2.Alpha = .8f;

            //screen.Animations["combat_effect_buff_" + character.HashId + "_6"] = new CustomAnimation(gradient, 0, delegate (float percentComplete)
            //{
            //    gradient.RenderScale = .9f + .1f * (float)Math.Sin(MsEllapsed * .002f);
            //    gradient.Offsets["off"] = new Offset(new Vector2(ToX(-20f), ToY(-6f)) * .5f * (float)Math.Sin(MsEllapsed * .002f));
            //});

            //SetTimeout(screen, delegate
            //{
            //    // spawn a lvl up bar
            //    Sprite bar1 = new Sprite(0, 0, Textures["report_lvlup_bar1-1"], 1f, sprite.zIndex + .1f);
            //    bar1.Position = sprite.Position + new Vector2(ToX(25), ToY(50));
            //    bar1.Key = "combat_effect_buff_" + character.HashId + "_" + Hash++;
            //    bar1.ScrollKey = "dungeonFloor";
            //    screen.Elements[bar1.Key] = bar1;

            //    List<AnimationStage> stages = new List<AnimationStage>();

            //    stages.Add(new AnimationStage("report_lvlup_bar1-11", 45, new Vector2(0, ToY(0))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-10", 45, new Vector2(0, ToY(-4))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-9", 45, new Vector2(0, ToY(-8))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-8", 45, new Vector2(0, ToY(-12))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-7", 45, new Vector2(0, ToY(-16))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-6", 45, new Vector2(0, ToY(-20))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-5", 45, new Vector2(0, ToY(-24))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-4", 45, new Vector2(0, ToY(-28))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-3", 45, new Vector2(0, ToY(-32))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-2", 45, new Vector2(0, ToY(-36))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-1", 45, new Vector2(0, ToY(-40))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-2", 45, new Vector2(0, ToY(-44))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-3", 45, new Vector2(0, ToY(-48))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-4", 45, new Vector2(0, ToY(-52))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-5", 45, new Vector2(0, ToY(-56))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-6", 45, new Vector2(0, ToY(-60))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-7", 45, new Vector2(0, ToY(-64))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-8", 45, new Vector2(0, ToY(-68))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-9", 45, new Vector2(0, ToY(-72))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-10", 45, new Vector2(0, ToY(-76))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-11", 45, new Vector2(0, ToY(-80))));

            //    // give it an animation
            //    screen.Animations["combat_effect_buff_" + character.HashId + "_" + Hash++] = new FrameAnimation(bar1, stages, callback: delegate
            //    {
            //        screen.Remove(bar1.Key);
            //    });
            //}, 300);

            //SetTimeout(screen, delegate
            //{
            //    // spawn a lvl up bar
            //    Sprite bar1 = new Sprite(0, 0, Textures["report_lvlup_bar2-1"], 1f, sprite.zIndex + .1f);
            //    bar1.Position = sprite.Position + new Vector2(ToX(75), ToY(100));
            //    bar1.Key = "combat_effect_buff_" + character.HashId + "_" + Hash++;
            //    bar1.ScrollKey = "dungeonFloor";
            //    screen.Elements[bar1.Key] = bar1;

            //    List<AnimationStage> stages = new List<AnimationStage>();

            //    stages.Add(new AnimationStage("report_lvlup_bar2-11", 45, new Vector2(0, ToY(0))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-10", 45, new Vector2(0, ToY(-4))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-9", 45, new Vector2(0, ToY(-8))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-8", 45, new Vector2(0, ToY(-12))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-7", 45, new Vector2(0, ToY(-16))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-6", 45, new Vector2(0, ToY(-20))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-5", 45, new Vector2(0, ToY(-24))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-4", 45, new Vector2(0, ToY(-28))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-3", 45, new Vector2(0, ToY(-32))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-2", 45, new Vector2(0, ToY(-36))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-1", 45, new Vector2(0, ToY(-40))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-2", 45, new Vector2(0, ToY(-44))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-3", 45, new Vector2(0, ToY(-48))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-4", 45, new Vector2(0, ToY(-52))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-5", 45, new Vector2(0, ToY(-56))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-6", 45, new Vector2(0, ToY(-60))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-7", 45, new Vector2(0, ToY(-64))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-8", 45, new Vector2(0, ToY(-68))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-9", 45, new Vector2(0, ToY(-72))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-10", 45, new Vector2(0, ToY(-76))));
            //    stages.Add(new AnimationStage("report_lvlup_bar2-11", 45, new Vector2(0, ToY(-80))));

            //    // give it an animation
            //    screen.Animations["combat_effect_buff_" + character.HashId + "_" + Hash++] = new FrameAnimation(bar1, stages, callback: delegate
            //    {
            //        screen.Remove(bar1.Key);
            //    });
            //}, 700);

            //SetTimeout(screen, delegate
            //{
            //    // spawn a lvl up bar
            //    Sprite bar1 = new Sprite(0, 0, Textures["report_lvlup_bar1-1"], .5f, sprite.zIndex - .1f);
            //    bar1.Position = sprite.Position + new Vector2(ToX(25), ToY(50));
            //    bar1.Key = "combat_effect_buff_" + character.HashId + "_" + Hash++;
            //    bar1.ScrollKey = "dungeonFloor";
            //    screen.Elements[bar1.Key] = bar1;

            //    List<AnimationStage> stages = new List<AnimationStage>();

            //    stages.Add(new AnimationStage("report_lvlup_bar1-11", 45, new Vector2(0, ToY(0))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-10", 45, new Vector2(0, ToY(-4))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-9", 45, new Vector2(0, ToY(-8))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-8", 45, new Vector2(0, ToY(-12))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-7", 45, new Vector2(0, ToY(-16))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-6", 45, new Vector2(0, ToY(-20))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-5", 45, new Vector2(0, ToY(-24))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-4", 45, new Vector2(0, ToY(-28))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-3", 45, new Vector2(0, ToY(-32))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-2", 45, new Vector2(0, ToY(-36))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-1", 45, new Vector2(0, ToY(-40))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-2", 45, new Vector2(0, ToY(-44))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-3", 45, new Vector2(0, ToY(-48))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-4", 45, new Vector2(0, ToY(-52))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-5", 45, new Vector2(0, ToY(-56))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-6", 45, new Vector2(0, ToY(-60))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-7", 45, new Vector2(0, ToY(-64))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-8", 45, new Vector2(0, ToY(-68))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-9", 45, new Vector2(0, ToY(-72))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-10", 45, new Vector2(0, ToY(-76))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-11", 45, new Vector2(0, ToY(-80))));

            //    // give it an animation
            //    screen.Animations["combat_effect_buff_" + character.HashId + "_" + Hash++] = new FrameAnimation(bar1, stages, callback: delegate
            //    {
            //        screen.Remove(bar1.Key);
            //    });

            //}, 1500);

            //SetTimeout(screen, delegate
            //{
            //    // spawn a lvl up bar
            //    Sprite bar1 = new Sprite(0, 0, Textures["report_lvlup_bar1-1"], 1f, sprite.zIndex - .1f);
            //    bar1.Position = sprite.Position + new Vector2(ToX(90), ToY(50));
            //    bar1.Key = "combat_effect_buff_" + character.HashId + "_" + Hash++;
            //    bar1.ScrollKey = "dungeonFloor";
            //    Elements[bar1.Key] = bar1;

            //    List<AnimationStage> stages = new List<AnimationStage>();

            //    stages.Add(new AnimationStage("report_lvlup_bar1-11", 45, new Vector2(0, ToY(0))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-10", 45, new Vector2(0, ToY(-4))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-9", 45, new Vector2(0, ToY(-8))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-8", 45, new Vector2(0, ToY(-12))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-7", 45, new Vector2(0, ToY(-16))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-6", 45, new Vector2(0, ToY(-20))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-5", 45, new Vector2(0, ToY(-24))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-4", 45, new Vector2(0, ToY(-28))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-3", 45, new Vector2(0, ToY(-32))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-2", 45, new Vector2(0, ToY(-36))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-1", 45, new Vector2(0, ToY(-40))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-2", 45, new Vector2(0, ToY(-44))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-3", 45, new Vector2(0, ToY(-48))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-4", 45, new Vector2(0, ToY(-52))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-5", 45, new Vector2(0, ToY(-56))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-6", 45, new Vector2(0, ToY(-60))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-7", 45, new Vector2(0, ToY(-64))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-8", 45, new Vector2(0, ToY(-68))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-9", 45, new Vector2(0, ToY(-72))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-10", 45, new Vector2(0, ToY(-76))));
            //    stages.Add(new AnimationStage("report_lvlup_bar1-11", 45, new Vector2(0, ToY(-80))));

            //    // give it an animation
            //    screen.Animations["combat_effect_buff_" + character.HashId + "_" + Hash++] = new FrameAnimation(bar1, stages, callback: delegate
            //    {
            //        screen.Remove(bar1.Key);
            //    });

            //    gradient.DoNotInheritAlpha = true;
            //    screen.Animations["combat_effect_buff_" + character.HashId + "_" + Hash++] = new Fade(gradient, 3000, 0f);
            //}, 1900);
        }
    }
}
