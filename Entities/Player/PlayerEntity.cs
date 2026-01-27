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
        private float currentStaminaBar;
        private float healthRegenTimer = 0f;
        private float manaRegenTimer = 0f;
        private float staminaBarRegenTimer = 0f;


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

        public float StaminaBar
        {
            get => currentStaminaBar;
            set => currentStaminaBar = Math.Clamp(value, 0f, stats.MaxStaminaBar);
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
            currentStaminaBar = stats.MaxStaminaBar;

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
                CanDash = true,
                DashColldown = 3.5f,
                activeDash = false,
                activeDashTimer = 0f,
                HasFireball = false,
                FireballCooldown = 5.0f
            };
        }

        public override void Update(GameTime gameTime)
        {
            HandleMovement(gameTime);
            HandleRegeneration(gameTime);
            HandleCooldown(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            // Additional player-specific drawing (e.g., animations) can go here
        }

        public void DrawUI(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color)
        {
            string status = $"HP: {CurrentHealth}/{stats.MaxHealth}  " +
                            $"Mana: {CurrentMana}/{stats.MaxMana}  " +
                            $"Stamina: {StaminaBar}/{stats.MaxStaminaBar}  " +
                            $"State: {currentState}  " +
                            $"Direction: {currentDirection}  " + 
                            $"Level: {stats.Level}  " +
                            $"XP: {stats.Experience}/{stats.ExperienceToNextLevel}  " + 
                            $"Speed: {velocity.Length()}";

            spriteBatch.DrawString(font, status, position, color);
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y + 15, 110, 50), Color.Black); // Background bar

            spriteBatch.Draw(texture, new Rectangle((int)position.X + 5, (int)position.Y + 20, 100, 10), Color.DarkRed);
            spriteBatch.Draw(texture, new Rectangle((int)position.X + 5, (int)position.Y + 35, 100, 10), Color.DarkGoldenrod);
            spriteBatch.Draw(texture, new Rectangle((int)position.X + 5, (int)position.Y + 50, 100, 10), Color.DarkBlue);

            spriteBatch.Draw(texture, new Rectangle((int)position.X + 5, (int)position.Y + 20, (int)(100 * (CurrentHealth / (float)stats.MaxHealth)), 10), Color.Red);
            spriteBatch.Draw(texture, new Rectangle((int)position.X + 5, (int)position.Y + 35, (int)(100 * (StaminaBar / stats.MaxStaminaBar)), 10), Color.Yellow);
            spriteBatch.Draw(texture, new Rectangle((int)position.X + 5, (int)position.Y + 50, (int)(100 * (CurrentMana / (float)stats.MaxMana)), 10), Color.Blue);
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
                if (InputSystem.IsKeyDown(Keys.LeftShift) && currentStaminaBar >= 0f)
                {
                    velocity += (inputDirection * attributes.MovementSpeed.RunSpeed)/1000; // Running speed
                    if (velocity.Y >= inputDirection.Y * attributes.MovementSpeed.RunSpeed && velocity.X >= inputDirection.X * attributes.MovementSpeed.RunSpeed)
                        velocity = inputDirection * attributes.MovementSpeed.RunSpeed;

                    currentState = PlayerState.Running;
                    currentStaminaBar -= 10f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (currentStaminaBar < 0f)
                        currentStaminaBar = 0f;
                }
                else if (((InputSystem.IsKeyDown(Keys.Space) && skills.CanDash) || skills.activeDash) && currentStaminaBar >= 30f)
                {
                    velocity = inputDirection * attributes.MovementSpeed.DashSpeed; // Dashing speed
                    currentState = PlayerState.Dashing;
                    skills.CanDash = false;
                    skills.DashColldown = 3.5f; // Reset dash cooldown
                    if (!skills.activeDash)
                    {
                        currentStaminaBar -= 30f; // Instant stamina cost if dash initiated
                        skills.activeDash = true;
                        skills.activeDashTimer = 0.5f; // Dash lasts for 0.5 seconds
                    }
                }
                else
                {
                    velocity = inputDirection * attributes.MovementSpeed.WalkSpeed; // Walking speed
                    currentState = PlayerState.Walking;
                }
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

            // Stamina bar regeneration
            staminaBarRegenTimer += deltaTime;
            if (staminaBarRegenTimer >= 1f) // Regen per second
            {
                StaminaBar += stats.StaminaBarRegenPerSecond;
                staminaBarRegenTimer = 0f;
            }
        }

        private void HandleCooldown(GameTime gameTime)
        {
            if (!skills.CanDash)
            {
                skills.DashColldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (skills.DashColldown <= 0f)
                {
                    skills.CanDash = true;
                }
            }
            if (!skills.HasFireball)
            {
                skills.FireballCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (skills.FireballCooldown <= 0f)
                {
                    skills.HasFireball = true;
                }
            }
            if (skills.activeDash)
            {
                skills.activeDashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (skills.activeDashTimer <= 0f)
                {
                    skills.activeDash = false;
                }
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
        Dashing,
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
        public float DashColldown;
        public bool activeDash;
        public float activeDashTimer;
        public bool HasFireball;
        public float FireballCooldown;
    }

    public enum MovementKeys
    {
        Up,
        Down,
        Left,
        Right
    }
}