using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ____.Systems;

namespace ____.GameStates
{
    public abstract class GameState
    {
        protected Game1 game1;
        protected GraphicsDevice graphicsDevice;
        protected ContentManager contentManager;
        protected SpriteFont font;
        protected Texture2D pixel;
        protected bool starting = true;
        protected Color BGcolor;

        public GameState(Game1 game, GraphicsDevice graphics, ContentManager content)
        {
            game1 = game;
            graphicsDevice = graphics;
            contentManager = content;
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            InputSystem.Initialize();
        }

        public virtual void LoadContent()
        {
            game1.SetBGColor(BGcolor);
        }
        public virtual void Update(GameTime gameTime)
        {
            InputSystem.Update();
            if (InputSystem.IsKeyPressed(Keys.Escape))
            {
                game1.Exit();
            }
        }
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}