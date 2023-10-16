using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static MonogameEngine.MonogameEngine;

namespace MonogameEngine
{
    partial class MonogameEngine
    {
        static Dictionary<string, AnimationSystem> AnimationSystems = new Dictionary<string, AnimationSystem>();
        void LoadAnimationSystems()
        {
            // go to characters folder
            string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\Content\sprites\characters\"));

            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            List<FileInfo> files = new List<FileInfo>();

            Stack<DirectoryInfo> tempFolders = new Stack<DirectoryInfo>();
            tempFolders.Push(new DirectoryInfo(newPath));

            DirectoryInfo currentFolder = tempFolders.Pop();

            // populate MediaFileInfo with all files we find in the video directory
            while (true)
            {
                // add to the fileSet
                foreach (FileInfo f in currentFolder.GetFiles())
                {
                    int lastDot = f.Name.LastIndexOf('.');
                    string fileExtension = f.Name.Substring(lastDot + 1, f.Name.Length - lastDot - 1).ToLower();
                    if (fileExtension == "json")
                        files.Add(f);
                }

                // add to the folderSet
                foreach (DirectoryInfo d in currentFolder.GetDirectories())
                    tempFolders.Push(d);

                if (tempFolders.Count == 0)
                    break;

                currentFolder = tempFolders.Pop();
            }

            foreach (FileInfo f in files)
            {
                AnimationSystem animationSystem = JsonConvert.DeserializeObject<AnimationSystem>(File.ReadAllText(f.FullName));

                if (animationSystem != null)
                    AnimationSystems[f.Name.Replace(".json", "")] = animationSystem;
            }

            // make an animation system to serialize

            ////// Idle Left
            //AnimationSequence IdleLeft = new AnimationSequence
            //{
            //    Loop = true,
            //    Flipped = true,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_idle_right_1", 170),
            //        new AnimationFrame("hero0_idle_right_2", 170),
            //        new AnimationFrame("hero0_idle_right_3", 170),
            //        new AnimationFrame("hero0_idle_right_4", 170)
            //    }
            //};
            //// Idle Right
            //AnimationSequence IdleRight = new AnimationSequence
            //{
            //    Loop = true,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_idle_right_1", 170),
            //        new AnimationFrame("hero0_idle_right_2", 170),
            //        new AnimationFrame("hero0_idle_right_3", 170),
            //        new AnimationFrame("hero0_idle_right_4", 170)
            //    }
            //};
            //// Walk Left
            //AnimationSequence WalkLeft = new AnimationSequence
            //{
            //    Direction = Direction.W,
            //    Flipped = true,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_walk_right_1", new Vector2(-58, 0), 101),
            //        new AnimationFrame("hero0_walk_right_2", new Vector2(-57, 0), 101),
            //        new AnimationFrame("hero0_walk_right_3", new Vector2(-38, 0), 151),
            //        new AnimationFrame("hero0_walk_right_4", new Vector2(-32, 0), 151),
            //        new AnimationFrame("hero0_walk_right_5", new Vector2(-11, 0), 151),
            //        new AnimationFrame("hero0_walk_right_6", new Vector2(-8,  0), 101),
            //        new AnimationFrame("hero0_walk_right_7", new Vector2(-45, 0), 101),
            //        new AnimationFrame("hero0_walk_right_8", new Vector2(-75, 0), 101)
            //    }
            //};
            //// Walk Right
            //AnimationSequence WalkRight = new AnimationSequence
            //{
            //    Direction = Direction.E,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_walk_right_1", new Vector2(58, 0), 101),
            //        new AnimationFrame("hero0_walk_right_2", new Vector2(57, 0), 101),
            //        new AnimationFrame("hero0_walk_right_3", new Vector2(38, 0), 151),
            //        new AnimationFrame("hero0_walk_right_4", new Vector2(32, 0), 151),
            //        new AnimationFrame("hero0_walk_right_5", new Vector2(11, 0), 151),
            //        new AnimationFrame("hero0_walk_right_6", new Vector2(8,  0), 101),
            //        new AnimationFrame("hero0_walk_right_7", new Vector2(45, 0), 101),
            //        new AnimationFrame("hero0_walk_right_8", new Vector2(75, 0), 101)
            //    }
            //};
            //// Walk Up
            //AnimationSequence WalkUp = new AnimationSequence
            //{
            //    Direction = Direction.N,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_walk_right_1", new Vector2(0, -58), 101),
            //        new AnimationFrame("hero0_walk_right_2", new Vector2(0, -57), 101),
            //        new AnimationFrame("hero0_walk_right_3", new Vector2(0, -38), 151),
            //        new AnimationFrame("hero0_walk_right_4", new Vector2(0, -32), 151),
            //        new AnimationFrame("hero0_walk_right_5", new Vector2(0, -11), 151),
            //        new AnimationFrame("hero0_walk_right_6", new Vector2(0, -8),  101),
            //        new AnimationFrame("hero0_walk_right_7", new Vector2(0, -45), 101),
            //        new AnimationFrame("hero0_walk_right_8", new Vector2(0, -75), 101)
            //    }
            //};
            //// Walk Down
            //AnimationSequence WalkDown = new AnimationSequence
            //{
            //    Direction = Direction.S,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_walk_right_1", new Vector2(0, 58), 101),
            //        new AnimationFrame("hero0_walk_right_2", new Vector2(0, 57), 101),
            //        new AnimationFrame("hero0_walk_right_3", new Vector2(0, 38), 151),
            //        new AnimationFrame("hero0_walk_right_4", new Vector2(0, 32), 151),
            //        new AnimationFrame("hero0_walk_right_5", new Vector2(0, 11), 151),
            //        new AnimationFrame("hero0_walk_right_6", new Vector2(0, 8),  101),
            //        new AnimationFrame("hero0_walk_right_7", new Vector2(0, 45), 101),
            //        new AnimationFrame("hero0_walk_right_8", new Vector2(0, 75), 101)
            //    }
            //};
            //// Attack Left
            //AnimationSequence AttackLeft = new AnimationSequence
            //{
            //    Flipped = true,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_attack_right_1", new Vector2(-66, -175), 101),
            //        new AnimationFrame("hero0_attack_right_2", new Vector2(-66, -175), 101),
            //        new AnimationFrame("hero0_attack_right_3", new Vector2(-66, -175), 101),
            //        new AnimationFrame("hero0_attack_right_4", new Vector2(-66, -175), 99),
            //        new AnimationFrame("hero0_attack_right_5", new Vector2(-66, -175), 180),
            //        new AnimationFrame("hero0_attack_right_6", new Vector2(-66, -175), 99),
            //        new AnimationFrame("hero0_attack_right_7", new Vector2(-66, -175), 101),
            //        new AnimationFrame("hero0_attack_right_8", new Vector2(-66, -175), 101)
            //    }
            //};
            //// Attack Right
            //AnimationSequence AttackRight = new AnimationSequence
            //{
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_attack_right_1", new Vector2(-66, -175), 101),
            //        new AnimationFrame("hero0_attack_right_2", new Vector2(-66, -175), 101),
            //        new AnimationFrame("hero0_attack_right_3", new Vector2(-66, -175), 101),
            //        new AnimationFrame("hero0_attack_right_4", new Vector2(-66, -175), 99),
            //        new AnimationFrame("hero0_attack_right_5", new Vector2(-66, -175), 180),
            //        new AnimationFrame("hero0_attack_right_6", new Vector2(-66, -175), 99),
            //        new AnimationFrame("hero0_attack_right_7", new Vector2(-66, -175), 101),
            //        new AnimationFrame("hero0_attack_right_8", new Vector2(-66, -175), 101)
            //    }
            //};
            //// Cast Left
            //AnimationSequence CastLeft = new AnimationSequence
            //{
            //    Loop = true,
            //    Flipped = true,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_attack_right_1", new Vector2(-66, -175), 120)
            //    }
            //};
            //// Cast Right
            //AnimationSequence CastRight = new AnimationSequence
            //{
            //    Loop = true,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_attack_right_1", new Vector2(-66, -175), 120)
            //    }
            //};
            //// Die Left
            //AnimationSequence DieLeft = new AnimationSequence
            //{
            //    Flipped = true,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_death_right_1", new Vector2(-83, -164), 101),
            //        new AnimationFrame("hero0_death_right_2", new Vector2(-83, -164), 101),
            //        new AnimationFrame("hero0_death_right_3", new Vector2(-83, -164), 101),
            //        new AnimationFrame("hero0_death_right_4", new Vector2(-83, -164), 350),
            //        new AnimationFrame("hero0_death_right_5", new Vector2(-83, -164), 140),
            //        new AnimationFrame("hero0_death_right_6", new Vector2(-83, -164), 140),
            //        new AnimationFrame("hero0_death_right_7", new Vector2(-83, -164), 200),
            //        new AnimationFrame("hero0_death_right_8", new Vector2(-83, -164), 175),
            //        new AnimationFrame("hero0_death_right_9", new Vector2(-83, -164), 400)
            //    }
            //};
            //// Die Right
            //AnimationSequence DieRight = new AnimationSequence
            //{
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_death_right_1", new Vector2(-83, -164), 101),
            //        new AnimationFrame("hero0_death_right_2", new Vector2(-83, -164), 101),
            //        new AnimationFrame("hero0_death_right_3", new Vector2(-83, -164), 101),
            //        new AnimationFrame("hero0_death_right_4", new Vector2(-83, -164), 350),
            //        new AnimationFrame("hero0_death_right_5", new Vector2(-83, -164), 140),
            //        new AnimationFrame("hero0_death_right_6", new Vector2(-83, -164), 140),
            //        new AnimationFrame("hero0_death_right_7", new Vector2(-83, -164), 200),
            //        new AnimationFrame("hero0_death_right_8", new Vector2(-83, -164), 175),
            //        new AnimationFrame("hero0_death_right_9", new Vector2(-83, -164), 400)
            //    }
            //};
            //// Damage Left
            //AnimationSequence DamageLeft = new AnimationSequence
            //{
            //    Flipped = true,
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_damage_right_1", new Vector2(4, 6), 120),
            //        new AnimationFrame("hero0_damage_right_2", new Vector2(4, 6), 120),
            //        new AnimationFrame("hero0_damage_right_3", new Vector2(4, 6), 120)
            //    }
            //};
            //// Damage Right
            //AnimationSequence DamageRight = new AnimationSequence
            //{
            //    Frames = new List<AnimationFrame>()
            //    {
            //        new AnimationFrame("hero0_damage_right_1", new Vector2(4, 6), 120),
            //        new AnimationFrame("hero0_damage_right_2", new Vector2(4, 6), 120),
            //        new AnimationFrame("hero0_damage_right_3", new Vector2(4, 6), 120)
            //    }
            //};

            //AnimationSystem system = new AnimationSystem();
            //system.Sequences["IdleLeft"] = IdleLeft;
            //system.Sequences["IdleRight"] = IdleRight;
            //system.Sequences["WalkLeft"] = WalkLeft;
            //system.Sequences["WalkRight"] = WalkRight;
            //system.Sequences["WalkUp"] = WalkUp;
            //system.Sequences["WalkDown"] = WalkDown;
            //system.Sequences["AttackLeft"] = AttackLeft;
            //system.Sequences["AttackRight"] = AttackRight;
            //system.Sequences["CastLeft"] = CastLeft;
            //system.Sequences["CastRight"] = CastRight;
            //system.Sequences["DieLeft"] = DieLeft;
            //system.Sequences["DieRight"] = DieRight;
            //system.Sequences["DamageLeft"] = DamageLeft;
            //system.Sequences["DamageRight"] = DamageRight;
            //system.Offsets["Floor"] = new Vector2(0, -10);
            //system.Offsets["Center"] = new Vector2(-102, 70);

            //string jsonString = JsonConvert.SerializeObject(system, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented,  });

            //var a = "";
        }
    }
}
