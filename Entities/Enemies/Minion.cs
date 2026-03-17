using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ____.Entities.Enemies
{
    public class Minion : BaseEnemyEntity
    {
        private int flightHeight;
        private Rectangle DrawHitbox;
        private MinionState currentState;

        public Minion(int flightHeight) : base(new(0, 0), 50, new(32, 32), 5, 20, Color.DarkBlue)
        {
            this.flightHeight = flightHeight;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            velocity.Normalize();

            if (inputDirection.X == 0 && inputDirection.Y < 0)
                currentDirection = PlayerDirection.Up;
            else if (inputDirection.X == 0 && inputDirection.Y > 0)
                currentDirection = PlayerDirection.Down;
            else if (inputDirection.X < 0 && inputDirection.Y == 0)
                currentDirection = PlayerDirection.Left;
            else if (inputDirection.X > 0 && inputDirection.Y == 0)
                currentDirection = PlayerDirection.Right;
            else if (inputDirection.X < 0 && inputDirection.Y < 0)
                currentDirection = PlayerDirection.UpLeft;
            else if (inputDirection.X > 0 && inputDirection.Y < 0)
                currentDirection = PlayerDirection.UpRight;
            else if (inputDirection.X < 0 && inputDirection.Y > 0)
                currentDirection = PlayerDirection.DownLeft;
            else if (inputDirection.X > 0 && inputDirection.Y > 0)
                currentDirection = PlayerDirection.DownRight;

            Vector2 newPosition = position + velocity * speed * dt;

            Rectangle checkHitbox = new Rectangle((int)newPosition.X, (int)newPosition.Y, hitbox.Width, hitbox.Height);

            if (Map.CurrentMap.IsWalkable(checkHitbox))
            {
                position = newPosition;
            }
            else
            {
                // Try sliding: check if moving only in X is possible
                Vector2 slideXPosition = new Vector2(newPosition.X, position.Y);
                Rectangle slideXHitbox = new Rectangle((int)slideXPosition.X, (int)slideXPosition.Y, hitbox.Width, hitbox.Height);
                if (Map.CurrentMap.IsWalkable(slideXHitbox))
                {
                    position = slideXPosition;
                }
                else
                {
                    // Try sliding in Y
                    Vector2 slideYPosition = new Vector2(position.X, newPosition.Y);
                    Rectangle slideYHitbox = new Rectangle((int)slideYPosition.X, (int)slideYPosition.Y, hitbox.Width, hitbox.Height);
                    if (Map.CurrentMap.IsWalkable(slideYHitbox))
                    {
                        position = slideYPosition;
                    }
                    else
                    {
                        // Can't move at all
                        velocity = Vector2.Zero;
                        currentState = MinionState.Idle;
                    }
                }
            }

            // Clamp position to map boundaries (after movement)
            position.X = Math.Clamp(position.X, Map.CurrentMap.Rec.Left, Map.CurrentMap.Rec.Right - hitbox.Width);
            position.Y = Math.Clamp(position.Y, Map.CurrentMap.Rec.Top, Map.CurrentMap.Rec.Bottom - hitbox.Height);

            hitbox.Location = position.ToPoint();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            DrawHitbox.Y = (int)(flightHeight + (float)Math.Sin(time * speed) * 20); // Oscillate up and down

            spriteBatch.Draw(texture, DrawHitbox, color);

            color.A = 30; // Make the hitbox semi-transparent
            base.Draw(gameTime, spriteBatch);
            color.A = 255; // Reset color alpha for next draw
        }
    }
    public enum MinionState
    {
        Idle,
        Flighting,
        Attacking,
        Dead
    }
}