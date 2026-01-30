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
        public BaseEnemyEntity(Vector2 position, float speed, Vector2 heightAndWidth, Texture2D texture, int hp = 100, Color? color = null)
            : base(position, speed, heightAndWidth, texture, hp, color)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}