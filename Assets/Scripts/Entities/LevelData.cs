using System;
using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>LevelData</c> represents the level data.
    /// </summary>
    [Serializable]
    public struct LevelData
    {
        /// <value>Property <c>name</c> represents the name.</value>
        public string name;

        /// <value>Property <c>sceneName</c> represents the scene name.</value>
        public string sceneName;
        
        /// <value>Property <c>currentAgeGroup</c> represents the current age group.</value>
        public AgeGroupProperties.Groups currentAgeGroup;
        
        /// <value>Property <c>activeShowableObjectsIds</c> represents the active showable objects IDs.</value>
        public int[] activeShowableObjectsIds;
        
        /// <summary>
        /// Method <c>LevelData</c> is the constructor of the class.
        /// </summary>
        /// <param name="name">The level name.</param>
        /// <param name="sceneName">The scene name.</param>
        /// <param name="currentAgeGroup">The current age group.</param>
        /// <param name="activeShowableObjectsIds">The active showable objects IDs.</param>
        public LevelData(string name, string sceneName, AgeGroupProperties.Groups currentAgeGroup, int[] activeShowableObjectsIds)
        {
            this.name = name;
            this.sceneName = sceneName;
            this.currentAgeGroup = currentAgeGroup;
            this.activeShowableObjectsIds = activeShowableObjectsIds;
        }
        
        /// <summary>
        /// Method <c>ToJson</c> converts the object to a JSON string.
        /// </summary>
        /// <returns>The JSON string.</returns>
        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
        
        /// <summary>
        /// Method <c>FromJson</c> converts a JSON string to an object.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <returns>The object.</returns>
        public static LevelData FromJson(string json)
        {
            return JsonUtility.FromJson<LevelData>(json);
        }
    }
}