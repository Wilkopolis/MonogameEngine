using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class CompoundElement : Element
        {
            protected List<Element> Children = new List<Element>();
            // if true, all our children will not have negative positions
            public bool PositiveJustify = true;

            public CompoundElement()
            {
                this.HitBoxType = HitBoxType.Compound;
            }

            public CompoundElement(float x, float y, float zindex, List<Element> elements = null)
            {
                this.HitBoxType = HitBoxType.Compound;

                this.Position = new Vector2(x, y);
                this.zIndex = zindex;

                if (elements != null)
                {
                    this.Children = elements;
                    foreach (Element element in this.Children)
                        element.Parent = this;
                }

                this.Resize();
            }

            public List<Element> GetElements()
            {
                return this.Children;
            }

            public override List<Element> GetAllInteractables()
            {
                List<Element> results = new List<Element>();
                foreach (Element element in this.Children)
                {
                    if (element.MouseOverCheck || element.Clickable || element.Draggable || element.ToolTip != "")
                        results.Add(element);

                    if (element is Screen || element is CompoundElement)
                        results.AddRange(element.GetAllInteractables());
                }

                return results;
            }

            public Element GetChild(string key)
            {
                foreach (Element child in this.Children)
                {
                    if (child.Key == key) return child;
                }

                return null;
            }

            public List<Element> GetChildren(string key)
            {
                List<Element> results = new List<Element>();
                foreach (Element child in this.Children)
                {
                    if (child.Key != null && child.Key.Contains(key)) 
                        results.Add(child);
                }

                return results;
            }

            public void RemoveChildren(string key)
            {
                int index = 0;
                while (index < this.Children.Count)
                {
                    Element child = this.Children[index];
                    if (child.Key != null && child.Key.Contains(key))
                    {
                        this.Remove(index);
                    }
                    else
                    {
                        index++;
                    }
                }
            }

            public void Remove(int index)
            {
                this.Remove(Children[index]);
            }

            public void Remove(Element element)
            {
                foreach (Screen screen in MonogameEngine.Screens.Values)
                    screen.Remove(element);

                this.Children.Remove(element);

                element = null;
            }

            public override void Unload()
            {
                while (this.Children.Count > 0) 
                {
                    Element element = Children[0];
                    element.Unload();

                    Remove(0);
                }
            }

            public void Add(Element element, bool resize = false)
            {
                this.Children.Add(element);
                element.Parent = this;

                if (element is not Text)
                {
                    element.MouseOverCheck = true;
                    element.Clickable = true;
                }

                if (resize)
                    this.Resize();
            }

            public override void Render(bool direct = false)
            {
                foreach(Element e in this.Children)
                    e.Render(direct);
            }

            public override void Draw()
            {
                if (!this.Visible)
                    return;

                this.Children = this.Children.OrderBy(e => e.zIndex).ToList();

                foreach (Element element in this.Children)
                {
                    if (!element.DoNotInheritAlpha)
                        element.Alpha = this.Alpha;

                    element.Draw();
                }
            }

            public override bool IsOnScreen()
            {
                foreach(Element e in this.Children)
                {
                    if (e.IsOnScreen())
                        return true;
                }

                return false;
            }
            
            public override void Resize()
            {
                // we've calculated the width and height perfectly and this 
                // will mess it up otherwise
                if (this.DontResize)
                    return;

                foreach (Element element in Children)
                    element.Resize();

                // if we are empty, make dimensions 0 x 0
                if (this.Children.Count == 0)
                {
                    this.Width = 0;
                    this.Height = 0;
                    return;
                }

                // find out calculated width and height
                float w1 = int.MaxValue;
                float w2 = int.MinValue;
                float h1 = int.MaxValue;
                float h2 = int.MinValue;
                foreach (Element element in Children)
                {
                    if (element.IgnoreDuringSizing)
                        continue;

                    // find the left most element
                    float leftSide = element.Position.X + element.Offset().X;
                    if (leftSide < w1)
                        w1 = leftSide;

                    // find the right most pixel of our elements
                    float rightSide = leftSide + element.Width;
                    if (rightSide > w2)
                        w2 = rightSide;

                    // maybe one day they'll fix this !
                    if (element is Text) 
                        continue;

                    // find the top most element
                    float topSide = element.Position.Y + element.Offset().Y;
                    if (topSide < h1)
                        h1 = topSide;

                    // find the bottom most pixel of our elements
                    float bottomSide = topSide + element.Height;
                    if (bottomSide > h2)
                        h2 = bottomSide;
                }

                this.Width = (int)(w2 - w1);
                this.Height = (int)(h2 - h1);

                if (this.PositiveJustify)
                {
                    if (w1 < 0)
                    {
                        // add -w1 to all elements
                        foreach (Element element in Children)
                            element.Position.X -= w1;
                    }
                    if (h1 < 0)
                    {
                        // add -h1 to all elements
                        foreach (Element element in Children)
                            element.Position.Y -= h1;
                    }
                }
            }
        }
    }
}
