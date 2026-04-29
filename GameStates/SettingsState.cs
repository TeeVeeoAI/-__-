using System.Collections.Generic;
using ____.GameStates.Items;
using ____.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ____.GameStates
{
    public class SettingsState : GameState
    {
        private enum SettingsAction
        {
            ToggleFullscreen,
            ToggleVSync,
            ReloadSettings,
            Keybinds,
            MovementKeys,
            AttackKeys,
            Back
        }

        private List<SettingsItem> settingsItems; // Using a list for the same reason as in MenuState.
        private int selectedIndex;

        public SettingsState(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            BGcolor = Color.Gray;
            settingsItems = new List<SettingsItem> {
                new("Toggle Fullscreen", new (100, 150, 300, 50)),
                new("Toggle VSync", new (100, 220, 300, 50)),
                new("Reload Settings", new (100, 290, 300, 50)),
                new("Keybinds", new (100, 360, 300, 50)),
                new("Movement Keys", new (100, 430, 300, 50)),
                new("Attack Keys", new (100, 500, 300, 50)),
                new("Back", new (100, 570, 300, 50))
            };
            selectedIndex = 0;
            settingsItems[selectedIndex].IsSelected = true;
            UpdateSettingLabels();
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
                settingsItems[selectedIndex].IsSelected = false;
                selectedIndex = (selectedIndex + 1) % settingsItems.Count;
                settingsItems[selectedIndex].IsSelected = true;
            }
            else if (InputSystem.IsKeyPressed(Keys.Up))
            {
                settingsItems[selectedIndex].IsSelected = false;
                selectedIndex = (selectedIndex - 1 + settingsItems.Count) % settingsItems.Count;
                settingsItems[selectedIndex].IsSelected = true;
            }

            foreach (var item in settingsItems)
            {
                if (!item.IsSelected)
                    continue;

                if ((InputSystem.IsLeftPressed() && item.Bounds.Contains(InputSystem.GetMousePosition())) || InputSystem.IsKeyPressed(Keys.Enter))
                {
                    selectedIndex = settingsItems.IndexOf(item);
                    switch ((SettingsAction)selectedIndex)
                    {
                        case SettingsAction.ToggleFullscreen:
                            game1.ToggleFullscreen();
                            break;
                        case SettingsAction.ToggleVSync:
                            game1.ToggleVSync();
                            break;
                        case SettingsAction.ReloadSettings:
                            game1.ReloadSettings();
                            break;
                        case SettingsAction.Back:
                            game1.ChangeGameState(new MenuState(game1, graphicsDevice, contentManager));
                            return;
                        default:
                            // No action for keybind labels yet.
                            break;
                    }

                    UpdateSettingLabels();
                    break;
                }
            }
        }

        private void UpdateSettingLabels()
        {
            if (settingsItems.Count >= 2)
            {
                settingsItems[0].Text = $"Toggle Fullscreen ({(game1.IsFullScreen ? "On" : "Off")})";
                settingsItems[1].Text = $"Toggle VSync ({(game1.IsVSync ? "On" : "Off")})";
            }
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