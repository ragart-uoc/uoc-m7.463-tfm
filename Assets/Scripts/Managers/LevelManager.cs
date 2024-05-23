using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TFM.Persistence;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>LevelManager</c> manages the level.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static LevelManager Instance;

        /// <value>Property <c>availableLevels</c> represents the available levels.</value>
        public List<Level> availableLevels;
        
        /// <value>Property <c>levels</c> represents the levels.</value>
        private readonly Dictionary<string, Level> _levels = new Dictionary<string, Level>();
        
        /// <value>Property <c>_currentLevelName</c> represents the current level name.</value>
        private string _currentLevelName;

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
            DontDestroyOnLoad(gameObject);
            
            // Initialize the levels
            foreach (var l in availableLevels)
            {
                if (!_levels.TryAdd(l.levelName, l))
                    _levels[l.levelName] = l;
                _levels[l.levelName].currentAgeGroup = l.initialAgeGroup;
                _levels[l.levelName].activeShowableObjects = Array.Empty<GameObject>();
            }
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _currentLevelName = CustomSceneManager.Instance.currentLevel.levelName;
            LateStart();
        }
        
        /// <summary>
        /// Method <c>LateStart</c> is called after the GameManager has loaded the game.
        /// </summary>
        private void LateStart()
        {
            StartCoroutine(LateStartCoroutine());
        }
        
        /// <summary>
        /// Method <c>LateStartCoroutine</c> is the coroutine for the late start.
        /// </summary>
        private IEnumerator LateStartCoroutine()
        {
            while (!GameManager.Instance.isGameLoaded)
                yield return null;
            RefreshSceneShowableObjects(true);
            UIManager.Instance.FadeOverlay(0f, 2f, Initialize);
        }

        /// <summary>
        /// Method <c>Initialize</c> initializes the level.
        /// </summary>
        public void Initialize()
        {
            // Check action sequences
            foreach (var levelSequenceEvent in
                     from levelSequenceEvent in _levels[_currentLevelName].levelSequenceEvents
                     where levelSequenceEvent.actionSequence != null
                     where levelSequenceEvent.completionEvent != null
                         && !EventManager.Instance.GetEventState(levelSequenceEvent.completionEvent)
                     where levelSequenceEvent.triggerEvent == null
                         || EventManager.Instance.GetEventState(levelSequenceEvent.triggerEvent)
                     select levelSequenceEvent)
            {
                EventManager.Instance.UpsertEventState(levelSequenceEvent.completionEvent, true);
                levelSequenceEvent.actionSequence.ExecuteSequence(UpdateLevel);
                return;
            }
            UIManager.Instance.EnableInteractions();
        }
        
        /// <summary>
        /// Method <c>UpdateLevel</c> updates the level.
        /// </summary>
        public void UpdateLevel()
        {
            RefreshSceneShowableObjects();
            UIManager.Instance.EnableInteractions();
            GameManager.Instance.SaveGameState();
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
                var wasActive = activeShowableObjectsSet.Contains(showableObject) || activeShowableObjectsIdsSet.Contains(showableObject.GetInstanceID());
                var isActive = showableObject.activeSelf;
                switch (shouldShow)
                {
                    case true when !isActive && !wasActive:
                        showableComponent.Fade(1.0f, 1.0f);
                        break;
                    case false when isActive || wasActive:
                        showableComponent.Fade(0f, 1.0f);
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
                .Select(levelObject => new LevelData(levelObject.Key, levelObject.Value.initialAgeGroup,
                    levelObject.Value.currentAgeGroup, levelObject.Value.activeShowableObjectsIds))
                .ToList();
        }
        
        /// <summary>
        /// Method <c>ImportData</c> imports the data.
        /// </summary>
        /// <param name="data">The level data.</param>
        public void ImportData(List<LevelData> data)
        {
            foreach (var level in data
                         .Select(levelData => availableLevels.Find(l => l.levelName == levelData.levelName))
                         .Where(level => level != null))
            {
                if (_levels.TryAdd(level.levelName, level))
                    _levels[level.levelName] = level;
            }
        }
    }
}