using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public enum TileType { Grass, Mountain, Water }
        public class MapTile
        {
            public TileType Type;
            public Object Entity;
        }
    }
}
