using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TFM.Actions;

namespace TFM.Entities
{
    /// <summary>
    /// ScriptableObject <c>Level</c> represents the level.
    /// </summary>
    [CreateAssetMenu(fileName = "Level", menuName = "Custom/Level")]
    public class Level : ScriptableObject
    {
        #region Scene
        
            /// <value>Property <c>sceneName</c> represents the scene name.</value>
            [Header("Scene")]
            public string sceneName;
            
            #if UNITY_EDITOR
                /// <value>Property <c>sceneAsset</c> represents the scene asset.</value>
                public SceneAsset sceneAsset;
            #endif
        
        #endregion
        
        #region Age group
        
            /// <value>Property <c>initialAgeGroup</c> represents the initial age group.</value>
            [Header("Age group")]
            public AgeGroupProperties.Groups initialAgeGroup;
            
            /// <value>Property <c>currentAgeGroup</c> represents the current age group.</value>
            [HideInInspector]
            public AgeGroupProperties.Groups currentAgeGroup;
            
        #endregion
        
        #region Showable objects
            
            /// <value>Property <c>activeShowableObjects</c> represents the active showable objects.</value>
            [HideInInspector]
            public GameObject[] activeShowableObjects;
            
            /// <value>Property <c>activeShowableObjectsIds</c> represents the active showable objects IDs.</value>
            [HideInInspector]
            public int[] activeShowableObjectsIds;
            
        #endregion

        #region Pause

            /// <value>Property <c>enablePause</c> represents whether the pause is enabled.</value>
            [Header("Pause")]
            public bool enablePause;
            
        #endregion
        
        #region Photo album
        
            /// <value>Property <c>hasAlbumPhoto</c> represents whether the level has an album photo.</value>
            [Header("Photo album")]
            public bool hasAlbumPhoto;
            
            /// <value>Property <c>albumPhotoImage</c> represents the album photo image.</value>
            public Sprite albumPhotoImage;
            
            /// <value>Property <c>albumPhotoRequiredEvents</c> represents the events required to show the photo in the album.</value>
            public List<Event> albumPhotoRequiredEvents;
            
            /// <value>Property <c>showPhotoAlbumIndicator</c> represents whether the photo album indicator can be shown in the level.</value>
            public bool showPhotoAlbumIndicator;
            
            /// <value>Property <c>photoAlbumRequiredEvents</c> represents the events required to show the photo album indicator.</value>
            public List<Event> photoAlbumIndicatorRequiredEvents;
            
        #endregion
        
        #region Level sequence events
            
            /// <value>Property <c>levelSequenceEvents</c> represents the level sequence events.</value>
            [Header("Level sequence events")]
            public List<EventTriggerActionSequence> levelSequenceEvents;

        #endregion
        
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
            newLevel.hasAlbumPhoto = level.hasAlbumPhoto;
            newLevel.albumPhotoImage = level.albumPhotoImage;
            newLevel.albumPhotoRequiredEvents = level.albumPhotoRequiredEvents;
            newLevel.showPhotoAlbumIndicator = level.showPhotoAlbumIndicator;
            newLevel.photoAlbumIndicatorRequiredEvents = level.photoAlbumIndicatorRequiredEvents;
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
