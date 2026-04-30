using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ____.Systems.LoadData.LoadSettings
{
    public class LoadSettings
    {
        public static Settings Load(string filePath = null)
        {
            string resolvedPath = ResolvePath(filePath);

            if (!File.Exists(resolvedPath))
            {
                return new Settings
                {
                    ScreenSize = new ScreenSize { Width = 1920, Height = 1080 },
                    Fullscreen = false,
                    VSync = false,
                    ShowFps = true
                };
            }

            string json = File.ReadAllText(resolvedPath);
            var wrapper = JsonSerializer.Deserialize<SettingsFile>(json);
            return wrapper.Settings;
        }
        
        private static string ResolvePath(string filePath)
        {
            string relativePath = string.IsNullOrWhiteSpace(filePath)
                ? Path.Combine("SavedData", "Settings", "Settings.json")
                : filePath;
            return Path.GetFullPath(relativePath);
        }
    }

    public class SettingsFile
    {
        [JsonPropertyName("Settings")]
        public Settings Settings { get; set; }
    }

    public class Settings
    {
        [JsonPropertyName("ScreenSize")]
        public ScreenSize ScreenSize { get; set; }
        [JsonPropertyName("Fullscreen")]
        public bool Fullscreen { get; set; }
        [JsonPropertyName("VSync")]
        public bool VSync { get; set; }
        [JsonPropertyName("ShowFps")]
        public bool ShowFps { get; set; }
    }

    public class ScreenSize
    {
        [JsonPropertyName("Width")]
        public int Width { get; set; }
        [JsonPropertyName("Height")]
        public int Height { get; set; }

        public Point ToPoint()
        {
            return new Point(Width, Height);
        }
    }
}