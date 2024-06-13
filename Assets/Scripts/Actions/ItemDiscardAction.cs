using TFM.Entities;
using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>ItemDiscardAction</c> represents the item discard action.
    /// </summary>
    [System.Serializable]
    public class ItemDiscardAction : ActionBase
    {
        /// <value>Property <c>item</c> represents the item to pick.</value>
        public Item item;
        
        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            ItemManager.Instance?.DiscardItem(item);
        }
    }
}