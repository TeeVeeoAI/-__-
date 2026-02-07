using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ____.Systems.Animations
{
    public class AnimationController
    {
        private Dictionary<string, Animation> animations;
        private Animation currentAnimation;
        private string currentAnimationName;

        public AnimationController()
        {
            animations = new Dictionary<string, Animation>();
        }

        public void AddAnimation(string name, Animation animation)
        {
            animations[name] = animation;
            
            if (currentAnimation == null)
            {
                currentAnimation = animation;
                currentAnimationName = name;
            }
        }

        public void Play(string name, bool restart = false)
        {
            if (!animations.ContainsKey(name))
                return;

            if (name != currentAnimationName)
            {
                currentAnimation = animations[name];
                currentAnimationName = name;
                currentAnimation.Reset();
            }
            else if (restart)
            {
                currentAnimation.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            currentAnimation?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color,
                        SpriteEffects effects = SpriteEffects.None, float scale = 1f)
        {
            currentAnimation?.Draw(spriteBatch, position, color, effects, scale);
        }

        public Animation GetCurrentAnimation()
        {
            return currentAnimation;
        }

        public string GetCurrentAnimationName()
        {
            return currentAnimationName;
        }

        public bool IsCurrentAnimationFinished()
        {
            return currentAnimation?.IsFinished ?? false;
        }
    }
}