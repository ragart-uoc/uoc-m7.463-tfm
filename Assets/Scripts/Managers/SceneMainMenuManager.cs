using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>SceneMainMenuManager</c> contains the logic for the main menu scene.
    /// </summary>
    public class SceneMainMenuManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneMainMenuManager Instance;
        
        /// <value>Property <c>continueButton</c> represents the continue button in the scene.</value>
        public Button continueButton;
        
        /// <value>Property <c>_saveFilePath</c> represents the path to the save file.</value>
        private string _saveFilePath;

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
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        public void Start()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            continueButton.interactable = File.Exists(_saveFilePath);
        }

        /// <summary>
        /// Method <c>StartGame</c> starts the game.
        /// </summary>
        public void StartGame()
        {
            // Destroy save file and GameManager
            if (File.Exists(_saveFilePath))
                File.Delete(_saveFilePath);
            Destroy(GameManager.Instance.gameObject);
            CustomSceneManager.Instance.LoadNewScene("3_Intro");
        }
        
        /// <summary>
        /// Method <c>LoadGame</c> loads the game.
        /// </summary>
        public void LoadGame()
        {
            
            CustomSceneManager.Instance.LoadNewScene("Loading");
        }

        /// <summary>
        /// Method <c>QuitGame</c> is used to quit the game.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}