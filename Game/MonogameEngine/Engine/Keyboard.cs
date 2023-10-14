using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum InputType
        {
            Quit, Debug1
        }

        public class Control
        {
            public InputType Input;
            public Keys Key1 = Keys.None;
            public Keys Key2 = Keys.None;
            // when you initially press
            public bool KeyPressCheck = false;
            // while its down
            public bool KeyHoldCheck = false;
            // when you release the key
            public bool KeyEventCheck = false;
        }

        public List<Control> Controls = new List<Control>
        {
            new Control { KeyPressCheck = false, KeyHoldCheck = false, KeyEventCheck = true,  Input = InputType.Quit,           Key1 = Keys.F10 },
            new Control { KeyPressCheck = false, KeyHoldCheck = false, KeyEventCheck = true,  Input = InputType.Debug1,         Key1 = Keys.F1  },
        };

        public List<Control> BackupControls = new List<Control>();

        void BackupControlSettings()
        {
            BackupControls = new List<Control>();

            foreach (Control control in Controls)
                BackupControls.Add(new Control() { Input = control.Input, Key1 = control.Key1, Key2 = control.Key2 });
        }

        void RestoreControlSettings()
        {
            // if we applied settings, empty this out and don't do it
            if (BackupControls.Count == 0)
                return;

            Controls = new List<Control>();

            foreach (Control control in BackupControls)
                Controls.Add(new Control() { Input = control.Input, Key1 = control.Key1, Key2 = control.Key2 });
        }


        List<Keys> KeyEvents = new List<Keys>();
        List<Keys> KeyPresses = new List<Keys>();
        Keys[] currPressedKeys;
        void KeyboardHandler()
        {
            if (!isActive)
                return;

            // return all the keys that are up now that were down previously
            Keys[] prevPressedKeys = PreviousKeyboardState.GetPressedKeys();
            currPressedKeys = CurrentKeyboardState.GetPressedKeys();

            KeyEvents = prevPressedKeys.Except(currPressedKeys).ToList();
            KeyPresses = currPressedKeys.Except(prevPressedKeys).ToList();

            if (GetFlag("InGame"))
                Game_KeyboardHandler();
            if (GetFlag("Test"))
                Test_KeyboardHandler();
        }
    }
}
