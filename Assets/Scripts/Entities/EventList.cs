using System.Collections.Generic;
using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>EventList</c> represents a list of events.
    /// </summary>
    [CreateAssetMenu(fileName = "EventList", menuName = "Custom/EventList")]
    public class EventList : ScriptableObject
    {
        /// <value>Property <c>events</c> represents the list of events.</value>
        public List<Event> events = new List<Event>();
        
        /// <value>Property <c>eventsFolder</c> represents the events folder.</value>
        public string eventsFolder = "Assets/ScriptableObjects/Events";
    }
}