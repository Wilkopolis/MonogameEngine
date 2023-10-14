using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework;

#pragma warning disable IDE0017 // Simplify object initialization

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum RotationStyle { 
            Fixed,     // Adds origin to position
            InPlace,   // Adds origin and scales it to sprite scale
            Remote     // Does NOT add origin to pixel position
        };

        public abstract partial class Element
        {
            public Texture2D Texture;
            public Color Color;

            public int Width;
            public int Height;

            // for linking to dictionaries
            public string Key;

            // for on screen position
            public Vector2 Position = Vector2.Zero;
            
            // for linking offsets to keys
            public Dictionary<string, Vector2> Offsets = new Dictionary<string, Vector2>();
            // for when the element is being drawn like a cursor
            public Vector2 CursorOffset = Vector2.Zero;

            // for any collective offsets
            public List<string> ScrollKeys = new List<string>();

            // for drawing
            public bool Visible = true;
            // for clicking
            public bool Clickable = false;
            // for describing
            public bool Describable = false;
            // for dragging
            public bool Draggable = false;
            // what type of cursor do you want to draw when moused over
            public CursorType Cursor = CursorType.Arrow;

            // for draw order
            public float zIndex = 1f;
            public Dictionary<string, float> zIndexOffsets = new Dictionary<string, float>();

            // for rotating
            public float Rotation = 0;
            public Vector2 Origin = Vector2.Zero;
            public RotationStyle RotationStyle = RotationStyle.Fixed;

            // built in effect
            public bool FlipHorizontal = false;
            // texture scale
            public float Scale = 1f;
            // sometimes it scales in funny directions
            public Vector2 ScaleVector = new Vector2(1, 1);

            // for screens / some box effects
            public Color ClearColor = Color.Transparent;

            // -1, never render again. 0 render every frame. 
            public float RenderPeriod = 0;
            public float LastRenderTime = Int32.MinValue;

            public float Alpha = 1f;
            public BlendState BlendState = BlendState.AlphaBlend;

            // this is for compound elements if they want to inherit alpha
            public bool DoNotInheritAlpha = false;

            // for compound elements where resize breaks because of text height stuff
            public bool DontResize = false;

            // animations we own
            public List<Animation> Animations = new List<Animation>();

            // buttons and dragging
            public bool Pressed;

            // for Items and Mods
            public Object Target;
            public string ToolTip = "";

            // for compound elements/screens
            public Element Parent;
            public bool IgnoreDuringSizing = false;

            // for things that are drawn to a renderTarget
            public RenderTarget2D RenderTarget2D;
            // you can't draw a render target to itself, store this here
            public RenderTarget2D Canvas;

            protected List<EffectAgent> EffectAgents = new List<EffectAgent>();

            // for miscellaneous flags and timekeeping
            public Dictionary<string, float> Flags = new Dictionary<string, float>() { { "hash", 0 } };

            public abstract void Draw();
            public abstract void Resize();
            public abstract void Unload();

            // the action to perform when we click the button
            public Action<Element> MouseDownHandler;
            // mouse downed and moused up
            public Action<Element> ClickHandler;
            // when you dragging on this element
            public Action<Element> DragHandler;
            // when you finish dragging this element
            public Action<Element> DragReleaseHandler;
            // when you mouse over the thing
            public Action<Element> MouseOverHandler;
            // when you leave the thing
            public Action<Element> MouseLeaveHandler;

            public Sound DragSound = null;
            public Sound ClickSound = null;
            public Sound MouseOverSound = null;
            public Sound MouseDownSound = null;
            public Sound MouseLeaveSound = null;
            public Sound DragReleaseSound = null;

            public void OnClick()
            {
                // play its sound effect
                if (this.ClickSound != null)
                    this.ClickSound.Play();

                if (this.ClickHandler != null)
                    LOG(this.Key + ".OnClick(): " + this.ClickHandler.Method.Name);

                this.ClickHandler?.Invoke(this);
            }

            public void OnDrag()
            {
                if (this.DragHandler != null)
                    LOG(this.Key + ".OnDrag(): " + this.DragHandler.Method.Name);

                // disable future click events
                this.Pressed = false;

                // play its sound effect
                if (this.DragSound != null)
                    this.DragSound.Play();

                this.DragHandler?.Invoke(this);
            }

            public void OnDragRelease()
            {
                if (this.DragReleaseHandler != null)
                    LOG(this.Key + ".OnDragRelease(): " + this.DragReleaseHandler.Method.Name);

                // play its sound effect
                if (this.DragReleaseSound != null)
                    this.DragReleaseSound.Play();

                this.DragReleaseHandler?.Invoke(this);
            }

            public virtual void OnMouseDown()
            {
                if (this.MouseDownHandler != null)
                    LOG(this.Key + ".OnMouseDown(): " + this.MouseDownHandler.Method.Name);

                this.Pressed = true;
                PressedElements.Add(this);

                // play its sound effect
                if (this.MouseDownSound != null)
                    this.MouseDownSound.Play();

                this.MouseDownHandler?.Invoke(this);
            }

            public virtual void OnMouseUp()
            {
                if (this.Pressed && this.Clickable)
                    this.OnClick();
            }

            public virtual void OnMouseOver()
            {
                // play its sound effect
                if (this.MouseOverSound != null)
                    this.MouseOverSound.Play();
                    
                this.MouseOverHandler?.Invoke(this);                
            }

            public void OnMouseLeave()
            {
                // play its sound effect
                if (this.MouseLeaveSound != null)
                    this.MouseLeaveSound.Play();
                    
                this.MouseLeaveHandler?.Invoke(this);    
            }

            public virtual Texture2D GetTexture()
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

            public abstract void Render(bool direct = false);

            public virtual void Fit(int dimensions, bool expand = false)
            {
                float hscale = dimensions / (float)this.Texture.Height;
                float wscale = dimensions / (float)this.Texture.Width;
                float min = hscale < wscale ? hscale : wscale;
                this.Scale = min;

                this.Resize();
            }

            public virtual List<Element> GetAllInteractables()
            {
                return new List<Element>();
            }

            public virtual bool IsVisible()
            {
                if (this.Visible && this.Parent != null)
                    return this.Parent.IsVisible();
                else
                    return this.Visible;
            }

            public virtual bool IsOnScreen()
            {
                // if offscreen, just say no
                float posx = this.Pos().X;
                float posy = this.Pos().Y;
                float right = this.Right();
                float bot = this.Bot();

                float screenX = 0;
                float screenY = 0;
                float screenRight = 1920;
                float screenBot = 1080;
                Screen screen = this.GetScreen();
                if (screen != null)
                {
                    screenX = screen.Pos().X;
                    screenY = screen.Pos().Y;
                    screenRight = screen.Right();
                    screenBot = screen.Bot();
                }

                bool check1 = (posx >= screenX && posx <= screenRight);
                bool check2 = (right >= screenX && right <= screenRight);
                bool check3 = (posy >= screenY && posy <= screenBot);
                bool check4 = (bot >= screenY && bot <= screenBot);
                bool result = (check1 || check2) && (check3 || check4);
                if (!result)
                    return result;

                if (this.Parent != null && !this.Parent.Visible)
                    return false;

                return result;
            }

            public bool IsPressed()
            {
                return this.Pressed && this.IsMouseOver();
            }

            public virtual Vector2 GetScreenCenter()
            {
                Vector2 Pos = this.Position;
                return new Vector2(Pos.X + this.Width / 2f, Pos.Y + this.Height / 2f);
            }

            public virtual Vector2 GetCenter()
            {
                Vector2 Pos = this.Pos();
                return new Vector2(Pos.X + this.Width / 2f, Pos.Y + this.Height / 2f);
            }

            public virtual float GetCenterX()
            {
                return this.Pos().X + this.Width / 2f;
            }

            public virtual float GetCenterY()
            {
                return this.Pos().Y + this.Height / 2f;
            }

            public virtual float Bot()
            {
                return this.Pos().Y + this.Height;
            }

            public virtual float Right()
            {
                return this.Pos().X + this.Width;
            }

            public virtual void Center(Vector2 center)
            {
                this.CenterX(center.X);
                this.CenterY(center.Y);
            }

            public virtual void ScreenCenter(Element element)
            {
                this.Center(element.GetScreenCenter());
            }

            public virtual void Center(Element element)
            {
                this.Center(element.GetCenter());
            }

            public virtual void CenterX(Element element)
            {
                this.CenterX(element.GetCenterX());
            }

            public virtual void CenterX(float xPos)
            {
                float newX = xPos - this.Width / 2.0f;
                this.Position = new Vector2(newX, this.Position.Y);
            }

            public virtual void CenterY(float yPos)
            {
                float newY = yPos - this.Height / 2.0f;
                this.Position = new Vector2(this.Position.X, newY);
            }

            // where in the window are we
            public Vector2 Pos()
            {
                if (this == DraggingElement)
                    return new Vector2(CurrentMouseState.X, CurrentMouseState.Y) + this.CursorOffset;

                Vector2 pos = this.Position + this.AnimationOffset() + this.Offset();

                foreach (string scrollKey in this.ScrollKeys)
                {
                    if (ScrollOffsets.ContainsKey(scrollKey))
                    {
                        Vector2 offset = new Vector2(ScrollOffsets[scrollKey].X, ScrollOffsets[scrollKey].Y);
                        pos += offset;
                    }
                }

                if (this.Parent != null)
                    pos += this.Parent.Pos();

                return pos;
            }

            public Screen GetScreen()
            {
                Element localElement = this;
                while (localElement != null)
                {
                    if (localElement.Parent is Screen)
                        return (Screen)localElement.Parent;
                    else 
                        localElement = localElement.Parent;
                }

                return null;
            }

            // where in the screen are we
            public Vector2 ScreenPos()
            {
                Vector2 pos = this.Position + this.AnimationOffset() + this.Offset();

                foreach (string scrollKey in this.ScrollKeys)
                {
                    if (ScrollOffsets.ContainsKey(scrollKey))
                    {
                        Vector2 offset = new Vector2(ScrollOffsets[scrollKey].X, ScrollOffsets[scrollKey].Y);
                        pos += offset;
                    }
                }

                if (this.Parent != null && this.Parent is not Screen)
                    pos += this.Parent.ScreenPos();

                if (this == DraggingElement)
                {
                    Screen screen = this.GetScreen();
                    return new Vector2(CurrentMouseState.X, CurrentMouseState.Y) + this.CursorOffset - screen.Pos();
                }

                return pos;
            }

            public Vector2 PosNoScroll()
            {
                Vector2 pos = this.Position + this.AnimationOffset() + this.Offset();

                if (this.Parent != null)
                    pos += this.Parent.PosNoScroll();

                return pos;
            }

            public float GetzIndex()
            {
                float result = this.zIndex;

                foreach (float offset in this.zIndexOffsets.Values)
                    result += offset;

                return result;
            }

            public Vector2 AnimationOffset()
            {
                Vector2 result = Vector2.Zero;
                foreach(Animation anim in Animations)
                {
                    Vector2 offset = new Vector2(anim.Offset.X, anim.Offset.Y);

                    result += offset;
                }

                return result;
            }

            public void Pause()
            {
                foreach(Animation anim in Animations)
                    anim.Pause();
                foreach(EffectAgent effect in EffectAgents)
                    effect.Paused = true;
            }

            public void Unpause()
            {
                foreach(Animation anim in Animations)
                    anim.Unpause();
                foreach(EffectAgent effect in EffectAgents)
                    effect.Paused = false;
            }

            public Vector2 Offset()
            {
                Vector2 result = Vector2.Zero;
                foreach(Vector2 offset in Offsets.Values)
                    result += new Vector2(offset.X, offset.Y);
                return result;
            }
        }
    }
}