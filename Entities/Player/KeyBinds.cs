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
        public string Up { get; set; }
        public string Down { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
        public string Sprint { get; set; }
        public string Dash { get; set; }
        public MovementKeys(string up, string down, string left, string right, string sprint, string dash)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            Sprint = sprint;
            Dash = dash;
        }
    }

    public class CombatKeys
    {
        public string Attack { get; set; }
        public CombatKeys(string attack)
        {
            Attack = attack;
        }
    }

    public static class DefaultKeyBinds
    {
        public static KeyBinds GetDefault()
        {
            return new KeyBinds
            (
                new MovementKeys
                (
                    Keys.W.ToString(),
                    Keys.S.ToString(),
                    Keys.A.ToString(),
                    Keys.D.ToString(),
                    Keys.LeftShift.ToString(),
                    Keys.Space.ToString()
                ),
                new CombatKeys
                (
                    "MouseLeft"
                )
            );
        }
    }
}