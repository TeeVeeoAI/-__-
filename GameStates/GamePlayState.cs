using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HLG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ____.GameStates
{
    public class GamePlayState : GameState
    {
        private Camera2D camera;

        public GamePlayState(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            BGcolor = Color.Green;
            camera = new Camera2D(graphicsDevice);
        }
        public override void LoadContent()
        {
            font = contentManager.Load<SpriteFont>("Fonts/DefaultFont");
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Gameplay-specific update logic here
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Begin(transformMatrix: camera.Get_transformation());
            spriteBatch.DrawString(font, "Gameplay State", new Vector2(100, 100), Color.White);
            spriteBatch.End();
        }
    }
}