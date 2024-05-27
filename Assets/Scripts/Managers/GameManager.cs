using System.IO;
using UnityEngine;
using TFM.Entities;
using Event = TFM.Entities.Event;

namespace TFM.Managers
{

    /// <summary>
    /// Class <c>GameManager</c> contains the logic for the game manager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static GameManager Instance;
        
        #region Unity Events and Delegates
        
            /// <summary>
            /// Delegate <c>GameLoadedEventHandler</c> represents the game loaded event handler.
            /// </summary>
            /// <param name="levelName">The name of the level.</param>
            /// <param name="sceneName">The name of the scene.</param>
            public delegate void GameManagerEvents(string levelName, string sceneName);
            
            /// <value>Event <c>OnSceneLoaded</c> represents the scene loaded event.</value>
            public event GameManagerEvents Ready;
            
        #endregion
        
        #region Game State
            
            /// <value>Property <c>gameStateData</c> represents the game state data.</value>
            public GameStateData gameStateData;

            /// <value>Property <c>_gameStateSavePath</c> represents the path to the save file.</value>
            private string _gameStateSavePath;
            
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
            
            // Don't destroy the game manager on load
            DontDestroyOnLoad(gameObject);
            
            // Set the path to the save file
            _gameStateSavePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        public void Start()
        {
            UIManager.Instance.EnableInteractions(false);
        }

        /// <summary>
        /// Method <c>OnEnable</c> is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            CustomSceneManager.Instance.LoadedLevel += OnLoadLevel;
            CustomSceneManager.Instance.UnloadedLevel += OnUnloadLevel;
        }

        /// <summary>
        /// Method <c>OnDisable</c> is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            CustomSceneManager.Instance.LoadedLevel -= OnLoadLevel;
            CustomSceneManager.Instance.UnloadedLevel -= OnUnloadLevel;
        }
        
        /// <summary>
        /// Method <c>OnLoadLevel</c> is called when the level is loaded.
        /// </summary>
        /// <param name="levelName">The name of the level.</param>
        /// <param name="sceneName">The name of the scene.</param>
        private void OnLoadLevel(string levelName, string sceneName)
        {
            if (EventManager.Instance != null)
                EventManager.Instance.EventTriggered += HandleEventTriggered;
            if (ItemManager.Instance != null)
            {
                ItemManager.Instance.ItemPicked += HandleItemPicked;
                ItemManager.Instance.ItemDiscarded += HandleItemDiscarded;
            }
            if (LevelManager.Instance != null)
                LevelManager.Instance.LevelUpdated += HandleLevelUpdated;
            UIManager.Instance.EnableInteractions(false);
            Ready?.Invoke(levelName, sceneName);
        }
        
        /// <summary>
        /// Method <c>OnUnloadLevel</c> is called when the level is unloaded.
        /// </summary>
        /// <param name="levelName">The name of the level.</param>
        /// <param name="sceneName">The name of the scene.</param>
        private void OnUnloadLevel(string levelName, string sceneName)
        {
            if (EventManager.Instance != null)
                EventManager.Instance.EventTriggered -= HandleEventTriggered;
            if (ItemManager.Instance != null)
            {
                ItemManager.Instance.ItemPicked -= HandleItemPicked;
                ItemManager.Instance.ItemDiscarded -= HandleItemDiscarded;
            }
            if (LevelManager.Instance != null)
                LevelManager.Instance.LevelUpdated -= HandleLevelUpdated;
        }

        /// <summary>
        /// Method <c>HandleEventTriggered</c> handles the event triggered.
        /// </summary>
        /// <param name="e">The event.</param>
        private void HandleEventTriggered(Event e)
        {
            SaveGameState();
        }
        
        /// <summary>
        /// Method <c>HandleItemPicked</c> handles the item picked.
        /// </summary>
        /// <param name="item">The item.</param>
        private void HandleItemPicked(Item item)
        {
            SaveGameState();
        }
        
        /// <summary>
        /// Method <c>HandleItemDiscarded</c> handles the item discarded.
        /// </summary>
        /// <param name="item">The item.</param>
        private void HandleItemDiscarded(Item item)
        {
            SaveGameState();
        }
        
        /// <summary>
        /// Method <c>HandleLevelUpdated</c> handles the level updated.
        /// </summary>
        /// <param name="level">The level.</param>
        private void HandleLevelUpdated(Level level)
        {
            SaveGameState();
        }

        /// <summary>
        /// Method <c>SaveGameState</c> saves the game state.
        /// </summary>
        public void SaveGameState()
        {
            UIManager.Instance.SetSaveIndicator(2.0f);
            var currentLevelName = LevelManager.Instance.GetCurrentLevelName();
            var events = EventManager.Instance.ExportData();
            var levels = LevelManager.Instance.ExportData();
            var items = ItemManager.Instance.ExportData();
            gameStateData = new GameStateData(currentLevelName, events, levels, items);
            var json = JsonUtility.ToJson(gameStateData);
            File.WriteAllText(_gameStateSavePath, json);
        }

        /// <summary>
        /// Method <c>LoadGameState</c> loads the game state.
        /// </summary>
        public void LoadGameState(bool loadScene = false)
        {
            if (!File.Exists(_gameStateSavePath))
                return;
            var json = File.ReadAllText(_gameStateSavePath);
            gameStateData = JsonUtility.FromJson<GameStateData>(json);
            if (loadScene
                    && !string.IsNullOrEmpty(gameStateData.currentLevelName))
                CustomSceneManager.Instance.LoadLevel(gameStateData.currentLevelName);
        }
    }
}
