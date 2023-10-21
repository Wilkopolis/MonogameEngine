using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using static MonogameEngine.MonogameEngine;
using BracketHouse.FontExtension;
using System.Reflection.Metadata;

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
                "Box", "Sprite", "Text", "Paragraph", "CompoundElement", "Button", "Dragging", 
                "Cursors", "Animation", "Character", "Sound", "Alpha", "Masks", "Cooldown", "Blur", 
                "Pixelate", "Noise", "ColorShift", "Stroke", "DropShadow", "Smoke", "Fog", "Glow", 
                "Effects"
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
                Text text = new Text(buttonNames[i], Fonts.Merriweather, 24);
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

            // paragraph
            Test_AddParagraphScreen();

            // compound element
            Test_AddCompoundElementScreen();

            // button + hitbox
            Test_AddButtonScreen();

            // dragging 
            Test_AddDragableScreen();

            // cursors
            Test_AddCursorsScreen();

            // animation
            Test_AddAnimationScreen();

            // Character
            Test_AddCharacterScreen();

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

            // loot beam
            Test_AddEffectsScreen();

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
            p1.Add(new Phrase("Box", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase("es are simple textures that are filled with a color. These are the basis for most effects. Fog/Smoke/Lightning etc. are shaders that run on textures. The ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("Box", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(" constructor is very simple:", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/box"]);
            codeExample.Position.Y = desc1.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

            //Box codeBg = new Box(1000, 114, Col(20, 17, 19), 0, 810);
            //codeBg.CenterX(viewport);
            //exampleScreen.Add("codeBg", codeBg);

            //List<Phrase> p2 = new List<Phrase>();
            //p2.Add(new Phrase("Box ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase("box1 = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p2.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p2.Add(new Phrase("Box", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("200", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("200", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("Color", Fonts.RobotoMono.Hue(Col(50, 120, 80)), 20));
            //p2.Add(new Phrase(".", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("Red", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("150", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("150", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph2 = new Paragraph(p2, 1200);
            //Element desc2 = MakeParagraph(paragraph2);
            //desc2.Position = new Vector2(codeBg.Pos().X + 18, codeBg.Pos().Y + 10);
            //exampleScreen.Add("description2", desc2);

            //List<Phrase> p3 = new List<Phrase>();
            //p3.Add(new Phrase("Box ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p3.Add(new Phrase("box2 = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p3.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p3.Add(new Phrase("Box", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p3.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("400", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p3.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("200", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p3.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("Color", Fonts.RobotoMono.Hue(Col(50, 120, 80)), 20));
            //p3.Add(new Phrase(".", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("Green", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("250", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p3.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("450", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p3.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph3 = new Paragraph(p3, 1200);
            //Element desc3 = MakeParagraph(paragraph3);
            //desc3.Position = new Vector2(codeBg.Pos().X + 18, desc2.Bot() + 4);
            //exampleScreen.Add("description3", desc3);

            //List<Phrase> p4 = new List<Phrase>();
            //p4.Add(new Phrase("Box ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p4.Add(new Phrase("box3 = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p4.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p4.Add(new Phrase("Box", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p4.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("200", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p4.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("400", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p4.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("Color", Fonts.RobotoMono.Hue(Col(50, 120, 80)), 20));
            //p4.Add(new Phrase(".", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("Blue", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("300", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p4.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("100", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p4.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph4 = new Paragraph(p4, 1200);
            //Element desc4 = MakeParagraph(paragraph4);
            //desc4.Position = new Vector2(codeBg.Pos().X + 18, desc3.Bot() + 4);
            //exampleScreen.Add("description4", desc4);

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

            Sprite sprite1 = new Sprite(25, 25, Textures["examples/battle_background"]);
            sprite1.Scale = .32f;
            viewport.Add("sprite1", sprite1);

            Sprite sprite2 = new Sprite(400, 10, Textures["examples/Attack0001"]) { zIndex = 1.1f };
            sprite2.Scale = 1f;
            viewport.Add("sprite2", sprite2);

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Sprite", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase("s are image files you load off your HDD/SSD. The texture you load is like a any texture. It can run effects, rotate, be transparent etc. ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("Like any MonoGame project you have to add your sprites to your content project and build them.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/sprite"]);
            codeExample.Position.Y = desc1.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

            //Box codeBg = new Box(1100, 86, Col(20, 17, 19), 0, 810);
            //codeBg.CenterX(viewport);
            //exampleScreen.Add("codeBg", codeBg);

            //List<Phrase> p2 = new List<Phrase>();
            //p2.Add(new Phrase("Sprite ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase("sprite1 = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p2.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p2.Add(new Phrase("Sprite", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("25", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("25", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("Textures[", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("\"examples/battle_background\"", Fonts.RobotoMono.Hue(Col(180, 90, 40)), 20));
            //p2.Add(new Phrase("]", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph2 = new Paragraph(p2, 1200);
            //Element desc2 = MakeParagraph(paragraph2);
            //desc2.Position = new Vector2(codeBg.Pos().X + 18, codeBg.Pos().Y + 10);
            //exampleScreen.Add("description2", desc2);

            //List<Phrase> p3 = new List<Phrase>();
            //p3.Add(new Phrase("Sprite ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p3.Add(new Phrase("sprite2 = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p3.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p3.Add(new Phrase("Sprite", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p3.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("400", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p3.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("10", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p3.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("Textures[", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("\"examples/Attack0001\"", Fonts.RobotoMono.Hue(Col(180, 90, 40)), 20));
            //p3.Add(new Phrase("]", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph3 = new Paragraph(p3, 1200);
            //Element desc3 = MakeParagraph(paragraph3);
            //desc3.Position = new Vector2(codeBg.Pos().X + 18, desc2.Bot() + 4);
            //exampleScreen.Add("description3", desc3);

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

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            bg.Alpha = .4f;
            viewport.Add("bg", bg);

            // Text example
            Text text = new Text(5, 5, "Text example.", Fonts.Bahnschrift, 16);

            // Paragraph example
            List<Phrase> p0 = new List<Phrase>();
            p0.Add(new Phrase("This is an example of SDF Font rendering. SDF Fonts aren't sprite fonts like XNA or traditional vector fonts like your OS. ",
                Fonts.K2D.Hue(Col(193, 190, 170)), 32) { StrokeSize = .5f });
            p0.Add(new Phrase("Instead SDF Fonts are part of a pipeline that converts ttf fonts into a SignedDistanceField, then a shader to draw them at runtime. ",
                Fonts.K2D.Hue(Col(193, 170, 190)), 32) { StrokeSize = .5f });
            p0.Add(new Phrase("This method was developed at valve during development of TF2. Its a method that gives great font approximation and perfomance. ",
                Fonts.K2D.Hue(Col(173, 190, 190)), 32) { StrokeSize = .5f });
            p0.Add(new Phrase("In monogame, this approach has numerous advantages over the provided spritefont. ",
                Fonts.K2D.Hue(Col(193, 190, 190)), 32) { StrokeSize = .5f });
            Paragraph paragraph0 = new Paragraph(p0, viewport.Width - 100);
            Element desc0 = MakeParagraph(paragraph0);
            desc0.Center(viewport.GetLocalCenter());
            viewport.Add("description0", desc0);

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("You can use spritefonts in monogame but this engine uses SDF fonts to render all its text.", Fonts.K2D, 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/text"]);
            codeExample.Position.Y = desc1.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

            //List<Phrase> p2 = new List<Phrase>();
            //p2.Add(new Phrase("Text ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase("text1 = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p2.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p2.Add(new Phrase("Text", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("20", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("20", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("\"This is an example of SDF Font rendering\"", Fonts.RobotoMono.Hue(Col(180, 90, 40)), 20));
            //p2.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("Fonts", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase(".", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("K2D", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase(",", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase("24", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p2.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph2 = new Paragraph(p2, 9999);
            //Element desc2 = MakeParagraph(paragraph2);
            //desc2.zIndex = 1.1f;
            //exampleScreen.Add("description2", desc2);

            //Box codeBg = new Box(desc2.Width + 40, desc2.Height + 30, Col(20, 17, 19), 0, 810);
            //codeBg.CenterX(viewport);
            //exampleScreen.Add("codeBg", codeBg);

            //desc2.Position = new Vector2(codeBg.Pos().X + 20, codeBg.Pos().Y + 15);

            exampleScreen.Position = new Vector2(270, 0);
            exampleScreen.RenderPeriod = -1;
            exampleScreen.Render(true);
        }

        void Test_AddParagraphScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["ParagraphExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            bg.Alpha = .4f;
            viewport.Add("bg", bg);

            // Paragraph example

            List<Phrase> p0 = new List<Phrase>();
            p0.Add(new Phrase("This is an example a paragraph. A paragraph is a collection of phrases that is written out to a texture.", Fonts.K2D.Hue(Col(193, 190, 170)), 32));
            p0.Add(new Phrase(" You can mix and match fonts, change colors. And give it a max width to write it out to.", Fonts.Merriweather.Hue(Col(193, 170, 190)), 32));
            p0.Add(new Phrase(" And its rendered out once to a texture. At that point you can apply shaders and whatever you like.", Fonts.OpenSans.Hue(Col(173, 190, 190)), 32) { Offset = new Vector2(0, -2) });
            Paragraph paragraph0 = new Paragraph(p0, viewport.Width - 100);
            Screen desc0 = MakeParagraph(paragraph0);

            Screen holder = new Screen(desc0.Width, desc0.Height);
            holder.Add("1", desc0);

            holder.Center(viewport.GetLocalCenter());
            viewport.Add("holder", holder);

            EffectAgent colorShift = holder.AddEffect(EffectType.Lighten);
            colorShift.Parameters["R"] = 1.0f;
            colorShift.Parameters["G"] = 1.0f;
            colorShift.Parameters["B"] = 1.0f;

            viewport.Animations["custom"] = new CustomAnimation(0, delegate (float tick)
            {
                colorShift.Parameters["R"] = 1f + (float)Math.Sin(MsEllapsed / 1000f) * .2f;
                colorShift.Parameters["G"] = 1f + (float)Math.Cos(MsEllapsed / 1500f) * .2f;
            });

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("You can use paragraphs to draw large amounts of text, and fit them within certain widths.", Fonts.K2D, 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/paragraph"]);
            codeExample.Position.Y = desc1.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

            exampleScreen.Position = new Vector2(270, 0);
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

            Sprite sprite1 = new Sprite(0, 0, Textures["examples/Attack0001"], .5f);
            viewport.Add("sprite1", sprite1);

            Box box1 = new Box(150, 12, Col(64, 70, 220));
            box1.Position.X = 53;
            box1.Position.Y = sprite1.Height + 10;

            CompoundElement combo = new CompoundElement();
            combo.Add(sprite1);
            combo.Add(box1);
            combo.Position = new Vector2(140, 85);
            viewport.Add("combo", combo);

            // I swear I made this as a compound element in another game
            Sprite sprite2 = new Sprite(500, 140, Textures["examples/description"]);
            sprite2.Key = "sprite2";
            viewport.Add("sprite2", sprite2);

            // draw the info

            List<Phrase> p1 = new List<Phrase>(); 
            p1.Add(new Phrase("You can collect elements into a collection called a ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("CompoundElement", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(". ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("CompoundElements", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(" have the total width + height of all the elements in your collection. ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("CompoundElements", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(" can reduce the complexity of having to move or interact with complex visuals, like a description box or a character with status bars.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/compound"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

            //Box codeBg = new Box(1100, 180, Col(20, 17, 19), 0, 820);
            //codeBg.CenterX(viewport);
            //exampleScreen.Add("codeBg", codeBg);

            //List<Phrase> p2 = new List<Phrase>();
            //p2.Add(new Phrase("CompoundElement ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase("combo = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p2.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p2.Add(new Phrase("CompoundElement", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p2.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p2.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph2 = new Paragraph(p2, 1200);
            //Element desc2 = MakeParagraph(paragraph2);
            //desc2.Position = new Vector2(codeBg.Pos().X + 18, codeBg.Pos().Y + 10);
            //exampleScreen.Add("description2", desc2);

            //List<Phrase> p3 = new List<Phrase>();
            //p3.Add(new Phrase("Sprite ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p3.Add(new Phrase("sprite1 = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p3.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p3.Add(new Phrase("Sprite", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p3.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("400", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p3.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("10", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p3.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("Textures[", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase("\"examples/Attack0001\"", Fonts.RobotoMono.Hue(Col(180, 90, 40)), 20));
            //p3.Add(new Phrase("]", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p3.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph3 = new Paragraph(p3, 1200);
            //Element desc3 = MakeParagraph(paragraph3);
            //desc3.Position = new Vector2(codeBg.Pos().X + 18, desc2.Bot() + 4);
            //exampleScreen.Add("description3", desc3);

            //List<Phrase> p4 = new List<Phrase>();
            //p4.Add(new Phrase("Box ", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p4.Add(new Phrase("box1 = ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p4.Add(new Phrase("new ", Fonts.RobotoMono.Hue(Col(70, 70, 120)), 20));
            //p4.Add(new Phrase("Box", Fonts.RobotoMono.Hue(Col(70, 140, 100)), 20));
            //p4.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("200", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p4.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("12", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p4.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("Color", Fonts.RobotoMono.Hue(Col(50, 120, 80)), 20));
            //p4.Add(new Phrase(".", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("CornflowerBlue", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("300", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p4.Add(new Phrase(", ", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p4.Add(new Phrase("100", Fonts.RobotoMono.Hue(Col(130, 180, 150)), 20));
            //p4.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph4 = new Paragraph(p4, 1200);
            //Element desc4 = MakeParagraph(paragraph4);
            //desc4.Position = new Vector2(codeBg.Pos().X + 18, desc3.Bot() + 4);
            //exampleScreen.Add("description4", desc4);

            //List<Phrase> p5 = new List<Phrase>();
            //p5.Add(new Phrase("combo ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p5.Add(new Phrase(".", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p5.Add(new Phrase("Add", Fonts.RobotoMono.Hue(Col(170, 160, 120)), 20));
            //p5.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p5.Add(new Phrase("sprite1", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p5.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph5 = new Paragraph(p5, 1200);
            //Element desc5 = MakeParagraph(paragraph5);
            //desc5.Position = new Vector2(codeBg.Pos().X + 18, desc4.Bot() + 4);
            //exampleScreen.Add("description5", desc5);

            //List<Phrase> p6 = new List<Phrase>();
            //p6.Add(new Phrase("combo ", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p6.Add(new Phrase(".", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p6.Add(new Phrase("Add", Fonts.RobotoMono.Hue(Col(170, 160, 120)), 20));
            //p6.Add(new Phrase("(", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //p6.Add(new Phrase("box1", Fonts.RobotoMono.Hue(Col(120, 122, 140)), 20));
            //p6.Add(new Phrase(");", Fonts.RobotoMono.Hue(Col(140, 140, 140)), 20));
            //Paragraph paragraph6 = new Paragraph(p6, 1200);
            //Element desc6 = MakeParagraph(paragraph6);
            //desc6.Position = new Vector2(codeBg.Pos().X + 18, desc5.Bot() + 4);
            //exampleScreen.Add("description6", desc6);

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
            buttonBg.Center(button);
            viewport.Add("buttonBg", buttonBg);

            viewport.Add("button1", button);

            button.MouseOverHandler = delegate {
                Text t = viewport.Elements["t2"] as Text;
                t.String = "Mouse over button: Yes";
                t.Resize();
                t.Render(true);
            };
            button.MouseLeaveHandler = delegate {
                Text t = viewport.Elements["t2"] as Text;
                t.String = "Mouse over button: No";
                t.Resize();
                t.Render(true);
            };

            Text t1 = new Text(5, viewport.Height - 30, "Button Pressed: ", Fonts.Bahnschrift, 16);
            viewport.Add("t1", t1);
            Text t2 = new Text(5, viewport.Height - 60, "Mouse over button: ", Fonts.Bahnschrift, 16);
            viewport.Add("t2", t2);

            // draw the pouch

            Sprite bagSprite = new Sprite(500, 340, Textures["examples/small/4"]);
            viewport.Add("bagSprite", bagSprite);

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("A ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("Button", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(" is a ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("CompoundElement", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(" that you can hook up with mouse interaction. ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("MouseOver", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(", ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("MouseLeave", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(", and ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("Click", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(" the above ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("Button", Fonts.RobotoMono.Hue(Col(133, 130, 190)), 20));
            p1.Add(new Phrase(" to see it in action.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Position = new Vector2(250, 688);
            exampleScreen.Add("description1", desc1);

            Sprite codePreview = new Sprite(0, 0, Textures["examples/code/button"]);
            codePreview.Center(viewport);
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
            Text text = new Text(26, 32, "Drag Me", Fonts.RobotoSlab, 16);
            text.Center(boxBg.GetLocalCenter());
            compoundElement.Add(text);
            compoundElement.Cursor = CursorType.Grab;
            compoundElement.Draggable = true;
            compoundElement.MouseOverCheck = true;
            viewport.Add("drag", compoundElement);

            compoundElement.DragHandler = delegate
            {
                compoundElement.CursorOffset = compoundElement.AbsPos() - new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
                DraggingElement = compoundElement;
            };

            compoundElement.DragReleaseHandler = delegate
            {
                Vector2 pos = compoundElement.Pos();
                DraggingElement.CursorOffset = Vector2.Zero;
                DraggingElement = null;
                compoundElement.Position = pos;
            };

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Here we hook up an element with some mouse interactivity. We also make use", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" of cursor offsets so the element doesn't snap to our cursor.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1100);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/drag"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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
            Text text = new Text(26, 34, "Pointer", Fonts.RobotoSlab, 16);
            text.Center(boxBg.GetLocalCenter());
            compoundElement1.Add(text);
            compoundElement1.Cursor = CursorType.Pointer;
            compoundElement1.MouseOverCheck = true;
            viewport.Add("Pointer", compoundElement1);

            CompoundElement compoundElement2 = new CompoundElement();
            Box boxBg2 = new Box(140, 100, Col(75, 40, 85));
            compoundElement2.Add(boxBg2);
            Text text2 = new Text(26, 34, "Grab", Fonts.RobotoSlab, 16);
            text2.Center(boxBg2.GetLocalCenter());
            compoundElement2.Add(text2);
            compoundElement2.Cursor = CursorType.Grab;
            compoundElement2.MouseOverCheck = true;
            viewport.Add("Grab", compoundElement2);

            CompoundElement compoundElement3 = new CompoundElement();
            Box boxBg3 = new Box(140, 100, Col(75, 40, 85));
            compoundElement3.Add(boxBg3);
            Text text3 = new Text(26, 34, "Holding", Fonts.RobotoSlab, 16);
            text3.Center(boxBg3.GetLocalCenter());
            compoundElement3.Add(text3);
            compoundElement3.Cursor = CursorType.Holding;
            compoundElement3.MouseOverCheck = true;
            viewport.Add("Holding", compoundElement3);

            CompoundElement compoundElement4 = new CompoundElement();
            Box boxBg4 = new Box(140, 100, Col(75, 40, 85));
            compoundElement4.Add(boxBg4);
            Text text4 = new Text(26, 34, "Glass", Fonts.RobotoSlab, 16);
            text4.Center(boxBg4.GetLocalCenter());
            compoundElement4.Add(text4);
            compoundElement4.Cursor = CursorType.Glass;
            compoundElement4.MouseOverCheck = true;
            viewport.Add("Glass", compoundElement4);

            CompoundElement compoundElement5 = new CompoundElement();
            Box boxBg5 = new Box(140, 100, Col(75, 40, 85));
            compoundElement5.Add(boxBg5);
            Text text5 = new Text(26, 34, "Return", Fonts.RobotoSlab, 16);
            text5.Center(boxBg5.GetLocalCenter());
            compoundElement5.Add(text5);
            compoundElement5.Cursor = CursorType.Return;
            compoundElement5.MouseOverCheck = true;
            viewport.Add("Return", compoundElement5);

            CompoundElement compoundElement6 = new CompoundElement();
            Box boxBg6 = new Box(140, 100, Col(75, 40, 85));
            compoundElement6.Add(boxBg6);
            Text text6 = new Text(26, 34, "Hidden", Fonts.RobotoSlab, 16);
            text6.Center(boxBg6.GetLocalCenter());
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

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("You can give different elements different cursors. The mouse handler code resolves which cursors to draw.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" You can define and add custom cursors like we have here.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/cursors"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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
            Text volume = new Text(20, 10, "Volume: 0.1", Fonts.K2D, 16);
            volume.RenderPeriod = 0;
            viewport.Add("volume", volume);

            Box ruler = new Box(200, 2, Col(50, 50, 57), 0, 21);
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
            Text sfx = new Text(100, 150, "SFX", Fonts.K2D, 16);
            viewport.Add("sfx", sfx);

            // add a sfx button
            Button button1 = new Button();
            Box button1Bg = new Box(80, 40, Col(35, 25, 50));
            button1.Add(button1Bg);
            Text button1Text = new Text(0, 0, "Play", Fonts.K2D, 16);
            button1Text.Center(button1Bg.GetLocalCenter());
            button1.Add(button1Text);
            viewport.Add("button1", button1);
            button1.Position = new Vector2(120, 190);
            button1.ClickHandler = delegate
            {
                Sound sound = new Sound(SoundEntries["sfx/quack"], .1f);
                sound.Play();
            };

            // add a music title
            Text music = new Text(100, 250, "Music", Fonts.K2D, 16);
            viewport.Add("music", music);

            // add a music button
            Button button2 = new Button();
            Box button2Bg = new Box(80, 40, Col(35, 25, 50));
            button2.Add(button2Bg);
            Text button2Text = new Text(0, 0, "Play", Fonts.K2D, 16);
            button2Text.Center(button2Bg.GetLocalCenter());
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

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Monogame provides basic sound playback. You can play effects, loop music, and change the volume.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("  It is up to you to work it into the other events and animations within your game.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/sound"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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
                Text title = new Text(0, 40, entry, Fonts.K2D, 14);
                title.CenterX(244 + i * 174);
                viewport.Add("title" + i, title);

                for (int j = 0; j < 11; j++)
                {
                    Box box = new Box(174, 45, Color.Red);
                    box.CenterX(244 + i * 174);
                    box.Position.Y = 75 + j * 45;
                    box.Alpha = 1 - j * .1f;
                    box.BlendState = blendState;
                    viewport.Add("box" + i + j, box);
                }
            }

            for (int j = 0; j < 11; j++)
            {
                Text title = new Text(110, 85 + j * 45, (1 - j * .1f).ToString("0.0"), Fonts.K2D, 14);
                viewport.Add("alpha" + j, title);
            }

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Alpha is how images blend in with the color behind them. Pay close attention to how you use color and alpha.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" All examples and sprites in these examples use premultiplied alpha. You can define custom blendstates to change how your textures", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" blend with the background.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/alpha"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Mask is a common computer graphics term but here we use them to occlude textures. Here we have an", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" Inclusive Mask, an Exclusive Mask, and a KeyMask. We use the key mask to cut out the text.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/masks"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            viewport.Animations["radial"] = new CustomAnimation(0, delegate (float percentCompelte)
            {
                mask.Parameters["Radius"] = 1 - (MsEllapsed % 2000 / 2000.0f);
            });

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("This cooldown effect you might see over a skill in most games. This shader uses a radial", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" mask which applies the effect with a given percent from 0 to 1.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/radial"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("This is a simple gaussian blur. Implemented in a shader.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/blur"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("This is a simple pixelation. Also implemented in a shader.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/pixelate"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("This is a simple noise effect. Also implemented in a shader.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 40;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/noise"]);
            codeExample.Position.Y = desc1.ScreenBot() + 40;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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
            p1.Add(new Phrase("Theres nothing really to demonstrate with the keyboard functions. We would do something here if this was a real demo", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 600);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Center(new Vector2(viewport.Width / 2, viewport.Height / 2));
            viewport.Add("description1", desc1);

            // draw the info

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddAnimationScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["AnimationExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            viewport.Flags["thrower"] = 0;
            viewport.Flags["animating"] = 0;

            // draw the examples

            AnimationSystem hero = AnimationSystems["hero"];
            hero.Load();

            Sprite sprite = new Sprite(Textures[hero.Sequences["IdleLeft"].Frames[0].FrameKey]);
            sprite.Scale = .4f;
            sprite.Resize();
            sprite.Center(viewport.GetLocalCenter());
            sprite.Position.X -= 250;
            viewport.Add("sprite", sprite);

            Character character = new Character();
            character.AnimationSystem = hero.Clone();
            character.Sprite = sprite;
            sprite.Target = character;
            character.AnimationSystem.SetSource(character);

            character.AnimateIdle();

            Sprite sprite2 = new Sprite(Textures[hero.Sequences["IdleLeft"].Frames[0].FrameKey]);
            sprite2.Scale = .4f;
            sprite2.Resize();
            sprite2.Center(viewport.GetLocalCenter());
            sprite2.Position.X += 250;
            viewport.Add("sprite2", sprite2);

            Character character2 = new Character();
            character2.AnimationSystem = hero.Clone();
            character2.Sprite = sprite2;
            sprite2.Target = character2;
            character2.AnimationSystem.SetSource(character2);

            character2.Direction = Direction.W;
            character2.AnimateIdle();

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("There are many types of animations availble to use. Here we see FrameAnimations and an ItemThrowAnimation.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" Frame animations are a series of frames and ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase("the ItemThrowAnimation moves and rotates the given sprite to the commanded position.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Center(new Vector2(viewport.Width / 2 + viewport.Position.X, 0));
            desc1.Position.Y = viewport.ScreenBot() + 20;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/animation"]);
            codeExample.Position.Y = desc1.ScreenBot() + 20;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddCharacterScreen()
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
            sprite.Center(viewport.GetLocalCenter() - hero.Offsets["Center"] * sprite.Scale);
            viewport.Add("sprite", sprite);

            Character character = new Character();
            character.AnimationSystem = hero.Clone();
            character.Sprite = sprite;
            sprite.Target = character;
            character.AnimationSystem.SetSource(character);

            character.AnimateIdle();

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("WASD to walk, Space to attack. The animation systems are stored in json. Each animation has a name, a list of frames,", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" and animation information. Information such as frame length, walk offset, etc.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1000);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Center(new Vector2(viewport.Width / 2 + viewport.Position.X, 0));
            desc1.Position.Y = viewport.ScreenBot() + 20;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/character"]);
            codeExample.Position.Y = desc1.ScreenBot() + 20;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            bg.Alpha = .5f;
            viewport.Add("bg", bg);

            AnimationSystem hero = AnimationSystems["hero"];
            hero.Load();

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Vector2 position = new Vector2(100 + i * 80, 55 + j * 80);
                    Sprite sprite = new Sprite(Textures[hero.Sequences["IdleLeft"].Frames[0].FrameKey]);
                    sprite.zIndex = 1.1f;
                    sprite.Scale = .2f;
                    sprite.Resize();
                    sprite.Center(position - hero.Offsets["Center"] * sprite.Scale);
                    viewport.Add("sprite" + viewport.Flags["hash"]++, sprite);

                    EffectAgent colorShift = sprite.AddEffect(EffectType.Lighten);
                    colorShift.Parameters["R"] = .2f + i / 10f * 1.6f;
                    colorShift.Parameters["G"] = .2f + j / 6f * 1.6f;
                }
            }

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Color shifts are useful for making things lighter, darker, or shifting color.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1000);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Center(new Vector2(viewport.Width / 2 + viewport.Position.X, 0));
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/colorShift"]);
            codeExample.Position.Y = desc1.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            viewport.Add("bg", bg);

            AnimationSystem hero = AnimationSystems["hero"];
            hero.Load();

            Sprite sprite1 = new Sprite(Textures[hero.Sequences["IdleLeft"].Frames[0].FrameKey]);
            sprite1.Scale = .2f;
            sprite1.Resize();
            sprite1.Center(viewport.GetLocalCenter() - hero.Offsets["Center"] * sprite1.Scale);
            sprite1.Position.X -= 250;
            viewport.Add("sprite1", sprite1);

            Sprite sprite2 = new Sprite(Textures[hero.Sequences["IdleLeft"].Frames[0].FrameKey]);
            sprite2.Scale = .2f;
            sprite2.Resize();
            sprite2.Center(viewport.GetLocalCenter() - hero.Offsets["Center"] * sprite2.Scale);
            viewport.Add("sprite2", sprite2);

            Sprite sprite3 = new Sprite(Textures[hero.Sequences["IdleLeft"].Frames[0].FrameKey]);
            sprite3.Scale = .2f;
            sprite3.Resize();
            sprite3.Center(viewport.GetLocalCenter() - hero.Offsets["Center"] * sprite3.Scale);
            sprite3.Position.X += 250;
            viewport.Add("sprite3", sprite3);

            Character character1 = new Character();
            Character character2 = new Character();
            Character character3 = new Character();
            character1.AnimationSystem = hero.Clone();
            character2.AnimationSystem = hero.Clone();
            character3.AnimationSystem = hero.Clone();
            character1.Sprite = sprite1;
            character2.Sprite = sprite2;
            character3.Sprite = sprite3;
            sprite1.Target = character1;
            sprite2.Target = character2;
            sprite3.Target = character3;
            character1.AnimationSystem.SetSource(character1);
            character2.AnimationSystem.SetSource(character2);
            character3.AnimationSystem.SetSource(character3);

            character1.AnimateIdle();
            character2.AnimateIdle();
            character3.AnimateIdle();

            Text text1 = new Text("Test", Fonts.Arial.Hue(Color.Goldenrod), 64);
            text1.CenterX(sprite1.GetCenter().X + hero.Offsets["Center"].X * sprite1.Scale);
            text1.Position.Y = sprite1.Position.Y + sprite1.Height + 20;
            viewport.Add("text1", text1);

            Text text2 = new Text("Test", Fonts.Arial.Hue(Color.Goldenrod), 64);
            text2.CenterX(sprite2.GetCenter().X + hero.Offsets["Center"].X * sprite2.Scale);
            text2.Position.Y = sprite2.Position.Y + sprite2.Height + 20;
            viewport.Add("text2", text2);

            Text text3 = new Text("Test", Fonts.Arial.Hue(Color.Goldenrod), 64);
            text3.CenterX(sprite3.GetCenter().X + hero.Offsets["Center"].X * sprite3.Scale);
            text3.Position.Y = sprite3.Position.Y + sprite3.Height + 20;
            viewport.Add("text3", text3);

            sprite1.AddStroke(EffectType.Stroke, 2f);
            sprite2.AddStroke(EffectType.Stroke, 2f, .05f, .05f, .8f);
            sprite3.AddStroke(EffectType.Stroke, 2f, .8f, .05f, .05f);

            text1.AddStroke(EffectType.Stroke, 2f);
            text2.AddStroke(EffectType.Stroke, 3f);
            text3.AddStroke(EffectType.Stroke, 4f);

            // draw the info
            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Stroke/Outline is useful to distinguish images from the background.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1000);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.Center(new Vector2(viewport.Width / 2 + viewport.Position.X, 0));
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/stroke"]);
            codeExample.Position.Y = desc1.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            viewport.Add("bg", bg);

            Text desc1 = new Text("Regular text can be hard to read against high contrast/colorful backgrounds.", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            desc1.Position = new Vector2(100, 150);
            viewport.Add("description1", desc1);

            Text desc2 = new Text("We can use stroke to make it pop.", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            desc2.Position = new Vector2(100, 250);
            viewport.Add("description2", desc2);

            Text desc3 = new Text("Additionally you can create a slick looking drop shadow.", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            desc3.Position = new Vector2(100, 350);
            viewport.Add("description3", desc3);

            desc2.AddStroke(EffectType.Stroke, 2f);

            Element shadow = MakeDropShadow(desc3, strokeRadius:1, blurRadius: 4);
            shadow.zIndex = desc3.zIndex - .001f;
            shadow.Position += new Vector2(2, 2);
            shadow.Alpha = .8f;
            viewport.Add("shadow", shadow);

            // draw the info

            Sprite codeExample = new Sprite(Textures["examples/code/dropshadow"]);
            codeExample.Position.Y = viewport.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            viewport.Add("bg", bg);

            // demo 1
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Sprite dagger = new Sprite(Textures["examples/dagger"]);
                    dagger.Fit(48);
                    dagger.Center(new Vector2(100 + i * 80, 60 + j * 80));
                    viewport.Add("dagger" + viewport.Flags["hash"]++, dagger);

                    Box glow = new Box(dagger.Width + 30, dagger.Height + 30, Color.Black);
                    glow.zIndex = dagger.zIndex - .001f;
                    EffectAgent effect = glow.AddEffect(EffectType.GlowSmoke);
                    effect.Parameters["Power"] = j;
                    effect.Parameters["Radius"] = 5 * i;
                    glow.Center(dagger.GetCenter());
                    glow.BlendState = BlendState.AlphaBlend;
                    viewport.Add("daggerglow" + viewport.Flags["hash"]++, glow);
                }
            }

            // demo 2
            Box bigGlow = new Box(500, 500, Color.Black);
            bigGlow.Visible = false;
            bigGlow.Center(viewport.GetLocalCenter());
            EffectAgent effect2 = bigGlow.AddEffect(EffectType.GlowSmoke);
            viewport.Add("bigglow", bigGlow);

            // demo 3
            Sprite desc = new Sprite(Textures["examples/description"]);
            desc.Visible = false;
            desc.Center(viewport.GetLocalCenter());
            viewport.Add("desc", desc);

            Box descGlow = new Box(desc.Width + 150, desc.Height + 150, Color.Black);
            descGlow.Visible = false;
            descGlow.Center(viewport.GetLocalCenter());
            descGlow.zIndex = desc.zIndex - .001f;
            EffectAgent effect3 = descGlow.AddEffect(EffectType.GlowSmoke);
            viewport.Add("descglow", descGlow);

            // draw the info

            Text blendState = new Text("Blendstate:", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            blendState.Position.X = 270;
            blendState.Position.Y = viewport.ScreenBot() + 10;
            exampleScreen.Add("blendstate", blendState);

            // alpha blend
            Button button1 = new Button();
            button1.Flags["active"] = 1;
            Box button1Bg = new Box(140, 60, Col(100, 50, 140)) { Key = "bg" };
            button1.Add(button1Bg);
            Text button1Text = new Text("AlphaBlend", Fonts.K2D.Hue(Col(180, 180, 180))) { Key = "text" };
            button1Text.Center(button1Bg.GetLocalCenter());
            button1.Add(button1Text);
            button1.Position = new Vector2(250, blendState.ScreenBot() + 10);
            exampleScreen.Add("button1", button1);

            // additive
            Button button2 = new Button();
            button2.Flags["active"] = 0;
            Box button2Bg = new Box(140, 60, Col(30, 20, 40)) { Key = "bg" };
            button2.Add(button2Bg);
            Text button2Text = new Text("Additive", Fonts.K2D.Hue(Col(80, 80, 80))) { Key = "text" };
            button2Text.Center(button2Bg.GetLocalCenter());
            button2.Add(button2Text);
            button2.Position = new Vector2(400, blendState.ScreenBot() + 10);
            exampleScreen.Add("button2", button2);

            // non premultiplied
            Button button3 = new Button();
            button3.Flags["active"] = 0;
            Box button3Bg = new Box(140, 60, Col(30, 20, 40)) { Key = "bg" };
            button3.Add(button3Bg);
            Text button3Text = new Text("NonPremux", Fonts.K2D.Hue(Col(80, 80, 80))) { Key = "text" };
            button3Text.Center(button3Bg.GetLocalCenter());
            button3.Add(button3Text);
            button3.Position = new Vector2(550, blendState.ScreenBot() + 10);
            exampleScreen.Add("button3", button3);

            // custom
            Button button4 = new Button();
            button4.Flags["active"] = 0;
            Box button4Bg = new Box(140, 60, Col(30, 20, 40)) { Key = "bg" };
            button4.Add(button4Bg);
            Text button4Text = new Text("Custom", Fonts.K2D.Hue(Col(80, 80, 80))) { Key = "text" };
            button4Text.Center(button4Bg.GetLocalCenter());
            button4.Add(button4Text);
            button4.Position = new Vector2(700, blendState.ScreenBot() + 10);
            exampleScreen.Add("button4", button4);

            button1.Target = BlendState.AlphaBlend;
            button2.Target = BlendState.Additive;
            button3.Target = BlendState.NonPremultiplied;
            button4.Target = BlendState.Opaque;
            button1.ClickHandler = Test_ClickSmokeBlendState;
            button2.ClickHandler = Test_ClickSmokeBlendState;
            button3.ClickHandler = Test_ClickSmokeBlendState;
            button4.ClickHandler = Test_ClickSmokeBlendState;

            // demos

            Text demo = new Text("Demo:", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            demo.Position.X = 1020;
            demo.Position.Y = viewport.ScreenBot() + 10;
            exampleScreen.Add("demo", demo);

            Button button5 = new Button();
            button5.Flags["active"] = 1;
            Box button5Bg = new Box(140, 60, Col(100, 50, 140)) { Key = "bg" };
            button5.Add(button5Bg);
            Text button5Text = new Text("Items", Fonts.K2D.Hue(Col(180, 180, 180))) { Key = "text" };
            button5Text.Center(button5Bg.GetLocalCenter());
            button5.Add(button5Text);
            button5.Position = new Vector2(1000, blendState.ScreenBot() + 10);
            exampleScreen.Add("button5", button5);

            Button button6 = new Button();
            button6.Flags["active"] = 0;
            Box button6Bg = new Box(140, 60, Col(30, 20, 40)) { Key = "bg" };
            button6.Add(button6Bg);
            Text button6Text = new Text("Large", Fonts.K2D.Hue(Col(80, 80, 80))) { Key = "text" };
            button6Text.Center(button6Bg.GetLocalCenter());
            button6.Add(button6Text);
            button6.Position = new Vector2(1150, blendState.ScreenBot() + 10);
            exampleScreen.Add("button6", button6);

            Button button7 = new Button();
            button7.Flags["active"] = 0;
            Box button7Bg = new Box(140, 60, Col(30, 20, 40)) { Key = "bg" };
            button7.Add(button7Bg);
            Text button7Text = new Text("Desc", Fonts.K2D.Hue(Col(80, 80, 80))) { Key = "text" };
            button7Text.Center(button7Bg.GetLocalCenter());
            button7.Add(button7Text);
            button7.Position = new Vector2(1300, blendState.ScreenBot() + 10);
            exampleScreen.Add("button7", button7);

            button5.Target = "items";
            button6.Target = "big";
            button7.Target = "desc";

            button5.ClickHandler = Test_ClickSmokeDemo;
            button6.ClickHandler = Test_ClickSmokeDemo;
            button7.ClickHandler = Test_ClickSmokeDemo;

            exampleScreen.Position = new Vector2(270, 0);

            // alpha

            Text alpha = new Text("Alpha: 1.0", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            alpha.Position.X = 40;
            alpha.Position.Y = 40;
            exampleScreen.Add("alpha", alpha);

            Button alphaButton1 = new Button();
            alphaButton1.ClickHandler = delegate
            {
                List<Element> smokes = viewport.GetElementsByPattern("glow");
                foreach (Element smoke in smokes)
                    smoke.Alpha -= .05f;

                alpha.String = "Alpha: " + smokes[0].Alpha.ToString("0.00");
                alpha.Resize();
                alpha.Render(true);
            };
            Text alphaButton1Text = new Text("-5%", Fonts.K2D.Hue(Col(180, 180, 180)), 18) { Key = "text", zIndex = 1.1f };
            alphaButton1.Add(alphaButton1Text);
            Box alphaButton1Bg = new Box(50, alphaButton1Text.Height + 20,  Col(100, 50, 140)) { Key = "bg" };
            alphaButton1.Add(alphaButton1Bg);
            alphaButton1Text.Center(alphaButton1Bg.GetLocalCenter());
            alphaButton1.Position = new Vector2(30, alpha.ScreenBot() + 10);
            alphaButton1.Resize();
            exampleScreen.Add("alphaButton1", alphaButton1);

            Button alphaButton2 = new Button();
            alphaButton2.ClickHandler = delegate
            {
                List<Element> smokes = viewport.GetElementsByPattern("glow");
                foreach (Element smoke in smokes)
                    smoke.Alpha -= .01f;

                alpha.String = "Alpha: " + smokes[0].Alpha.ToString("0.00");
                alpha.Resize();
                alpha.Render(true);
            };
            Text alphaButton2Text = new Text("-1%", Fonts.K2D.Hue(Col(180, 180, 180)), 18) { Key = "text", zIndex = 1.1f };
            alphaButton2.Add(alphaButton2Text);
            Box alphaButton2Bg = new Box(50, alphaButton2Text.Height + 20, Col(100, 50, 140)) { Key = "bg" };
            alphaButton2.Add(alphaButton2Bg);
            alphaButton2Text.Center(alphaButton2Bg.GetLocalCenter());
            alphaButton2.Position = new Vector2(85, alpha.ScreenBot() + 10);
            alphaButton2.Resize();
            exampleScreen.Add("alphaButton2", alphaButton2);

            Button alphaButton3 = new Button();
            alphaButton3.ClickHandler = delegate
            {
                List<Element> smokes = viewport.GetElementsByPattern("glow");
                foreach (Element smoke in smokes)
                    smoke.Alpha += .01f;

                alpha.String = "Alpha: " + smokes[0].Alpha.ToString("0.00");
                alpha.Resize();
                alpha.Render(true);
            };
            Text alphaButton3Text = new Text("+1%", Fonts.K2D.Hue(Col(180, 180, 180)), 18) { Key = "text", zIndex = 1.1f };
            alphaButton3.Add(alphaButton3Text);
            Box alphaButton3Bg = new Box(50, alphaButton3Text.Height + 20, Col(100, 50, 140)) { Key = "bg" };
            alphaButton3.Add(alphaButton3Bg);
            alphaButton3Text.Center(alphaButton3Bg.GetLocalCenter());
            alphaButton3.Position = new Vector2(140, alpha.ScreenBot() + 10);
            alphaButton3.Resize();
            exampleScreen.Add("alphaButton3", alphaButton3);

            Button alphaButton4 = new Button();
            alphaButton4.ClickHandler = delegate
            {
                List<Element> smokes = viewport.GetElementsByPattern("glow");
                foreach (Element smoke in smokes)
                    smoke.Alpha += .05f;

                alpha.String = "Alpha: " + smokes[0].Alpha.ToString("0.00");
                alpha.Resize();
                alpha.Render(true);
            };
            Text alphaButton4Text = new Text("+5%", Fonts.K2D.Hue(Col(180, 180, 180)), 18) { Key = "text", zIndex = 1.1f };
            alphaButton4.Add(alphaButton4Text);
            Box alphaButton4Bg = new Box(50, alphaButton4Text.Height + 20, Col(100, 50, 140)) { Key = "bg" };
            alphaButton4.Add(alphaButton4Bg);
            alphaButton4Text.Center(alphaButton4Bg.GetLocalCenter());
            alphaButton4.Position = new Vector2(195, alpha.ScreenBot() + 10);
            alphaButton4.Resize();
            exampleScreen.Add("alphaButton4", alphaButton4);

            // radius

            Text radius = new Text("Radius: 40", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            radius.Position.X = 40;
            radius.Position.Y = 140;
            exampleScreen.Add("radius", radius);

            Button radiusButton1 = new Button();
            radiusButton1.ClickHandler = delegate
            {
                EffectAgent effectAgent = bigGlow.GetEffect(EffectType.GlowSmoke);
                effectAgent.Parameters["Radius"] -= 5;
                effectAgent = descGlow.GetEffect(EffectType.GlowSmoke);
                effectAgent.Parameters["Radius"] -= 5;
                radius.String = "Radius: " + effectAgent.Parameters["Radius"];
                radius.Resize();
                radius.Render(true);
            };
            Text radiusButton1Text = new Text("-5", Fonts.K2D.Hue(Col(180, 180, 180)), 18) { Key = "text", zIndex = 1.1f };
            radiusButton1.Add(radiusButton1Text);
            Box radiusButton1Bg = new Box(50, radiusButton1Text.Height + 20, Col(100, 50, 140)) { Key = "bg" };
            radiusButton1.Add(radiusButton1Bg);
            radiusButton1Text.Center(radiusButton1Bg.GetLocalCenter());
            radiusButton1.Position = new Vector2(30, radius.ScreenBot() + 10);
            radiusButton1.Resize();
            exampleScreen.Add("radiusButton1", radiusButton1);

            Button radiusButton2 = new Button();
            radiusButton2.ClickHandler = delegate
            {
                EffectAgent effectAgent = bigGlow.GetEffect(EffectType.GlowSmoke);
                effectAgent.Parameters["Radius"] -= 1;
                effectAgent = descGlow.GetEffect(EffectType.GlowSmoke);
                effectAgent.Parameters["Radius"] -= 1;
                radius.String = "Radius: " + effectAgent.Parameters["Radius"];
                radius.Resize();
                radius.Render(true);
            };
            Text radiusButton2Text = new Text("-1", Fonts.K2D.Hue(Col(180, 180, 180)), 18) { Key = "text", zIndex = 1.1f };
            radiusButton2.Add(radiusButton2Text);
            Box radiusButton2Bg = new Box(50, radiusButton2Text.Height + 20, Col(100, 50, 140)) { Key = "bg" };
            radiusButton2.Add(radiusButton2Bg);
            radiusButton2Text.Center(radiusButton2Bg.GetLocalCenter());
            radiusButton2.Position = new Vector2(85, radius.ScreenBot() + 10);
            radiusButton2.Resize();
            exampleScreen.Add("radiusButton2", radiusButton2);

            Button radiusButton3 = new Button();
            radiusButton3.ClickHandler = delegate
            {
                EffectAgent effectAgent = bigGlow.GetEffect(EffectType.GlowSmoke);
                effectAgent.Parameters["Radius"] += 1;
                effectAgent = descGlow.GetEffect(EffectType.GlowSmoke);
                effectAgent.Parameters["Radius"] += 1;
                radius.String = "Radius: " + effectAgent.Parameters["Radius"];
                radius.Resize();
                radius.Render(true);
            };
            Text radiusButton3Text = new Text("+1", Fonts.K2D.Hue(Col(180, 180, 180)), 18) { Key = "text", zIndex = 1.1f };
            radiusButton3.Add(radiusButton3Text);
            Box radiusButton3Bg = new Box(50, radiusButton3Text.Height + 20, Col(100, 50, 140)) { Key = "bg" };
            radiusButton3.Add(radiusButton3Bg);
            radiusButton3Text.Center(radiusButton3Bg.GetLocalCenter());
            radiusButton3.Position = new Vector2(140, radius.ScreenBot() + 10);
            radiusButton3.Resize();
            exampleScreen.Add("radiusButton3", radiusButton3);

            Button radiusButton4 = new Button();
            radiusButton4.ClickHandler = delegate
            {
                EffectAgent effectAgent = bigGlow.GetEffect(EffectType.GlowSmoke);
                effectAgent.Parameters["Radius"] += 5;
                effectAgent = descGlow.GetEffect(EffectType.GlowSmoke);
                effectAgent.Parameters["Radius"] += 5;
                radius.String = "Radius: " + effectAgent.Parameters["Radius"];
                radius.Resize();
                radius.Render(true);
            };
            Text radiusButton4Text = new Text("+5", Fonts.K2D.Hue(Col(180, 180, 180)), 18) { Key = "text", zIndex = 1.1f };
            radiusButton4.Add(radiusButton4Text);
            Box radiusButton4Bg = new Box(50, radiusButton4Text.Height + 20, Col(100, 50, 140)) { Key = "bg" };
            radiusButton4.Add(radiusButton4Bg);
            radiusButton4Text.Center(radiusButton4Bg.GetLocalCenter());
            radiusButton4.Position = new Vector2(195, radius.ScreenBot() + 10);
            radiusButton4.Resize();
            exampleScreen.Add("radiusButton4", radiusButton4);

            // easing method

            Text easing = new Text("Easing Method: 0", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            easing.Position.X = 40;
            easing.Position.Y = 240;
            exampleScreen.Add("easing", easing);

            for (int i = 0; i < 7; i++)
            {
                Button powerButton = new Button();
                int locali = i;
                powerButton.ClickHandler = delegate
                {
                    EffectAgent effectAgent = bigGlow.GetEffect(EffectType.GlowSmoke);
                    effectAgent.Parameters["Power"] = locali;
                    effectAgent = descGlow.GetEffect(EffectType.GlowSmoke);
                    effectAgent.Parameters["Power"] = locali;
                    easing.String = "Easing Method: " + locali;
                    easing.Resize();
                    easing.Render(true);

                    List<Element> ourButtons = exampleScreen.GetElementsByPattern("powerButton");
                    foreach (Element ourButton in ourButtons)
                    {
                        Button button = ourButton as Button;
                        button.Flags["active"] = 0;
                        Box bg = button.GetChild("bg") as Box;
                        bg.Color = Col(30, 20, 40);
                        bg.Resize();
                        Text text = button.GetChild("text") as Text;
                        text.Color = Col(80, 80, 80);
                        text.Render(true);
                    }

                    powerButton.Flags["active"] = 1;
                    Box inbg = powerButton.GetChild("bg") as Box;
                    inbg.Color = Col(100, 50, 140);
                    inbg.Resize();
                    Text intext = powerButton.GetChild("text") as Text;
                    intext.Color = Col(180, 180, 180);
                    intext.Render(true);
                };
                Text powerButtonText = new Text("Method " + i, Fonts.K2D.Hue(Col(80, 80, 80)), 18) { Key = "text", zIndex = 1.1f };
                powerButton.Add(powerButtonText);
                Box powerButtonBg = new Box(180, powerButtonText.Height + 20, Col(30, 20, 40)) { Key = "bg" };
                powerButton.Add(powerButtonBg);
                powerButtonText.Center(powerButtonBg.GetLocalCenter());
                powerButton.Position = new Vector2(50, 280 + i * 50);
                powerButton.Resize();
                exampleScreen.Add("powerButton" + i, powerButton);
            }

            exampleScreen.Elements["powerButton0"].ClickHandler(exampleScreen.Elements["powerButton0"]);

            // custom blend state

            Text tag1 = new Text("AlphaBlendFunction:", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            tag1.Position.X = 240 - tag1.Width;
            tag1.Position.Y = viewport.ScreenBot() + 120 + 0;
            exampleScreen.Add("customtag1", tag1);

            Text tag2 = new Text("AlphaDestinationBlend:", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            tag2.Position.X = 240 - tag2.Width;
            tag2.Position.Y = viewport.ScreenBot() + 150 + 3;
            exampleScreen.Add("customtag2", tag2);

            Text tag3 = new Text("AlphaSourceBlend:", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            tag3.Position.X = 240 - tag3.Width;
            tag3.Position.Y = viewport.ScreenBot() + 210 + 6;
            exampleScreen.Add("customtag3", tag3);

            Text tag4 = new Text("ColorBlendFunction:", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            tag4.Position.X = 240 - tag4.Width;
            tag4.Position.Y = viewport.ScreenBot() + 270 + 9;
            exampleScreen.Add("customtag4", tag4);

            Text tag5 = new Text("ColorDestinationBlend:", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            tag5.Position.X = 240 - tag5.Width;
            tag5.Position.Y = viewport.ScreenBot() + 300 + 12;
            exampleScreen.Add("customtag5", tag5);

            Text tag6 = new Text("ColorSourceBlend:", Fonts.K2D.Hue(Col(143, 140, 140)), 20);
            tag6.Position.X = 240 - tag6.Width;
            tag6.Position.Y = viewport.ScreenBot() + 360 + 15;
            exampleScreen.Add("customtag6", tag6);

            // AlphaBlendFunction

            List<BlendFunction> blendFunctions = Enum.GetValues(typeof(BlendFunction)).Cast<BlendFunction>().ToList();
            List<Blend> blends = Enum.GetValues(typeof(Blend)).Cast<Blend>().ToList();

            Button lastButton = null;
            for (int i = 0; i < blendFunctions.Count; i++)
            {
                BlendFunction blendFunction = blendFunctions[i];

                Button localButton = new Button();
                localButton.ClickHandler = Test_ClickBlendFunction;
                localButton.Target = blendFunction;
                Text localButtonText = new Text(blendFunction.ToString(), Fonts.K2D.Hue(Col(80, 80, 80)), 12) { Key = "text", zIndex = 1.1f };
                localButton.Add(localButtonText);
                Box localButtonBg = new Box(localButtonText.Width + 20, localButtonText.Height + 10, Col(30, 20, 40)) { Key = "bg" };
                localButton.Add(localButtonBg);
                localButtonText.Center(localButtonBg.GetLocalCenter());
                localButton.Position = new Vector2(1300, blendState.ScreenBot() + 10);
                localButton.Resize();
                exampleScreen.Add("customButton1_" + exampleScreen.Flags["hash"]++, localButton);

                localButton.CenterY(tag1.GetCenter().Y + 1);
                if (lastButton != null)
                    localButton.Position.X = lastButton.Right() + 5;
                else
                    localButton.Position.X = 250;

                lastButton = localButton;

                if (i == 0)
                    Test_ClickBlendFunction(localButton);
            }

            // AlphaDestinaionBlend

            lastButton = null;
            for (int i = 0; i < blends.Count; i++)
            {
                Blend blend = blends[i];

                Button localButton = new Button();
                localButton.ClickHandler = Test_ClickBlendFunction;
                localButton.Target = blend;
                Text localButtonText = new Text(blend.ToString(), Fonts.K2D.Hue(Col(80, 80, 80)), 12) { Key = "text", zIndex = 1.1f };
                localButton.Add(localButtonText);
                Box localButtonBg = new Box(localButtonText.Width + 20, localButtonText.Height + 10, Col(30, 20, 40)) { Key = "bg" };
                localButton.Add(localButtonBg);
                localButtonText.Center(localButtonBg.GetLocalCenter());
                localButton.Position = new Vector2(1300, blendState.ScreenBot() + 10);
                localButton.Resize();
                exampleScreen.Add("customButton2_" + exampleScreen.Flags["hash"]++, localButton);

                localButton.CenterY(tag2.GetCenter().Y + 1);
                if (lastButton != null)
                    localButton.Position.X = lastButton.Right() + 5;
                else
                    localButton.Position.X = 250;

                if (localButton.Right() > exampleScreen.Width)
                {
                    localButton.Position.X = 250;
                    localButton.Position.Y += 30;
                }

                lastButton = localButton;

                if (i == 0)
                    Test_ClickBlendFunction(localButton);
            }

            // AlphaSourceBlend

            lastButton = null;
            for (int i = 0; i < blends.Count; i++)
            {
                Blend blend = blends[i];

                Button localButton = new Button();
                localButton.ClickHandler = Test_ClickBlendFunction;
                localButton.Target = blend;
                Text localButtonText = new Text(blend.ToString(), Fonts.K2D.Hue(Col(80, 80, 80)), 12) { Key = "text", zIndex = 1.1f };
                localButton.Add(localButtonText);
                Box localButtonBg = new Box(localButtonText.Width + 20, localButtonText.Height + 10, Col(30, 20, 40)) { Key = "bg" };
                localButton.Add(localButtonBg);
                localButtonText.Center(localButtonBg.GetLocalCenter());
                localButton.Position = new Vector2(1300, blendState.ScreenBot() + 10);
                localButton.Resize();
                exampleScreen.Add("customButton3_" + exampleScreen.Flags["hash"]++, localButton);

                localButton.CenterY(tag3.GetCenter().Y + 1);
                if (lastButton != null)
                    localButton.Position.X = lastButton.Right() + 5;
                else
                    localButton.Position.X = 250;

                if (localButton.Right() > exampleScreen.Width)
                {
                    localButton.Position.X = 250;
                    localButton.Position.Y += 30;
                }

                lastButton = localButton;

                if (i == 0)
                    Test_ClickBlendFunction(localButton);
            }

            // ColorBlendFunction

            lastButton = null;
            for (int i = 0; i < blendFunctions.Count; i++)
            {
                BlendFunction blendFunction = blendFunctions[i];

                Button localButton = new Button();
                localButton.ClickHandler = Test_ClickBlendFunction;
                localButton.Target = blendFunction;
                Text localButtonText = new Text(blendFunction.ToString(), Fonts.K2D.Hue(Col(80, 80, 80)), 12) { Key = "text", zIndex = 1.1f };
                localButton.Add(localButtonText);
                Box localButtonBg = new Box(localButtonText.Width + 20, localButtonText.Height + 10, Col(30, 20, 40)) { Key = "bg" };
                localButton.Add(localButtonBg);
                localButtonText.Center(localButtonBg.GetLocalCenter());
                localButton.Position = new Vector2(1300, blendState.ScreenBot() + 10);
                localButton.Resize();
                exampleScreen.Add("customButton4_" + exampleScreen.Flags["hash"]++, localButton);

                localButton.CenterY(tag4.GetCenter().Y + 1);
                if (lastButton != null)
                    localButton.Position.X = lastButton.Right() + 5;
                else
                    localButton.Position.X = 250;

                lastButton = localButton;

                if (i == 0)
                    Test_ClickBlendFunction(localButton);
            }

            // ColorDestinationFunction

            lastButton = null;
            for (int i = 0; i < blends.Count; i++)
            {
                Blend blend = blends[i];

                Button localButton = new Button();
                localButton.ClickHandler = Test_ClickBlendFunction;
                localButton.Target = blend;
                Text localButtonText = new Text(blend.ToString(), Fonts.K2D.Hue(Col(80, 80, 80)), 12) { Key = "text", zIndex = 1.1f };
                localButton.Add(localButtonText);
                Box localButtonBg = new Box(localButtonText.Width + 20, localButtonText.Height + 10, Col(30, 20, 40)) { Key = "bg" };
                localButton.Add(localButtonBg);
                localButtonText.Center(localButtonBg.GetLocalCenter());
                localButton.Position = new Vector2(1300, blendState.ScreenBot() + 10);
                localButton.Resize();
                exampleScreen.Add("customButton5_" + exampleScreen.Flags["hash"]++, localButton);

                localButton.CenterY(tag5.GetCenter().Y + 1);
                if (lastButton != null)
                    localButton.Position.X = lastButton.Right() + 5;
                else
                    localButton.Position.X = 250;

                if (localButton.Right() > exampleScreen.Width)
                {
                    localButton.Position.X = 250;
                    localButton.Position.Y += 30;
                }

                lastButton = localButton;

                if (i == 0)
                    Test_ClickBlendFunction(localButton);
            }

            // ColorSourceFunction

            lastButton = null;
            for (int i = 0; i < blends.Count; i++)
            {
                Blend blend = blends[i];

                Button localButton = new Button();
                localButton.ClickHandler = Test_ClickBlendFunction;
                localButton.Target = blend;
                Text localButtonText = new Text(blend.ToString(), Fonts.K2D.Hue(Col(80, 80, 80)), 12) { Key = "text", zIndex = 1.1f };
                localButton.Add(localButtonText);
                Box localButtonBg = new Box(localButtonText.Width + 20, localButtonText.Height + 10, Col(30, 20, 40)) { Key = "bg" };
                localButton.Add(localButtonBg);
                localButtonText.Center(localButtonBg.GetLocalCenter());
                localButton.Position = new Vector2(1300, blendState.ScreenBot() + 10);
                localButton.Resize();
                exampleScreen.Add("customButton6_" + exampleScreen.Flags["hash"]++, localButton);

                localButton.CenterY(tag6.GetCenter().Y + 1);
                if (lastButton != null)
                    localButton.Position.X = lastButton.Right() + 5;
                else
                    localButton.Position.X = 250;

                if (localButton.Right() > exampleScreen.Width)
                {
                    localButton.Position.X = 250;
                    localButton.Position.Y += 30;
                }

                lastButton = localButton;

                if (i == 0)
                    Test_ClickBlendFunction(localButton);
            }

            exampleScreen.HideByPattern("custom");
        }

        void Test_ClickAlphaButton(Element element)
        {
            // update our selected state
            Screen smokeScreen = Screens["SmokeExample"];
            Screen viewport = smokeScreen.Elements["viewport"] as Screen;

            string number = element.Key.Substring(element.Key.Length - 1, 1);
            float alpha = 1 - Convert.ToInt16(number) * .1f;

            List<Element> ourButtons = smokeScreen.GetElementsByPattern("alphaButton");
            foreach (Element ourButton in ourButtons)
            {
                Button button = ourButton as Button;
                button.Flags["active"] = 0;
                Box bg = button.GetChild("bg") as Box;
                bg.Color = Col(30, 20, 40);
                bg.Resize();
                Text text = button.GetChild("text") as Text;
                text.Color = Col(80, 80, 80);
                text.Render(true);
            }

            // make us active
            Button inButton = element as Button;
            inButton.Flags["active"] = 1;
            Box inbg = inButton.GetChild("bg") as Box;
            inbg.Color = Col(100, 50, 140);
            inbg.Resize();
            Text intext = inButton.GetChild("text") as Text;
            intext.Color = Col(180, 180, 180);
            intext.Render(true);

            List<Element> smokes = viewport.GetElementsByPattern("glow");
            foreach (Element smoke in smokes)
                smoke.Alpha = alpha;
        }

        void Test_ClickBlendFunction(Element element)
        {
            // update our selected state
            Screen smokeScreen = Screens["SmokeExample"];

            string key = element.Key;
            string buttonPattern = key.Split('_')[0];

            List<Element> ourButtons = smokeScreen.GetElementsByPattern(buttonPattern);
            foreach (Element ourButton in ourButtons)
            {
                Button button = ourButton as Button;
                button.Flags["active"] = 0;
                Box bg = button.GetChild("bg") as Box;
                bg.Color = Col(30, 20, 40);
                bg.Resize();
                Text text = button.GetChild("text") as Text;
                text.Color = Col(80, 80, 80);
                text.Render(true);
            }

            // make us active
            Button inButton = element as Button;
            inButton.Flags["active"] = 1;
            Box inbg = inButton.GetChild("bg") as Box;
            inbg.Color = Col(100, 50, 140);
            inbg.Resize();
            Text intext = inButton.GetChild("text") as Text;
            intext.Color = Col(180, 180, 180);
            intext.Render(true);

            if (smokeScreen.Elements["button4"].Flags["active"] == 1)
                Test_UpdateCustomBlendState();
        }

        void Test_UpdateCustomBlendState()
        {
            Screen smokeScreen = Screens["SmokeExample"];
            Screen viewport = smokeScreen.Elements["viewport"] as Screen;

            // build the blendstate from all the tags
            BlendState blendState = new BlendState();

            // AlphaBlendFunction
            List<Element> buttons = smokeScreen.GetElementsByPattern("customButton1");
            foreach (Element button in buttons)
            {
                if (button.Flags["active"] == 1)
                {
                    BlendFunction blendFunction = (BlendFunction)button.Target;
                    blendState.AlphaBlendFunction = blendFunction;
                }
            }
            // AlphaDestinationBlend
            buttons = smokeScreen.GetElementsByPattern("customButton2");
            foreach (Element button in buttons)
            {
                if (button.Flags["active"] == 1)
                {
                    Blend blend = (Blend)button.Target;
                    blendState.AlphaDestinationBlend = blend;
                }
            }
            // AlphaSourceBlend
            buttons = smokeScreen.GetElementsByPattern("customButton3");
            foreach (Element button in buttons)
            {
                if (button.Flags["active"] == 1)
                {
                    Blend blend = (Blend)button.Target;
                    blendState.AlphaSourceBlend = blend;
                }
            }
            // ColorBlendFunction
            buttons = smokeScreen.GetElementsByPattern("customButton4");
            foreach (Element button in buttons)
            {
                if (button.Flags["active"] == 1)
                {
                    BlendFunction blendFunction = (BlendFunction)button.Target;
                    blendState.ColorBlendFunction = blendFunction;
                }
            }
            // ColorDestinationBlend
            buttons = smokeScreen.GetElementsByPattern("customButton5");
            foreach (Element button in buttons)
            {
                if (button.Flags["active"] == 1)
                {
                    Blend blend = (Blend)button.Target;
                    blendState.ColorDestinationBlend = blend;
                }
            }
            // ColorSourceBlend
            buttons = smokeScreen.GetElementsByPattern("customButton6");
            foreach (Element button in buttons)
            {
                if (button.Flags["active"] == 1)
                {
                    Blend blend = (Blend)button.Target;
                    blendState.ColorSourceBlend = blend;
                }
            }

            // apply it to the current demo
            List<Element> smokes = viewport.GetElementsByPattern("glow");
            foreach (Element smoke in smokes)
                smoke.BlendState = blendState;
        }

        void Test_ClickSmokeBlendState(Element element)
        {
            Screen smokeScreen = Screens["SmokeExample"];
            Screen viewport = smokeScreen.Elements["viewport"] as Screen;

            // make all the buttons inactive
            for (int i = 1; i < 5; i++)
            {
                Button button = smokeScreen.Elements["button" + i] as Button;
                button.Flags["active"] = 0;
                Box bg = button.GetChild("bg") as Box;
                bg.Color = Col(30, 20, 40);
                bg.Resize();
                Text text = button.GetChild("text") as Text;
                text.Color = Col(80, 80, 80);
                text.Render(true);
            }

            // make us active
            Button inButton = element as Button;
            inButton.Flags["active"] = 1;
            Box inbg = inButton.GetChild("bg") as Box;
            inbg.Color = Col(100, 50, 140);
            inbg.Resize();
            Text intext = inButton.GetChild("text") as Text;
            intext.Color = Col(180, 180, 180);
            intext.Render(true);

            // apply the blendstate to all the demos
            BlendState target = element.Target as BlendState;

            // custom blendstate
            if (target == BlendState.Opaque)
            {
                smokeScreen.ShowByPattern("custom");
                Test_UpdateCustomBlendState();
            }
            else
            {
                smokeScreen.HideByPattern("custom");

                List<Element> smokes = viewport.GetElementsByPattern("glow");
                foreach (Element smoke in smokes)
                    smoke.BlendState = target;
            }
        }

        void Test_ClickSmokeDemo(Element element)
        {
            Screen smokeScreen = Screens["SmokeExample"];
            Screen viewport = smokeScreen.Elements["viewport"] as Screen;

            // make all the buttons inactive
            for (int i = 5; i < 8; i++)
            {
                Button button = smokeScreen.Elements["button" + i] as Button;
                Box bg = button.GetChild("bg") as Box;
                bg.Color = Col(30, 20, 40);
                bg.Resize();
                Text text = button.GetChild("text") as Text;
                text.Color = Col(80, 80, 80);
                text.Render(true);
            }

            // make us active
            Button inButton = element as Button;
            Box inbg = inButton.GetChild("bg") as Box;
            inbg.Color = Col(100, 50, 140);
            inbg.Resize();
            Text intext = inButton.GetChild("text") as Text;
            intext.Color = Col(180, 180, 180);
            intext.Render(true);

            // set the right demo visible
            string target = element.Target as string;
            // hide all the demos
            viewport.HideByPattern("glow");
            viewport.HideByPattern("dagger");
            viewport.Elements["desc"].Visible = false;
            // show the right one
            if (target == "items")
            {
                viewport.ShowByPattern("dagger");
            }
            else if (target == "big")
            {
                viewport.Elements["bigglow"].Visible = true;
            }
            else
            {
                viewport.Elements["desc"].Visible = true;
                viewport.Elements["descglow"].Visible = true;
            }
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

            Box box = new Box(viewport.Width, viewport.Height, Color.Black);
            EffectAgent agent = box.AddEffect(EffectType.Fog);
            agent.Parameters["X"] = viewport.Width / 2;
            agent.Parameters["Y"] = viewport.Height / 2;
            agent.TimeScale = .1f;
            viewport.Add("box", box);

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("This is a shader I found on shadertoy. You can adjust the angle with the center parameter.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" It could also be clouds.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/fog"]);
            codeExample.Position.Y = desc1.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

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

            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            bg.Alpha = .4f;
            viewport.Add("bg", bg);

            Sprite item1 = new Sprite(0, 0, Textures["examples/plate"]);
            item1.Center(viewport.GetLocalCenter());
            item1.Position.X -= 250;
            item1.AddStroke(EffectType.Stroke, 1);
            viewport.Add("item1", item1);

            Sprite item2 = new Sprite(0, 0, Textures["examples/sword"]);
            item2.Center(viewport.GetLocalCenter());
            item2.AddStroke(EffectType.Stroke, 1);
            viewport.Add("item2", item2);

            Sprite item3 = new Sprite(0, 0, Textures["examples/tome"]);
            item3.Center(viewport.GetLocalCenter());
            item3.AddStroke(EffectType.Stroke, 1);
            item3.Position.X += 250;
            viewport.Add("item3", item3);

            AnimateSparkle(item1, .3f);
            AnimateSparkle(item2, .3f);
            AnimateSparkle(item3, .3f);

            Element glow1 = MakeSpriteGlow(item1, 5, 8, Col(215, 170, 50));
            glow1.Alpha = .25f;
            viewport.Add("glow1", glow1);

            Element glow2 = MakeSpriteGlow(item2, 5, 8, Col(215, 170, 50));
            glow2.Alpha = .25f;
            viewport.Add("glow2", glow2);

            Element glow3 = MakeSpriteGlow(item3, 5, 8, Col(215, 170, 50));
            glow3.Alpha = .25f;
            viewport.Add("glow3", glow3);

            item1.MouseOverCheck = true;
            item1.MouseOverHandler = delegate
            {
                viewport.Animations["fade1"] = new Fade(glow1, 200, .95f);
                item1.Flags["sparkleIntensity"] /= 8f;
            };

            item1.MouseLeaveHandler = delegate
            {
                viewport.Animations["fade1"] = new Fade(glow1, 200, .25f);
                item1.Flags["sparkleIntensity"] *= 8f;
            };

            item2.MouseOverCheck = true;
            item2.MouseOverHandler = delegate
            {
                viewport.Animations["fade2"] = new Fade(glow2, 200, .95f);
                item2.Flags["sparkleIntensity"] /= 8f;
            };

            item2.MouseLeaveHandler = delegate
            {
                viewport.Animations["fade2"] = new Fade(glow2, 200, .25f);
                item2.Flags["sparkleIntensity"] *= 8f;
            };

            item3.MouseOverCheck = true;
            item3.MouseOverHandler = delegate
            {
                viewport.Animations["fade3"] = new Fade(glow3, 200, .95f);
                item3.Flags["sparkleIntensity"] /= 8f;
            };

            item3.MouseLeaveHandler = delegate
            {
                viewport.Animations["fade3"] = new Fade(glow3, 200, .25f);
                item3.Flags["sparkleIntensity"] *= 8f;
            };

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Here we use several shaders to make the glowing outline. We color in the image black,", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" we draw a stroke outline, we blur it, then we draw it to a white background to make a mask. ", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            p1.Add(new Phrase(" Then we draw the glow with the resultant mask. We also have a nice sparkle effect. Go ahead, mouse over them.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

            Sprite codeExample = new Sprite(Textures["examples/code/strokeGlow"]);
            codeExample.Position.Y = desc1.ScreenBot() + 60;
            codeExample.CenterX(viewport.GetCenter().X);
            exampleScreen.Add("codeExample", codeExample);

            exampleScreen.Position = new Vector2(270, 0);
        }

        void Test_AddEffectsScreen()
        {
            Screen exampleScreen = new Screen(1920 - 270, 1080) { ClearColor = Col(15, 13, 13), zIndex = 2f };
            Screens["EffectsExample"] = exampleScreen;

            // draw the viewport

            Box viewportBg = new Box(1000, 600, Col(47, 37, 37));
            viewportBg.Center(new Vector2(exampleScreen.Width / 2f, exampleScreen.Height / 2f - 200));
            exampleScreen.Add("viewport_bg", viewportBg);

            Screen viewport = new Screen(viewportBg.Width - 4, viewportBg.Height - 4) { ClearColor = Col(12, 10, 10), zIndex = 1.1f };
            viewport.Center(viewportBg);
            exampleScreen.Add("viewport", viewport);

            // draw the examples
            Sprite bg = new Sprite(0, 0, Textures["examples/battle_background"], zIndex: 0);
            bg.Alpha = .4f;
            viewport.Add("bg", bg);

            // loot beam
            Sprite chestSprite = new Sprite(Textures["examples/chest1"]);
            chestSprite.Position = new Vector2(50, 50);
            viewport.Add("chestSprite", chestSprite);
            Sprite chestSprite2 = new Sprite(Textures["examples/chest2"]);
            chestSprite2.Position = new Vector2(50, 50);
            viewport.Add("chestSprite2", chestSprite2);

            Box lootbeam = new Box(300, 750, Color.Black, 85, -230);
            lootbeam.BlendState = BlendState.Additive;
            //lootbeam.Alpha = .9f;
            lootbeam.Origin = new Vector2(lootbeam.Width / 2, lootbeam.Height / 2);
            lootbeam.Rotation = -(float)Math.PI / 2;
            lootbeam.AddEffect(EffectType.ChestLootBeam);
            viewport.Add("lootbeam", lootbeam);

            lootbeam.zIndex = 1.1f;
            chestSprite2.zIndex = 1.2f;

            // Box lootbeam4 = new Box(0, 0, ToX(200), ToY(750), Color.Black, 12.19f);
            //    lootbeam4.Rotation = -(float)Math.PI / 2f;
            //    EffectAgent chestLootBeam = lootbeam4.AddEffect(EffectType.ChestLootBeam);
            //    lootbeam4.Center(Elements["combat_loot_chest"].GetCenter() + new Vector2(ToX(-300), ToY(415)));
            //    lootbeam4.Key = "combat_loot_beam";
            //    lootbeam4.ScrollKey = "combatChest";
            //    lootbeam4.Visible = false;
            //    Elements[lootbeam4.Key] = lootbeam4;

            // takeAllGlow.Center(takeAllButton.GetCenter() + new Vector2(0, ToY(-4)));

            // takeAllGlow.Key = "combat_loot_takeall_glow";
            // Elements[takeAllGlow.Key] = takeAllGlow;

            // zap

            // level up

            // draw the info

            List<Phrase> p1 = new List<Phrase>();
            p1.Add(new Phrase("Here are some other miscellaneous effects.", Fonts.K2D.Hue(Col(143, 140, 140)), 20));
            Paragraph paragraph1 = new Paragraph(p1, 1200);
            Element desc1 = MakeParagraph(paragraph1);
            desc1.CenterX(viewport.GetCenter().X);
            desc1.Position.Y = viewport.ScreenBot() + 60;
            exampleScreen.Add("description1", desc1);

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

                string pressedString = "Button Pressed: No";
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

                        gold.CenterX(bag.GetCenter().X);
                        gold.Position.X += Random.Next(-15, 15);

                        viewport.Add(gold);

                        exampleScreen.Flags["nextGold"] = MsEllapsed + Random.Next(10, 40);
                    }

                    pressedString = "Button Pressed: Yes";
                }
                if (t1.String != pressedString)
                {
                    t1.String = pressedString;
                    t1.Resize();
                    t1.Render(true);
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
                    slider.Position.X -= slider.GetScreen().AbsPos().X;

                    slider.Position.X = Math.Clamp(slider.Position.X, ruler.Pos().X + 10, ruler.Pos().X + ruler.Width - 20);

                    float percent = (slider.Position.X - ruler.Position.X - 10) / (ruler.Width - 30);

                    volume.String = "Volume: " + percent.ToString("0.0");
                    volume.Resize();
                    Setting.SFXVOLUME = percent;
                    Setting.MUSICVOLUME = percent;

                    TargetCursor = CursorType.Holding;
                }
            }
            // animation example
            if (Screens.ContainsKey("AnimationExample") && Screens["AnimationExample"].Visible)
            {
                Screen exampleScreen = Screens["AnimationExample"];
                Screen viewport = (Screen)exampleScreen.Elements["viewport"];
                Element sprite = viewport.Elements["sprite"];
                Element sprite2 = viewport.Elements["sprite2"];
                Character character = sprite.Target as Character;
                Character character2 = sprite2.Target as Character;

                bool firep1Potion = false;
                bool firep2Potion = false;
                if (viewport.Flags["animating"] == 0)
                {
                    if (viewport.Flags["thrower"] == 0)
                    {
                        firep1Potion = true;
                    }
                    else
                    {
                        firep2Potion = true;
                    }
                }

                if (firep1Potion)
                {
                    viewport.Flags["animating"] = 1;
                    viewport.Flags["thrower"] = 1;

                    character.AnimateAttack(character.Direction, callback:delegate
                    {
                        character.AnimateIdle();
                    });

                    SetTimeout(viewport, delegate
                    {
                        Sprite potionSprite = new Sprite(Textures["examples/potion"]);
                        potionSprite.Origin = new Vector2(potionSprite.Width / 2f, potionSprite.Height / 2f);
                        potionSprite.Center(character.GetEmitter());
                        viewport.Add("potionSprite", potionSprite);
                        // hit him in the head
                        viewport.Animations["itemThrow"] = new ItemThrowAnimation(potionSprite, character2.GetCenter() - new Vector2(0, 70), delegate
                        {
                            viewport.Remove("potionSprite");
                        });

                        SetTimeout(viewport, delegate
                        {
                            character2.AnimateDamage(callback: delegate
                            {
                                character2.AnimateIdle();
                            });
                        }, 940);

                    }, 400);

                    SetTimeout(viewport, delegate
                    {
                        viewport.Flags["animating"] = 0;
                    }, 5400);
                }
                if (firep2Potion)
                {
                    viewport.Flags["animating"] = 1;
                    viewport.Flags["thrower"] = 0;

                    character2.AnimateAttack(character2.Direction, callback: delegate
                    {
                        character2.AnimateIdle();
                    });

                    SetTimeout(viewport, delegate
                    {
                        Sprite potionSprite = new Sprite(Textures["examples/potion"]);
                        potionSprite.Origin = new Vector2(potionSprite.Width / 2f, potionSprite.Height / 2f);
                        potionSprite.Center(character2.GetEmitter());
                        viewport.Add("potionSprite", potionSprite);
                        // hit him in the head
                        Animation ithrow = new ItemThrowAnimation(potionSprite, character.GetCenter() - new Vector2(0, 70), delegate
                        {
                            viewport.Remove("potionSprite");
                        });
                        viewport.Animations["itemThrow"] = ithrow;

                        SetTimeout(viewport, delegate
                        {
                            character.AnimateDamage(callback: delegate
                            {
                                character.AnimateIdle();
                            });
                        }, 940);
                    }, 400);

                    SetTimeout(viewport, delegate
                    {
                        viewport.Flags["animating"] = 0;
                    }, 5400);
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

                Vector2 animOffset = Vector2.Zero;

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
                        // get position with current animation offsets
                        Vector2 position = character.Sprite.Pos();
                        // remove animations
                        character.AnimateIdle();
                        character.Sprite.Position = position; 
                    }

                    if (Attack)
                    {
                        character.AnimateAttack(character.Direction, callback:delegate
                        {
                            character.AnimateIdle();
                        });
                    }

                    if (Moving && !character.Moving)
                    {
                        character.AnimateWalking(MovingDirection, 2000);
                    }
                }

                if (character.Sprite.Animations.Count > 0)
                {
                    if (character.Sprite.Animations[0] is WalkAnimation walk)
                    {
                        animOffset = walk.Offset;
                        Text debug = new Text(animOffset.ToString(), Fonts.Catamaran, 14);
                        debug.Position = character.Sprite.Pos();
                        debug.Position.Y += character.Sprite.Height;
                        viewport.Add("debug", debug);
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
