using System.Collections.Generic;
using UnityEngine;
using TFM.Actions;
using UnityEditor;

namespace TFM.Entities
{
    /// <summary>
    /// ScriptableObject <c>Level</c> represents the level.
    /// </summary>
    [CreateAssetMenu(fileName = "Level", menuName = "Custom/Level")]
    public class Level : ScriptableObject
    {
        /// <value>Property <c>sceneName</c> represents the scene name.</value>
        public string sceneName;
        
        #if UNITY_EDITOR
        /// <value>Property <c>sceneAsset</c> represents the scene asset.</value>
        public SceneAsset sceneAsset;
        #endif
        
        /// <value>Property <c>initialAgeGroup</c> represents the initial age group.</value>
        public AgeGroupProperties.Groups initialAgeGroup;

        /// <value>Property <c>levelSequenceEvents</c> represents the level sequence events.</value>
        public List<EventTriggerActionSequence> levelSequenceEvents;
        
        /// <value>Property <c>levelData</c> represents the level data.</value>
        [HideInInspector]
        public AgeGroupProperties.Groups currentAgeGroup;
        
        /// <value>Property <c>activeShowableObjects</c> represents the active showable objects.</value>
        [HideInInspector]
        public GameObject[] activeShowableObjects;
        
        /// <value>Property <c>activeShowableObjectsIds</c> represents the active showable objects IDs.</value>
        [HideInInspector]
        public int[] activeShowableObjectsIds;
    }
}
