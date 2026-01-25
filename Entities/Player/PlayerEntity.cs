using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Player.Inventory;
using ____.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ____.Entities.Player
{
    public class PlayerEntity : BaseEntity
    {
        private PlayerState currentState = PlayerState.Idle;
        private PlayerDirection currentDirection = PlayerDirection.Down;
        
        // Separate stats and scaling
        private PlayerStats stats;
        private StatScaling scaling;
        
        // Current values (these change during gameplay)
        private int currentHealth;
        private int currentMana;
        private float healthRegenTimer = 0f;
        private float manaRegenTimer = 0f;

        
        public int CurrentHealth 
        { 
            get => currentHealth;
            set => currentHealth = Math.Clamp(value, 0, stats.MaxHealth);
        }
        
        public int CurrentMana
        {
            get => currentMana;
            set => currentMana = Math.Clamp(value, 0, stats.MaxMana);
        }
        
        private PlayerInventory inventory = new PlayerInventory();
        private PlayerAttributes attributes;
        private PlayerSkills skills;

        private List<Keys> movementKeys = new List<Keys> { Keys.W, Keys.S, Keys.A, Keys.D };

        public PlayerEntity(Vector2 position, Texture2D texture, StatScaling statScaling = null)
            : base(position, 100f, new Vector2(50, 50), texture, 100, Color.Black)
        {
            // Initialize scaling config 
            scaling = statScaling ?? new StatScaling();
            
            // Initialize stats with scaling reference
            stats = new PlayerStats(scaling);
            
            // Set current health/mana to max
            currentHealth = stats.MaxHealth;
            currentMana = stats.MaxMana;
            
            // Calculate speed based on stats and scaling
            speed = scaling.BaseSpeed * (1 + stats.Agility * scaling.SpeedPerAgility);

            attributes = new PlayerAttributes
            {
                MovementSpeed = new MovementSpeed
                {
                    WalkSpeed = speed,
                    RunSpeed = speed * scaling.RunSpeedMultiplier,
                    DashSpeed = speed * scaling.DashSpeedMultiplier
                },
                AttackSpeed = scaling.BaseAttackSpeed * (1 + stats.Agility * scaling.AttackSpeedPerAgility),
                Defense = scaling.BaseDefense + (stats.Stamina * scaling.DefensePerStamina)
            };
            
            skills = new PlayerSkills
            {
                CanDash = false,
                HasFireball = false
            };
        }

        public override void Update(GameTime gameTime)
        {
            HandleMovement(gameTime);
            HandleRegeneration(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            // Additional player-specific drawing (e.g., animations) can go here
        }
        
        private void HandleMovement(GameTime gameTime)
        {
            Vector2 inputDirection = Vector2.Zero;

            if (InputSystem.IsKeyDown(movementKeys[(int)MovementKeys.Up]))
            {
                inputDirection.Y -= 1;
                currentDirection = PlayerDirection.Up;
            }
            if (InputSystem.IsKeyDown(movementKeys[(int)MovementKeys.Down]))
            {
                inputDirection.Y += 1;
                currentDirection = PlayerDirection.Down;
            }
            if (InputSystem.IsKeyDown(movementKeys[(int)MovementKeys.Left]))
            {
                inputDirection.X -= 1;
                currentDirection = PlayerDirection.Left;
            }
            if (InputSystem.IsKeyDown(movementKeys[(int)MovementKeys.Right]))
            {
                inputDirection.X += 1;
                currentDirection = PlayerDirection.Right;
            }

            if (inputDirection != Vector2.Zero)
            {
                inputDirection.Normalize();
                velocity = inputDirection * speed;
                currentState = PlayerState.Walking;
            }
            else
            {
                velocity *= friction;
                if (velocity.Length() < 0.1f)
                {
                    velocity = Vector2.Zero;
                    currentState = PlayerState.Idle;
                }
            }

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            hitbox.Location = position.ToPoint();
        }
        
        private void HandleRegeneration(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Health regeneration
            healthRegenTimer += deltaTime;
            if (healthRegenTimer >= 1f) // Regen per second
            {
                CurrentHealth += (int)stats.HealthRegenPerSecond;
                healthRegenTimer = 0f;
            }
            
            // Mana regeneration
            manaRegenTimer += deltaTime;
            if (manaRegenTimer >= 1f) // Regen per second
            {
                CurrentMana += (int)stats.ManaRegenPerSecond;
                manaRegenTimer = 0f;
            }
        }
        
        public void TakeDamage(int damage)
        {
            // Apply damage reduction
            float actualDamage = damage * (1 - stats.DamageReduction);
            actualDamage -= attributes.Defense;
            actualDamage = Math.Max(1, actualDamage); // Minimum 1 damage
            
            CurrentHealth -= (int)actualDamage;
            
            if (CurrentHealth <= 0)
            {
                OnDeath();
            }
        }
        
        public void Heal(int amount)
        {
            CurrentHealth += amount;
        }
        
        public int CalculateDamage()
        {
            float damage = stats.BaseDamage;
            
            // Check for critical hit
            if (new Random().NextDouble() < stats.CriticalChance)
            {
                damage *= 2f; // Critical hit doubles damage
            }
            
            return (int)damage;
        }
        
        private void OnDeath()
        {
            // Handle player death
            currentState = PlayerState.Dead;
        }
        
        // Public accessors for UI/debugging
        public PlayerStats GetStats() => stats;
        public PlayerAttributes GetAttributes() => attributes;
        public PlayerSkills GetSkills() => skills;
    }

    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Attacking,
        Dead
    }

    public enum PlayerDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public struct PlayerAttributes
    {
        public MovementSpeed MovementSpeed;
        public float AttackSpeed;
        public float Defense;
    }

    public struct MovementSpeed
    {
        public float WalkSpeed;
        public float RunSpeed;
        public float DashSpeed;
    }

    public struct PlayerSkills
    {
        public bool CanDash;
        public bool HasFireball;
    }

    public enum MovementKeys
    {
        Up,
        Down,
        Left,
        Right
    }
}