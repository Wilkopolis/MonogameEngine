using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum EffectType
        {
            Lighten,
            // Masks
            IncludeMask, ExcludeMask, KeyMask, RadialMask,
            // Blur
            Blur, Pixelate, Noise
        }

        public abstract partial class Element
        {
            public virtual EffectAgent AddEffect(EffectType type)
            {
                if (this is CompoundElement || this is Button || this is Screen)
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
                        result.Effect = Effects["ColorShift"];
                        result.Type = EffectAgentType.ColorShift;
                        result.Parameters["R"] = 1.25f;
                        result.Parameters["G"] = 1.25f;
                        result.Parameters["B"] = 1.25f;
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
                int priority = 99;

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
