using DungeonFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WorldInitializer
    {
        private const int NumberOfChunks = 5; // Anzahl der Chunks in jeder Richtung (x und y)
        private const int DefaultTileId = 1; // Standard-Tile-ID, die für die Initialisierung verwendet wird

        public static World CreateWorldWithChunks(TileAtlas tileAtlas)
        {
            World world = new World(tileAtlas);

            // Erstelle und initialisiere Chunks
            for (int chunkX = 0; chunkX < NumberOfChunks; chunkX++)
            {
                for (int chunkY = 0; chunkY < NumberOfChunks; chunkY++)
                {
                    CreateAndInitializeChunk(world, chunkX, chunkY);
                }
            }

            return world;
        }

        private static void CreateAndInitializeChunk(World world, int chunkX, int chunkY)
        {
            Chunk chunk = new Chunk(chunkX, chunkY);

            // Initialisiere alle Tiles im Chunk mit der DefaultTileId
            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int y = 0; y < Chunk.Height; y++)
                {
                    chunk.Tiles[x, y] = DefaultTileId;
                }
            }

            // Speichere den initialisierten Chunk in der Welt
            world.SaveChunk(chunk.X, chunk.Y, chunk);
        }
    }

}
