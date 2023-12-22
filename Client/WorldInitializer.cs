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
        private const int ChunkSize = 16; // Größe eines Chunks (16x16 Tiles)
        private const int NumberOfChunks = 5; // Anzahl der Chunks in jeder Richtung (x und y)
        private const int DefaultTileId = 1; // Standard-Tile-ID, die für die Initialisierung verwendet wird

        public static World CreateWorldWithChunks(TileAtlas tileAtlas)
        {
            World world = new World(ChunkSize, tileAtlas);

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
            Chunk chunk = new Chunk(chunkX, chunkY, ChunkSize, ChunkSize);

            // Initialisiere alle Tiles im Chunk mit der DefaultTileId
            for (int x = 0; x < ChunkSize; x++)
            {
                for (int y = 0; y < ChunkSize; y++)
                {
                    chunk.Tiles[x, y] = DefaultTileId;
                }
            }

            // Speichere den initialisierten Chunk in der Welt
            world.SaveChunk(chunk.X, chunk.Y, chunk);
        }
    }

}
