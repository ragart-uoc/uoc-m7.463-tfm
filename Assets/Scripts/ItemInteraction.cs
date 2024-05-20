using TFM.Actions;

namespace TFM
{
    /// <summary>
    /// Struct <c>ItemInteraction</c> contains the logic for the item interaction.
    /// </summary>
    [System.Serializable]
    public struct ItemInteraction
    {
        /// <value>Property <c>item</c> represents the item.</value>
        public Item item;
        
        /// <value>Property <c>actionSequence</c> represents the action sequence.</value>
        public ActionSequence actionSequence;
    }
}
