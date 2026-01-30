using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Entities.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ____.Entities.Enemies
{
    public class BaseEnemyEntity : BaseEntity
    {
        protected bool damageByPlayer = false;
        protected int damage;
        public BaseEnemyEntity(Vector2 position, float speed, Vector2 heightAndWidth, Texture2D texture, int damage = 10, int hp = 100, Color? color = null)
            : base(position, speed, heightAndWidth, texture, hp, color)
        {
            this.damage = damage;
        }

        public override void Update(GameTime gameTime)
        {

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