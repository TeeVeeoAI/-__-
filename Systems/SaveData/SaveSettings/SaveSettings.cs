using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ____.Systems.LoadData.LoadSettings;

namespace ____.Systems.SaveData.SaveSettings
{
    public class SaveSettings
    {
        public static void Save(Settings settings, string filePath = null) // Class Settings is in LoadSettings because.
        {
            string resolvedPath = ResolvePath(filePath);

            // Ensure directory exists
            string directory = Path.GetDirectoryName(resolvedPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(resolvedPath, json);
        }
        
        private static string ResolvePath(string filePath)
        {
            return string.IsNullOrWhiteSpace(filePath)
                ? Path.Combine("SavedData", "Settings", "Settings.json")
                : filePath;
        }
    }
}