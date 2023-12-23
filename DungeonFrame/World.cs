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

        public World(TileAtlas tileAtlas)
        {
            _tileAtlas = tileAtlas;
            Chunks = new Dictionary<(int, int), Chunk>();
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
                Chunks[(x, y)] = new Chunk(x, y);
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
