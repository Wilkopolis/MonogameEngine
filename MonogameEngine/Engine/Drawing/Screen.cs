using Microsoft.Xna.Framework;
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
        // screens
        static Dictionary<string, Screen> Screens = new Dictionary<string, Screen>();
        // to track pressed buttons
        static List<Element> PressedElements = new List<Element>();

        // shaders
        static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
        static Dictionary<string, bool> GlobalFlags = new Dictionary<string, bool>();
        // debug
        static Dictionary<string, Element> DebugElements = new Dictionary<string, Element>();

        public class Screen : Element
        {
            // for drawing, mouse interaction, describing, everything
            public Dictionary<string, Element> Elements = new Dictionary<string, Element>();
            // animations
            public new Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();
            // stuff we want to keep track of for later
            public Dictionary<string, Element> Storage = new Dictionary<string, Element>();
            public Dictionary<string, List<Element>> DeepStorage = new Dictionary<string, List<Element>>();

            public Action<float> UpdateHandler = null;

            public Screen(int width, int height)
            {
                this.Texture = new RenderTarget2D(graphics, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

                this.Width = width;
                this.Height = height;
            }

            public void Add(string key, Element element)
            {
                element.Parent = this;
                element.Key = key;
                this.Elements[element.Key] = element;
            }

            public void Add(Element element)
            {
                element.Parent = this;
                this.Elements[element.Key] = element;
            }

            public override List<Element> GetAllInteractables()
            {
                List<Element> results = new List<Element>();
                foreach (Element element in this.Elements.Values)
                {
                    if (!element.IsVisible() || !element.IsOnScreen())
                        continue;

                    if (element.MouseOverCheck || element.Clickable || element.Draggable || element.ToolTip != "")
                        results.Add(element);
                    else if (element is Screen || element is CompoundElement)
                        results.AddRange(element.GetAllInteractables());
                }

                return results;
            }

            public override void Draw()
            {
                if (!this.Visible)
                    return;

                spriteBatch.Begin(SpriteSortMode.Immediate, this.BlendState);
                spriteBatch.Draw(this.Texture, this.ScreenPos(), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.End();
            }

            public override void Resize()
            {
                throw new NotImplementedException();
            }

            public override void Unload()
            {
                foreach (Element element in Elements.Values)
                    Remove(element);
            }

            public void Update(float deltaTime)
            {
                // do any animations
                for (int i = 0; i < this.Animations.Count; i++)
                {
                    var entry = this.Animations.ElementAt(i);
                    Animation animation = entry.Value;

                    // make some software animations lower framerate
                    if (MsEllapsed - animation.LastFrame >= animation.FrameLength)
                    {
                        if (animation.Running)
                            animation.Tick();

                        animation.LastFrame = MsEllapsed;

                        if (animation.Over)
                        {
                            this.Animations.Remove(entry.Key);
                            i--;

                            animation.Callback?.Invoke();
                        }
                    }
                }

                // update all our children
                foreach (Element element in this.Elements.Values)
                {
                    if (element is Screen screen)
                        screen.Update(deltaTime);
                }

                this.UpdateHandler?.Invoke(deltaTime);
            }

            public override void Render(bool direct = false)
            {
                // if we are called to render, then render everything out
                if (!direct)
                {
                    if (this.RenderPeriod == -1)
                        return;

                    if (MsEllapsed - this.LastRenderTime < this.RenderPeriod)
                        return;
                }

                // render out all our elements
                foreach (Element element in this.Elements.Values)
                    element.Render(direct);

                // draw everything to the render target
                graphics.SetRenderTarget((RenderTarget2D)this.Texture);
                graphics.Clear(this.ClearColor);

                // sort all the elements by zIndex
                List<Element> toDraw = new List<Element>(this.Elements.Values);
                toDraw = toDraw.OrderBy(o => o.GetzIndex()).ToList();

                // draw elements accoring to z-index
                foreach (Element element in toDraw)
                    element.Draw();

                graphics.SetRenderTarget(null);
            }

            public void Remove(Element element)
            {
                if (element is CompoundElement compound)
                {
                    foreach (Element child in compound.GetElements())
                        Remove(child);
                }

                element.Unload();

                if (Elements.ContainsValue(element))
                    Elements.Remove(element.Key);

                element = null;
            }

            public void Remove(String key)
            {
                if (key == null)
                    return;

                Element element = null;

                if (Elements.ContainsKey(key))
                    element = Elements[key];

                if (element == null)
                    return;

                element.Unload();

                if (Elements.ContainsKey(key))
                    Elements.Remove(key);
                if (Animations.ContainsKey(key))
                    Animations.Remove(key);

                element = null;
            }

            public void RemoveByPattern(string pattern)
            {
                if (pattern == null)
                    return;

                var keys = new List<string>(Elements.Keys);
                foreach (string key in keys)
                {
                    if (key.IndexOf(pattern) >= 0 && Elements.ContainsKey(key))
                    {
                        Remove(Elements[key]);
                    }
                }

                var keys5 = new List<string>(Animations.Keys);
                foreach (string key in keys5)
                {
                    if (key.IndexOf(pattern) >= 0)
                    {
                        Animations.Remove(key);
                    }
                }
            }

            public void HideByPattern(string pattern)
            {
                var keys = new List<string>(Elements.Keys);
                foreach (string key in keys)
                {
                    if (key.IndexOf(pattern) >= 0)
                    {
                        Elements[key].Visible = false;
                    }
                }
            }

            public void ShowByPattern(string pattern)
            {
                var keys = new List<string>(Elements.Keys);
                foreach (string key in keys)
                {
                    if (key.IndexOf(pattern) >= 0)
                    {
                        Elements[key].Visible = true;
                    }
                }
            }

            public List<Element> GetElementsByPattern(string pattern)
            {
                List<Element> result = new List<Element>();

                var keys = new List<string>(Elements.Keys);
                foreach (string key in keys)
                {
                    if (key.IndexOf(pattern) >= 0)
                    {
                        result.Add(Elements[key]);
                    }
                }

                return result;
            }
        }
    }
}
