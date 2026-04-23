using System.Collections.Generic;
using ____.GameStates.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ____.GameStates
{
    public class SettingsState : GameState
    {
        private List<SettingsItem> settingsItems; // Using a list for the same reason as in MenuState.
        public SettingsState(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            BGcolor = Color.Gray;
            settingsItems = new List<SettingsItem> {
                new("Keybinds", new (100, 150, 200, 50)),
                new("Movement Keys", new (120, 220, 200, 50)),
                new("Attack Keys", new (120, 290, 200, 50)),
                new("Exit", new (100, 360, 200, 50))
            };
        }

        public override void LoadContent()
        {
            font = contentManager.Load<SpriteFont>("Fonts/DefaultFont");
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Settings-specific update logic here
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Settings Menu", new Vector2(100, 100) + font.MeasureString("Settings Menu"), Color.White);
            foreach (var item in settingsItems)
            {
                item.Draw(spriteBatch, font);
            }

            spriteBatch.End();
        }
    }
}