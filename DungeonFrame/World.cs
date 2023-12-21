﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame
{
    public class World
    {

        public int X, Y;
        public int[,] Tiles;

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