using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ____.Entities.Player
{
    public class KeyBinds
    {
        public MovementKeys Movement { get; set; }
        public CombatKeys Combat { get; set; }
    }

    public class MovementKeys
    {
        public Keys Up { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
    }

    public class CombatKeys
    {
        public Keys Attack { get; set; }
    }
}