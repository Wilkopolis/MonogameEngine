using Microsoft.Xna.Framework;
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
            ColorShift,
            RectangleMask,
            KeyMask,
            RadialMask,
            Blur,
            Pixelate,
            Noise
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

                    case EffectAgentType.RectangleMask:

                        // apply our offsets to X and Y
                        float maskX = this.Parameters["X"];
                        float maskY = this.Parameters["Y"];

                        if (this.ScrollKey != "")
                        {
                            maskX += ScrollOffsets[this.ScrollKey].X;
                            maskY += ScrollOffsets[this.ScrollKey].Y;
                        }

                        float elementX = element.Pos().X;
                        float elementY = element.Pos().Y;
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
                }

                foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
                    pass.Apply();
            }
        }
    }
}
