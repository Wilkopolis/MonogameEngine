using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#pragma warning disable IDE0017 // Simplify object initialization

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum EffectAgentType {
            ColorShift,
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
                }

                foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
                    pass.Apply();
            }
        }
    }
}
