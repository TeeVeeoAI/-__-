using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ____.GameStates.Items
{
    public class MenuItem
    {
        private string text;
        private bool isSelected;
        private Rectangle bounds;

        public Rectangle Bounds{ get => bounds; }
        public MenuItem(string text, Rectangle bounds)
        {
            this.text = text;
            isSelected = false;
            this.bounds = bounds;
        }
    }
}