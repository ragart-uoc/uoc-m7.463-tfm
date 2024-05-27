using TFM.Entities;
using TFM.Managers;
using Event = TFM.Entities.Event;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>EventTriggerAction</c> represents the trigger event action.
    /// </summary>
    [System.Serializable]
    public class EventTriggerAction : ActionBase
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
