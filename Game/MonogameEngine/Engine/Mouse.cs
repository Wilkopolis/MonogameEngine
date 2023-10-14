using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

#pragma warning disable IDE0017 // Simplify object initialization
#pragma warning disable IDE0019 // Use pattern matching

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public static Element DraggingElement;
        bool LeftDownEvent, LeftUpEvent, LeftDraggingEvent;
        MState OnLeftClickMouseState = new MState(Mouse.GetState());
        MState OnRightClickMouseState = new MState(Mouse.GetState());
        MState OnMiddleClickMouseState = new MState(Mouse.GetState());
        DateTime OnLeftClickTime;
        DateTime OnRightClickTime;
        DateTime OnMiddleClickTime;
        bool RightDownEvent, RightUpEvent, RightDraggingEvent;
        bool MiddleDownEvent, MiddleUpEvent, MiddleDraggingEvent;
        public static List<Element> MouseOverElements = new List<Element>();
        public static List<Element> LastMouseOverElements = new List<Element>();
        public static List<Element> DragTargets = new List<Element>();
        public static Dictionary<string, Offset> ScrollOffsets = new Dictionary<string, Offset>();
        public class Offset
        {
            public float X;
            public float Y;

            public Offset()
            {

            }

            public Offset(float x, float y)
            {
                this.X = x;
                this.Y = y;
            }
            public Offset(Vector2 vector)
            {
                this.X = vector.X;
                this.Y = vector.Y;
            }

            public static Offset operator +(Offset a, Offset b) => new Offset(a.X + b.X, a.Y + b.Y);
            public static Offset operator -(Offset a, Offset b) => new Offset(a.X - b.X, a.Y - b.Y);
            public static Vector2 operator +(Offset a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
            public static Vector2 operator -(Offset a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
            public static Vector2 operator +(Vector2 a, Offset b) => new Vector2(a.X + b.X, a.Y + b.Y);
            public static Vector2 operator -(Vector2 a, Offset b) => new Vector2(a.X - b.X, a.Y - b.Y);
        }

        // mouse scrolling
        public static int PreviousScrollWheelValue = 0;

        public class MState
        {
            public int X = 0;
            public int Y = 0;
            public ButtonState LeftButton = ButtonState.Released;
            public ButtonState MiddleButton = ButtonState.Released;
            public ButtonState RightButton = ButtonState.Released;
            public int ScrollWheelValue = 0;

            public MState(MouseState input)
            {
                this.X = input.X;
                this.Y = input.Y;
                this.LeftButton = input.LeftButton;
                this.MiddleButton = input.MiddleButton;
                this.RightButton = input.RightButton;
                this.ScrollWheelValue = input.ScrollWheelValue;
            }
        }

        void MouseHandler()
        {
            // apply the mouse state to any buttons
            LeftDownEvent = CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
            LeftUpEvent = CurrentMouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed;
            RightDownEvent = CurrentMouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
            RightUpEvent = CurrentMouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed;
            MiddleDownEvent = CurrentMouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released;
            MiddleUpEvent = CurrentMouseState.MiddleButton == ButtonState.Released && PreviousMouseState.MiddleButton == ButtonState.Pressed;
            LeftDraggingEvent = false;
            RightDraggingEvent = false;
            MiddleDraggingEvent = false;

            // check if this is a mouse dragging event
            if (CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                bool DragDistance = (Math.Abs(OnLeftClickMouseState.X - CurrentMouseState.X) + Math.Abs(OnLeftClickMouseState.Y - CurrentMouseState.Y)) > 5;
                bool TimePressed = (DateTime.Now - OnLeftClickTime).TotalMilliseconds > 10;
                LeftDraggingEvent = (DragDistance && TimePressed) && !LeftDownEvent;
            }
            if (CurrentMouseState.RightButton == ButtonState.Pressed)
            {
                bool DragDistance = (Math.Abs(OnRightClickMouseState.X - CurrentMouseState.X) + Math.Abs(OnRightClickMouseState.Y - CurrentMouseState.Y)) > 5;
                bool TimePressed = (DateTime.Now - OnRightClickTime).TotalMilliseconds > 10;
                RightDraggingEvent = (DragDistance && TimePressed) && !RightDownEvent;
            }
            if (CurrentMouseState.MiddleButton == ButtonState.Pressed)
            {
                bool DragDistance = (Math.Abs(OnMiddleClickMouseState.X - CurrentMouseState.X) + Math.Abs(OnMiddleClickMouseState.Y - CurrentMouseState.Y)) > 5;
                bool TimePressed = (DateTime.Now - OnMiddleClickTime).TotalMilliseconds > 10;
                MiddleDraggingEvent = (DragDistance && TimePressed) && !MiddleDownEvent;
            }

            if (MiddleDownEvent)
            {
                OnMiddleClickMouseState = CurrentMouseState;
                OnMiddleClickTime = DateTime.Now;
            }

            // reset this every frame
            TargetCursor = CursorType.Arrow;

            if (!isActive)
                return;

            // scrolling up
            if (CurrentMouseState.ScrollWheelValue < PreviousScrollWheelValue)
                OnScrollUp();
            // scrolling down
            else if (CurrentMouseState.ScrollWheelValue > PreviousScrollWheelValue)
                OnScrollDown();

            // on mouse depart            
            foreach (Element element in LastMouseOverElements)
            {
                if (!element.IsVisible() || !element.IsOnScreen() || !element.IsMouseOver())
                {
                    element.OnMouseLeave();
                }                    
            }

            // get all the screens we are gonna do mouse events on
            List<Element> interactables = new List<Element>();
            foreach (Screen screen in Screens.Values)
                interactables.AddRange(screen.GetAllInteractables());

            // every run recalc if we are mousing over an element
            MouseOverElements = new List<Element>();
            foreach (Element element in interactables)
            {
                bool mouseOver = element.IsMouseOver();
                if (mouseOver)
                {
                    if (!LastMouseOverElements.Contains(element))
                        element.OnMouseOver();

                    if (element.Draggable)
                        DragTargets.Add(element);
                    MouseOverElements.Add(element);

                    if (element.Cursor != CursorType.Arrow)
                        TargetCursor = element.Cursor;
                }
            }

            if (DraggingElement != null)
                TargetCursor = CursorType.Holding;

            // check if we should be dragging the element
            if (LeftDraggingEvent)
            {
                DragTargets.Sort(delegate (Element a, Element b) { return a.GetzIndex().CompareTo(b.GetzIndex()); });

                if (DragTargets.Count > 0 && DragTargets[0].Draggable)
                    DragTargets[0].OnDrag();

                DragTargets = new List<Element>();
            }

            // check if we start clicking a button or dragging a dragable
            if (LeftDownEvent)
            {
                OnLeftClickMouseState = CurrentMouseState;
                OnLeftClickTime = DateTime.Now;

                // if its a button or a draggable, mark it as pressed
                foreach (Element e in MouseOverElements)
                {
                    // set mouse down on a target for potential dragging later
                    DragTargets.Add(e);
                }
                // only do the first OnMouseDown
                foreach (Element e in MouseOverElements)
                {
                    e.OnMouseDown();
                    break;
                }
            }

            // check if we click any buttons or release any dragged element
            if (LeftUpEvent)
            {                    
                // if we are clicking on a button, do its handler
                foreach (Element e in MouseOverElements)
                {
                    if (e.IsMouseOver())
                        e.OnMouseUp();
                }

                // if we are dragging an item, handle its release
                if (DraggingElement != null)
                {
                    DraggingElement.OnDragRelease();
                    DraggingElement = null;
                }

                // reset the pressed states
                while (PressedElements.Count > 0)
                {
                    PressedElements[0].Pressed = false;
                    PressedElements.RemoveAt(0);
                }

                DragTargets = new List<Element>();
            }

            // do this after setting cursor cause we might change it up    
            // items and elements that can be described
            List<Element> descOver = new List<Element>();
            foreach (Element element in MouseOverElements)
            {
                if (!element.IsVisible()  || !element.Describable || !element.IsOnScreen())
                    continue;

                bool mouseOver = element.IsMouseOver();
                if (mouseOver) 
                {
                    descOver.Add(element);
                }
            }

            // tooltips
            Element tooltip = null;
            foreach (Element element in MouseOverElements)
            {
                if (element.ToolTip != "")
                    tooltip = element;
            }
            
            // if there is a tooltip, draw its tooltip
            if (tooltip == null)
                HideTooltip();
            else
                ShowTooltip(tooltip);

            if (GetFlag("Test"))
                Test_MouseHandler();

            LastMouseOverElements = MouseOverElements;
            PreviousScrollWheelValue = CurrentMouseState.ScrollWheelValue;
        }

        void OnScrollUp()
        {
        }

        void OnScrollDown()
        {
        }

        public class Tooltip
        {
            public string Title = "";
            public string Body = "";
            public Color TitleColor = new Color(59 / 255f, 54 / 255f, 54 / 255f);
            public Color BGColor = new Color(49 / 255f, 44 / 255f, 44 / 255f);
            public Color BorderColor = new Color(30 / 255f, 30 / 255f, 30 / 255f);

            public Tooltip(string title, string body)
            {
                this.Title = title;
                this.Body = body;
            }
        }

        void HideTooltip()
        {
            //if (!Elements.ContainsKey("tooltip"))
            //    return;

            //Animations.Remove("tooltip");
            //Remove("tooltip");
        }

        void ShowTooltip(Element target)
        {
            //if (target.ToolTip == "")
            //    return;

            //if (Elements.ContainsKey("tooltip"))
            //{
            //    if (Elements["tooltip"].ToolTip == target.ToolTip)
            //    {
            //        // position it
            //        Elements["tooltip"].Position = new Vector2(ToX(CurrentMouseState.X), ToY(CurrentMouseState.Y));
            //        Elements["tooltip"].Position += new Vector2(ToX(16), ToY(18));

            //        return;
            //    }
            //}

            //// make the tooltip element
            //CompoundElement tooltip = new CompoundElement();
            //tooltip.zIndex = 99f;

            //Box bg = new Box(0, 0, 1f, ToY(26), new Color(40 / 255f, 35 / 255f, 75 / 255f), .5f);
            //bg.AddBorder(1, new Color(.6f, .5f, .8f));
            //tooltip.Add(bg);

            //Text text = new Text(ToX(8), ToY(1), target.ToolTip, Fonts.K2DMedium12, 1f);
            //tooltip.Add(text);

            //bg.Width = text.Width + ToX(16);
            //bg.Resize();
            //tooltip.Resize();

            //// position it
            //tooltip.Position = new Vector2(ToX(CurrentMouseState.X), ToY(CurrentMouseState.Y));
            //tooltip.Position += new Vector2(ToX(16), ToY(18));

            //tooltip.Key = "tooltip";
            //tooltip.ToolTip = target.ToolTip;

            //// add it
            //Elements[tooltip.Key] = tooltip;
        }

        public void OnDragRelease()
        {

        }
    }
}
