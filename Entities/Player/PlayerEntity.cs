using System;
using System.Collections.Generic;
using ____.Player.Inventory;
using ____.Systems;
using ____.Systems.Animations;
using ____.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ____.Entities.Player
{
    public class PlayerEntity : BaseEntity
    {
        #region Player-specific fields and properties
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
        private float runColdownTimer = 0f;

        private Texture2D spriteSheet;
        private AnimationController animationController;
    
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

        private PlayerInventory inventory = new(10);
        private PlayerAttributes attributes;
        private PlayerSkills skills;

        public PlayerInventory Inventory
        {
            get => inventory;
        }
        private KeyBinds keyBinds;

        #endregion

        #region Constructor and content loading
        public PlayerEntity(StatScaling statScaling = null)
            : base(new(-25, -25), 100f, new(50, 50), 100, Color.Black)
        {
            // Initialize scaling config 
            scaling = statScaling ?? new();

            keyBinds = KeyBindsLoader.Load();

            // Initialize stats with scaling reference
            stats = new(scaling);

            // Set current health/mana to max
            currentHealth = stats.MaxHealth;
            currentMana = stats.MaxMana;
            currentStaminaBar = stats.MaxStaminaBar;

            // Calculate speed based on stats and scaling
            speed = scaling.BaseSpeed * (1 + stats.Agility * scaling.SpeedPerAgility);

            attributes = new()
            {
                MovementSpeed = new()
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
                DashColldown = 0.5f,
                activeDash = false,
                activeDashTimer = 0f,
                HasFireball = false,
                FireballCooldown = 5.0f
            };

            animationController = new();
            instance = this;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager); // Load base content (e.g., hitbox texture) currently just a white pixel

            spriteSheet = contentManager.Load<Texture2D>("Sprites/player_spritesheet");

            var walkDownAnim = new Animation(spriteSheet, 4, 32, 32, 0.1f, true, 0);
            animationController.AddAnimation("walk_down", walkDownAnim);
            
            // Row 1: Walking Up
            var walkUpAnim = new Animation(spriteSheet, 4, 32, 32, 0.1f, true, 1);
            animationController.AddAnimation("walk_up", walkUpAnim);
            
            // Row 2: Walking Right
            var walkRightAnim = new Animation(spriteSheet, 4, 32, 32, 0.1f, true, 2);
            animationController.AddAnimation("walk_right", walkRightAnim);
            
            // Row 3: Walking Left
            var walkLeftAnim = new Animation(spriteSheet, 4, 32, 32, 0.1f, true, 3);
            animationController.AddAnimation("walk_left", walkLeftAnim);
            
            // Start with down animation
            animationController.Play("walk_down");
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            HandleMovement(gameTime);
            HandleRegeneration(gameTime);
            HandleCooldown(gameTime);
            UpdateAnimation();
            animationController.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //base.Draw(gameTime, spriteBatch);
            //Additional player-specific drawing (e.g., animations) can go here
            animationController.Draw(spriteBatch, position - hitbox.Size.ToVector2()/5, Color.White, SpriteEffects.None, 2f);
            //spriteBatch.Draw(texture, hitbox, Color.Red * 0.5f); // Draw hitbox for debugging
        }

        public void DrawUI(SpriteBatch spriteBatch, SpriteFont font, Color color)
        {
            
            Vector2 statusPosition = new(10, 50);
            // Draw player stats (debugging purposes)
            string status = $"HP: {CurrentHealth}/{stats.MaxHealth}  " +
                            $"Mana: {CurrentMana}/{stats.MaxMana}  " +
                            $"Stamina: {StaminaBar}/{stats.MaxStaminaBar}  " +
                            $"State: {currentState}  " +
                            $"Direction: {currentDirection}  " +
                            $"Level: {stats.Level}  " +
                            $"XP: {stats.Experience}/{stats.ExperienceToNextLevel}  " +
                            $"Speed: {velocity.Length()}";


            spriteBatch.DrawString(font, status, statusPosition, color);

            int separate = 25; // Separation between bars
            Vector2 barPosition = new(10, 50 + separate);
            // Draw health, stamina, and mana bars
            spriteBatch.Draw(texture, new Rectangle((int)barPosition.X, (int)barPosition.Y, 104, 38), Color.Black); // Background bar

            spriteBatch.Draw(texture, new Rectangle((int)barPosition.X + 2, (int)barPosition.Y + 2, 100, 10), Color.DarkRed);         // Health bar background
            spriteBatch.Draw(texture, new Rectangle((int)barPosition.X + 2, (int)barPosition.Y + 14, 100, 10), Color.DarkGoldenrod);  // Stamina bar background
            spriteBatch.Draw(texture, new Rectangle((int)barPosition.X + 2, (int)barPosition.Y + 26, 100, 10), Color.DarkBlue);       // Mana bar background

            spriteBatch.Draw(texture, new Rectangle((int)barPosition.X + 2, (int)barPosition.Y + 2, (int)(100 * (CurrentHealth / (float)stats.MaxHealth)), 10), Color.Red); // Health bar
            spriteBatch.Draw(texture, new Rectangle((int)barPosition.X + 2, (int)barPosition.Y + 14, (int)(100 * (StaminaBar / stats.MaxStaminaBar)), 10), Color.Yellow);   // Stamina bar
            spriteBatch.Draw(texture, new Rectangle((int)barPosition.X + 2, (int)barPosition.Y + 26, (int)(100 * (CurrentMana / (float)stats.MaxMana)), 10), Color.Blue);   // Mana bar

            
        }

        #region Movement, Regeneration, Cooldown, Animation Handling
        private void HandleInputExceptMovement(GameTime gameTime)
        {
            
        }
        private void HandleMovement(GameTime gameTime)
        {
            #region Handle input
            Vector2 inputDirection = Vector2.Zero;

            if (InputSystem.IsKeyDown(keyBinds.Movement.Up))
            {
                inputDirection.Y -= 1;
            }
            if (InputSystem.IsKeyDown(keyBinds.Movement.Down))
            {
                inputDirection.Y += 1;
            }
            if (InputSystem.IsKeyDown(keyBinds.Movement.Left))
            {
                inputDirection.X -= 1;
            }
            if (InputSystem.IsKeyDown(keyBinds.Movement.Right))
            {
                inputDirection.X += 1;
            }
            #endregion
            
            #region Determine direction and state based on input
            if (inputDirection != Vector2.Zero)
            {
                #region Determine direction based on input
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
                #endregion

                inputDirection.Normalize();
                if (((InputSystem.IsKeyDown(Keys.Space) && skills.CanDash) || skills.activeDash) && currentStaminaBar >= 30f)
                {
                    velocity = inputDirection * attributes.MovementSpeed.DashSpeed; // Dashing speed
                    currentState = PlayerState.Dashing;
                    skills.CanDash = false;
                    skills.DashColldown = 0.5f; // Reset dash cooldown
                    if (!skills.activeDash)
                    {
                        currentStaminaBar -= 30f; // Instant stamina cost if dash initiated
                        skills.activeDash = true;
                        skills.activeDashTimer = 0.5f; // Dash lasts for 0.5 seconds
                    }
                }
                else if (InputSystem.IsKeyDown(Keys.LeftShift) && currentStaminaBar >= 0f && runColdownTimer <= 0f)
                {
                    velocity = inputDirection * attributes.MovementSpeed.RunSpeed;

                    currentState = PlayerState.Running;
                    currentStaminaBar -= 10f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    velocity = inputDirection * attributes.MovementSpeed.WalkSpeed; // Walking speed
                    currentState = PlayerState.Walking;
                }
                if (currentStaminaBar < 0f)
                {
                    currentStaminaBar = 0f;
                    runColdownTimer = 2f; // 2 second cooldown after stamina depletes
                }
            }
            else
            {
                velocity *= friction;
                if (velocity.Length() < 1f)
                {
                    velocity = Vector2.Zero;
                    currentState = PlayerState.Idle;
                }
            }
            #endregion
            
            #region Collision detection and response
            Vector2 newPosition = position + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                        currentState = PlayerState.Idle;
                    }
                }
            }
            #endregion

            #region clamp position to map boundaries
            // Clamp position to map boundaries (after movement)
            position.X = Math.Clamp(position.X, Map.CurrentMap.Rec.Left, Map.CurrentMap.Rec.Right - hitbox.Width);
            position.Y = Math.Clamp(position.Y, Map.CurrentMap.Rec.Top, Map.CurrentMap.Rec.Bottom - hitbox.Height);

            hitbox.Location = position.ToPoint();
            #endregion
        }

        #region Regeneration and Cooldown Handling
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
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!skills.CanDash)
            {
                skills.DashColldown -= deltaTime;
                if (skills.DashColldown <= 0f)
                {
                    skills.CanDash = true;
                }
            }

            if (!skills.HasFireball)
            {
                skills.FireballCooldown -= deltaTime;
                if (skills.FireballCooldown <= 0f)
                {
                    skills.HasFireball = true;
                }
            }

            if (skills.activeDash)
            {
                skills.activeDashTimer -= deltaTime;
                if (skills.activeDashTimer <= 0f)
                {
                    skills.activeDash = false;
                }
            }

            if (currentState != PlayerState.Running && runColdownTimer >= 0f)
            {
                runColdownTimer -= deltaTime;
                if (runColdownTimer < 0f)
                    runColdownTimer = 0f;
            }
        }
        #endregion

        #region Animation Handling
        private void UpdateAnimation()
        {
            // Play the appropriate animation based on direction
            string animName = "";
            
            switch (currentDirection)
            {
                case PlayerDirection.Down:
                    animName = "walk_down";
                    break;
                case PlayerDirection.Up:
                    animName = "walk_up";
                    break;
                case PlayerDirection.Right:
                    animName = "walk_right";
                    break;
                case PlayerDirection.Left:
                    animName = "walk_left";
                    break;
            }
            
            // Only play if moving, otherwise keep on first frame
            if (velocity.Length() > 0)
            {
                animationController.Play(animName);
            }
            else
            {
                // Show idle frame (first frame of current direction)
                animationController.Play(animName);
                animationController.GetCurrentAnimation().Reset();
            }
        }
        #endregion
        #endregion

        #region Combat and Damage Handling
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
        #endregion

        #region Getters for stats, attributes, and skills
        // Public accessors for UI/debugging
        public PlayerStats GetStats() => stats;
        public PlayerAttributes GetAttributes() => attributes;
        public PlayerSkills GetSkills() => skills;
        #endregion

        static PlayerEntity instance;
        public static PlayerEntity Instance {
            get
            {
                return instance;
            } 
        }
    }

    #region Supporting Enums and Structs
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
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
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
    #endregion
}
