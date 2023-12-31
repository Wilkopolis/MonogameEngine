﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;

#pragma warning disable IDE0017 // Simplify object initialization

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum EffectAgentType {
            Color,
            ColorShift,
            ColorSmoke,
            Gradient,
            RectangleMask,
            KeyMask,
            RadialMask,
            Blur,
            Pixelate,
            Noise,
            Stroke,
            Fog,
            ChestLootBeam,
            Zap,
            ZapDistort,
            LvlUpBeam
        };

        public class EffectAgent
        {
            public Dictionary<string, float> Parameters = new Dictionary<string, float>();
            public Effect Effect;
            public EffectAgentType Type;
            // for tracking frost and stuff
            public EffectType EffectType;
            public float Priority = 10;
            // for offsetting multiple ones
            public float TimeScale = 1;
            public float TimeOffset = 0;
            // for a black/white mask
            public Texture2D Texture;
            // Render only (only is applied during a render)
            public bool RenderOnly = false;
            public bool DrawOnly = false;
            // for some time based ones
            public long StartTime = 0;
            public long Elapsed = 0;
            public bool Paused = false;
            public long LastFrame = 0;
            public bool Relative = false;

            // in case we have a scroll key
            public string ScrollKey = "";

            public EffectAgent()
            {
                this.StartTime = MsEllapsed;
                this.LastFrame = MsEllapsed;
            }

            public EffectAgent Clone()
            {
                EffectAgent result = new EffectAgent();
                result.Effect = this.Effect;
                result.Type = this.Type;
                result.Parameters = new Dictionary<string, float>(this.Parameters);
                result.Priority = this.Priority;
                result.StartTime = this.StartTime;
                result.LastFrame = this.LastFrame;
                result.EffectType = this.EffectType;
                result.ScrollKey = this.ScrollKey;
                result.Relative = this.Relative;

                return result;
            }

            public void Apply(Element element)
            {
                switch (this.Type)
                {
                    case EffectAgentType.ColorShift:

                        this.Effect.Parameters["RScale"].SetValue(this.Parameters["R"]);
                        this.Effect.Parameters["GScale"].SetValue(this.Parameters["G"]);
                        this.Effect.Parameters["BScale"].SetValue(this.Parameters["B"]);

                        break;

                    case EffectAgentType.Color:

                        this.Effect.Parameters["R"].SetValue(this.Parameters["R"]);
                        this.Effect.Parameters["G"].SetValue(this.Parameters["G"]);
                        this.Effect.Parameters["B"].SetValue(this.Parameters["B"]);
                        this.Effect.Parameters["forceAlpha"].SetValue(this.Parameters["forceAlpha"]);

                        break;

                    case EffectAgentType.ColorSmoke:

                        this.Effect.Parameters["time"].SetValue(MsEllapsed);
                        this.Effect.Parameters["R"].SetValue(this.Parameters["R"]);
                        this.Effect.Parameters["G"].SetValue(this.Parameters["G"]);
                        this.Effect.Parameters["B"].SetValue(this.Parameters["B"]);
                        this.Effect.Parameters["A"].SetValue(1);
                        this.Effect.Parameters["w"].SetValue(element.Width);
                        this.Effect.Parameters["h"].SetValue(element.Height);
                        this.Effect.Parameters["radius"].SetValue(this.Parameters["Radius"]);
                        this.Effect.Parameters["power"].SetValue(this.Parameters["Power"]);

                        break;

                    case EffectAgentType.RectangleMask:

                        // apply our offsets to X and Y
                        float maskX = this.Parameters["X"];
                        float maskY = this.Parameters["Y"];

                        if (this.ScrollKey != "")
                        {
                            maskX += ScrollOffsets[this.ScrollKey].X;
                            maskY += ScrollOffsets[this.ScrollKey].Y;
                        }

                        float elementX = element.AbsPos().X;
                        float elementY = element.AbsPos().Y;
                        if (this.Relative)
                        {
                            maskX += elementX;
                            maskY += elementY;
                        }

                        //crop anything outside of the rectangle
                        this.Effect.Parameters["pos_x"].SetValue(elementX);
                        this.Effect.Parameters["pos_y"].SetValue(elementY);
                        this.Effect.Parameters["tex_w"].SetValue(element.Width);
                        this.Effect.Parameters["tex_h"].SetValue(element.Height);
                        this.Effect.Parameters["x"].SetValue(maskX);
                        this.Effect.Parameters["y"].SetValue(maskY);
                        this.Effect.Parameters["w"].SetValue(this.Parameters["W"]);
                        this.Effect.Parameters["h"].SetValue(this.Parameters["H"]);
                        this.Effect.Parameters["flipped"].SetValue(element.FlipHorizontal ? 1 : 0);

                        break;

                    case EffectAgentType.KeyMask:

                        this.Effect.Parameters["Mask"].SetValue(this.Texture);
                        this.Effect.Parameters["maskOffsetX"].SetValue(this.Parameters["X"]);
                        this.Effect.Parameters["maskOffsetY"].SetValue(this.Parameters["Y"]);
                        this.Effect.Parameters["maskWidth"].SetValue(this.Texture.Width);
                        this.Effect.Parameters["maskHeight"].SetValue(this.Texture.Height);
                        this.Effect.Parameters["textureWidth"].SetValue(element.Width);
                        this.Effect.Parameters["textureHeight"].SetValue(element.Height);

                        break;

                    case EffectAgentType.RadialMask:

                        float adjustedPer = (1 - this.Parameters["Radius"]) * 2f * (float)Math.PI;
                        this.Effect.Parameters["p1"].SetValue(adjustedPer);

                        break;

                    case EffectAgentType.Blur:

                        this.Effect.Parameters["Size"].SetValue(this.Parameters["Radius"]);
                        this.Effect.Parameters["w"].SetValue(element.Width);
                        this.Effect.Parameters["h"].SetValue(element.Height);

                        break;

                    case EffectAgentType.Pixelate:

                        this.Effect.Parameters["tileSize"].SetValue(this.Parameters["Radius"]);
                        this.Effect.Parameters["w"].SetValue(element.Width);
                        this.Effect.Parameters["h"].SetValue(element.Height);

                        break;

                    case EffectAgentType.Noise:

                        this.Effect.Parameters["noiseAmount"].SetValue(this.Parameters["Amount"]);

                        break;

                    case EffectAgentType.Stroke:

                        this.Effect.Parameters["r"].SetValue(this.Parameters["R"]);
                        this.Effect.Parameters["g"].SetValue(this.Parameters["G"]);
                        this.Effect.Parameters["b"].SetValue(this.Parameters["B"]);
                        this.Effect.Parameters["a"].SetValue(this.Parameters["A"]);
                        this.Effect.Parameters["outlineSize"].SetValue(this.Parameters["Radius"]);
                        this.Effect.Parameters["w"].SetValue(element.Width);
                        this.Effect.Parameters["h"].SetValue(element.Height);

                        break;

                    case EffectAgentType.Fog:

                        this.Effect.Parameters["time"].SetValue((MsEllapsed + this.TimeOffset) * this.TimeScale);
                        this.Effect.Parameters["center"].SetValue(new Vector2(this.Parameters["X"], this.Parameters["Y"]));

                        break;

                    case EffectAgentType.ChestLootBeam:

                        this.Effect.Parameters["time"].SetValue((MsEllapsed + this.TimeOffset) * this.TimeScale);
                        this.Effect.Parameters["w"].SetValue((float)element.Width);
                        this.Effect.Parameters["h"].SetValue((float)element.Height);
                        this.Effect.Parameters["r"].SetValue(this.Parameters["R"]);
                        this.Effect.Parameters["g"].SetValue(this.Parameters["G"]);
                        this.Effect.Parameters["b"].SetValue(this.Parameters["B"]);

                        break;

                    case EffectAgentType.Zap:

                        this.Effect.Parameters["ColorR"].SetValue(this.Parameters["R"]);
                        this.Effect.Parameters["ColorG"].SetValue(this.Parameters["G"]);
                        this.Effect.Parameters["ColorB"].SetValue(this.Parameters["B"]);
                        this.Effect.Parameters["width"].SetValue(element.Width);
                        this.Effect.Parameters["height"].SetValue(element.Height);
                        this.Effect.Parameters["time"].SetValue((MsEllapsed + this.TimeOffset) * this.TimeScale);

                        break;

                    case EffectAgentType.ZapDistort:

                        this.Effect.Parameters["offset"].SetValue(this.Parameters["X"]);

                        this.Effect.Parameters["p1"].SetValue(2f * this.Parameters["Y"]);
                        this.Effect.Parameters["p2"].SetValue(3f);
                        this.Effect.Parameters["p3"].SetValue(4f);
                        this.Effect.Parameters["p4"].SetValue(19f);
                        this.Effect.Parameters["p5"].SetValue(17f);

                        this.Effect.Parameters["q1"].SetValue(.5f);
                        this.Effect.Parameters["q2"].SetValue(.2f);
                        this.Effect.Parameters["q3"].SetValue(.27f);
                        this.Effect.Parameters["q4"].SetValue(.02f);
                        this.Effect.Parameters["q5"].SetValue(.01f);

                        break;

                    case EffectAgentType.LvlUpBeam:

                        this.Effect.Parameters["time"].SetValue((MsEllapsed + this.TimeOffset) * this.TimeScale);
                        this.Effect.Parameters["x"].SetValue(this.Parameters["X"]);
                        this.Effect.Parameters["y"].SetValue(this.Parameters["Y"]);
                        this.Effect.Parameters["w"].SetValue(element.Width);
                        this.Effect.Parameters["h"].SetValue(element.Height);

                        break;

                    case EffectAgentType.Gradient:
                        //position in the text where the gradient is centered
                        this.Effect.Parameters["x1"].SetValue(this.Parameters["CX"]);
                        this.Effect.Parameters["y1"].SetValue(this.Parameters["CY"]);
                        // how far before the gradient should hit 0
                        this.Effect.Parameters["radius"].SetValue(this.Parameters["Radius"]);
                        // alpha 2
                        this.Effect.Parameters["a1"].SetValue(this.Parameters["Alpha1"]);
                        this.Effect.Parameters["a2"].SetValue(this.Parameters["Alpha2"]);
                        break;
                }

                foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
                    pass.Apply();
            }
        }
    }
}
