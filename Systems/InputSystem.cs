using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ____.Systems
{
    public static class InputSystem
    {
        private static KeyboardState kstateNew = Keyboard.GetState();
        private static KeyboardState kstateOld = kstateNew;
        private static MouseState mstateNew = Mouse.GetState();
        private static MouseState mstateOld = mstateNew;

        public static void Initialize()
        {
            kstateNew = Keyboard.GetState();
            mstateNew = Mouse.GetState();
            kstateOld = kstateNew;
            mstateOld = mstateNew;
        }

        public static void Update()
        {
            kstateOld = kstateNew;
            mstateOld = mstateNew;
            kstateNew = Keyboard.GetState();
            mstateNew = Mouse.GetState();
        }

        // Queue for text input characters (from Window.TextInput)
        private static Queue<char> typedChars = new Queue<char>();

        public static void QueueTypedChar(char c)
        {
            typedChars.Enqueue(c);
        }

        public static bool TryGetTypedChar(out char c)
        {
            if (typedChars.Count > 0)
            {
                c = typedChars.Dequeue();
                return true;
            }
            c = '\0';
            return false;
        }

        public static void ClearTypedBuffer()
        {
            typedChars.Clear();
        }

        public static KeyboardState NewState { get => kstateNew; }
        public static KeyboardState OldState { get => kstateOld; }
        public static MouseState NewMouseState { get => mstateNew; }
        public static MouseState OldMouseState { get => mstateOld; }

        public static bool IsLeftPressed()
        {
            return mstateNew.LeftButton == ButtonState.Pressed && mstateOld.LeftButton == ButtonState.Released;
        }

        public static bool IsLeftReleased()
        {
            return mstateNew.LeftButton == ButtonState.Released && mstateOld.LeftButton == ButtonState.Pressed;
        }

        public static bool IsLeftDown()
        {
            return mstateNew.LeftButton == ButtonState.Pressed;
        }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(mstateNew.X, mstateNew.Y);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return kstateNew.IsKeyDown(key) && kstateOld.IsKeyUp(key);
        }

        

        public static bool IsKeyReleased(Keys key)
        {
            return kstateNew.IsKeyUp(key) && kstateOld.IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return kstateNew.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return kstateNew.IsKeyUp(key);
        }

        public static Keys[] GetPressedKeys()
        {
            return kstateNew.GetPressedKeys();
        }
    }
}