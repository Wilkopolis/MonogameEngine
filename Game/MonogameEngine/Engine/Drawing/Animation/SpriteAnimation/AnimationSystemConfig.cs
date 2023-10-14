using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonogameEngine
{
    partial class MonogameEngine
    {
        Dictionary<string, AnimationSystem> AnimationSystems = new Dictionary<string, AnimationSystem>();
        void LoadAnimationSystems()
        {
            // go to characters folder

            // find .json file

            // read in object in jsonsoft

            // build the animation system
        }

        //void AddWikiWarriorAnimationSystem(Character character)
        //{
        //    // Idle Left
        //    AnimationSequence IdleLeft = new StaticAnimationSequence
        //    {
        //        Source = character,
        //        Loop = true,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_idle_1", 187),
        //            new AnimationFrame("wiki_warrior_idle_2", 187),
        //            new AnimationFrame("wiki_warrior_idle_3", 187),
        //            new AnimationFrame("wiki_warrior_idle_4", 187)
        //        }
        //    };
        //    // Idle Right
        //    AnimationSequence IdleRight = new StaticAnimationSequence
        //    {
        //        Source = character,
        //        Loop = true,
        //        Flipped = true,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_idle_1", 187),
        //            new AnimationFrame("wiki_warrior_idle_2", 187),
        //            new AnimationFrame("wiki_warrior_idle_3", 187),
        //            new AnimationFrame("wiki_warrior_idle_4", 187)
        //        }
        //    };
        //    // Cast Left
        //    AnimationSequence CastLeft = new StaticAnimationSequence
        //    {
        //        Source = character,
        //        Direction = Direction.W,
        //        Loop = true,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_cast_1", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_2", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_3", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_4", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_5", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_6", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_7", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_8", new Vector2(ToX(16), ToY(34)), 101)
        //        }
        //    };
        //    // Cast Right
        //    AnimationSequence CastRight = new StaticAnimationSequence
        //    {
        //        Source = character,
        //        Direction = Direction.E,
        //        Loop = true,
        //        Flipped = true,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_cast_1", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_2", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_3", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_4", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_5", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_6", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_7", new Vector2(ToX(16), ToY(34)), 101),
        //            new AnimationFrame("wiki_warrior_cast_8", new Vector2(ToX(16), ToY(34)), 101)
        //        }
        //    };
        //    // Walk Left
        //    AnimationSequence WalkLeft = new MovingAnimationSequence
        //    {
        //        Source = character,
        //        Direction = Direction.W,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_walk_1", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(-21), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_2", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(-21), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_3", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(-22), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_4", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(-30), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_5", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(-19), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_6", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(-18), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_7", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(-15), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_8", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(-17), 0), 101)
        //        }
        //    };
        //    // Walk Right
        //    AnimationSequence WalkRight = new MovingAnimationSequence
        //    {
        //        Source = character,
        //        Direction = Direction.E,
        //        Flipped = true,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_walk_1", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(21), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_2", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(21), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_3", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(22), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_4", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(30), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_5", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(19), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_6", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(18), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_7", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(15), 0), 101),
        //            new AnimationFrame("wiki_warrior_walk_8", new Vector2(ToX(27), ToY(30)), new Vector2(ToX(17), 0), 101)
        //        }
        //    };
        //    // Walk Up
        //    AnimationSequence WalkUp = new MovingAnimationSequence
        //    {
        //        Source = character,
        //        Direction = Direction.N,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_walk_1", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(-21)), 101),
        //            new AnimationFrame("wiki_warrior_walk_2", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(-21)), 101),
        //            new AnimationFrame("wiki_warrior_walk_3", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(-22)), 101),
        //            new AnimationFrame("wiki_warrior_walk_4", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(-30)), 101),
        //            new AnimationFrame("wiki_warrior_walk_5", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(-19)), 101),
        //            new AnimationFrame("wiki_warrior_walk_6", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(-18)), 101),
        //            new AnimationFrame("wiki_warrior_walk_7", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(-15)), 101),
        //            new AnimationFrame("wiki_warrior_walk_8", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(-17)), 101)
        //        }
        //    };
        //    // Walk Down
        //    AnimationSequence WalkDown = new MovingAnimationSequence
        //    {
        //        Source = character,
        //        Direction = Direction.S,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_walk_1", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(21)), 101),
        //            new AnimationFrame("wiki_warrior_walk_2", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(21)), 101),
        //            new AnimationFrame("wiki_warrior_walk_3", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(22)), 101),
        //            new AnimationFrame("wiki_warrior_walk_4", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(30)), 101),
        //            new AnimationFrame("wiki_warrior_walk_5", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(19)), 101),
        //            new AnimationFrame("wiki_warrior_walk_6", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(18)), 101),
        //            new AnimationFrame("wiki_warrior_walk_7", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(15)), 101),
        //            new AnimationFrame("wiki_warrior_walk_8", new Vector2(ToX(27), ToY(30)), new Vector2(0, ToY(17)), 101)
        //        }
        //    };
        //    // Attack Left
        //    AnimationSequence AttackLeft = new StaticAnimationSequence
        //    {
        //        Source = character,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_attack_1", new Vector2(ToX(-64), ToY(-91)), 151),
        //            new AnimationFrame("wiki_warrior_attack_2", new Vector2(ToX(-64), ToY(-91)), 151),
        //            new AnimationFrame("wiki_warrior_attack_3", new Vector2(ToX(-64), ToY(-91)), 200),
        //            new AnimationFrame("wiki_warrior_attack_4", new Vector2(ToX(-64), ToY(-91)), 151)
        //        }
        //    };
        //    // Attack Right
        //    AnimationSequence AttackRight = new StaticAnimationSequence
        //    {
        //        Source = character,
        //        Flipped = true,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_attack_1", new Vector2(ToX(-64), ToY(-91)), 151),
        //            new AnimationFrame("wiki_warrior_attack_2", new Vector2(ToX(-64), ToY(-91)), 151),
        //            new AnimationFrame("wiki_warrior_attack_3", new Vector2(ToX(-64), ToY(-91)), 200),
        //            new AnimationFrame("wiki_warrior_attack_4", new Vector2(ToX(-64), ToY(-91)), 151)
        //        }
        //    };
        //    // Die Left
        //    AnimationSequence DieLeft = new StaticAnimationSequence
        //    {
        //        Source = character,
        //        AddOffset = true,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_death_1", new Vector2(ToX(-108), ToY(-63)), 200),
        //            new AnimationFrame("wiki_warrior_death_2", new Vector2(ToX(-108), ToY(-63)), 200),
        //            new AnimationFrame("wiki_warrior_death_3", new Vector2(ToX(-108), ToY(-63)), 200),
        //            new AnimationFrame("wiki_warrior_death_4", new Vector2(ToX(-108), ToY(-63)), 200),
        //            new AnimationFrame("wiki_warrior_death_5", new Vector2(ToX(-108), ToY(-63)), 200)
        //        }
        //    };
        //    // Die Right
        //    AnimationSequence DieRight = new StaticAnimationSequence
        //    {
        //        Source = character,
        //        AddOffset = true,
        //        Flipped = true,
        //        Frames = new List<AnimationFrame>()
        //        {
        //            new AnimationFrame("wiki_warrior_death_1", new Vector2(ToX(-108), ToY(-63)), 200),
        //            new AnimationFrame("wiki_warrior_death_2", new Vector2(ToX(-108), ToY(-63)), 200),
        //            new AnimationFrame("wiki_warrior_death_3", new Vector2(ToX(-108), ToY(-63)), 200),
        //            new AnimationFrame("wiki_warrior_death_4", new Vector2(ToX(-108), ToY(-63)), 200),
        //            new AnimationFrame("wiki_warrior_death_5", new Vector2(ToX(-108), ToY(-63)), 200)
        //        }
        //    };

        //    character.AnimationSystem = new AnimationSystem()
        //    {
        //        IdleLeft = IdleLeft,
        //        IdleRight = IdleRight,
        //        WalkLeft = WalkLeft,
        //        WalkRight = WalkRight,
        //        WalkUp = WalkUp,
        //        WalkDown = WalkDown,
        //        AttackLeft = AttackLeft,
        //        AttackRight = AttackRight,
        //        CastLeft = CastLeft,
        //        CastRight = CastRight,
        //        DieLeft = DieLeft,
        //        DieRight = DieRight,
        //        SpriteDimensions = new Vector2(375, 399),
        //        DamageTextOffset = new Vector2(0, ToY(130)),
        //        EmitterOffset = new Vector2(ToX(140), ToY(-20)),
        //        SmokeOffset = new Vector2(ToX(-150), ToY(20)),
        //        CenterOffset = new Vector2(0, ToY(-170))
        //    };
        //}
    }
}
