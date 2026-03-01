using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using ____.World;

namespace ____.Systems.LoadData.LoadWorld
{
    public static class LoadMap
    {
        public static Map Load(string filePath = null)
        {
            string resolvedPath = ResolvePath(filePath);

            if (!File.Exists(resolvedPath))
            {
                return new Map(10, 10);
            }

            string json = File.ReadAllText(resolvedPath);
            MapFileData fileData = JsonSerializer.Deserialize<MapFileData>(json);

            if (fileData == null || fileData.Map == null)
            {
                return new Map(10, 10);
            }

            MapData data = fileData.Map;
            Map map = new(data.Width, data.Height, data.TileWidth, data.TileHeight);

            if (data.Tiles != null)
            {
                foreach (TileData tileData in data.Tiles)
                {
                    if (tileData.X < 0 || tileData.X >= data.Width || tileData.Y < 0 || tileData.Y >= data.Height)
                    {
                        continue;
                    }

                    TileType type = TileType.Grass;
                    if (!string.IsNullOrWhiteSpace(tileData.Type))
                    {
                        Enum.TryParse(tileData.Type, true, out type);
                    }

                    map.SetTile(tileData.X, tileData.Y, new Tile(tileData.X, tileData.Y, type));
                }
            }

            return map;
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
            public int TileWidth { get; set; } = 640;
            public int TileHeight { get; set; } = 640;
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
