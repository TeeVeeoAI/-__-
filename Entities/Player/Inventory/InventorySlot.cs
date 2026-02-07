using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Items;

namespace ____.Entities.Player.Inventory
{
    public class InventorySlot
    {
        private BaseItem item;
        public BaseItem Item
        {
            get => item;
            set => item = value;
        }
    }
}