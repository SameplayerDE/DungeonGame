using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame
{
    public class Tile
    {

        public int Id { get; set; }
        public int Depth { get; set; }
        public bool IsWalkable { get; set; }

        public Tile() { }

        public Tile(int id, int depth, bool isWalkable)
        {
            Id = id;
            Depth = depth;
            IsWalkable = isWalkable;
        }
    }
}
