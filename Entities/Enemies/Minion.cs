using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Entities.Player;
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
        private MinionDirection currentDirection;

        public Minion(int flightHeight) : base(new(0, 0), 100, new(32, 32), 5, 20, Color.DarkBlue)
        {
            this.flightHeight = flightHeight;
            this.DrawHitbox = new Rectangle((int)position.X-flightHeight, (int)position.Y-flightHeight, hitbox.Width, hitbox.Height);
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            PlayerEntity player = PlayerEntity.Instance;
            Vector2 directionToPlayer = player.Position - position;

            velocity = directionToPlayer;
            
            velocity.Normalize();

            if (velocity.X == 0 && velocity.Y < 0)
                currentDirection = MinionDirection.Up;
            else if (velocity.X == 0 && velocity.Y > 0)
                currentDirection = MinionDirection.Down;
            else if (velocity.X < 0 && velocity.Y == 0)
                currentDirection = MinionDirection.Left;
            else if (velocity.X > 0 && velocity.Y == 0)
                currentDirection = MinionDirection.Right;
            else if (velocity.X < 0 && velocity.Y < 0)
                currentDirection = MinionDirection.UpLeft;
            else if (velocity.X > 0 && velocity.Y < 0)
                currentDirection = MinionDirection.UpRight;
            else if (velocity.X < 0 && velocity.Y > 0)
                currentDirection = MinionDirection.DownLeft;
            else if (velocity.X > 0 && velocity.Y > 0)
                currentDirection = MinionDirection.DownRight;

            if (velocity.Length() > 0)
            {
                currentState = MinionState.Flighting;
            }
            else
            {
                currentState = MinionState.Idle;
            }

            velocity *= speed;

            Vector2 newPosition = position + velocity * dt;

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

            DrawHitbox.Location = new Point((int)position.X - flightHeight, (int)position.Y - flightHeight);

            hitbox.Location = position.ToPoint();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            float oscillation = flightHeight + (float)Math.Sin(time) * 5; // Oscillate with a frequency of 3 and amplitude of 5
            DrawHitbox.Y -= (int)oscillation; // Oscillate up and down

            color.A = (byte)(20 - (int)(0.5*oscillation)); // Make the hitbox semi-transparent
            base.Draw(gameTime, spriteBatch);
            color.A = 255; // Reset color alpha for next draw

            spriteBatch.Draw(texture, DrawHitbox, color);

        }
    }
    public enum MinionState
    {
        Idle,
        Flighting,
        Attacking,
        Dead
    }
    public enum MinionDirection
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
}