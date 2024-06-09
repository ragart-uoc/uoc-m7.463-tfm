using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TFM.Entities;
using Event = TFM.Entities.Event;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>LevelManager</c> manages the level.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static LevelManager Instance;
        
        #region Unity Events and Delegates
        
            /// <summary>
            /// Delegate <c>LevelManagerEvents</c> represents the level manager events.
            /// </summary>
            public delegate void LevelManagerEvents(Level level);
            
            /// <value>Event <c>Ready</c> represents the ready event.</value>
            public event LevelManagerEvents Ready;

            /// <value>Event <c>OnLevelUpdated</c> represents the level updated event.</value>
            public event LevelManagerEvents LevelUpdated;
            
        #endregion
        
        #region Levels and scenes
            
            /// <value>Property <c>levels</c> represents the levels.</value>
            private readonly Dictionary<string, Level> _levels = new Dictionary<string, Level>();

            /// <value>Property <c>_currentLevelName</c> represents the current level name.</value>
            private string _currentLevelName;
            
        #endregion

        /// <summary>
        /// Method <c>Awake</c> initializes the game manager.
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
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Method <c>OnEnable</c> is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            GameManager.Instance.Ready += HandleGameReady;
            EventManager.Instance.EventTriggered += HandleEventTriggered;
        }
        
        /// <summary>
        /// Method <c>OnDisable</c> is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            GameManager.Instance.Ready -= HandleGameReady;
            EventManager.Instance.EventTriggered -= HandleEventTriggered;
        }

        [SerializeField]
        private Level[] showAvailableLevels;

        [SerializeField]
        private Level[] showLevels;
        
        /// <summary>
        /// Method <c>HandleGameReady</c> is called when the game is ready.
        /// </summary>
        /// <param name="levelName">The name of the level.</param>
        /// <param name="sceneName">The name of the scene.</param>
        private void HandleGameReady(string levelName, string sceneName)
        {
            _currentLevelName = levelName;

            // Initialize the levels
            foreach (var level in CustomSceneManager.Instance.availableLevels.levels
                         .Select(Level.CreateInstanceFromAnother))
            {
                if (!_levels.TryAdd(level.name, level))
                    _levels[level.name] = level;
                _levels[level.name].currentAgeGroup = level.initialAgeGroup;
                _levels[level.name].activeShowableObjects = Array.Empty<GameObject>();
            }

            showAvailableLevels = CustomSceneManager.Instance.availableLevels.levels.ToArray();
            showLevels = _levels.Values.ToArray();
            
            // Import data
            ImportData(GameManager.Instance.gameStateData.levels);
            
            // Ready event
            Ready?.Invoke(_levels[_currentLevelName]);
            
            // Show the overlay
            UIManager.Instance.ShowHideOverlay();
            
            // Refresh the scene showable objects
            RefreshSceneShowableObjects(true);
            
            // Fade out the overlay
            UIManager.Instance.FadeOverlay(0f, 2f, Initialize);
        }
        
        /// <summary>
        /// Method <c>HandleEventTriggered</c> handles the event triggered.
        /// </summary>
        private void HandleEventTriggered(Event e)
        {
            ExecuteActionSequences(UpdateLevel);
        }

        /// <summary>
        /// Method <c>Initialize</c> initializes the level.
        /// </summary>
        private void Initialize()
        {
            ExecuteActionSequences(UpdateLevel);
        }
        
        /// <summary>
        /// Method <c>ExecuteActionSequences</c> executes the action sequences.
        /// </summary>
        /// <param name="callback">The callback.</param>
        private void ExecuteActionSequences(Action callback)
        {
            foreach (var levelSequenceEvent in
                from levelSequenceEvent in _levels[_currentLevelName].levelSequenceEvents
                where levelSequenceEvent.actionSequence != null
                where levelSequenceEvent.triggerEvent == null
                    || EventManager.Instance.GetEventState(levelSequenceEvent.triggerEvent)
                where levelSequenceEvent.completionEvent == null
                      || !EventManager.Instance.GetEventState(levelSequenceEvent.completionEvent)
                select levelSequenceEvent)
            {
                levelSequenceEvent.actionSequence.ExecuteSequence(callback);
                return;
            }
            callback?.Invoke();
        }
        
        /// <summary>
        /// Method <c>UpdateLevel</c> updates the level.
        /// </summary>
        private void UpdateLevel()
        {
            RefreshSceneShowableObjects();
            UIManager.Instance.EnableInteractions();
            LevelUpdated?.Invoke(_levels[_currentLevelName]);
        }
        
        /// <summary>
        /// Method <c>GetShowableObjects</c> gets the active showable objects.
        /// </summary>
        private GameObject[] GetShowableObjects(bool activeOnly = false)
        {
            return Resources.FindObjectsOfTypeAll<ObjectShowable>()
                .Where(showableObject => !activeOnly || showableObject.gameObject.activeSelf)
                .Select(showableObject => showableObject.gameObject)
                .ToArray();
        }
        
        /// <summary>
        /// Method <c>RefreshSceneShowableObjects</c> refreshes the scene showable objects.
        /// </summary>
        private void RefreshSceneShowableObjects(bool hideAll = false)
        {
            var showableObjects = GetShowableObjects();
            var currentLevel = _levels[_currentLevelName];
            var activeShowableObjectsSet = new HashSet<GameObject>(currentLevel.activeShowableObjects);
            var activeShowableObjectsIdsSet = new HashSet<int>(currentLevel.activeShowableObjectsIds);
            foreach (var showableObject in showableObjects)
            {
                if (hideAll)
                    showableObject.SetActive(false);
                var showableComponent = showableObject.GetComponent<ObjectShowable>();
                var shouldShow = showableComponent.CheckConditions(EventManager.Instance.Events, currentLevel.currentAgeGroup);
                var wasActive = activeShowableObjectsSet.Contains(showableObject)
                                || activeShowableObjectsIdsSet.Contains(showableObject.GetInstanceID());
                var isActive = showableObject.activeSelf;
                switch (shouldShow)
                {
                    case true when !isActive && !wasActive:
                        showableComponent.Fade(1.0f, 1.0f);
                        break;
                    case false when isActive || wasActive:
                        showableComponent.Fade(0f, 1.0f);
                        showableComponent.gameObject.SetActive(false);
                        break;
                    default:
                        showableObject.SetActive(shouldShow);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Method <c>ExportData</c> exports the data.
        /// </summary>
        public List<LevelData> ExportData()
        {
            _levels[_currentLevelName].activeShowableObjects = GetShowableObjects(true);
            _levels[_currentLevelName].activeShowableObjectsIds = _levels[_currentLevelName].activeShowableObjects
                .Select(showableObject => showableObject.GetInstanceID())
                .ToArray();
            return _levels
                .Select(levelObject => new LevelData(
                    levelObject.Key,
                    levelObject.Value.sceneName,
                    levelObject.Value.currentAgeGroup,
                    levelObject.Value.activeShowableObjectsIds))
                .ToList();
        }

        /// <summary>
        /// Method <c>ImportData</c> imports the data.
        /// </summary>
        /// <param name="data">The level data.</param>
        private void ImportData(List<LevelData> data)
        {
            if (data == null)
                return;
            foreach (var levelData in data)
            {
                if (!_levels.TryGetValue(levelData.name, out var level))
                    continue;
                level.ImportData(levelData);
            }
            showLevels = _levels.Values.ToArray(); // DEBUG
        }

        /// <summary>
        /// Method <c>GetCurrentLevelName</c> gets the current level name.
        /// </summary>
        public string GetCurrentLevelName()
        {
            return _currentLevelName;
        }
        
        /// <summary>
        /// Method <c>CurrentLevelAllowsPause</c> checks if the current level allows pause.
        /// </summary>
        /// <returns>Whether the level allows pause.</returns>
        public bool CurrentLevelAllowsPause()
        {
            return _levels[_currentLevelName].enablePause;
        }

        /// <summary>
        /// Method <c>ChangeAgeGroup</c> changes the age group.
        /// </summary>
        /// <param name="ageGroup">The age group.</param>
        public void ChangeAgeGroup(AgeGroupProperties.Groups ageGroup)
        {
            UIManager.Instance.EnableInteractions(false);
            _levels[_currentLevelName].currentAgeGroup = ageGroup;
            UpdateLevel();
        }
    }
}