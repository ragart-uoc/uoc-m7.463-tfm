using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR) 
namespace TFM.Entities
{
    /// <summary>
    /// Class <c>EventListEditor</c> represents the editor for the event list.
    /// </summary>
    [CustomEditor(typeof(EventList))]
    public class EventListEditor : Editor
    {
        /// <summary>
        /// Method <c>OnInspectorGUI</c> is called to draw the inspector GUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var eventList = (EventList)target;
            if (GUILayout.Button("Update Event List"))
                UpdateEventList(eventList);
        }

        /// <summary>
        /// Method <c>UpdateEventList</c> updates the event list.
        /// </summary>
        /// <param name="eventList">The event list.</param>
        private void UpdateEventList(EventList eventList)
        {
            // Get all the events from the events folder
            var eventAssets = AssetDatabase.FindAssets("t:Event", new[] { eventList.eventsFolder });
            foreach (var eventAsset in eventAssets)
            {
                // Get the event
                var eventFile = AssetDatabase.LoadAssetAtPath<Event>(AssetDatabase.GUIDToAssetPath(eventAsset));
                // Set the event name from the event file name
                eventFile.eventName = eventFile.name;
                // Add the event to the event list
                if (!eventList.events.Contains(eventFile))
                    eventList.events.Add(eventFile);
            }
            EditorUtility.SetDirty(eventList);
            Debug.Log("Event list updated.");
        }
    }
}
#endif
