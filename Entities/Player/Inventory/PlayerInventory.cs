using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Entities.Player.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ____.Player.Inventory
{
    public class PlayerInventory
    {
        private InventorySlot[] slots; // Using an array for fixed size inventory, can be changed to List<InventorySlot> if dynamic resizing is needed
        private int capacity;
        public InventorySlot[] AllSlots
        {
            get => slots;
        }
        public int Capacity
        {
            get => capacity;
        }

        public PlayerInventory(int size)
        {
            capacity = size;
            slots = new InventorySlot[capacity];
        }
        public InventorySlot GetSlot(int index)
        {
            if (index < 0 || index >= capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range of the inventory slots.");
            }
            return slots[index];
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(
                texture, 
                new Rectangle(Game1.screenSize.X/2 - Game1.screenSize.X/3, Game1.screenSize.Y/2- Game1.screenSize.Y/3, (int)(Game1.screenSize.X/1.5), (int)(Game1.screenSize.Y/1.5)), 
                new Color(30,30,30,100)
            );
            int x = 0;
            int y = 0;
            for(int i = 0; i < capacity; i++)
            {

                spriteBatch.Draw(
                    texture,
                    new Rectangle(
                        Game1.screenSize.X/2 - Game1.screenSize.X/3 + x * (int)((Game1.screenSize.X/1.5) / (capacity/4)) , 
                        Game1.screenSize.Y/2 - Game1.screenSize.Y/3 + y * (int)((Game1.screenSize.Y/1.5) / (capacity/4)), 
                        (int)(Game1.screenSize.X/1.5) / (capacity/4), 
                        (int)(Game1.screenSize.Y/1.5) / (capacity/4)
                    ),
                    new Color(200,200,200,100)
                );

                x++;
                if (x >= 4)
                {
                    x = 0;
                    y++;
                }
            }
        }
    }
}