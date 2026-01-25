using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.GameStates.Items;
using ____.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ____.GameStates
{
    public class MenuState : GameState
    {
        private List<MenuItem> menuItems;
        private int selectedIndex;
        public MenuState(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            BGcolor = Color.Black;
            menuItems = new List<MenuItem> { 
                new("Start Game", new (100, 150, 200, 50)), 
                new("Settings", new (100, 220, 200, 50)),
                new("Exit", new (100, 290, 200, 50))
            };
            selectedIndex = 0;
            menuItems[selectedIndex].IsSelected = true;
        }
        public override void LoadContent()
        {
            font = contentManager.Load<SpriteFont>("Fonts/DefaultFont");
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (InputSystem.IsKeyPressed(Keys.Down))
            {
                menuItems[selectedIndex].IsSelected = false;
                selectedIndex = (selectedIndex + 1) % menuItems.Count;
                menuItems[selectedIndex].IsSelected = true;
            }
            else if (InputSystem.IsKeyPressed(Keys.Up))
            {
                menuItems[selectedIndex].IsSelected = false;
                selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;
                menuItems[selectedIndex].IsSelected = true;
            }
            foreach (var item in menuItems)
            {
                if (item.IsSelected)
                {
                    if ((InputSystem.IsLeftPressed() && item.Bounds.Contains(InputSystem.GetMousePosition())) || InputSystem.IsKeyPressed(Keys.Enter)){
                        selectedIndex = menuItems.IndexOf(item);
                    }
                    else
                    {
                        continue;
                    }
                    switch (selectedIndex)
                    {
                        case 0:
                            game1.ChangeGameState(new GamePlayState(game1, graphicsDevice, contentManager));
                            break;
                        case 1:
                            game1.ChangeGameState(new SettingsState(game1, graphicsDevice, contentManager));
                            break;
                        case 2:
                            game1.Exit();
                            break;
                    }
                    break;
                }
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Main Menu", new Vector2(100, 100), Color.White);
            foreach (var item in menuItems)
            {
                item.Draw(spriteBatch, font);
            }
            spriteBatch.End();
        }
    }
}