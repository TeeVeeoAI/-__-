using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ____.Systems;
using System;

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
        public Color BGcolor;

        public GameState(Game1 game, GraphicsDevice graphics, ContentManager content)
        {
            Console.WriteLine("GamePlayState initialized.");
            game1 = game;
            graphicsDevice = graphics;
            contentManager = content;
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            InputSystem.Initialize();
        }

        public virtual void LoadContent()
        {
            
        }
        public virtual void Update(GameTime gameTime)
        {
            InputSystem.Update();
            if (starting)
            {
                starting = false;
                return; // Skip the first update to avoid input issues
            }
            if (InputSystem.IsKeyPressed(Keys.Escape))
            {
                game1.Exit();
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            game1.SetBGColor(BGcolor);           
        }
    }
}