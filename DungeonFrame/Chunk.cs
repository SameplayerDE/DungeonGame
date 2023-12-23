using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DungeonFrame
{
    public class Chunk
    {
        public static int Width { get; set; } = 16;
        public static int Height { get; set; } = 16;
        public int X { get; set; }
        public int Y { get; set; }
        [JsonIgnore]
        public int[,] Tiles { get; private set; }

        // Eindimensionales Array für die Serialisierung
        public int[] SerializableTiles
        {
            get => FlattenArray(Tiles);
            set => Tiles = UnflattenArray(value, Width, Height);
        }

        public Chunk(int x, int y)
        {
            X = x;
            Y = y;
            Tiles = new int[Width, Height];
        }

        // Wandelt ein 2D-Array in ein 1D-Array um
        private static int[] FlattenArray(int[,] array)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            int[] flatArray = new int[rows * columns];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    flatArray[j * columns + i] = array[j, i];
                }
            }
            return flatArray;
        }

        // Wandelt ein 1D-Array in ein 2D-Array um
        private static int[,] UnflattenArray(int[] flatArray, int width, int height)
        {
            int[,] array = new int[height, width];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    array[j, i] = flatArray[j * width + i];
                }
            }
            return array;
        }

        public Chunk() { }
    }
}
