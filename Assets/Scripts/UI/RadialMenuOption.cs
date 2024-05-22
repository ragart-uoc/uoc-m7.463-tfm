using UnityEngine;
using TFM.Persistence;

namespace TFM.UI
{
    /// <summary>
    /// Struct <c>RadialMenuOption</c> contains the logic for the radial menu option.
    /// </summary>
    [System.Serializable]
    public struct RadialMenuOption 
    {
        /// <value>Property <c>icon</c> represents the icon image.</value>
        public Sprite icon;
        
        /// <value>Property <c>title</c> represents the title of the option.</value>
        public string title;
        
        /// <value>Property <c>type</c> represents the type of the option.</value>
        public ItemProperties.Types type;
        
        /// <value>Property <c>item</c> represents the item.</value>
        public Item item;
    }
}
