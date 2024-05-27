using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// Method <c>ItemData</c> represents the item data.
    /// </summary>
    [System.Serializable]
    public class ItemData
    {
        /// <value>Property <c>ItemName</c> represents the item name.</value>
        public string itemName;
        
        /// <value>Property <c>ItemState</c> represents the possible item states.</value>
        public enum ItemState
        {
            Picked,
            Discarded
        }
        
        /// <value>Property <c>itemState</c> represents the item state.</value>
        public ItemState itemState;
        
        /// <summary>
        /// Method <c>ItemData</c> is the constructor of the class.
        /// </summary>
        /// <param name="itemName">The item name.</param>
        /// <param name="itemState">The item state.</param>
        public ItemData(string itemName, ItemState itemState)
        {
            this.itemName = itemName;
            this.itemState = itemState;
        }
        
        /// <summary>
        /// Method <c>ToJson</c> converts the object to a JSON string.
        /// </summary>
        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
        
        /// <summary>
        /// Method <c>FromJson</c> converts a JSON string to an object.
        /// </summary>
        public static ItemData FromJson(string json)
        {
            return JsonUtility.FromJson<ItemData>(json);
        }
    }
}
