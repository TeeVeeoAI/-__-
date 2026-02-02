using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ____.Entities
{
    public abstract class BaseEntity
    {
        protected Vector2 position;
        protected float speed; // pixels per second
        protected Vector2 velocity;
        protected float friction;
        protected Rectangle hitbox;
        protected Texture2D texture;
        protected Color color;
        protected int hp;

        public Rectangle Hitbox
        {
            get => hitbox;
        }
        public Vector2 Position
        {
            get => position;
        }
        public int HP
        {
            get => hp;
        }

        public BaseEntity(Vector2 position, float speed, Vector2 heightAndWidth, Texture2D texture, int hp = 100, Color? color = null)
        {
            this.position = position;
            this.speed = speed;
            this.hitbox = new Rectangle((int)position.X, (int)position.Y, (int)heightAndWidth.X, (int)heightAndWidth.Y);
            this.texture = texture;
            this.hp = hp;
            this.color = color ?? Color.White;
            this.velocity = Vector2.Zero;
            this.friction = 0.98f;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, color);
        }
    }
}