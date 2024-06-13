using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TFM.Entities;
using Event = TFM.Entities.Event;

namespace TFM.Managers.SceneManagers
{
    public class SceneAlbumManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneAlbumManager Instance;
        
        #region Unity Events and Delegates
        
            /// <summary>
            /// Delegate <c>SceneAlbumManagerEvents</c> represents the scene album manager events.
            /// </summary>
            public delegate void SceneAlbumManagerEvents(List<Level> levels = null);
                
            /// <value>Event <c>Ready</c> represents the ready event.</value>
            public event SceneAlbumManagerEvents Ready;
            
        #endregion
        
        /// <value>Property <c>albumPhotoPrefab</c> represents the album photo prefab in the scene.</value>
        public GameObject albumPhotoPrefab;

        /// <value>Property <c>rootCanvas</c> represents the root canvas in the scene.</value>
        public Transform rootCanvas;

        /// <value>Property <c>dragEnabled</c> represents whether dragging is enabled in the scene.</value>
        [HideInInspector]
        public bool dragEnabled;

        /// <value>Property <c>dragRequiredEvents</c> represents the events required for dragging in the scene.</value>
        public List<Event> dragRequiredEvents;
        
        /// <value>Property <c>_albumRegions</c> represents the album regions in the scene.</value>
        private AlbumRegion[] _albumRegions;
        
        /// <value>Property <c>albumLevels</c> represents the levels that can be added as album photos.</value>
        private List<Level> _albumLevels;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
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
        }

        /// <summary>
        /// Method <c>OnEnable</c> is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            LevelManager.Instance.Ready += HandleLevelReady;
        }
        
        /// <summary>
        /// Method <c>OnDisable</c> is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            LevelManager.Instance.Ready -= HandleLevelReady;
        }
        
        /// <summary>
        /// Method <c>HandleLevelReady</c> handles the level ready event.
        /// </summary>
        /// <param name="level">The level.</param>
        private void HandleLevelReady(Level level)
        {
            // Get the album regions
            _albumRegions = FindObjectsOfType<AlbumRegion>();
            
            // Get the album levels
            _albumLevels = LevelManager.Instance.GetAlbumLevels();
            
            // Create the album photos
            CreateAlbumPhotos();
            
            // Check if the drag is enabled
            dragEnabled = dragRequiredEvents == null
                          || dragRequiredEvents.Count == 0
                          || dragRequiredEvents.Any(reqE => EventManager.Instance.GetEventState(reqE));

            // Invoke the ready event
            Ready?.Invoke(_albumLevels);
        }
        
        /// <summary>
        /// Method <c>CreateAlbumPhotos</c> creates the album photos.
        /// </summary>
        private void CreateAlbumPhotos()
        {
            foreach (var albumLevel in _albumLevels)
            {
                var albumPhoto = Instantiate(albumPhotoPrefab, rootCanvas).GetComponent<AlbumPhoto>();
                    albumPhoto.levelName = albumLevel.name;
                    albumPhoto.GetComponent<Image>().sprite = albumLevel.albumPhotoImage;
                foreach (var albumRegion in _albumRegions)
                {
                    if (albumRegion.ageGroup == albumLevel.currentAgeGroup)
                        albumRegion.AddPhoto(albumPhoto, false);
                }
            }
        }
    }
}
