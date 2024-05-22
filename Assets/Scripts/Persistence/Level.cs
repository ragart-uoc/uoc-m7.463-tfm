using System.Collections.Generic;
using UnityEngine;
using TFM.Actions;

namespace TFM.Persistence
{
    /// <summary>
    /// ScriptableObject <c>Level</c> represents the level.
    /// </summary>
    [CreateAssetMenu(fileName = "Level", menuName = "Custom/Level")]
    public class Level : ScriptableObject
    {
        /// <value>Property <c>levelName</c> represents the level name.</value>
        public string levelName;
        
        /// <value>Property <c>initialAgeGroup</c> represents the initial age group.</value>
        public AgeGroupProperties.Groups initialAgeGroup;

        /// <value>Property <c>levelSequenceEvents</c> represents the level sequence events.</value>
        public List<TriggerEventActionSequence> levelSequenceEvents;
        
        /// <value>Property <c>levelData</c> represents the level data.</value>
        [HideInInspector]
        public AgeGroupProperties.Groups currentAgeGroup;
        
        /// <value>Property <c>activeShowableObjects</c> represents the active showable objects.</value>
        [HideInInspector]
        public GameObject[] activeShowableObjects;
        
        [HideInInspector]
        public int[] activeShowableObjectsIds;
    }
}
