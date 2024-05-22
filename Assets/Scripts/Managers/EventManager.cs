using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TFM.Persistence;
using Event = TFM.Persistence.Event;

namespace TFM.Managers
{
    public class EventManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static EventManager Instance;

        /// <value>Property <c>AvailableEvents</c> represents the available events.</value>
        public List<Event> availableEvents;
        
        /// <value>Property <c>Events</c> represents the events.</value>
        public readonly Dictionary<Event, bool> Events = new Dictionary<Event, bool>();

        /// <summary>
        /// Method <c>Awake</c> initializes the game manager.
        /// </summary>
        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize the events
            foreach (var e in availableEvents)
                Events.Add(e, false);
        }
        
        /// <summary>
        /// Method <c>UpsertEventState</c> upserts the event state.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <param name="state">The state of the global event.</param>
        public void UpsertEventState(Event e, bool state)
        {
            if (!Events.TryAdd(e, state))
                Events[e] = state;
            GameManager.Instance.SaveGameState();
        }
        
        /// <summary>
        /// Method <c>GetEventState</c> gets the event state
        /// </summary>
        /// <param name="e">The  event.</param>
        public bool GetEventState(Event e)
        {
            return Events.ContainsKey(e) && Events[e];
        }
        
        /// <summary>
        /// Method <c>ExportData</c> exports the data.
        /// </summary>
        public List<EventData> ExportData()
        {
            return Events
                .Select(eventObject => new EventData(eventObject.Key.eventName, eventObject.Value))
                .ToList();
        }
        
        /// <summary>
        /// Method <c>ImportData</c> imports the data.
        /// </summary>
        /// <param name="data">The event data.</param>
        public void ImportData(List<EventData> data)
        {
            Events.Clear();
            foreach (var eventData in data)
            {
                var eventObject = availableEvents.Find(e => e.eventName == eventData.eventName);
                if (eventObject != null)
                    Events.Add(eventObject, eventData.eventState);
            }
        }

    }
}
