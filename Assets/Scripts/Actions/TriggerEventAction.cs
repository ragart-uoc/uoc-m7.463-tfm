using TFM.Managers;
using Event = TFM.Persistence.Event;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>TriggerEventAction</c> represents a trigger event action.
    /// </summary>
    [System.Serializable]
    public class TriggerEventAction : ActionBase
    {
        /// <value>Property <c>Event</c> represents the event to trigger.</value>
        public Event eventObject;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            EventManager.Instance.UpsertEventState(eventObject, true);
        }
    }
}
