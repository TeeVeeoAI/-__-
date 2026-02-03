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
        private List<InventorySlot> slots;
        private int capacity;
        public List<InventorySlot> AllSlots
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
            slots = new List<InventorySlot>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                slots.Add(new InventorySlot());
            }
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
                new Rectangle(Game1.screenSize.X/2 - Game1.screenSize.X/4, Game1.screenSize.Y/2- Game1.screenSize.Y/4, Game1.screenSize.X/2, Game1.screenSize.Y/2), 
                new Color(30,30,30,100)
            );
            for(int i = 0; i < capacity; i++)
            {
                spriteBatch.Draw(
                    texture,
                    new Rectangle((Game1.screenSize.X/2 - Game1.screenSize.X/4)/(capacity/4), (Game1.screenSize.Y/2- Game1.screenSize.Y/4)/4, Game1.screenSize.X/2, Game1.screenSize.Y/2),
                    new Color(50, 50, 50, 125)
                );
            }
        }
    }
}