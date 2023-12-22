using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame
{
    public class World
    {

        private TileAtlas _tileAtlas;
        public int Width, Height;
        public int X, Y;
        public int[,] Tiles;

        public World(int width, int height, TileAtlas tileAtlas)
        {
            Width = width;
            Height = height;
            Tiles = new int[Width, Height];
            _tileAtlas = tileAtlas;
        }

        public int Get(int x, int y)
        {
            return Tiles[x, y];
        }

        public int Set(int x, int y, int value)
        {
            return Tiles[x, y] = value;
        }

    }
}
