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
            Lighten
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
                int priority = 0;

                switch (type)
                {
                    case EffectType.Lighten:
                        result = new EffectAgent();
                        result.Effect = Effects["ColorShift"];
                        result.Type = EffectAgentType.ColorShift;
                        result.Parameters["R"] = 1.25f;
                        result.Parameters["G"] = 1.25f;
                        result.Parameters["B"] = 1.25f;
                        priority = 99;
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

                return null;
            }

            public virtual EffectAgent AddBlur(EffectType blur, float amount = 0, float x = 0, float y = 0)
            {
                if (this is CompoundElement || this is Button)
                {
                    Oops();
                    return null;
                }

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
