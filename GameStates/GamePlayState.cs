using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Camera;
using ____.Entities.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ____.Systems;

namespace ____.GameStates
{
    public class GamePlayState : GameState
    {
        private Camera2D camera;
        private PlayerEntity player;
        private FpsCounter fpsCounter;

        public GamePlayState(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            BGcolor = Color.Green;
            camera = new Camera2D(graphicsDevice);
            fpsCounter = new FpsCounter();
        }
        public override void LoadContent()
        {
            font = contentManager.Load<SpriteFont>("Fonts/DefaultFont");
            player = new PlayerEntity(new Vector2(0, 0), pixel);
            player.LoadContent(contentManager);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            player.Update(gameTime);
            camera.Pos = player.Position;

            fpsCounter.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Begin(transformMatrix: camera.Get_transformation());
            spriteBatch.DrawString(font, "Gameplay State", new Vector2(100, 100), Color.White);
            player.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            DrawUI(gameTime, spriteBatch);
        }

        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            player.DrawUI(spriteBatch, font, new Vector2(10, 50), Color.White);
            fpsCounter.Draw(spriteBatch, font, new Vector2(10, 10), Color.White);

            spriteBatch.End();
        }
    }
}