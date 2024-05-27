using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TFM.Entities;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>CustomSceneManager</c> contains the logic for the custom scene manager.
    /// </summary>
    public class CustomSceneManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static CustomSceneManager Instance;
        
        #region Unity Events and Delegates

        /// <summary>
        /// Delegate <c>SceneChange</c> represents the scene change.
        /// </summary>
        /// <param name="levelName">The name of the level.</param>
        /// <param name="sceneName">The name of the scene.</param>
        public delegate void CustomSceneManagerEvents(string levelName, string sceneName);
            
            /// <value>Event <c>LoadLevel</c> represents the load level event.</value>
            public event CustomSceneManagerEvents LoadedLevel;
            
            /// <value>Event <c>UnloadLevel</c> represents the unload level event.</value>
            public event CustomSceneManagerEvents UnloadedLevel;
            
        #endregion
        
        #region Levels and scenes
            
            /// <value>Property <c>availableLevels</c> represents the available levels.</value>
            public LevelList availableLevels;
            
            /// <value>Property <c>levels</c> represents the levels.</value>
            private readonly Dictionary<string, string> _levels = new Dictionary<string, string>();
            
            /// <value>Property <c>_currentSceneName</c> represents the current scene name.</value>
            private string _currentSceneName;
            
            /// <value>Property <c>_nextSceneName</c> represents the next scene name.</value>
            private string _nextSceneName;
            
            /// <value>Property <c>_currentLevelName</c> represents the current level name.</value>
            private string _currentLevelName;
            
            /// <value>Property <c>_nextLevelName</c> represents the next level name.</value>
            private string _nextLevelName;

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
            DontDestroyOnLoad(gameObject);
            
            // Get the current scene name
            _currentSceneName = SceneManager.GetActiveScene().name;

            // Initialize levels
            foreach (var level in availableLevels.levels)
            {
                _levels.Add(level.name, level.sceneName);
                if (level.sceneName == _currentSceneName)
                    _currentLevelName = level.name;
            }
        }

        /// <summary>
        /// Method <c>LoadLevel</c> loads new level.
        /// </summary>
        /// <param name="levelName">The level name.</param>
        public void LoadLevel(string levelName)
        {
            StartCoroutine(LoadLevelCoroutine(levelName));
        }

        /// <summary>
        /// Method <c>LoadLevelCoroutine</c> loads the level.
        /// </summary>
        /// <param name="levelName">The level name.</param>
        private IEnumerator LoadLevelCoroutine(string levelName)
        {
            OnUnloadLevel(_currentLevelName, _currentSceneName);
            _nextSceneName = _levels[levelName];
            _nextLevelName = levelName;
            var asyncLevelLoad = SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Single);
            while (!asyncLevelLoad!.isDone)
                yield return null;
            LoadedLevel?.Invoke(_nextLevelName, _nextSceneName);
        }

        /// <summary>
        /// Method <c>OnUnloadLevel</c> unloads the level.
        /// </summary>
        /// <param name="levelName">The level name.</param>
        /// <param name="sceneName">The scene name.</param>
        private void OnUnloadLevel(string levelName, string sceneName)
        {
            UnloadedLevel?.Invoke(levelName, sceneName);
        }
    }
}
