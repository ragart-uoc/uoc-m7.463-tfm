using System.Collections.Generic;
using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>ItemList</c> represents a list of items.
    /// </summary>
    [CreateAssetMenu(fileName = "ItemList", menuName = "Custom/ItemList")]
    public class ItemList : ScriptableObject
    {
        /// <value>Property <c>items</c> represents the list of items.</value>
        public List<Item> items = new List<Item>();
        
        /// <value>Property <c>itemsFolder</c> represents the items folder.</value>
        public string itemsFolder = "Assets/ScriptableObjects/Items";
    }
}