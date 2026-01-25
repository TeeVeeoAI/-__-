using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ____.GameStates
{
    public class SettingsState : GameState
    {
        public SettingsState(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            BGcolor = Color.Gray;
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
            spriteBatch.DrawString(font, "Settings Menu", new Vector2(100, 100), Color.White);
            spriteBatch.End();
        }
    }
}