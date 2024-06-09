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
        
        /// <value>Property <c>currentAgeGroup</c> represents the current age group.</value>
        //[HideInInspector]
        public AgeGroupProperties.Groups currentAgeGroup;
        
        /// <value>Property <c>activeShowableObjects</c> represents the active showable objects.</value>
        //[HideInInspector]
        public GameObject[] activeShowableObjects;
        
        /// <value>Property <c>activeShowableObjectsIds</c> represents the active showable objects IDs.</value>
        //[HideInInspector]
        public int[] activeShowableObjectsIds;

        /// <value>Property <c>enablePause</c> represents whether the pause is enabled.</value>
        public bool enablePause;

        /// <value>Property <c>enablePhotoAlbum</c> represents whether the photo album can be enabled.</value>
        public bool enablePhotoAlbum;
        
        /// <value>Property <c>photoAlbumRequiredEvents</c> represents the events required to show the photo album.</value>
        public List<Event> photoAlbumRequiredEvents;

        /// <value>Property <c>levelSequenceEvents</c> represents the level sequence events.</value>
        public List<EventTriggerActionSequence> levelSequenceEvents;
        
        /// <summary>
        /// Method <c>CreateInstance</c> creates an instance of the class.
        /// </summary>
        public static Level CreateInstance()
        {
            return CreateInstance<Level>();
        }
        
        /// <summary>
        /// Method <c>CreateInstanceFromData</c> creates an instance of the class from the data.
        /// </summary>
        /// <param name="levelData">The level data.</param>
        public static Level CreateInstanceFromData(LevelData levelData)
        {
            var newLevel = CreateInstance<Level>();
            newLevel.ImportData(levelData);
            return newLevel;
        }
        
        /// <summary>
        /// Method <c>CreateInstanceFromAnother</c> creates an instance of the class from another instance.
        /// </summary>
        /// <param name="level">The level instance.</param>
        public static Level CreateInstanceFromAnother(Level level)
        {
            var newLevel = CreateInstance<Level>();
            newLevel.name = level.name;
            newLevel.sceneName = level.sceneName;
            newLevel.initialAgeGroup = level.initialAgeGroup;
            newLevel.currentAgeGroup = level.currentAgeGroup;
            newLevel.levelSequenceEvents = level.levelSequenceEvents;
            newLevel.activeShowableObjects = level.activeShowableObjects;
            newLevel.activeShowableObjectsIds = level.activeShowableObjectsIds;
            newLevel.enablePause = level.enablePause;
            newLevel.enablePhotoAlbum = level.enablePhotoAlbum;
            newLevel.photoAlbumRequiredEvents = level.photoAlbumRequiredEvents;
            return newLevel;
        }
        
        /// <summary>
        /// Method <c>ImportData</c> imports the data.
        /// </summary>
        /// <param name="levelData">The level data.</param>
        public void ImportData(LevelData levelData)
        {
            name = levelData.name;
            sceneName = levelData.sceneName;
            currentAgeGroup = levelData.currentAgeGroup;
            activeShowableObjectsIds = levelData.activeShowableObjectsIds;
        }
    }
}
