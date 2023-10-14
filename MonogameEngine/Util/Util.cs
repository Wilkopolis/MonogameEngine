using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonogameEngine
{
    public static class Extensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

        public static T Prev<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) - 1;
            return (j == -1) ? Arr[Arr.Length - 1] : Arr[j];
        }
    }

    public partial class MonogameEngine
    {
        public static void Oops()
        {
            LOG("oops");
        }

        public static Button DebugButton(int w, int h, string msg, Color color, Action clickHandler)
        {
            Button result = new Button();

            Box bg = new Box(w, h, color);
            result.Add(bg);

            Text text = new Text(text: msg, Fonts.Library[FontFamily.Merriweather][16]);
            text.Center(bg);
            result.Add(text);

            result.ClickHandler = delegate { 
                clickHandler();
            };
            result.Resize();

            return result;
        }

        public class ElementProp
        {
            public bool Interactive = false;
            public bool Describable = false;

            public ElementProp(bool interactive, bool describable)
            {
                this.Interactive = interactive;
                this.Describable = describable;
            }
        }

        //public static Dictionary<string, ElementProp> backupElements = new Dictionary<string, ElementProp>();
        //public static List<Animation> backupAnimations = new List<Animation>();

        //void BackupElements()
        //{
        //    // backup all elements
        //    backupElements = new Dictionary<string, ElementProp>();
        //    backupButtons = new Dictionary<string, ElementProp>();
        //    backupDescribables = new Dictionary<string, ElementProp>();
        //    backupDraggables = new Dictionary<string, ElementProp>();
        //    List<string> keys = new List<string>(Elements.Keys);
        //    foreach (string key in keys)
        //    {
        //        backupElements[key] = new ElementProp(Elements[key].Interactive, Elements[key].Describable);
        //    }
        //    keys = new List<string>(Buttons.Keys);
        //    foreach (string key in keys)
        //    {
        //        backupButtons[key] = new ElementProp(Buttons[key].Interactive, Buttons[key].Describable);
        //    }
        //    keys = new List<string>(Describables.Keys);
        //    foreach (string key in keys)
        //    {
        //        backupDescribables[key] = new ElementProp(Describables[key].Interactive, Describables[key].Describable);
        //    }
        //    keys = new List<string>(Draggables.Keys);
        //    foreach (string key in keys)
        //    {
        //        backupDraggables[key] = new ElementProp(Draggables[key].Interactive, Draggables[key].Describable);
        //    }

        //    // now set them all to uninteractive
        //    keys = new List<string>(Elements.Keys);
        //    foreach (string key in keys)
        //    {
        //        Elements[key].Interactive = false;
        //        Elements[key].Describable = false;
        //    }
        //    keys = new List<string>(Buttons.Keys);
        //    foreach (string key in keys)
        //    {
        //        Buttons[key].Interactive = false;
        //        Buttons[key].Describable = false;
        //    }
        //    keys = new List<string>(Describables.Keys);
        //    foreach (string key in keys)
        //    {
        //        Describables[key].Interactive = false;
        //        Describables[key].Describable = false;
        //    }
        //    keys = new List<string>(Draggables.Keys);
        //    foreach (string key in keys)
        //    {
        //        Draggables[key].Interactive = false;
        //        Draggables[key].Describable = false;
        //    }
        //}

        //void RestoreElements()
        //{
        //    List<string> keys = new List<string>(backupElements.Keys);
        //    foreach (string key in keys)
        //    {
        //        if (Elements.ContainsKey(key))
        //        {
        //            Elements[key].Interactive = backupElements[key].Interactive;
        //            Elements[key].Describable = backupElements[key].Describable;
        //        }
        //    }
        //    keys = new List<string>(backupButtons.Keys);
        //    foreach (string key in keys)
        //    {
        //        if (Buttons.ContainsKey(key))
        //        {
        //            Buttons[key].Interactive = backupButtons[key].Interactive;
        //            Buttons[key].Describable = backupButtons[key].Describable;
        //        }
        //    }
        //    keys = new List<string>(backupDescribables.Keys);
        //    foreach (string key in keys)
        //    {
        //        if (Describables.ContainsKey(key))
        //        {
        //            Describables[key].Interactive = backupDescribables[key].Interactive;
        //            Describables[key].Describable = backupDescribables[key].Describable;
        //        }
        //    }
        //    keys = new List<string>(backupDraggables.Keys);
        //    foreach (string key in keys)
        //    {
        //        if (Draggables.ContainsKey(key))
        //        {
        //            Draggables[key].Interactive = backupDraggables[key].Interactive;
        //            Draggables[key].Describable = backupDraggables[key].Describable;
        //        }
        //    }
        //}

        public static void RemoveAllElements()
        {
            foreach (Screen screen in Screens.Values)
                screen.Unload();

            Screens.Clear();
        }

        public static void LOG(string message)
        {
            System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString() + ": " + message);
        }

        public static bool GetFlag(string key)
        {
            key = key.ToUpper();

            if (GlobalFlags.ContainsKey(key))
                return GlobalFlags[key];

            return false;
        }

        public static void SetFlag(string key, bool state)
        {
            key = key.ToUpper();
            GlobalFlags[key] = state;
        }

        public static Animation SetTimeout(Screen screen, Action a, int millis, string keyBase = "")
        {
            string key = keyBase;
            if (keyBase == "")
                key = "t" + Hash++;

            Animation t = new TimeBuffer(millis, a);
            t.Key = key;
            screen.Animations[t.Key] = t;
            return t;
        }

        public static Object PickWeighted(List<Option> options)
        {
            double sum = 0;
            for (int i = options.Count - 1; i >= 0; i--)
            {
                sum += options[i].Weight;
            }

            double seed = Random.NextDouble() * sum;

            double step = 0;
            for (int i = options.Count - 1; i >= 0; i--)
            {
                step += options[i].Weight;
                if (seed < step)
                    return options[i].Choice;
            }
            return null;
        }

        public static List<Object> PickWeightedAmount(List<Option> options, int amount = 1)
        {
            List<Object> results = new List<Object>();

            for (int j = 0; j < amount; j++)
            {
                double sum = 0;
                for (int i = options.Count - 1; i >= 0; i--)
                    sum += options[i].Weight;

                double seed = Random.NextDouble() * sum;

                double step = 0;
                for (int i = options.Count - 1; i >= 0; i--)
                {
                    step += options[i].Weight;
                    if (seed < step)
                    {
                        results.Add(options[i].Choice);
                        options.RemoveAt(i);
                        break;
                    }
                }
            }

            return results;
        }

        public static IList<T> Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static T PopRandom<T>(IList<T> list)
        {
            if (list.Count == 0)
                return default(T);

            int index = Random.Next(list.Count);

            T result = list[index];
            list.RemoveAt(index);

            return result;
        }

        public static T PeekRandom<T>(IList<T> list)
        {
            if (list.Count == 0)
                return default(T);

            int index = Random.Next(list.Count);

            T result = list[index];

            return result;
        }

        public static Vector2 Rotate(Vector2 origin, Vector2 input, double radians)
        {
            Vector2 point = input - origin;
            double dx = point.X * Math.Cos(radians) - point.Y * Math.Sin(radians);
            double dy = point.X * Math.Sin(radians) + point.Y * Math.Cos(radians);
            return new Vector2(origin.X + (float)dx, origin.Y + (float)dy);
        }

        public static float pDistance(Vector2 lineStart, Vector2 lineEnd, Vector2 point) 
        {
            float A = point.X - lineStart.X;
            float B = point.Y - lineStart.Y;
            float C = lineEnd.X - lineStart.X;
            float D = lineEnd.Y - lineStart.Y;

            float dot = A * C + B * D;
            float len_sq = C * C + D * D;
            float param = -1;

            if (len_sq != 0) //in case of 0 length line
                param = dot / len_sq;

            float xx, yy;

            if (param < 0) 
            {
                xx = lineStart.X;
                yy = lineStart.Y;
            } 
            else if (param > 1) 
            {
                xx = lineEnd.X;
                yy = lineEnd.Y;
            }
            else 
            {
                xx = lineStart.X + param * C;
                yy = lineStart.Y + param * D;
            }

            var dx = point.X - xx;
            var dy = point.Y - yy;
            
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        
        float sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = sign(pt, v1, v2);
            d2 = sign(pt, v2, v3);
            d3 = sign(pt, v3, v1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }
    }

    public class Option
    {
        public double Weight;
        public Object Choice;
    }
}
