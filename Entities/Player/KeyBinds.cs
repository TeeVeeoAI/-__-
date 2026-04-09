using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ____.Entities.Player
{
    public class KeyBinds
    {
        public MovementKeys Movement { get; set; }
        public CombatKeys Combat { get; set; }
        public KeyBinds(MovementKeys movement, CombatKeys combat)
        {
            Movement = movement;
            Combat = combat;
        }
    }

    public class MovementKeys
    {
        public Keys Up { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Sprint { get; set; }
        public Keys Dash { get; set; }
        public MovementKeys(Keys up, Keys down, Keys left, Keys right, Keys sprint, Keys dash)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;

        }
    }

    public class CombatKeys
    {
        public Keys Attack { get; set; }
        public CombatKeys(Keys attack)
        {
            Attack = attack;
        }
    }

    public class KeyBindsLoader
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

    public static class DefaultKeyBinds
    {
        public static KeyBinds GetDefault()
        {
            return new KeyBinds
            {
                Movement = new MovementKeys
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D
                },
                Combat = new CombatKeys
                {
                    Attack = Keys.Space
                }
            };
        }
    }
}