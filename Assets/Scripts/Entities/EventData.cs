using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// Method <c>EventData</c> represents the event data.
    /// </summary>
    [System.Serializable]
    public class EventData
    {
        /// <value>Property <c>EventName</c> represents the event name.</value>
        public string eventName;

        /// <value>Property <c>EventState</c> represents the event state.</value>
        public bool eventState;
        
        /// <summary>
        /// Method <c>EventData</c> is the constructor of the class.
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <param name="eventState">The event state.</param>
        public EventData(string eventName, bool eventState)
        {
            this.eventName = eventName;
            this.eventState = eventState;
        }
        
        /// <summary>
        /// Method <c>ToJson</c> converts the object to a JSON string.
        /// </summary>
        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
        
        /// <summary>
        /// Method <c>FromJson</c> converts a JSON string to an object.
        /// </summary>
        public static EventData FromJson(string json)
        {
            return JsonUtility.FromJson<EventData>(json);
        }
    }
}
