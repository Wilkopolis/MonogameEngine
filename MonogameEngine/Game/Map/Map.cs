using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum MapType { World, Mission1 };
        public class Map
        {
            public MapType Type;

            public Dictionary<string, MapTile> Tiles;
        }


    }
}
