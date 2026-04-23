using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ____.Entities.Player;

namespace ____.Systems.SaveData.SavePlayer
{
    public static class SaveKeybinds
    {
        public static void Save(KeyBinds keyBinds, string filePath = null)
        {
            string resolvedPath = ResolvePath(filePath);

            // Ensure directory exists
            string directory = Path.GetDirectoryName(resolvedPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(keyBinds, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(resolvedPath, json);
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
