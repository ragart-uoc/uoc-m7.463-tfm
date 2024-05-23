using System.IO;
using UnityEngine;
using TFM.Persistence;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>GameManager</c> contains the logic for the game manager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static GameManager Instance;

        /// <value>Property <c>_saveFilePath</c> represents the path to the save file.</value>
        private string _saveFilePath;
        
        /// <value>Property <c>isGameLoaded</c> represents if the game is loaded.</value>
        [HideInInspector]
        public bool isGameLoaded;

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
            _saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        public void Start()
        {
            UIManager.Instance.EnableInteractions(false);
            LoadGameState();
            isGameLoaded = true;
        }


        /// <summary>
        /// Method <c>SaveGameState</c> saves the game state.
        /// </summary>
        public void SaveGameState()
        {
            var events = EventManager.Instance.ExportData();
            var levels = LevelManager.Instance.ExportData();
            var items = ItemManager.Instance.ExportData();
            var gameStateData = new GameStateData(events, levels, items);
            var json = JsonUtility.ToJson(gameStateData);
            File.WriteAllText(_saveFilePath, json);
        }

        /// <summary>
        /// Method <c>LoadGameState</c> loads the game state.
        /// </summary>
        public void LoadGameState()
        {
            if (!File.Exists(_saveFilePath))
                return;
            var json = File.ReadAllText(_saveFilePath);
            var gameStateData = JsonUtility.FromJson<GameStateData>(json);
            EventManager.Instance.ImportData(gameStateData.events);
            LevelManager.Instance.ImportData(gameStateData.levels);
            ItemManager.Instance.ImportData(gameStateData.items);
        }
    }
}
