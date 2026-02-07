using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Camera;
using ____.Entities.Player;
using ____.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ____.Systems;
using Microsoft.Xna.Framework.Input;

namespace ____.GameStates
{
    public class GamePlayState : GameState
    {
        private Camera2D camera;
        private PlayerEntity player;
        private FpsCounter fpsCounter;
        private GamePlaySubState currentSubState;

        public GamePlayState(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            BGcolor = Color.Green;
            camera = new Camera2D(graphicsDevice);
            fpsCounter = new FpsCounter();
            currentSubState = GamePlaySubState.Normal;
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
            
            if (currentSubState == GamePlaySubState.Normal)
            {
                player.Update(gameTime);
            } else if (currentSubState == GamePlaySubState.Inventory)
            {
                // Update inventory logic here
            }

            HandleInput();
            
            camera.Pos = player.Position;

            fpsCounter.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Begin(transformMatrix: camera.Get_transformation());

            player.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            DrawUI(gameTime, spriteBatch);
        }

        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            player.DrawUI(spriteBatch, font, Color.White);
            fpsCounter.Draw(spriteBatch, font, new Vector2(10, 10), Color.White);

            if (currentSubState == GamePlaySubState.Inventory)
            {
                player.Inventory.Draw(spriteBatch, pixel);
            }

            spriteBatch.End();
        }

        public void HandleInput()
        {
            if (InputSystem.IsKeyPressed(Keys.I))
            {
                currentSubState = currentSubState == GamePlaySubState.Inventory ? GamePlaySubState.Normal : GamePlaySubState.Inventory;
            }
        }
    }

    public enum GamePlaySubState
    {
        Normal,
        Inventory,
        PauseMenu
    }
}