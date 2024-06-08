namespace TFM.Entities
{
    /// <summary>
    /// Class <c>ItemProperties</c> contains the properties for the item picked.
    /// </summary>
    public abstract class ItemProperties
    {
        /// <value>Property <c>ItemType</c> represents the item types.</value>
        public enum Types
        {
            People,
            Places,
            Things,
            Events,
            Feelings
        }
    }
}
