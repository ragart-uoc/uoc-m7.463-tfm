using UnityEngine;

namespace TFM.Persistence
{
    /// <summary>
    /// ScriptableObject <c>Level</c> represents the event.
    /// </summary>
    [CreateAssetMenu(fileName = "Event", menuName = "Custom/Event")]
    public class Event : ScriptableObject
    {
        /// <value>Property <c>eventName</c> represents the event name.</value>
        public string eventName;
    }
}
