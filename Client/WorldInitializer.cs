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
            FastNoiseLite noise = new FastNoiseLite();

            // Initialisiere alle Tiles im Chunk mit der DefaultTileId
            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int y = 0; y < Chunk.Height; y++)
                {
                    var noiseValue = noise.GetNoise(Chunk.Width * chunkX + x, Chunk.Height * chunkY + y);
                    var tile = DefaultTileId;
                    // Je nach Noise-Wert das passende Tile setzen
                    if (noiseValue >= 0.75f)
                    {
                        tile = 0; // Wasser
                    }
                    else if (noiseValue >= 0.5f)
                    {
                        tile = 1; // Gras
                    }
                    else if (noiseValue >= 0.25f)
                    {
                        tile = 2; // Erde
                    }
                    else
                    {
                        tile = 3; // Stein
                    }
                    chunk.Tiles[x, y] = tile;
                }
            }

            // Speichere den initialisierten Chunk in der Welt
            world.SaveChunk(chunk.X, chunk.Y, chunk);
        }
    }

}
