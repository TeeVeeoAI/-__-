using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ____.Systems
{
    public class FpsCounter
    {
        private int frameCount;
        private double elapsedTime;
        private int currentFps;

        public int CurrentFps { get => currentFps; }

        public FpsCounter()
        {
            frameCount = 0;
            elapsedTime = 0;
            currentFps = 0;
        }

        public void Update(GameTime gameTime)
        {
            frameCount++;
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime >= 1.0)
            {
                currentFps = frameCount;
                frameCount = 0;
                elapsedTime -= 1.0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, $"FPS: {currentFps}", position, color);
        }
    }
}
