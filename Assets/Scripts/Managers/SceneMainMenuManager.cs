using UnityEngine;
using UnityEngine.SceneManagement;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>SceneMainMenuManager</c> contains the logic for the main menu scene.
    /// </summary>
    public class SceneMainMenuManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneMainMenuManager Instance;

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
        /// Method <c>StartGame</c> starts the game.
        /// </summary>
        public void StartGame()
        {
            SceneManager.LoadScene("3_Intro");
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