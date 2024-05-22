using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TFM.Persistence;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>ItemManager</c> contains the logic for managing the game.
    /// </summary>
    public class ItemManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static ItemManager Instance;
        
        /// <value>Property <c>availableItems</c> represents the available items.</value>
        public List<Item> availableItems;
        
        /// <value>Property <c>pickedItems</c> represents the picked items.</value>
        //[HideInInspector]
        public List<Item> pickedItems;
        
        /// <value>Property <c>discardedItems</c> represents the discarded items.</value>
        //[HideInInspector]
        public List<Item> discardedItems;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        public void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize the lists
            pickedItems = new List<Item>();
            discardedItems = new List<Item>();
        }
        
        /// <summary>
        /// Method <c>AddItem</c> adds an item to the picked items.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(Item item)
        {
            if (pickedItems.Contains(item))
                return;
            pickedItems.Add(item);
            GameManager.Instance.SaveGameState();
        }
        
        /// <summary>
        /// Method <c>DiscardItem</c> discards an item from the picked items.
        /// </summary>
        /// <param name="item">The item to discard.</param>
        public void DiscardItem(Item item)
        {
            if (!pickedItems.Contains(item))
                return;
            pickedItems.Remove(item);
            discardedItems.Add(item);
            GameManager.Instance.SaveGameState();
        }
        
        /// <summary>
        /// Method <c>IsItemPicked</c> checks if an item is picked.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True if the item is picked, false otherwise.</returns>
        private bool IsItemPicked(Item item)
        {
            return pickedItems.Contains(item);
        }
        
        /// <summary>
        /// Method <c>IsItemDiscarded</c> checks if an item is discarded.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True if the item is discarded, false otherwise.</returns>
        private bool IsItemDiscarded(Item item)
        {
            return discardedItems.Contains(item);
        }
        
        /// <summary>
        /// Method <c>IsItemPickedOrDiscarded</c> checks if an item is picked or discarded.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True if the item is picked or discarded, false otherwise.</returns>
        public bool IsItemPickedOrDiscarded(Item item)
        {
            return IsItemPicked(item) || IsItemDiscarded(item);
        }
        
        /// <summary>
        /// Method <c>GetPickedItems</c> gets the picked items.
        /// </summary>
        public List<Item> GetPickedItems(ItemProperties.Types type)
        {
            return pickedItems.FindAll(item => item.Type == type);
        }
        
        /// <summary>
        /// Method <c>ExportData</c> exports the data.
        /// </summary>
        public List<ItemData> ExportData()
        {
            var items = pickedItems.Select(item => new ItemData(item.Title, ItemData.ItemState.Picked)).ToList();
                items.AddRange(discardedItems.Select(item => new ItemData(item.Title, ItemData.ItemState.Discarded)));
            return items;
        }
        
        /// <summary>
        /// Method <c>ImportData</c> imports the data.
        /// </summary>
        /// <param name="data">The item data.</param>
        public void ImportData(List<ItemData> data)
        {
            pickedItems.Clear();
            discardedItems.Clear();
            foreach (var itemData in data)
            {
                var item = availableItems.Find(i => i.Title == itemData.itemName);
                if (item == null)
                    continue;
                if (itemData.itemState == ItemData.ItemState.Picked)
                    pickedItems.Add(item);
                else
                    discardedItems.Add(item);
            }
        }
    }
}