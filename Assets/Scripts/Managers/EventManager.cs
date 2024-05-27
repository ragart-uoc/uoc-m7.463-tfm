using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TFM.Entities;
using Event = TFM.Entities.Event;

namespace TFM.Managers
{
    public class EventManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static EventManager Instance;
        
        #region Unity Events and Delegates

            /// <summary>
            /// Delegate <c>EventManagerEvents</c> represents the event manager events.
            /// </summary>
            public delegate void EventManagerEvents(Event e = null);
            
            /// <value>Event <c>Ready</c> represents the ready event.</value>
            public event EventManagerEvents Ready;

            /// <value>Event <c>OnEventTriggered</c> represents the event triggered event.</value>
            public event EventManagerEvents EventTriggered;
        
        #endregion
        
        #region Events

            /// <value>Property <c>AvailableEvents</c> represents the available events.</value>
            public List<Event> availableEvents;
            
            /// <value>Property <c>Events</c> represents the events.</value>
            public readonly Dictionary<Event, bool> Events = new Dictionary<Event, bool>();
        
        #endregion

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
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            ImportData(GameManager.Instance.gameStateData.events);
            Ready?.Invoke();
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
            EventTriggered?.Invoke(e);
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
        private void ImportData(List<EventData> data)
        {
            if (data == null)
                return;
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
