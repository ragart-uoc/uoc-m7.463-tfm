using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TFM.Managers;
using TFM.Managers.SceneManagers;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>AlbumRegion</c> represents the album regions.
    /// </summary>
    public class AlbumRegion : MonoBehaviour, IDropHandler
    {
        /// <value>Property <c>ageGroup</c> represents the age group of the album region.</value>
        public AgeGroupProperties.Groups ageGroup;
        
        /// <value>Property <c>_photoPlaceholders</c> represents the photo placeholders in the album region.</value>
        private AlbumPhotoPlaceholder[] _photoPlaceholders;

        /// <value>Property <c>lockedRegionIndicator</c> represents the locked region indicator.</value>
        public Transform lockedRegionIndicator;
        
        /// <value>Property <c>regionUnlocked</c> represents whether the region is unlocked.</value>
        private bool _regionUnlocked;

        /// <value>Property <c>requiredEvents</c> represents the events required for unlocking the region.</value>
        public List<Event> requiredEvents;
        
        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _photoPlaceholders = GetComponentsInChildren<AlbumPhotoPlaceholder>();
        }

        /// <summary>
        /// Method <c>OnEnable</c> is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            SceneAlbumManager.Instance.Ready += HandleSceneAlbumReady;
        }
        
        /// <summary>
        /// Method <c>OnDisable</c> is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            SceneAlbumManager.Instance.Ready -= HandleSceneAlbumReady;
        }
        
        /// <summary>
        /// Method <c>HandleSceneAlbumReady</c> handles the scene album ready event.
        /// </summary>
        /// <param name="levels">The levels in the scene album.</param>
        private void HandleSceneAlbumReady(List<Level> levels)
        {
            foreach (var level in levels)
            {
                Debug.Log(level.name);
                Debug.Log(level.initialAgeGroup.ToString());
                Debug.Log(level.currentAgeGroup.ToString());
            }
            var filteredLevels = levels
                .Where(l => l.initialAgeGroup == ageGroup || l.currentAgeGroup == ageGroup)
                .ToList();
            _regionUnlocked = CheckRegionLockStatus()
                              || filteredLevels.Count > 0;
            lockedRegionIndicator.gameObject.SetActive(!_regionUnlocked);
        }
        
        /// <summary>
        /// Method <c>OnDrop</c> is called when a photo is dropped in the album region.
        /// </summary>
        /// <param name="eventData">The data of the event.</param>
        public void OnDrop(PointerEventData eventData)
        {
            if (!_regionUnlocked || SceneAlbumManager.Instance.dragEnabled == false)
                return;
            // Get the album photo component
            var albumPhoto = eventData.pointerDrag.GetComponent<AlbumPhoto>();
            if (albumPhoto == null)
                return;
            // Change the level age group
            LevelManager.Instance.ChangeAgeGroup(albumPhoto.levelName, ageGroup);
            // Move the photo to the correct placeholder
            AddPhoto(albumPhoto);
        }
        
        /// <summary>
        /// Method <c>AddPhoto</c> adds a photo to the album region.
        /// </summary>
        /// <param name="albumPhoto"></param>
        /// <param name="animateMove"></param>
        public void AddPhoto(AlbumPhoto albumPhoto, bool animateMove = true)
        {
            foreach (var photoPlaceholder in _photoPlaceholders)
            {
                if (photoPlaceholder.level.name != albumPhoto.levelName)
                    continue;
                albumPhoto.SetPlaceholder(photoPlaceholder.transform, animateMove);
                break;
            }
        }
        
        /// <summary>
        /// Method <c>CheckRegionLockStatus</c> checks the region lock status.
        /// </summary>
        public bool CheckRegionLockStatus()
        {
            return requiredEvents == null
                   || requiredEvents.Count == 0
                   || requiredEvents.All(reqE => EventManager.Instance.GetEventState(reqE));
        }
    }
}
