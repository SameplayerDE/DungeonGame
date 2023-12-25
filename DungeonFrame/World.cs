using Microsoft.Xna.Framework.Input;
using QColonFrame;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace DungeonFrame
{
    public class World
    {
        private TileAtlas _tileAtlas;
        public ConcurrentDictionary<(int, int), Chunk> Chunks;

        //DEMO
        public int Seed = 101199;
        public float NoiseScale = 0.4f;
        public QCTerrainGenerator NoiseGenerator;
        //DEMO

        public static string WorldDataPath = "WorldData";

        public static void EnsureWorldDataFolderExists()
        {
            if (!Directory.Exists(WorldDataPath))
            {
                Directory.CreateDirectory(WorldDataPath);
            }
        }

        static World()
        {
            ////DEBUG
            if (Directory.Exists(WorldDataPath))
            {
                Directory.Delete(WorldDataPath, true);
            }
            ////DEBUG
            //
            EnsureWorldDataFolderExists();
        }

        public World(TileAtlas tileAtlas)
        {
            NoiseGenerator = new QCTerrainGenerator(Seed);
            _tileAtlas = tileAtlas;
            Chunks = new ConcurrentDictionary<(int, int), Chunk>();
        }

        public Tile GetTile(int x, int y)
        {
            int chunkX = x / Chunk.Width;
            int chunkY = y / Chunk.Height;
            int localX = x % Chunk.Width;
            int localY = y % Chunk.Height;

            var chunkKey = (chunkX, chunkY);
            if (!Chunks.ContainsKey(chunkKey))
            {
               LoadChunk(chunkX, chunkY);
            }

            int id = Chunks[chunkKey].Tiles[localX, localY];
            return _tileAtlas.GetTile(id);
        }

        public Tile GetTileById(int id)
        {
            return _tileAtlas.GetTile(id);
        }

        public void SetTile(int x, int y, int tileId)
        {
            int chunkX = x / Chunk.Width;
            int chunkY = y / Chunk.Height;
            int localX = x % Chunk.Width;
            int localY = y % Chunk.Height;

            var chunkKey = (chunkX, chunkY);
            if (!Chunks.ContainsKey(chunkKey))
            {
                LoadChunk(chunkX, chunkY);
            }

            Chunks[chunkKey].Tiles[localX, localY] = tileId;
        }

        public async Task LoadChunkAsync(int x, int y)
        {
            await Task.Run(() =>
            {
                LoadChunk(x, y);
            });
        }

        public void LoadChunk(int x, int y)
        {
            string filePath = GetChunkFilePath(x, y);
            Chunk chunk;
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                chunk = JsonSerializer.Deserialize<Chunk>(json);
            }
            else
            {
                chunk = new Chunk(x, y);
                PopulateCunk(chunk);
            }
            Chunks[(x, y)] = chunk;
            chunk.IsLoaded = true;
        }

        private void PopulateCunk(Chunk chunk)
        {
            // Initialisiere alle Tiles im Chunk mit der DefaultTileId
            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int y = 0; y < Chunk.Height; y++)
                {
                   // var noiseValue = NoiseGenerator.GetNoise((Chunk.Width * chunk.X + x) * NoiseScale, (Chunk.Height * chunk.Y + y) * NoiseScale, 10, 2, 0.5f) * -1;
                    var noiseValue = NoiseGenerator.GetNoise((Chunk.Width * chunk.X + x), (Chunk.Height * chunk.Y + y), QCTerrainGenerator.TerrainType.MOUNTAIN);
                    var tile = 0;
                    // Je nach Noise-Wert das passende Tile setzen
                    if (noiseValue >= 0.75f)
                    {
                        tile = 3; // Stein
                    }
                    else if (noiseValue >= 0.5f)
                    {
                        tile = 2; // ERDE
                    }
                    else if (noiseValue >= 0.25f)
                    {
                        tile = 1; // GRAS
                    }
                    else
                    {
                        tile = 0; // WASSER
                    }
                    //chunk.Tiles[x, y] = (int)(noiseValue * 100f);
                    chunk.Tiles[x, y] = tile;
                }
            }
        }

        public async Task SaveChunkAsync(int x, int y)
        {
            await Task.Run(() =>
            {
                SaveChunk(x, y);
            });
        }

        public void SaveChunk(int x, int y)
        {
            if (Chunks.ContainsKey((x, y)))
            {
                Chunks[(x, y)].IsLoaded = false;
                string json = JsonSerializer.Serialize(Chunks[(x, y)]);
                string filePath = GetChunkFilePath(x, y);
                File.WriteAllText(filePath, json);
            }
        }

        public async Task SaveChunkAsync(int x, int y, Chunk chunk)
        {
            await Task.Run(() =>
            {
                SaveChunk(x, y, chunk);
            });
        }

        public void SaveChunk(int x, int y, Chunk chunk)
        {
            string json = JsonSerializer.Serialize(chunk);
            string filePath = GetChunkFilePath(x, y);
            File.WriteAllText(filePath, json);
        }

        public async Task UnloadChunkAsync(int x, int y)
        {
            await Task.Run(() =>
            {
                UnloadChunk(x, y);
            });
        }

        public void UnloadChunk(int x, int y)
        {
            (int x, int y) chunkKey = (x, y);

            if (Chunks.TryRemove(chunkKey, out var chunk))
            {
                SaveChunk(x, y, chunk);
            }
        }

        private string GetChunkFilePath(int x, int y)
        {
            return $"WorldData/chunk_{x}_{y}.json";
        }
    }
}
