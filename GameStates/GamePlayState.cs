using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ____.Camera;
using ____.Entities.Enemies;
using ____.Entities.Player;
using ____.Systems;
using ____.Systems.EnemySpawnSystem;
using ____.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ____.GameStates
{
    public class GamePlayState : GameState
    {
        private Camera2D camera;
        private PlayerEntity player;
        private List<BaseEnemyEntity> enemies; //Using a list for dynamic enemy management.
        private EnemySpawnSystem enemySpawnSystem;
        private FpsCounter fpsCounter;
        private GamePlaySubState currentSubState;
        private Map map;

        public GamePlayState(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            
            BGcolor = Color.Green;
            camera = new(graphicsDevice);
            fpsCounter = new();
            currentSubState = GamePlaySubState.LoadingMap;
            map = Map.Load();
            player = new();
            enemies = new();
            enemySpawnSystem = new();
            enemySpawnSystem.AddEnemyType(() => new Minion(10), weight: 10);
        }
        public override void LoadContent()
        {
            font = contentManager.Load<SpriteFont>("Fonts/DefaultFont");
            player.LoadContent(contentManager);
            map.LoadContent(contentManager);
            camera.LoadContent(contentManager, map.Rec);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentSubState == GamePlaySubState.Normal)
            {
                player.Update(gameTime);
                enemySpawnSystem.Update(gameTime, enemies, camera, map);
                CollisionDetection(gameTime);
                foreach (var enemy in enemies)
                {
                    enemy.Update(gameTime);
                }
            }
            else if (currentSubState == GamePlaySubState.Inventory)
            {
                // Update inventory logic here
            } else if (currentSubState == GamePlaySubState.LoadingMap)
            {
                if (map != null)
                {
                    currentSubState = GamePlaySubState.Normal;
                }
            }

            map.Update(gameTime);

            HandleInput();

            camera.Pos = player.Position;

            fpsCounter.Update(gameTime);
        }

        public void CollisionDetection(GameTime gameTime)
        {
            if (enemies.Count == 0) return;
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];
                if (DetectionNative.aabb_overlaps(DetectionNative.ToNativeRect(player.Hitbox), DetectionNative.ToNativeRect(enemy.Hitbox)) == 1)
                {
                    // Enemy attempts to attack player's entity on contact with a cooldown
                    if (enemy.TryAttack(player))
                    {
                        // Optionally do anything when enemy lands an attack, e.g. change state
                        // enemy.SetState? (if you add state transitions).
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Begin(transformMatrix: camera.Get_transformation());

            map.Draw(spriteBatch, camera, graphicsDevice);
            

            foreach (var enemy in enemies)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

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
        PauseMenu,
        LoadingMap
    }
}
