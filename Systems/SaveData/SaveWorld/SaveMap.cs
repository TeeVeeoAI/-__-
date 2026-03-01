using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ____.World;

namespace ____.Systems.SaveData.SaveWorld
{
    public static class SaveMap
    {
        public static void Save(Map map, string filePath = null)
        {
            string resolvedPath = ResolvePath(filePath);
            string directory = Path.GetDirectoryName(resolvedPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            List<TileData> tiles = new();
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Tile tile = map.GetTile(x, y);
                    if (tile == null)
                    {
                        continue;
                    }

                    tiles.Add(new TileData
                    {
                        X = tile.X,
                        Y = tile.Y,
                        Type = tile.Type.ToString()
                    });
                }
            }

            MapFileData fileData = new()
            {
                Map = new MapData
                {
                    Width = map.Width,
                    Height = map.Height,
                    TileWidth = map.TileWidth,
                    TileHeight = map.TileHeight,
                    Tiles = tiles
                }
            };

            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(fileData, options);
            File.WriteAllText(resolvedPath, json);
        }

        private static string ResolvePath(string filePath)
        {
            string relativePath = string.IsNullOrWhiteSpace(filePath)
                ? Path.Combine("SavedData", "World", "Map.json")
                : filePath;

            return Path.GetFullPath(relativePath);
        }

        private sealed class MapFileData
        {
            public MapData Map { get; set; }
        }

        private sealed class MapData
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int TileWidth { get; set; }
            public int TileHeight { get; set; }
            public List<TileData> Tiles { get; set; }
        }

        private sealed class TileData
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string Type { get; set; }
        }
    }
}
