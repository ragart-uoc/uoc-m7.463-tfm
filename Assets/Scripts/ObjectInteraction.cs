using TFM.Actions;
using TFM.Persistence;

namespace TFM
{
    /// <summary>
    /// Struct <c>ObjectInteraction</c> contains pairs of items and action sequences for interactions.
    /// </summary>
    [System.Serializable]
    public struct ObjectInteraction
    {
        /// <value>Property <c>item</c> represents the item.</value>
        public Item item;
        
        /// <value>Property <c>actionSequence</c> represents the action sequence.</value>
        public ActionSequence actionSequence;
        
        /// <value>Property <c>discardItem</c> represents whether the item should be discarded after the interaction.</value>
        public bool discardItem;
    }
}
