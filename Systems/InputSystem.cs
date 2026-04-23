using System.Collections.Generic;
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
        public static bool IsRightPressed()
        {
            return mstateNew.RightButton == ButtonState.Pressed && mstateOld.RightButton == ButtonState.Released;
        }

        public static bool IsLeftReleased()
        {
            return mstateNew.LeftButton == ButtonState.Released && mstateOld.LeftButton == ButtonState.Pressed;
        }
        public static bool IsRightReleased()
        {
            return mstateNew.RightButton == ButtonState.Released && mstateOld.RightButton == ButtonState.Pressed;
        }

        public static bool IsLeftDown()
        {
            return mstateNew.LeftButton == ButtonState.Pressed;
        }
        public static bool IsRightDown()
        {
            return mstateNew.RightButton == ButtonState.Pressed;
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

        public static bool IsKeyPreesed(string key) 
        // I spelled "Pressed" wrong, but i don't want to change it now because it would break other stuff.
        // I have no idea if this is the best way to do this, 
        // but i couldn't think of a better way to handle mouse buttons in the keybinds system.
        {
            if (key == "MouseLeft")
                return IsLeftPressed();
            if (key == "MouseRight")
                return IsRightReleased();
            else if (System.Enum.TryParse(key, out Keys k))
                return IsKeyPressed(k);
            return false;
            
        }

        public static bool IsKeyDown(string key)
        {
            if (key == "MouseLeft")
                return IsLeftDown();
            if (key == "MouseRight")
                return IsRightDown();
            else if (System.Enum.TryParse(key, out Keys k))
                return IsKeyDown(k);
            return false;
        }
    }
}