using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MonogameEngine
{
    // resolution/padding
    enum Framing { LETTER_BOX_H, LETTER_BOX_V, NONE };

    static class Setting
    {
        // General
        public static int DISPLAYWIDTH = 1920;
        public static int DISPLAYHEIGHT = 1080;
        public static string DISPLAYMODE = "Windowed";
        public static bool SKIPSPLASH = false;
        // Audio
        public static float MUSICVOLUME = 1.0f;
        public static float SFXVOLUME = 0.0f;
        // Advanced - Not shown in game
        public static int STARTINGX = -1;
        public static int STARTINGY = -1;
        public static bool FIRSTRUN = true;
    }

    partial class MonogameEngine
    {
        // constants
        string SETTINGS_FILE_PATH = "settings.cfg";

        /* 
            Read settings/key assosciations from file, update settings object    
        */
        void LoadSettingsAndKeys()
        {
            // Load user settings
            string[] fileLines = { };
            try
            {
                fileLines = File.ReadAllLines(SETTINGS_FILE_PATH);
            }
            catch (Exception e)
            {
                if (e is System.IO.FileNotFoundException)
                {
                    //System.Windows.Forms.MessageBox.Show("Could not find the settings file:\n\n" + e.Message + "\n\nUsing default settings.");
                }
                else
                {
                    //System.Windows.Forms.MessageBox.Show("Unknown exception: \n" + e.Message + "\n\nUsing default settings.");
                }
            }

            string currentSettingString = "", currentValueString = "";
            foreach (string line in fileLines)
            {
                try
                {
                    // remove all whitespace
                    string cleanLine = Regex.Replace(line, @"\s+", "");

                    // skip comment lines
                    if (cleanLine == "" || cleanLine.Substring(0, 1) == "#")
                        continue;

                    // parse the result into a pair
                    string[] settingTuple = cleanLine.Split('=');

                    // get the key/value
                    string settingName = settingTuple[0].ToUpper();
                    string valueString = settingTuple[1];
                    // used for helpful error message
                    currentSettingString = settingName;
                    currentValueString = valueString;
                    // get the key type
                    // its a setting
                    if (typeof(Setting).GetField(settingName) != null)
                    {
                        FieldInfo field = typeof(Setting).GetField(settingName);

                        if (settingName == "DISPLAYWIDTH")
                        {
                            if (Convert.ToInt32(valueString) <= 720 || Convert.ToInt32(valueString) >= 8640)
                            {
                                //System.Windows.Forms.MessageBox.Show("Invalid Width: \"" + valueString + "\".");
                            }
                            valueString = Math.Min(Math.Max(1280, Convert.ToInt32(valueString)), 8640).ToString();
                        }
                        else if (settingName == "DISPLAYHEIGHT")
                        {
                            if (Convert.ToInt32(valueString) <= 720 || Convert.ToInt32(valueString) >= 8640)
                            {
                                //System.Windows.Forms.MessageBox.Show("Invalid Height: \"" + valueString + "\".");
                            }
                            valueString = Math.Min(Math.Max(720, Convert.ToInt32(valueString)), 8640).ToString();
                        }
                        else if (settingName == "PORT")
                        {
                            if (Convert.ToInt32(valueString) <= 1024 || Convert.ToInt32(valueString) >= 65535)
                            {
                                //System.Windows.Forms.MessageBox.Show("Invalid Port: \"" + valueString + "\", using 52525.");
                                valueString = "52525";
                            }
                        }
                        else if (settingName == "MUSICVOLUME")
                        {
                            if (Convert.ToDecimal(valueString) < 0 || Convert.ToDecimal(valueString) > 1)
                            {
                                //System.Windows.Forms.MessageBox.Show("Invalid Port: \"" + valueString + "\", using 52525.");
                                valueString = "1";
                            }
                        }
                        else if (settingName == "SFXVOLUME")
                        {
                            if (Convert.ToDecimal(valueString) < 0 || Convert.ToDecimal(valueString) > 1)
                            {
                                //System.Windows.Forms.MessageBox.Show("Invalid Port: \"" + valueString + "\", using 52525.");
                                valueString = "1";
                            }
                        }
                        else if (settingName == "SKIPSPLASH")
                        {
                            if (Convert.ToDecimal(valueString) == 1)
                            {
                                valueString = "true";
                            }
                            else
                            {
                                valueString = "false";
                            }
                        }

                        // try to cast the string value to the desired type
                        var value = Convert.ChangeType(valueString, field.FieldType);

                        // set the setting value
                        field.SetValue(null, value);
                    }
                    // its a keybinding
                    //else if (Enum.Parse(typeof(Command), settingName) != null)
                    //{
                    //    Command command = (Command)Enum.Parse(typeof(Command), settingName);

                    //    // get the exiting handler
                    //    CommandHandler handler = KeyMap[command];

                    //    // parse the key                       
                    //    Keys value = (Keys)Enum.Parse(typeof(Keys), valueString);

                    //    if (KeyAlreadyBound(value))
                    //        UnbindKey(value);

                    //    KeyMap[command].Keys.Add(value);
                    //}
                    // its ???
                    else
                        throw new System.NullReferenceException();
                }
                catch (Exception e)
                {
                    // field not found
                    if (e is System.NullReferenceException)
                    {
                        //System.Windows.Forms.MessageBox.Show("Unknown setting: \"" + currentSettingString + "\", ignoring it.");
                    }
                    else if (e is System.FormatException)
                    {
                        FieldInfo field = typeof(Setting).GetField(currentSettingString);
                        var value = field.GetValue(null);
                        //System.Windows.Forms.MessageBox.Show("Unsupported value: \"" + currentValueString + "\", for setting: \"" + currentSettingString + "\", using default value: \"" + value.ToString() + "\".");
                    }
                    else if (e is System.ArgumentException)
                    {
                        //System.Windows.Forms.MessageBox.Show("Unknown key: \"" + currentValueString + "\", ignoring binding entry for: \"" + currentSettingString + "\".");
                    }
                    else
                    {
                        //System.Windows.Forms.MessageBox.Show("Unknown exception: \n" + e.Message + "\n\nUsing default setting for \"" + currentSettingString + "\" .");
                    }
                }
            }
        }

        int SCREEN_WIDTH = 1920;
        int SCREEN_HEIGHT = 1080;
        void ApplySettings()
        {
            // apply Width and Height
            graphicsManager.PreferredBackBufferWidth = Setting.DISPLAYWIDTH;
            graphicsManager.PreferredBackBufferHeight = Setting.DISPLAYHEIGHT;

            SCREEN_WIDTH = Setting.DISPLAYWIDTH;
            SCREEN_HEIGHT = Setting.DISPLAYHEIGHT;

            // display mode
            Window.IsBorderless = false;
            switch (Setting.DISPLAYMODE)
            {
                case "Borderless":
                    Window.IsBorderless = true;
                    break;
                case "Fullscreen":
                    graphicsManager.IsFullScreen = true;
                    break;
                // windowed
                default:

                    break;
            }

            Setting.MUSICVOLUME = (float)Math.Clamp(Setting.MUSICVOLUME, 0, 1);
            Setting.SFXVOLUME = (float)Math.Clamp(Setting.SFXVOLUME, 0, 1);

            // always allow user to resize window
            this.Window.AllowUserResizing = false;
            //Window.ClientSizeChanged += OnResize;

            // apply Cursor            
            this.IsMouseVisible = true;

            // apply settings
            graphicsManager.ApplyChanges();

            OnResize(new Object(), new EventArgs());
        }

        void SaveSettings()
        {

        }
    }
}
