using System;
using ____.Entities.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ____.Entities.Enemies
{
    public class BaseEnemyEntity : BaseEntity
    {
        protected bool damageByPlayer = false;
        protected int damage;

        private float attackCooldown = 1.0f; // seconds
        private float attackCooldownTimer = 0f;

        public int Damage => damage;
        public float AttackCooldown
        {
            get => attackCooldown;
            set => attackCooldown = Math.Max(0.05f, value);
        }

        public bool CanAttack => attackCooldownTimer <= 0f;

        public BaseEnemyEntity(Vector2 position, float speed, Vector2 heightAndWidth, int damage = 10, int hp = 100, Color? color = null)
            : base(position, speed, heightAndWidth, hp, color)
        {
            this.damage = damage;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (attackCooldownTimer > 0f)
            {
                attackCooldownTimer -= dt;
                if (attackCooldownTimer < 0f) attackCooldownTimer = 0f;
            }
        }

        public bool TryAttack(PlayerEntity player)
        {
            if (player == null) return false;
            if (!CanAttack) return false;

            player.TakeDamage(Damage);
            attackCooldownTimer = attackCooldown;
            return true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public virtual void TakeDamage(int damage, bool byPlayer = false)
        {
            hp -= damage;
            if (byPlayer)
            {
                damageByPlayer = true;
            }
        }

        public virtual void Kill(bool byPlayer = false)
        {
            if (byPlayer)
            {
                damageByPlayer = true;
            }
            hp = 0;
        }

        public virtual bool IsDead()
        {
            return hp <= 0;
        }
    }
}