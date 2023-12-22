using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DungeonFrame
{
    public class TileAtlas
    {
        private Dictionary<int, Tile> _tiles = new Dictionary<int, Tile>();

        public static TileAtlas LoadTilesFromJson(string filePath)
        {
            var result = new TileAtlas();
            string json = File.ReadAllText(filePath);
            List<Tile> tiles = JsonSerializer.Deserialize<List<Tile>>(json);

            foreach (var tile in tiles)
            {
                result.AddTile(tile);
            }
            return result;
        }

        public Tile GetTile(int id)
        {
            return _tiles.TryGetValue(id, out Tile tile) ? tile : null;
        }

        public void AddTile(Tile tile)
        {
            _tiles[tile.Id] = tile;
        }
    }
}
