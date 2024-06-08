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
        /// <value>Property <c>sceneName</c> represents the scene name.</value>
        public string sceneName;
        
        /// <value>Property <c>initialAgeGroup</c> represents the initial age group.</value>
        public AgeGroupProperties.Groups initialAgeGroup;
        
        /// <value>Property <c>currentAgeGroup</c> represents the current age group.</value>
        public AgeGroupProperties.Groups currentAgeGroup;
        
        /// <value>Property <c>activeShowableObjects</c> represents the active showable objects.</value>
        public GameObject[] activeShowableObjects;
        
        /// <value>Property <c>activeShowableObjectsIds</c> represents the active showable objects IDs.</value>
        public int[] activeShowableObjectsIds;

        /// <value>Property <c>enablePause</c> represents whether the pause is enabled.</value>
        public bool enablePause;
        
        /// <summary>
        /// Method <c>LevelData</c> is the constructor of the class.
        /// </summary>
        /// <param name="sceneName">The scene name.</param>
        /// <param name="initialAgeGroup">The initial age group.</param>
        /// <param name="currentAgeGroup">The current age group.</param>
        /// <param name="activeShowableObjectsIds">The active showable objects IDs.</param>
        /// <param name="enablePause">Whether the pause is enabled.</param>
        public LevelData(string sceneName, AgeGroupProperties.Groups initialAgeGroup, AgeGroupProperties.Groups currentAgeGroup, int[] activeShowableObjectsIds, bool enablePause = true)
        {
            this.sceneName = sceneName;
            this.initialAgeGroup = initialAgeGroup;
            this.currentAgeGroup = currentAgeGroup;
            this.activeShowableObjects = Array.Empty<GameObject>();
            this.activeShowableObjectsIds = activeShowableObjectsIds;
            this.enablePause = enablePause;
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