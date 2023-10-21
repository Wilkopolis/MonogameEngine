using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonogameEngine.MonogameEngine;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum EffectType
        {
            Color,
            Gradient,
            Lighten,
            Stroke,
            GlowSmoke,
            Fog,
            ChestLootBeam,
            Zap,
            ZapDistort,
            LevelUp1Front,
            LevelUp1Back,
            LevelUp2Front,
            LevelUp2Back,
            // Masks
            IncludeMask, ExcludeMask, KeyMask, RadialMask,
            // Blur
            Blur, Pixelate, Noise
        }

        public abstract partial class Element
        {
            public virtual EffectAgent AddEffect(EffectType type)
            {
                if (this is CompoundElement || this is Button)
                {
                    Oops();
                    return null;
                }

                EffectAgent result = null;
                int priority = 10;

                switch (type)
                {
                    case EffectType.Lighten:
                        result = new EffectAgent();
                        result.Effect = Effects["color_shift"];
                        result.Type = EffectAgentType.ColorShift;
                        result.Parameters["R"] = 1.25f;
                        result.Parameters["G"] = 1.25f;
                        result.Parameters["B"] = 1.25f;
                        break;
                    case EffectType.GlowSmoke:
                        result = new EffectAgent();
                        result.Effect = Effects["color_smoke"];
                        result.Type = EffectAgentType.ColorSmoke;
                        result.Parameters["R"] = 1f;
                        result.Parameters["G"] = .8f;
                        result.Parameters["B"] = .2f;
                        result.Parameters["Radius"] = 40f;
                        result.Parameters["Power"] = 0f;
                        break;
                    case EffectType.Fog:
                        result = new EffectAgent();
                        result.Effect = Effects["fog"];
                        result.Type = EffectAgentType.Fog;
                        break;
                    case EffectType.ChestLootBeam:
                        result = new EffectAgent();
                        result.Effect = Effects["chest_beam"];
                        result.Type = EffectAgentType.ChestLootBeam;
                        result.TimeScale = .006f;
                        result.Parameters["R"] = .7f * 1.5f;
                        result.Parameters["G"] = .5f * 1.5f;
                        result.Parameters["B"] = .2f * 1.5f;
                        break;
                    case EffectType.Zap:
                        result = new EffectAgent();
                        result.Effect = Effects["zap"];
                        result.Type = EffectAgentType.Zap;
                        result.TimeScale = .001f;
                        result.Parameters["R"] = .7f * 1.5f;
                        result.Parameters["G"] = .5f * 1.5f;
                        result.Parameters["B"] = .2f * 1.5f;
                        break;
                    case EffectType.ZapDistort:
                        result = new EffectAgent();
                        result.Effect = Effects["zap_distort"];
                        result.Type = EffectAgentType.ZapDistort;
                        result.TimeScale = .001f;
                        result.Parameters["X"] = (float)Random.NextDouble() * 10;
                        result.Parameters["Y"] = Random.Next(2) - 1;
                        break;
                    case EffectType.LevelUp1Front:
                        result = new EffectAgent();
                        result.Effect = Effects["level_up_beam"];
                        result.Type = EffectAgentType.LvlUpBeam;
                        result.TimeScale = .002f;
                        result.Parameters["X"] = 0;
                        result.Parameters["Y"] = 0;
                        break;
                    case EffectType.LevelUp1Back:
                        result = new EffectAgent();
                        result.Effect = Effects["level_up_beam"];
                        result.Type = EffectAgentType.LvlUpBeam;
                        result.TimeScale = .002f;
                        result.Parameters["X"] = 1;
                        result.Parameters["Y"] = 0;
                        break;
                    case EffectType.LevelUp2Front:
                        result = new EffectAgent();
                        result.Effect = Effects["level_up_beam"];
                        result.Type = EffectAgentType.LvlUpBeam;
                        result.TimeScale = .002f;
                        result.Parameters["X"] = 0;
                        result.Parameters["Y"] = 1;
                        break;
                    case EffectType.LevelUp2Back:
                        result = new EffectAgent();
                        result.Effect = Effects["level_up_beam"];
                        result.Type = EffectAgentType.LvlUpBeam;
                        result.TimeScale = .002f;
                        result.Parameters["X"] = 1;
                        result.Parameters["Y"] = 1;
                        break;
                    case EffectType.Gradient:
                        result = new EffectAgent();
                        result.Effect = Effects["gradient"];
                        result.Type = EffectAgentType.Gradient;
                        result.Parameters["CX"] = .5f;
                        result.Parameters["CY"] = .5f;
                        result.Parameters["Radius"] = .5f;
                        result.Parameters["Alpha1"] = 1;
                        result.Parameters["Alpha2"] = 0;
                        break;
                }

                result.Priority = priority;
                result.EffectType = type;
                this.EffectAgents.Add(result);

                return result;
            }

            public virtual EffectAgent AddMask(EffectType mask, float x = 0, float y = 0, float w = 0, float h = 0, string scrollKey = "", Texture2D texture = null)
            {
                if (this is CompoundElement || this is Button)
                {
                    Oops();
                    return null;
                }

                EffectAgent result = null;
                int priority = 99;

                switch (mask)
                {
                    case EffectType.ExcludeMask:

                        result = new EffectAgent();
                        result.Type = EffectAgentType.RectangleMask;
                        result.Effect = Effects["rectangular_mask"];
                        result.Parameters["X"] = x;
                        result.Parameters["Y"] = y;
                        result.Parameters["W"] = w;
                        result.Parameters["H"] = h;
                        result.ScrollKey = scrollKey;

                        break;

                    case EffectType.IncludeMask:

                        result = new EffectAgent();
                        result.Type = EffectAgentType.RectangleMask;
                        result.Effect = Effects["inverse_mask"];
                        result.Parameters["X"] = x;
                        result.Parameters["Y"] = y;
                        result.Parameters["W"] = w;
                        result.Parameters["H"] = h;
                        result.ScrollKey = scrollKey;

                        break;

                    case EffectType.KeyMask:

                        result = new EffectAgent();
                        result.Type = EffectAgentType.KeyMask;
                        result.Effect = Effects["key_mask"];
                        result.Parameters["X"] = x;
                        result.Parameters["Y"] = y;
                        result.ScrollKey = scrollKey;
                        result.Texture = texture;

                        break;

                    case EffectType.RadialMask:

                        result = new EffectAgent();
                        result.Type = EffectAgentType.RadialMask;
                        result.Effect = Effects["radial_mask"];
                        result.Parameters["Radius"] = x;

                        break;
                }

                result.Priority = priority;
                result.EffectType = mask;
                this.EffectAgents.Add(result);

                return result;
            }

            public virtual EffectAgent AddBlur(EffectType blur, float amount = 0)
            {
                if (this is CompoundElement || this is Button)
                {
                    Oops();
                    return null;
                }

                EffectAgent result = null;
                int priority = 89;

                switch (blur)
                {
                    case EffectType.Blur:

                        result = new EffectAgent();
                        result.Type = EffectAgentType.Blur;
                        result.Parameters["Radius"] = amount;
                        result.Effect = Effects["blur"];

                        break;

                    case EffectType.Pixelate:

                        result = new EffectAgent();
                        result.Type = EffectAgentType.Pixelate;
                        result.Parameters["Radius"] = amount;
                        result.Effect = Effects["pixelate"];

                        break;

                    case EffectType.Noise:

                        result = new EffectAgent();
                        result.Type = EffectAgentType.Noise;
                        result.Parameters["Amount"] = amount;
                        result.Effect = Effects["noise"];

                        break;
                }

                result.Priority = priority;
                result.EffectType = blur;
                this.EffectAgents.Add(result);

                return null;
            }

            public virtual EffectAgent AddStroke(EffectType stroke, float radius, float r = 0, float g = 0, float b = 0, float a = 1)
            {
                if (this is CompoundElement || this is Button)
                {
                    Oops();
                    return null;
                }

                EffectAgent result = null;
                int priority = 10;

                switch (stroke)
                {
                    case EffectType.Stroke:

                        result = new EffectAgent();
                        result.Type = EffectAgentType.Stroke;
                        result.Parameters["Radius"] = radius;
                        result.Parameters["R"] = r;
                        result.Parameters["G"] = g;
                        result.Parameters["B"] = b;
                        result.Parameters["A"] = a;
                        result.Effect = Effects["stroke"];

                        break;
                }

                result.Priority = priority;
                result.EffectType = stroke;
                this.EffectAgents.Add(result);

                return null;
            }

            public virtual EffectAgent AddBorderRadius(int radius)
            {
                if (this is CompoundElement || this is Button)
                {
                    Oops();
                    return null;
                }

                return null;
            }

            public virtual EffectAgent GetEffect(EffectType type)
            {
                foreach (EffectAgent ea in this.EffectAgents)
                {
                    if (ea.EffectType == type)
                    {
                        return ea;
                    }
                }
                return null;
            }

            public virtual bool HasEffect(EffectType type)
            {
                return this.EffectAgents.Any(ea => ea.EffectType == type);
            }

            public void RemoveEffect(EffectType type)
            {
                this.EffectAgents.RemoveAll(ea => ea.EffectType == type);
            }

            public void RemoveAllEffects()
            {
                this.EffectAgents.RemoveAll(ea => true);
            }

            public virtual List<EffectAgent> GetAllEffects()
            {
                return this.EffectAgents;
            }
        }
    }
}
