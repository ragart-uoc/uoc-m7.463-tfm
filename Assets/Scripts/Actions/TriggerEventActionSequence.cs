using TFM.Persistence;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>TriggerEventActionSequence</c> represents a sequence of actions that will be executed when a trigger event is fired.
    /// </summary>
    [System.Serializable]
    public struct TriggerEventActionSequence
    {
        /// <value>Property <c>triggerEvent</c> represents the trigger event that will start the action sequence.</value>
        public Event triggerEvent;
        
        /// <value>Property <c>actionSequence</c> represents the action sequence that will be executed when the trigger event is fired.</value>
        public ActionSequence actionSequence;
        
        /// <value>Property <c>completionEvent</c> represents the event that will be fired when the action sequence is completed.</value>
        public Event completionEvent;
    }
}
