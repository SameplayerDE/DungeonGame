using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DungeonFrame
{
    public class World
    {
        private TileAtlas _tileAtlas;
        public Dictionary<(int, int), Chunk> Chunks;
        public int ChunkSize;

        private static string WorldDataPath = "WorldData";

        public static void EnsureWorldDataFolderExists()
        {
            if (!Directory.Exists(WorldDataPath))
            {
                Directory.CreateDirectory(WorldDataPath);
            }
        }

        static World()
        {
            EnsureWorldDataFolderExists();
        }

        public World(int chunkSize, TileAtlas tileAtlas)
        {
            _tileAtlas = tileAtlas;
            Chunks = new Dictionary<(int, int), Chunk>();
            ChunkSize = chunkSize;
        }

        public Tile GetTile(int x, int y)
        {
            int chunkX = x / ChunkSize;
            int chunkY = y / ChunkSize;
            int localX = x % ChunkSize;
            int localY = y % ChunkSize;

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
            int chunkX = x / ChunkSize;
            int chunkY = y / ChunkSize;
            int localX = x % ChunkSize;
            int localY = y % ChunkSize;

            var chunkKey = (chunkX, chunkY);
            if (!Chunks.ContainsKey(chunkKey))
            {
                LoadChunk(chunkX, chunkY);
            }

            Chunks[chunkKey].Tiles[localX, localY] = tileId;
        }

        public void LoadChunk(int x, int y)
        {
            string filePath = GetChunkFilePath(x, y);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Chunk chunk = JsonSerializer.Deserialize<Chunk>(json);
                Chunks[(x, y)] = chunk;
            }
            else
            {
                Chunks[(x, y)] = new Chunk(x, y, ChunkSize, ChunkSize);
            }
        }

        public void SaveChunk(int x, int y)
        {
            if (Chunks.ContainsKey((x, y)))
            {
                string json = JsonSerializer.Serialize(Chunks[(x, y)]);
                string filePath = GetChunkFilePath(x, y);
                File.WriteAllText(filePath, json);
            }
        }

        public void SaveChunk(int x, int y, Chunk chunk)
        {
            string json = JsonSerializer.Serialize(chunk);
            string filePath = GetChunkFilePath(x, y);
            File.WriteAllText(filePath, json);
        }

        public void UnloadChunk(int x, int y)
        {
            SaveChunk(x, y);
            Chunks.Remove((x, y));
        }

        private string GetChunkFilePath(int x, int y)
        {
            return $"WorldData/chunk_{x}_{y}.json";
        }
    }
}
