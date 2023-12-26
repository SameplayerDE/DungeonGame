using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame
{
    public class Tile
    {
        public int SourceX { get; set; } = 0; //where the tile is in the tileset
        public int SourceY { get; set; } = 0; //where the tile is in the tileset
        public int SourceL { get; set; } = 32; //where the tile is in the tileset
        public int SourceB { get; set; } = 64; //where the tile is in the tileset
        public int BaseXOffset { get; set; } = 0; //where the base of the tile starts, offset
        public int BaseYOffset { get; set; } = 0; //where the base of the tile starts, offset

        public int Id { get; set; } //the id of the tile
        public int Depth { get; set; } //the old depth
        public bool IsWalkable { get; set; } //never used

        public Tile() { }

        public Tile(int id, int depth, bool isWalkable)
        {
            Id = id;
            Depth = depth;
            IsWalkable = isWalkable;
        }
    }
}
