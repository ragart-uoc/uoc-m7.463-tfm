using UnityEngine;

namespace TFM
{
    /// <summary>
    /// ScriptableObject <c>Item</c> represents an item.
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        /// <value>Property <c>icon</c> represents the icon of the item.</value>
        [SerializeField]
        private Sprite icon;
        
        /// <value>Property <c>title</c> represents the title of the item.</value>
        [SerializeField]
        private string title;
        
        /// <value>Property <c>description</c> represents the description of the item.</value>
        [SerializeField]
        private string description;
        
        /// <value>Property <c>type</c> represents the type of the item.</value>
        [SerializeField]
        private ItemProperties.Types type;
        
        /// <value>Property <c>Icon</c> represents the icon of the item.</value>
        public Sprite Icon => icon;
        
        /// <value>Property <c>Title</c> represents the title of the item.</value>
        public string Title => title;
        
        /// <value>Property <c>Description</c> represents the description of the item.</value>
        public string Description => description;
        
        /// <value>Property <c>Type</c> represents the type of the item.</value>
        public ItemProperties.Types Type => type;
    }
}
