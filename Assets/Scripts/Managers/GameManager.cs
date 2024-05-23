using System.IO;
using UnityEngine;
using TFM.Persistence;
using UnityEngine.SceneManagement;

namespace TFM.Managers
{

    /// <summary>
    /// Class <c>GameManager</c> contains the logic for the game manager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static GameManager Instance;
        
        /// <summary>
        /// Delegate <c>GameLoadedEventHandler</c> represents the game loaded event handler.
        /// </summary>
        public delegate void GameManagerEvents(string sceneName);
        
        /// <value>Event <c>OnSceneLoaded</c> represents the scene loaded event.</value>
        public event GameManagerEvents OnSceneLoaded;

        /// <value>Property <c>_saveFilePath</c> represents the path to the save file.</value>
        private string _saveFilePath;
        
        /// <value>Property <c>gameStateData</c> represents the game state data.</value>
        public GameStateData gameStateData;

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
            _saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        public void Start()
        {
            UIManager.Instance.EnableInteractions(false);
            LoadGameState();
        }
        
        /// <summary>
        /// Method <c>OnEnable</c> is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            CustomSceneManager.Instance.LoadScene += OnLoadScene;
        }
        
        /// <summary>
        /// Method <c>OnDisable</c> is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            CustomSceneManager.Instance.LoadScene -= OnLoadScene;
        }
        
        /// <summary>
        /// Method <c>OnLoadScene</c> is called when the scene is loaded.
        /// </summary>
        /// <param name="sceneName"></param>
        private void OnLoadScene(string sceneName)
        {
            UIManager.Instance.EnableInteractions(false);
            OnSceneLoaded?.Invoke(sceneName);
        }

        /// <summary>
        /// Method <c>SaveGameState</c> saves the game state.
        /// </summary>
        public void SaveGameState()
        {
            var currentSceneName = LevelManager.Instance.GetCurrentSceneName();
            var events = EventManager.Instance.ExportData();
            var levels = LevelManager.Instance.ExportData();
            var items = ItemManager.Instance.ExportData();
            gameStateData = new GameStateData(currentSceneName, events, levels, items);
            var json = JsonUtility.ToJson(gameStateData);
            File.WriteAllText(_saveFilePath, json);
        }

        /// <summary>
        /// Method <c>LoadGameState</c> loads the game state.
        /// </summary>
        private void LoadGameState()
        {
            if (!File.Exists(_saveFilePath))
                return;
            var json = File.ReadAllText(_saveFilePath);
            gameStateData = JsonUtility.FromJson<GameStateData>(json);
            if (!string.IsNullOrEmpty(gameStateData.currentSceneName))
                CustomSceneManager.Instance.LoadNewScene(gameStateData.currentSceneName);
        }
    }
}
