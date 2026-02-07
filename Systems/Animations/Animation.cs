using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ____.Systems.Animations
{
    public class Animation
    {
        public Texture2D SpriteSheet { get; private set; }
        public int FrameCount { get; private set; }
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }
        public float FrameTime { get; private set; }
        public bool IsLooping { get; private set; }
        public int CurrentFrame { get; private set; }
        public bool IsFinished { get; private set; }

        private float elapsedTime;
        private int row; 
    
        public Animation(Texture2D spriteSheet, int frameCount, int frameWidth, int frameHeight, 
                        float frameTime, bool isLooping = true, int row = 0)
        {
            SpriteSheet = spriteSheet;
            FrameCount = frameCount;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            FrameTime = frameTime;
            IsLooping = isLooping;
            this.row = row;
            CurrentFrame = 0;
            IsFinished = false;
            elapsedTime = 0f;
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished && !IsLooping)
                return;

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime >= FrameTime)
            {
                elapsedTime -= FrameTime;
                CurrentFrame++;

                if (CurrentFrame >= FrameCount)
                {
                    if (IsLooping)
                    {
                        CurrentFrame = 0;
                    }
                    else
                    {
                        CurrentFrame = FrameCount - 1;
                        IsFinished = true;
                    }
                }
            }
        }

        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(
                CurrentFrame * FrameWidth,
                row * FrameHeight,
                FrameWidth,
                FrameHeight
            );
        }

        public void Reset()
        {
            CurrentFrame = 0;
            elapsedTime = 0f;
            IsFinished = false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, 
                        SpriteEffects effects = SpriteEffects.None, float scale = 1f)
        {
            spriteBatch.Draw(
                SpriteSheet,
                position,
                GetSourceRectangle(),
                color,
                0f,
                Vector2.Zero,
                scale,
                effects,
                0f
            );
        }
    }
}