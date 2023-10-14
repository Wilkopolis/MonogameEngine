using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static MonogameEngine.MonogameEngine;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        string PROGRESS_FILE_PATH = "progress.cnp";
        string WORLD_FILE_PATH = "world.map";
        string CIV_FILE_PATH = "empire.civ";
        Dictionary<string, string> ProgressFlags = new Dictionary<string, string>() {
        };

        void SaveProgress()
        {
            // make it alphabetical
            List<string> strings = new List<string>();
            List<string> keys = ProgressFlags.Keys.ToList();
            keys.Sort();

            foreach (string key in keys)
            {
                string value = ProgressFlags[key];
                strings.Add(key + " = " + value);
            }

            File.WriteAllLines(PROGRESS_FILE_PATH, strings.ToArray());
        }

        void LoadProgress()
        {
            // Load user settings
            string[] fileLines = { };
            try
            {
                fileLines = File.ReadAllLines(PROGRESS_FILE_PATH);
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

            foreach (string line in fileLines)
            {
                try
                {
                    // remove all whitespace
                    string cleanLine = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", "");

                    // skip comment lines
                    if (cleanLine == "" || cleanLine.Substring(0, 1) == "#")
                        continue;

                    // parse the result into a pair
                    string[] settingTuple = cleanLine.Split('=');

                    // get the key/value
                    string settingName = settingTuple[0].ToUpper();
                    string valueString = settingTuple[1];
                    // if its a certain value thats not a process flag, capture it here
                    ProgressFlags[settingName] = valueString;
                }
                // used for helpful error message
                catch (Exception e)
                {
                }
            }
        }

        void SaveMaps()
        {
            string[] fileLines = { };
            try
            {
                //fileLines = File.ReadAllLines(SHOP_FILE_PATH);
            }
            catch (Exception e)
            {
            }

            foreach (string line in fileLines)
            {
                try
                {
                    // remove all whitespace
                    string cleanLine = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", "");

                    // skip comment lines
                    if (cleanLine == "" || cleanLine.Substring(0, 1) == "#")
                        continue;
                }
                // used for helpful error message
                catch (Exception e)
                {
                }
            }
        }

        void SaveWorld()
        {
            List<string> strings = new List<string>();

            // shop entities

            //strings.Add("#Entities");
            //foreach (ShopEntity shopEntity in Shop.Entities)
            //{
            //    strings.Add(shopEntity.Id + ":Type:" + shopEntity.Type.ToString());
            //    strings.Add(shopEntity.Id + ":X:" + shopEntity.X);
            //    strings.Add(shopEntity.Id + ":Y:" + shopEntity.Y);
            //}

            //strings.Add("#Customers");
            //foreach (ShopEntity shopEntity in Shop.Customers)
            //{
            //    strings.Add(shopEntity.Id + ":Type:" + shopEntity.Type.ToString());
            //    strings.Add(shopEntity.Id + ":X:" + shopEntity.X);
            //    strings.Add(shopEntity.Id + ":Y:" + shopEntity.Y);
            //}

            //strings.Add("Gold:" + Shop.Gold);

            //File.WriteAllLines(SHOP_FILE_PATH, strings.ToArray());
        }

        bool GetProgressFlagAsBool(string key)
        {
            key = key.ToUpper();
            
            if (ProgressFlags.ContainsKey(key))
                return ProgressFlags[key] == "True";

            return false;
        }

        int GetProgressFlagAsInt(string key)
        {
            key = key.ToUpper();
            
            if (ProgressFlags.ContainsKey(key))
                return Convert.ToInt16(ProgressFlags[key]);

            return 0;
        }

        string GetProgressFlag(string key)
        {
            key = key.ToUpper();
            
            if (ProgressFlags.ContainsKey(key))
                return ProgressFlags[key];

            return "";
        }

        void SetProgressFlag(string key, string value)
        {
            key = key.ToUpper();
            ProgressFlags[key] = value;
            SaveProgress();
        }

        void SetProgressFlag(string key, bool value)
        {
            key = key.ToUpper();
            ProgressFlags[key] = value.ToString();
            SaveProgress();
        }

        public static SavedGame Save;
        void LoadSavedGame()
        {
            Save = new SavedGame(); 
        }

        void SaveGame()
        {
            // write to file
        }
    }

    public class SavedGame
    {
        public bool Pristine = true;
    }
}
