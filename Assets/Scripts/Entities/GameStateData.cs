using System.Collections.Generic;
using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>GameStateData</c> represents the game state data.
    /// </summary>
    [System.Serializable]
    public class GameStateData
    {
        /// <value>Property <c>currentLevelName</c> represents the current level name.</value>
        public string currentLevelName;
        
        /// <value>Property <c>events</c> represents the events.</value>
        public List<EventData> events;
        
        /// <value>Property <c>levels</c> represents the levels.</value>
        public List<LevelData> levels;
        
        /// <value>Property <c>items</c> represents the items.</value>
        public List<ItemData> items;
        
        /// <summary>
        /// Method <c>GameStateData</c> is the constructor of the class.
        /// </summary>
        /// <param name="currentLevelName">The current level name.</param>
        /// <param name="events">The events.</param>
        /// <param name="levels">The levels.</param>
        /// <param name="items">The items.</param>
        public GameStateData(string currentLevelName, List<EventData> events, List<LevelData> levels, List<ItemData> items)
        {
            this.currentLevelName = currentLevelName;
            this.events = events;
            this.levels = levels;
            this.items = items;
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
        /// <param name="json">The JSON string.</param>
        public static GameStateData FromJson(string json)
        {
            return JsonUtility.FromJson<GameStateData>(json);
        }
    }
}
