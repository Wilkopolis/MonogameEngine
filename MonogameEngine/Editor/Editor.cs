using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MonogameEngine.MonogameEngine;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        void Editor_SwitchToEditor()
        {
            RemoveAllElements();

            SetFlag("InEditor", true);

            Editor_Init();
        }

        void Editor_Update(float delta)
        {

        }

        void Editor_KeyboardHandler()
        {

        }

        //                        //
        //  Screen Specific Code  //
        //                        //

        void Editor_Init()
        {

        }
    }
}
