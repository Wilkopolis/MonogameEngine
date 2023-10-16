using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using static MonogameEngine.MonogameEngine;

#pragma warning disable IDE0017 // Simplify object initialization
#pragma warning disable IDE0028 // Simplify collection initialization

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        bool Test()
        {
            SetupExamples();
            return true;
        }

        void SetupExamples()
        {
            RemoveAllElements();

            SetFlag("Test", true);
            ScrollOffsets["exampleButtons"] = new Offset();

            // make screen one
            Screen container = new Screen(1920, 1080) { ClearColor = Col(12, 10, 10) };
            Screens["primary"] = container;

            // make the scroll bar on the left (like three.js examples)
            List<string> buttonNames = new List<string>
            {
                "Box", "Sprite", "Text", "CompoundElement", "Button", "Dragging", "Tooltip", "Cursors",
                "Sound", "Alpha", "Masks", "Cooldown", "Blur", "Pixelate", "Noise", "Keyboard",
                "Character", "ColorShift", "Stroke", "DropShadow", "Smoke", "Fog", "Glow", "LootBeam", "Sparkle", 
                "LevelUp", "Zap"
            };

            for (int i = 0; i < buttonNames.Count; i++)
            {
                // make each button
                Button button = new Button();
                Box bg = new Box(254, 54, Col(45, 47, 46));
                button.Add(bg);
                Box fg = new Box(250, 50, Col(15, 17, 16), 2, 2);
                fg.Key = "passiveFG";
                button.Add(fg);
                Box fg2 = new Box(250, 50, Col(65, 51, 16), 2, 2);
                fg2.Key = "activeFG";
                fg2.Visible = false;
                button.Add(fg2);
                Text text = new Text(buttonNames[i], Fonts.Library[FontFamily.RobotoSlab][14]);
                text.Key = "text";
                text.Fit(240);
                text.Center(bg);
                button.Add(text);
                button.Key = "button" + i;
                button.Position = new Vector2(5, i * 58 + 8);
                container.Add(button);

                button.ScrollKeys.Add("exampleButtons");

                button.ClickHandler = Test_ClickExampleButton;
            }

            Box scrollbar = new Box(4, 240, Col(45, 47, 46), 264, 8);
            scrollbar.Key = "exampleScrollBar";
            container.Add(scrollbar);

            // make each of the example screens

            // box 
            Test_AddBoxScreen();

            // sprite
            Test_AddSpriteScreen();

            // text
            Test_AddTextScreen();

            // compound element
            Test_AddCompoundElementScreen();

            // button + hitbox
            Test_AddButtonScreen();

            // dragging 
            Test_AddDragableScreen();

            // tooltip
            Test_AddTooltipScreen();

            // cursors
            Test_AddCursorsScreen();

            // sound demo
            Test_AddSoundScreen();

            // alpha
            Test_AddAlphaScreen();

            // masks, rectangle, inverse, black/white
            Test_AddMaskScreen();

            // cooldown indicator (square / radial)
            Test_AddCooldownScreen();

            // blurs
            Test_AddBlurScreen();

            // pixelate
            Test_AddPixelateScreen();

            // noise
            Test_AddNoiseScreen();

            // keyboard
            Test_AddKeyboardScreen();

            // character animation system demo
            Test_AddCharacterAnimationScreen();

            // color shift
            Test_AddColorShiftScreen();

            // stroke
            Test_AddStrokeScreen();

            // drop shadow
            Test_AddDropShadowScreen();

            // smoke
            Test_AddSmokeScreen();

            // fog
            Test_AddFogScreen();

            // glow
            Test_AddGlowScreen();

            // stroke glow
            Test_AddStrokeGlowScreen();

            // loot beam
            Test_AddLootBeamScreen();

            // sparkle effect
            Test_AddSparkleScreen();

            // level up / buff effect
            Test_AddLevelUpScreen();

            // zap animation
            Test_AddZapScreen();

            container.Elements["button0"].ClickHandler(container.Elements["button0"]);
        }

        void Test_AddBoxScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["BoxExample"] = exampleScreen;

            // draw the viewport
            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples
            Box box1 = new Box(200, 200, Color.Red, 150, 150);
            viewport.Add("box1", box1);

            Box box2 = new Box(400, 200, Color.Green, 450, 250);
            viewport.Add("box2", box2);

            Box box3 = new Box(200, 400, Color.Blue, 300, 100);
            viewport.Add("box3", box3);

            box1.zIndex = 1.1f;
            box2.zIndex = 1.1f;
            box3.zIndex = 1;

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Box", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase("es are simple textures that are filled with a color. These are the basis for most effects. Fog/Smoke/Lightning etc. are shaders that run on textures. The ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("Box", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(" constructor is very simple:", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Position = new Vector2(250, 700);
            exampleScreen.Add("description1", desc1);

            Box codeBg = new Box(1000, 114, Col(20, 17, 19), 0, 810);
            codeBg.CenterX(viewport);
            exampleScreen.Add("codeBg", codeBg);

            List<Phrase> p2 = new List<Phrase>();
            p2.Add(new Phrase("Box ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase("box1 = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p2.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p2.Add(new Phrase("Box", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("200", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("200", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("Color", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(50, 120, 80))));
            p2.Add(new Phrase(".", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("Red", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("150", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("150", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph2 = new Paragraph(p2, 1200);
            Element desc2 = MakeParagraph(paragraph2);
            desc2.Position = new Vector2(codeBg.Pos().X + 18, codeBg.Pos().Y + 10);
            exampleScreen.Add("description2", desc2);

            List<Phrase> p3 = new List<Phrase>();
            p3.Add(new Phrase("Box ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p3.Add(new Phrase("box2 = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p3.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p3.Add(new Phrase("Box", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p3.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("400", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p3.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("200", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p3.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("Color", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(50, 120, 80))));
            p3.Add(new Phrase(".", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("Green", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("250", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p3.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("450", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p3.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph3 = new Paragraph(p3, 1200);
            Element desc3 = MakeParagraph(paragraph3);
            desc3.Position = new Vector2(codeBg.Pos().X + 18, desc2.Bot() + 4);
            exampleScreen.Add("description3", desc3);

            List<Phrase> p4 = new List<Phrase>();
            p4.Add(new Phrase("Box ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p4.Add(new Phrase("box3 = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p4.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p4.Add(new Phrase("Box", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p4.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("200", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p4.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("400", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p4.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("Color", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(50, 120, 80))));
            p4.Add(new Phrase(".", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("Blue", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("300", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p4.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("100", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p4.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph4 = new Paragraph(p4, 1200);
            Element desc4 = MakeParagraph(paragraph4);
            desc4.Position = new Vector2(codeBg.Pos().X + 18, desc3.Bot() + 4);
            exampleScreen.Add("description4", desc4);

            exampleScreen.Position = new Vector2(270, 0);
            exampleScreen.RenderPeriod = -1;
            exampleScreen.Render(true);
        }
        
        void Test_AddSpriteScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["SpriteExample"] = exampleScreen;

            // draw the viewport
            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples
            Sprite sprite1 = new Sprite(25, 25, Textures["examples/battle_background"]) { zIndex = .1f };
            sprite1.Scale = .32f;
            viewport.Add("sprite1", sprite1);

            Sprite sprite2 = new Sprite(400, 10, Textures["examples/Attack0001"]);
            sprite2.Scale = 1f;
            viewport.Add("sprite2", sprite2);

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Sprite", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase("s are image files you load off your HDD/SSD. The texture you load is like a box texture. It can run effects, rotate, be transparent etc. Like any monogame project you have to add your sprites to your content project and build them.", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Position = new Vector2(250, 700);
            exampleScreen.Add("description1", desc1);

            Box codeBg = new Box(1100, 86, Col(20, 17, 19), 0, 810);
            codeBg.CenterX(viewport);
            exampleScreen.Add("codeBg", codeBg);

            List<Phrase> p2 = new List<Phrase>();
            p2.Add(new Phrase("Sprite ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase("sprite1 = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p2.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p2.Add(new Phrase("Sprite", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("25", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("25", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("Textures[", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("\"examples/battle_background\"", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(180, 90, 40))));
            p2.Add(new Phrase("]", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph2 = new Paragraph(p2, 1200);
            Element desc2 = MakeParagraph(paragraph2);
            desc2.Position = new Vector2(codeBg.Pos().X + 18, codeBg.Pos().Y + 10);
            exampleScreen.Add("description2", desc2);

            List<Phrase> p3 = new List<Phrase>();
            p3.Add(new Phrase("Sprite ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p3.Add(new Phrase("sprite2 = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p3.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p3.Add(new Phrase("Sprite", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p3.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("400", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p3.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("10", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p3.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("Textures[", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("\"examples/Attack0001\"", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(180, 90, 40))));
            p3.Add(new Phrase("]", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph3 = new Paragraph(p3, 1200);
            Element desc3 = MakeParagraph(paragraph3);
            desc3.Position = new Vector2(codeBg.Pos().X + 18, desc2.Bot() + 4);
            exampleScreen.Add("description3", desc3);

            exampleScreen.Position = new Vector2(270, 0);
            exampleScreen.RenderPeriod = -1;
            exampleScreen.Render(true);
        }

        void Test_AddTextScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["TextExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Text t1 = new Text(112, 112, "Bahnschrit - abcdefghijklmnopqrstuvwxyz", Fonts.Library[FontFamily.Bahnschrift][24].Hue(Col(200, 200, 200)));
            Text t2 = new Text(112, 112 + 44 * 1, "Merriweather - abcdefghijklmnopqrstuvwxyz", Fonts.Library[FontFamily.Merriweather][24].Hue(Col(200, 200, 200)));
            Text t3 = new Text(112, 112 + 44 * 2, "Acme - abcdefghijklmnopqrstuvwxyz", Fonts.Library[FontFamily.Acme][24].Hue(Col(200, 200, 200)));
            Text t4 = new Text(112, 112 + 44 * 3, "RobotoSlab - abcdefghijklmnopqrstuvwxyz", Fonts.Library[FontFamily.RobotoSlab][24].Hue(Col(200, 200, 200)));
            Text t5 = new Text(112, 112 + 44 * 4, "RobotoMono - abcdefghijklmnopqrstuvwxyz", Fonts.Library[FontFamily.RobotoMono][24].Hue(Col(200, 200, 200)));
            Text t6 = new Text(112, 112 + 44 * 5, "Catamaran - abcdefghijklmnopqrstuvwxyz", Fonts.Library[FontFamily.Catamaran][24].Hue(Col(200, 200, 200)));
            Text t7 = new Text(112, 112 + 44 * 6, "K2D - abcdefghijklmnopqrstuvwxyz", Fonts.Library[FontFamily.K2D][24].Hue(Col(200, 200, 200)));
            Text t8 = new Text(112, 112 + 44 * 7, "OpenSans - abcdefghijklmnopqrstuvwxyz", Fonts.Library[FontFamily.OpenSans][24].Hue(Col(200, 200, 200)));

            viewport.Add("t1", t1);
            viewport.Add("t2", t2);
            viewport.Add("t3", t3);
            viewport.Add("t4", t4);
            viewport.Add("t5", t5);
            viewport.Add("t6", t6);
            viewport.Add("t7", t7);
            viewport.Add("t8", t8);

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Text", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(" uses spritefonts to draw strings to the screen. You can use any font you can find online. Just have to set them up in the content builder pipeline.", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Position = new Vector2(250, 700);
            exampleScreen.Add("description1", desc1);

            Box codeBg = new Box(1570, 52, Col(20, 17, 19), 0, 810);
            codeBg.CenterX(viewport);
            exampleScreen.Add("codeBg", codeBg);

            List<Phrase> p2 = new List<Phrase>();
            p2.Add(new Phrase("Text ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase("text1 = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p2.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p2.Add(new Phrase("Text", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("112", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("112", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("\"Bahnschrit - abcdefghijklmnopqrstuvwxyz\"", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(180, 90, 40))));
            p2.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("Fonts", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase(".", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("Library", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("[", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("FontFamily", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase(".", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("Bahnschrift", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("]", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("[", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase("24", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p2.Add(new Phrase("]);", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph2 = new Paragraph(p2, 9999);
            Element desc2 = MakeParagraph(paragraph2);
            desc2.Position = new Vector2(codeBg.Pos().X + 18, codeBg.Pos().Y + 10);
            exampleScreen.Add("description2", desc2);

            exampleScreen.Position = new Vector2(270, 0);
            exampleScreen.RenderPeriod = -1;
            exampleScreen.Render(true);
        }

        void Test_AddCompoundElementScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["CompoundElementExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Sprite sprite1 = new Sprite(140, 80, Textures["examples/Attack0001"], .5f);
            viewport.Add("sprite1", sprite1);

            Box box1 = new Box(150, 12, Col(64, 70, 220));
            box1.Position.Y = sprite1.Position.Y + sprite1.Height + 10;
            box1.Position.X = 195;
            viewport.Add("box1", box1);

            // I swear I made this as a compound element in another game
            Sprite sprite2 = new Sprite(500, 140, Textures["examples/description"]);
            sprite2.Key = "sprite2";
            viewport.Add("sprite2", sprite2);

            // draw the info

            List<Phrase> p1 = new List<Phrase>(); 
            p1.Add(new Phrase("You can collect elements into a collection called a ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("CompoundElement", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(". ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("CompoundElements", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(" have the total width + height of all the pixels in your collection. ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("CompoundElements", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(" can reduce the complexity of having to move or interact with complex visuals, like a description box or a character with status bars.", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Position = new Vector2(250, 688);
            exampleScreen.Add("description1", desc1);

            Box codeBg = new Box(1100, 180, Col(20, 17, 19), 0, 820);
            codeBg.CenterX(viewport);
            exampleScreen.Add("codeBg", codeBg);

            List<Phrase> p2 = new List<Phrase>();
            p2.Add(new Phrase("CompoundElement ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase("combo = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p2.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p2.Add(new Phrase("CompoundElement", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p2.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p2.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph2 = new Paragraph(p2, 1200);
            Element desc2 = MakeParagraph(paragraph2);
            desc2.Position = new Vector2(codeBg.Pos().X + 18, codeBg.Pos().Y + 10);
            exampleScreen.Add("description2", desc2);

            List<Phrase> p3 = new List<Phrase>();
            p3.Add(new Phrase("Sprite ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p3.Add(new Phrase("sprite1 = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p3.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p3.Add(new Phrase("Sprite", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p3.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("400", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p3.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("10", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p3.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("Textures[", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase("\"examples/Attack0001\"", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(180, 90, 40))));
            p3.Add(new Phrase("]", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p3.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph3 = new Paragraph(p3, 1200);
            Element desc3 = MakeParagraph(paragraph3);
            desc3.Position = new Vector2(codeBg.Pos().X + 18, desc2.Bot() + 4);
            exampleScreen.Add("description3", desc3);

            List<Phrase> p4 = new List<Phrase>();
            p4.Add(new Phrase("Box ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p4.Add(new Phrase("box1 = ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p4.Add(new Phrase("new ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 70, 120))));
            p4.Add(new Phrase("Box", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(70, 140, 100))));
            p4.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("200", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p4.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("12", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p4.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("Color", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(50, 120, 80))));
            p4.Add(new Phrase(".", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("CornflowerBlue", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("300", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p4.Add(new Phrase(", ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p4.Add(new Phrase("100", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(130, 180, 150))));
            p4.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph4 = new Paragraph(p4, 1200);
            Element desc4 = MakeParagraph(paragraph4);
            desc4.Position = new Vector2(codeBg.Pos().X + 18, desc3.Bot() + 4);
            exampleScreen.Add("description4", desc4);

            List<Phrase> p5 = new List<Phrase>();
            p5.Add(new Phrase("combo ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p5.Add(new Phrase(".", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p5.Add(new Phrase("Add", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(170, 160, 120))));
            p5.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p5.Add(new Phrase("sprite1", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p5.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph5 = new Paragraph(p5, 1200);
            Element desc5 = MakeParagraph(paragraph5);
            desc5.Position = new Vector2(codeBg.Pos().X + 18, desc4.Bot() + 4);
            exampleScreen.Add("description5", desc5);

            List<Phrase> p6 = new List<Phrase>();
            p6.Add(new Phrase("combo ", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p6.Add(new Phrase(".", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p6.Add(new Phrase("Add", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(170, 160, 120))));
            p6.Add(new Phrase("(", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            p6.Add(new Phrase("box1", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(120, 122, 140))));
            p6.Add(new Phrase(");", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(140, 140, 140))));
            Paragraph paragraph6 = new Paragraph(p6, 1200);
            Element desc6 = MakeParagraph(paragraph6);
            desc6.Position = new Vector2(codeBg.Pos().X + 18, desc5.Bot() + 4);
            exampleScreen.Add("description6", desc6);

            exampleScreen.Position = new Vector2(270, 0);
            exampleScreen.RenderPeriod = -1;
            exampleScreen.Render(true);
        }

        void Test_AddButtonScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["ButtonExample"] = exampleScreen;

            // initialize some variables
            exampleScreen.Flags["nextGold"] = 0;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the button
            
            Button button = new Button();
            Sprite buttonFg = new Sprite(0, 0, Textures["examples/button_fg"], .5f);
            button.Add(buttonFg);

            button.PushedOffset.X = 0;
            Sprite buttonBg = new Sprite(0, 0, Textures["examples/button_bg"].Texture, .5f);
            buttonBg.zIndex = .9f;
            button.Resize();
            button.Position = new Vector2(300, 450);
            buttonBg.ScreenCenter(button);
            viewport.Add("buttonBg", buttonBg);

            viewport.Add("button1", button);

            button.MouseOverHandler = delegate {
                Text t = viewport.Elements["t2"] as Text;
                t.String = "Mouse over button: Yes";
            };
            button.MouseLeaveHandler = delegate {
                Text t = viewport.Elements["t2"] as Text;
                t.String = "Mouse over button: No";
            };

            Text t1 = new Text(5, viewport.Height - 30, "Button Pressed: ", Fonts.Library[FontFamily.Bahnschrift][16]);
            viewport.Add("t1", t1);
            Text t2 = new Text(5, viewport.Height - 60, "Mouse over button: ", Fonts.Library[FontFamily.Bahnschrift][16]);
            viewport.Add("t2", t2);

            // draw the pouch

            Sprite bagSprite = new Sprite(500, 340, Textures["examples/small/4"]);
            viewport.Add("bagSprite", bagSprite);

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("A ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("Button", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(" is a ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("CompoundElement", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(" that you can hook up with mouse interaction. ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("MouseOver", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(", ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("MouseLeave", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(", and ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("Click", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(" the above ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("Button", Fonts.Library[FontFamily.RobotoMono][16].Hue(Col(133, 130, 190))) { Offset = new Vector2(0, 1) });
            p1.Add(new Phrase(" to see it in action.", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Position = new Vector2(250, 688);
            exampleScreen.Add("description1", desc1);

            Sprite codePreview = new Sprite(0, 0, Textures["examples/code/button"]);
            codePreview.ScreenCenter(viewport);
            codePreview.Position.Y = 800;
            exampleScreen.Add("codePreview", codePreview);

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddDragableScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["DraggingExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // we have to draw dragged elements to the screen they are on
            CompoundElement compoundElement = new CompoundElement();
            Box boxBg = new Box(140, 100, Col(75, 40, 85));
            compoundElement.Add(boxBg);
            Text text = new Text(26, 32, "Drag Me", Fonts.Library[FontFamily.RobotoSlab][16]);
            compoundElement.Add(text);
            compoundElement.Cursor = CursorType.Grab;
            compoundElement.Draggable = true;
            compoundElement.MouseOverCheck = true;
            viewport.Add("drag", compoundElement);

            compoundElement.DragHandler = delegate
            {
                compoundElement.CursorOffset = compoundElement.Pos() - new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
                DraggingElement = compoundElement;
            };

            compoundElement.DragReleaseHandler = delegate
            {
                Vector2 pos = compoundElement.ScreenPos();
                DraggingElement.CursorOffset = Vector2.Zero;
                DraggingElement = null;
                compoundElement.Position = pos;
            };

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddTooltipScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["TooltipExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("For now we're not going to not implement tooltip. We don't know how we're going to use it in the future and ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("I don't want to spend a bunch of effort on a feature we don't know how we want to work. ", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            Paragraph paragraph1 = new Paragraph(p1, 600);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Center(new Vector2(viewport.Width / 2, viewport.Height / 2));
            viewport.Add("description1", desc1);

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddCursorsScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["CursorsExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // we have to draw dragged elements to the screen they are on
            CompoundElement compoundElement1 = new CompoundElement();
            Box boxBg = new Box(140, 100, Col(75, 40, 85));
            compoundElement1.Add(boxBg);
            Text text = new Text(26, 34, "Pointer", Fonts.Library[FontFamily.RobotoSlab][16]);
            text.CenterX(boxBg.Width / 2);
            compoundElement1.Add(text);
            compoundElement1.Cursor = CursorType.Pointer;
            compoundElement1.MouseOverCheck = true;
            viewport.Add("Pointer", compoundElement1);

            CompoundElement compoundElement2 = new CompoundElement();
            Box boxBg2 = new Box(140, 100, Col(75, 40, 85));
            compoundElement2.Add(boxBg2);
            Text text2 = new Text(26, 34, "Grab", Fonts.Library[FontFamily.RobotoSlab][16]);
            text2.CenterX(boxBg2.Width / 2);
            compoundElement2.Add(text2);
            compoundElement2.Cursor = CursorType.Grab;
            compoundElement2.MouseOverCheck = true;
            viewport.Add("Grab", compoundElement2);

            CompoundElement compoundElement3 = new CompoundElement();
            Box boxBg3 = new Box(140, 100, Col(75, 40, 85));
            compoundElement3.Add(boxBg3);
            Text text3 = new Text(26, 34, "Holding", Fonts.Library[FontFamily.RobotoSlab][16]);
            text3.CenterX(boxBg3.Width / 2);
            compoundElement3.Add(text3);
            compoundElement3.Cursor = CursorType.Holding;
            compoundElement3.MouseOverCheck = true;
            viewport.Add("Holding", compoundElement3);

            CompoundElement compoundElement4 = new CompoundElement();
            Box boxBg4 = new Box(140, 100, Col(75, 40, 85));
            compoundElement4.Add(boxBg4);
            Text text4 = new Text(26, 34, "Glass", Fonts.Library[FontFamily.RobotoSlab][16]);
            text4.CenterX(boxBg4.Width / 2);
            compoundElement4.Add(text4);
            compoundElement4.Cursor = CursorType.Glass;
            compoundElement4.MouseOverCheck = true;
            viewport.Add("Glass", compoundElement4);

            CompoundElement compoundElement5 = new CompoundElement();
            Box boxBg5 = new Box(140, 100, Col(75, 40, 85));
            compoundElement5.Add(boxBg5);
            Text text5 = new Text(26, 34, "Return", Fonts.Library[FontFamily.RobotoSlab][16]);
            text5.CenterX(boxBg5.Width / 2);
            compoundElement5.Add(text5);
            compoundElement5.Cursor = CursorType.Return;
            compoundElement5.MouseOverCheck = true;
            viewport.Add("Return", compoundElement5);

            CompoundElement compoundElement6 = new CompoundElement();
            Box boxBg6 = new Box(140, 100, Col(75, 40, 85));
            compoundElement6.Add(boxBg6);
            Text text6 = new Text(26, 34, "Hidden", Fonts.Library[FontFamily.RobotoSlab][16]);
            text6.CenterX(boxBg6.Width/2);
            compoundElement6.Add(text6);
            compoundElement6.Cursor = CursorType.Hidden;
            compoundElement6.MouseOverCheck = true;
            viewport.Add("Hidden", compoundElement6);

            compoundElement1.Position = new Vector2(230, 130);
            compoundElement2.Position = new Vector2(430, 130);
            compoundElement3.Position = new Vector2(630, 130);
            compoundElement4.Position = new Vector2(230, 330);
            compoundElement5.Position = new Vector2(430, 330);
            compoundElement6.Position = new Vector2(630, 330);

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddSoundScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["SoundExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            viewport.Flags["draggingSlider"] = 0;
            Setting.SFXVOLUME = .1f;
            Setting.MUSICVOLUME = .1f;

            // draw the examples

            // add a volume slider
            Text volume = new Text(20, 10, "Volume: 0.1", Fonts.Library[FontFamily.K2D][16]);
            viewport.Add("volume", volume);

            Box ruler = new Box(200, 2, Col(50, 50, 57), 0, 25);
            ruler.Position.X = volume.Position.X + volume.Width + 20;
            viewport.Add("ruler", ruler);

            Box slide = new Box(10, 20, Col(90, 90, 97), zIndex: 1.1f);
            slide.Position.X = ruler.Position.X + 10;
            slide.Position.Y = ruler.Position.Y - 9;
            slide.Draggable = true;
            slide.MouseOverCheck = true;
            slide.Cursor = CursorType.Grab;
            slide.DragHandler = delegate
            {
                viewport.Flags["draggingSlider"] = 1;
            };
            viewport.Add("slide", slide);

            // add a sfx title
            Text sfx = new Text(100, 150, "SFX", Fonts.Library[FontFamily.K2D][16]);
            viewport.Add("sfx", sfx);

            // add a sfx button
            Button button1 = new Button();
            Box button1Bg = new Box(80, 40, Col(35, 25, 50));
            button1.Add(button1Bg);
            Text button1Text = new Text(20, 3, "Play", Fonts.Library[FontFamily.K2D][16]);
            button1.Add(button1Text);
            viewport.Add("button1", button1);
            button1.Position = new Vector2(120, 190);
            button1.ClickHandler = delegate
            {
                Sound sound = new Sound(SoundEntries["sfx/quack"], .1f);
                sound.Play();
            };

            // add a music title
            Text music = new Text(100, 250, "Music", Fonts.Library[FontFamily.K2D][16]);
            viewport.Add("music", music);

            // add a music button
            Button button2 = new Button();
            Box button2Bg = new Box(80, 40, Col(35, 25, 50));
            button2.Add(button2Bg);
            Text button2Text = new Text(20, 3, "Play", Fonts.Library[FontFamily.K2D][16]);
            button2.Add(button2Text);
            viewport.Add("button2", button2);
            button2.Position = new Vector2(120, 290);
            button2.ClickHandler = delegate
            {
                foreach (Sound sound1 in Sounds.Values)
                    sound1.Stop();

                Sounds.Clear();

                Sound sound = new Sound(SoundEntries["music/menu"], .1f);
                sound.Song = true;
                sound.Play();
                Sounds["music"] = sound;
            };

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddAlphaScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["AlphaExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex:0);
            viewport.Add("bg", bg);

            List<BlendState> blendStates = new List<BlendState>() { BlendState.AlphaBlend, BlendState.NonPremultiplied, BlendState.Additive, BlendState.Opaque };

            for (int i = 0; i < blendStates.Count; i++)
            {
                BlendState blendState = blendStates[i];
                string entry = blendState.ToString().Split('.')[1];
                Text title = new Text(0, 40, entry, Fonts.Library[FontFamily.K2D][14]);
                title.CenterX(245 + i * 175);
                viewport.Add("title" + i, title);

                for (int j = 0; j < 11; j++)
                {
                    Box box = new Box(175, 45, Color.Red);
                    box.CenterX(245 + i * 175);
                    box.Position.Y = 75 + j * 45;
                    box.Alpha = 1 - j * .1f;
                    box.BlendState = blendState;
                    viewport.Add("box" + i + j, box);
                }
            }

            for (int j = 0; j < 11; j++)
            {
                Text title = new Text(110, 85 + j * 45, (1 - j * .1f).ToString("0.0"), Fonts.Library[FontFamily.K2D][14]);
                viewport.Add("alpha" + j, title);
            }

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddMaskScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["MasksExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            viewport.Add("bg", bg);

            bg.AddMask(EffectType.IncludeMask, 100, 100, viewport.Width - 200, viewport.Height - 200).Relative = true;
            bg.AddMask(EffectType.ExcludeMask, 200, 150, 100, 200).Relative = true;
            bg.AddMask(EffectType.KeyMask, 500, 200, texture: Textures["examples/key_mask"].Texture);

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddCooldownScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["CooldownExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            viewport.Add("bg", bg);

            Box radial = new Box(viewport.Width, viewport.Height, Color.Black) { Alpha = .8f };
            EffectAgent mask = radial.AddMask(EffectType.RadialMask, .5f);
            viewport.Add("radial", radial);

            viewport.Animations["radial"] = new CustomAnimation(radial, 0, delegate (float percentCompelte)
            {
                mask.Parameters["Radius"] = 1 - (MsEllapsed % 2000 / 2000.0f);
            });

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddBlurScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["BlurExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Sprite sprite1 = new Sprite(-20, 0, Textures["examples/Attack0001"]);
            sprite1.Scale = .8f;
            viewport.Add("sprite1", sprite1);

            Sprite sprite2 = new Sprite(300, 0, Textures["examples/Attack0001"]);
            sprite2.Scale = .8f;
            viewport.Add("sprite2", sprite2);

            Sprite sprite3 = new Sprite(620, 0, Textures["examples/Attack0001"]);
            sprite3.Scale = .8f;
            viewport.Add("sprite3", sprite3);

            sprite2.AddBlur(EffectType.Blur, 4);
            sprite3.AddBlur(EffectType.Blur, 12);
            sprite2.BlendState = BlendState.AlphaBlend;
            sprite3.BlendState = BlendState.AlphaBlend;

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddPixelateScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["PixelateExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Sprite sprite1 = new Sprite(-20, 0, Textures["examples/Attack0001"]);
            sprite1.Scale = .8f;
            viewport.Add("sprite1", sprite1);

            Sprite sprite2 = new Sprite(300, 0, Textures["examples/Attack0001"]);
            sprite2.Scale = .8f;
            viewport.Add("sprite2", sprite2);

            Sprite sprite3 = new Sprite(620, 0, Textures["examples/Attack0001"]);
            sprite3.Scale = .8f;
            viewport.Add("sprite3", sprite3);

            sprite2.AddBlur(EffectType.Pixelate, 4);
            sprite3.AddBlur(EffectType.Pixelate, 12);
            sprite2.BlendState = BlendState.AlphaBlend;
            sprite3.BlendState = BlendState.AlphaBlend;

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddNoiseScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["NoiseExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Sprite sprite1 = new Sprite(-20, 0, Textures["examples/Attack0001"]);
            sprite1.Scale = .8f;
            viewport.Add("sprite1", sprite1);

            Sprite sprite2 = new Sprite(300, 0, Textures["examples/Attack0001"]);
            sprite2.Scale = .8f;
            viewport.Add("sprite2", sprite2);

            Sprite sprite3 = new Sprite(620, 0, Textures["examples/Attack0001"]);
            sprite3.Scale = .8f;
            viewport.Add("sprite3", sprite3);

            sprite2.AddBlur(EffectType.Noise, .2f);
            sprite3.AddBlur(EffectType.Noise, .4f);

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddKeyboardScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["KeyboardExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Theres nothing really to demonstrate with the keyboard functions. We would do something here if this was a real demo", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            Paragraph paragraph1 = new Paragraph(p1, 600);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Center(new Vector2(viewport.Width / 2, viewport.Height / 2));
            viewport.Add("description1", desc1);

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddFrameAnimationScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["Frame AnimationExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddCharacterAnimationScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["CharacterExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            AnimationSystem hero = AnimationSystems["hero"];
            hero.Load();

            Sprite sprite = new Sprite(Textures[hero.Sequences["IdleLeft"].Frames[0].FrameKey]);
            sprite.Scale = .4f;
            sprite.Resize();
            sprite.Center(viewport.GetRelativeCenter());
            viewport.Add("sprite", sprite);

            Character character = new Character();
            character.AnimationSystem = hero.Clone();
            character.Sprite = sprite;
            sprite.Target = character;
            character.AnimationSystem.SetSource(character);

            character.AnimateIdle();

            // draw the sprite center
            //Box centerPixel = new Box(3, 3, Color.Red);
            //centerPixel.Center(sprite.GetLocalCenter() + hero.Offsets["Center"] * sprite.Scale);
            //viewport.Add("centerPixel", centerPixel);

            // draw the bounds
            //Box outline = new Box(sprite.Width + 4, sprite.Height + 4, Color.Orange);
            //outline.Center(sprite.GetLocalCenter());
            //outline.zIndex = sprite.zIndex - .002f;
            //viewport.Add("outline", outline);
            //Box outlineFg = new Box(sprite.Width, sprite.Height, Col(12, 10, 10));
            //outlineFg.Center(sprite.GetLocalCenter());
            //outlineFg.zIndex = sprite.zIndex - .001f;
            //viewport.Add("outlineFg", outlineFg);

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("WASD to walk, Space to attack. I'm not going to improve this anymore. If this was a demo for somebody, I would.", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            p1.Add(new Phrase("Depending on the game characters may work in so many different ways. Not worth the effort right now.", Fonts.Library[FontFamily.K2D][16].Hue(Col(143, 140, 140))));
            Paragraph paragraph1 = new Paragraph(p1, 600);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Center(new Vector2(viewport.Width / 2 + viewport.Position.X, 0));
            desc1.Position.Y = 700;
            exampleScreen.Add("description1", desc1);

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddColorShiftScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["ColorShiftExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddStrokeScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["StrokeExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddDropShadowScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["DropShadowExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddSmokeScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["SmokeExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddFogScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["FogExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddGlowScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["GlowExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddStrokeGlowScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["StrokeGlowExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddLootBeamScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["LootBeamExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddSparkleScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["SparkleExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddLevelUpScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["LevelUpExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddZapScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["ZapExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_MouseHandler()
        {
            Screen screen = Screens["primary"];

            // button scroll over
            if (CurrentMouseState.X < 260)
            {
                // scrolling up
                if (CurrentMouseState.ScrollWheelValue < PreviousScrollWheelValue)
                {
                    ScrollOffsets["exampleButtons"].Y -= 58;
                    ScrollOffsets["exampleButtons"].Y = Math.Clamp(ScrollOffsets["exampleButtons"].Y, -496, 0);
                }
                // scrolling down
                else if (CurrentMouseState.ScrollWheelValue > PreviousScrollWheelValue)
                {
                    ScrollOffsets["exampleButtons"].Y += 58;
                    ScrollOffsets["exampleButtons"].Y = Math.Clamp(ScrollOffsets["exampleButtons"].Y, -496, 0);
                }

                float percentComplete = ScrollOffsets["exampleButtons"].Y / -496;

                Element scrollBar = screen.Elements["exampleScrollBar"];
                scrollBar.Position.Y = 8 + (1080 - scrollBar.Height - 16) * percentComplete;
            }

            // button example
            if (Screens.ContainsKey("ButtonExample") && Screens["ButtonExample"].Visible)
            {
                Screen exampleScreen = Screens["ButtonExample"];
                Screen viewport = (Screen)exampleScreen.Elements["viewport"];
                Button button = (Button)viewport.Elements["button1"];
                Element bag = viewport.Elements["bagSprite"];
                Text t1 = (Text)viewport.Elements["t1"];

                t1.String = "Button Pressed: No";
                if (button.IsPressed())
                {
                    // run the gold faucet
                    float nextGold = exampleScreen.Flags["nextGold"];
                    if (MsEllapsed > nextGold)
                    {
                        // spawn gold
                        Sprite gold = new Sprite(Textures["examples/coins/1"]);
                        gold.zIndex = bag.zIndex + .001f;
                        gold.Position.Y = Random.Next(-30, -20);
                        gold.Key = "gold" + exampleScreen.Flags["hash"]++;

                        List<AnimationStage> stages = new List<AnimationStage>();
                        stages.Add(new AnimationStage("examples/coins/1", Random.Next(70, 80)));
                        stages.Add(new AnimationStage("examples/coins/2", Random.Next(70, 80)));
                        stages.Add(new AnimationStage("examples/coins/3", Random.Next(70, 80)));
                        viewport.Animations[gold.Key] = new FrameAnimation(gold, stages) { Loop = true, Elapsed = Random.Next(400) };

                        gold.CenterX(bag.GetScreenCenter().X);
                        gold.Position.X += Random.Next(-15, 15);

                        viewport.Add(gold);

                        exampleScreen.Flags["nextGold"] = MsEllapsed + Random.Next(10, 40);
                    }

                    t1.String = "Button Pressed: Yes";
                }

                // update the gold faucet
                List<Element> golds = viewport.GetElementsByPattern("gold");
                foreach (Element gold in golds)
                {
                    gold.Position.Y += 5;
                }

                int floor1 = (int)bag.Position.Y + 70;
                int floor2 = (int)bag.Position.Y + 114;

                bag.Texture = Textures["examples/small/4"].Texture;
                foreach (Element gold in golds)
                {
                    if (gold.Position.Y > floor1)
                    {
                        bag.Texture = Textures["examples/small/3"].Texture;
                    }
                    if (gold.Position.Y > floor2)
                    {
                        viewport.Remove(gold);
                    }
                }
            }
            // sound example
            if (Screens.ContainsKey("SoundExample") && Screens["SoundExample"].Visible)
            {
                Screen exampleScreen = Screens["SoundExample"];
                Screen viewport = (Screen)exampleScreen.Elements["viewport"];
                Element slider = viewport.Elements["slide"];
                Element ruler = viewport.Elements["ruler"];
                Text volume = (Text)viewport.Elements["volume"];

                if (LeftUpEvent && viewport.Flags["draggingSlider"] == 1)
                    viewport.Flags["draggingSlider"] = 0;

                if (viewport.Flags["draggingSlider"] == 1)
                {
                    slider.Position.X = CurrentMouseState.X;
                    slider.Position.X -= slider.GetScreen().Pos().X;

                    slider.Position.X = Math.Clamp(slider.Position.X, ruler.ScreenPos().X + 10, ruler.ScreenPos().X + ruler.Width - 20);

                    float percent = (slider.Position.X - ruler.Position.X - 10) / (ruler.Width - 30);

                    volume.String = "Volume: " + percent.ToString("0.0");
                    Setting.SFXVOLUME = percent;
                    Setting.MUSICVOLUME = percent;
                }
            }
            // character example
            if (Screens.ContainsKey("CharacterExample") && Screens["CharacterExample"].Visible)
            {
                Screen exampleScreen = Screens["CharacterExample"];
                Screen viewport = (Screen)exampleScreen.Elements["viewport"];
                Element sprite = viewport.Elements["sprite"];
                Character character = sprite.Target as Character;

                bool Moving = false;
                bool Attack = false;
                Direction MovingDirection = Direction.N;

                // this overrides other input
                if (character.Attacking)
                { 
                }
                else
                {
                    // a simple movement logic, could be improved
                    if (CurrentKeyboardState.IsKeyDown(Keys.D))
                    {
                        Moving = true;
                        MovingDirection = Direction.E;
                    }
                    if (CurrentKeyboardState.IsKeyDown(Keys.A))
                    {
                        Moving = true;
                        MovingDirection = Direction.W;
                    }
                    if (CurrentKeyboardState.IsKeyDown(Keys.W))
                    {
                        Moving = true;
                        MovingDirection = Direction.N;
                    }
                    if (CurrentKeyboardState.IsKeyDown(Keys.S))
                    {
                        Moving = true;
                        MovingDirection = Direction.S;
                    }
                    if (CurrentKeyboardState.IsKeyDown(Keys.Space))
                    {
                        Attack = true;
                    }

                    if (character.Moving && !Moving)
                    {
                        character.AnimateIdle();
                    }

                    if (Attack)
                    {
                        character.AnimateAttack(character.Direction);
                    }

                    if (Moving && !character.Moving)
                    {
                        character.AnimateWalking(MovingDirection, 2000);
                    }
                }
            }
        }

        void Test_KeyboardHandler()
        {
            foreach (Control control in Controls)
            {
                if (control.Key1 != Keys.None)
                {
                    if (control.KeyPressCheck && KeyPresses.Contains(control.Key1))
                    {
                        Test_HandleInput(control.Input);
                        continue;
                    }
                    if (control.KeyHoldCheck && currPressedKeys.Contains(control.Key1))
                    {
                        Test_HandleInput(control.Input);
                        continue;
                    }
                    if (control.KeyEventCheck && KeyEvents.Contains(control.Key1))
                    {
                        Test_HandleInput(control.Input);
                        continue;
                    }
                }
                if (control.Key2 != Keys.None)
                {
                    if (control.KeyPressCheck && KeyPresses.Contains(control.Key2))
                    {
                        Test_HandleInput(control.Input);
                        continue;
                    }
                    if (control.KeyHoldCheck && currPressedKeys.Contains(control.Key2))
                    {
                        Test_HandleInput(control.Input);
                        continue;
                    }
                    if (control.KeyEventCheck && KeyEvents.Contains(control.Key2))
                    {
                        Test_HandleInput(control.Input);
                        continue;
                    }
                }
            }
        }

        void Test_HandleInput(InputType input)
        {
            switch (input)
            {
                case InputType.Debug1:

                    Test();

                break;

                case InputType.Quit:

                    this.Exit();

                break;
            }
        }

        void Test_ClickExampleButton(Element element)
        {
            Screen screen = Screens["primary"];

            // set all the other ones as inactive
            List<Element> buttons = screen.GetElementsByPattern("button");

            foreach (Element element1 in buttons)
            {
                Button butt = (Button)element1;

                Element fg1 = butt.GetChild("activeFG");
                fg1.Visible = false;

                Element fg2 = butt.GetChild("passiveFG");
                fg2.Visible = true;
            }

            // set the button as active
            Button button = element as Button;

            Element activeFg = button.GetChild("activeFG");
            activeFg.Visible = true;

            Element passiveFg = button.GetChild("passiveFG");
            passiveFg.Visible = false;

            // hide all the other example screens
            foreach (Screen screen1 in Screens.Values)
                screen1.Visible = false;

            screen.Visible = true;

            // set this button's screen to true
            Text text = (Text)button.GetChild("text");
            if (Screens.ContainsKey(text.String + "Example"))
                Screens[text.String + "Example"].Visible = true;
        }
    }
}
