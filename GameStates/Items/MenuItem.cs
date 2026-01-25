using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ____.GameStates.Items
{
    public class MenuItem
    {
        private string text;
        private bool isSelected;
        private Rectangle bounds;
        private Color normalColor = Color.White;
        private Color selectedColor = Color.Yellow;

        public string Text { get => text; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }
        public Rectangle Bounds{ get => bounds; }
        public MenuItem(string text, Rectangle bounds)
        {
            this.text = text;
            isSelected = false;
            this.bounds = bounds;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Color drawColor = isSelected ? selectedColor : normalColor;
            Vector2 textSize = font.MeasureString(text);
            Vector2 position = new Vector2(
                bounds.X + (bounds.Width - textSize.X) / 2,
                bounds.Y + (bounds.Height - textSize.Y) / 2
            );
            spriteBatch.DrawString(font, text, position, drawColor);
        }
    }
}