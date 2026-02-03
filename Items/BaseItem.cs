using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ____.Items
{
    public abstract class BaseItem
    {
        protected string itemName;
        protected string description;
        protected int maxStackSize;
        protected ItemType itemType;
        public string ItemName
        {
            get => itemName;
        }
        public string Description
        {
            get => description;
        }
        public int MaxStackSize
        {
            get => maxStackSize;
        }
        public BaseItem(string itemName, string description, int maxStackSize = 1)
        {
            this.itemName = itemName;
            this.description = description;
            this.maxStackSize = maxStackSize;
        }
        public enum ItemType
        {
            Consumable,
            Equipment,
            Material,
            Quest
        }
    }
}