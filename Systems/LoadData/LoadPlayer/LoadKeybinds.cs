using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ____.Entities.Player;

namespace ____.Systems.LoadData.LoadPlayer
{
    public static class LoadKeybinds
    {
        public static KeyBinds Load(string filePath = null)
        {
            string resolvedPath = ResolvePath(filePath);

            if (!File.Exists(resolvedPath))
            {
                return DefaultKeyBinds.GetDefault();
            }

            string json = File.ReadAllText(resolvedPath);
            KeyBinds fileData = JsonSerializer.Deserialize<KeyBinds>(json);

            return fileData ?? DefaultKeyBinds.GetDefault();
        }

        private static string ResolvePath(string filePath)
        {
            string relativePath = string.IsNullOrWhiteSpace(filePath)
                ? Path.Combine("SavedData", "Player", "KeyBinds.json")
                : filePath;

            return Path.GetFullPath(relativePath);
        }
    }
}