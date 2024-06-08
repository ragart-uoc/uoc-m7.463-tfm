using UnityEngine;
using UnityEngine.InputSystem;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>PauseManager</c> is a singleton class that manages the pause state of the game.
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static PauseManager Instance;
        
        /// <value>Property <c>IsPaused</c> represents the pause state of the game.</value>
        public bool isPaused;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        public void Awake()
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
            GameManager.Instance.Ready += HandleGameReady;
        }
        
        /// <summary>
        /// Method <c>OnDisable</c> is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            GameManager.Instance.Ready -= HandleGameReady;
        }
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        public void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame
                && LevelManager.Instance.CurrentLevelAllowsPause())
            {
                TogglePause();
            }
        }
        
        /// <summary>
        /// Method <c>HandleGameReady</c> is called when the game is ready.
        /// </summary>
        /// <param name="levelName">The name of the level.</param>
        /// <param name="sceneName">The name of the scene.</param>
        private void HandleGameReady(string levelName, string sceneName)
        {
            isPaused = false;
            UIManager.Instance.pauseResumeButton.onClick.AddListener(TogglePause);
            UIManager.Instance.pauseMainMenuButton.onClick.AddListener(GameManager.Instance.MainMenu);
            UIManager.Instance.pauseQuitButton.onClick.AddListener(GameManager.Instance.QuitGame);
        }
        
        /// <summary>
        /// Method <c>TogglePause</c> toggles the pause state of the game.
        /// </summary>
        public void TogglePause()
        {
            isPaused = !isPaused;
            // Pause or resume time and audio
            Time.timeScale = isPaused ? 0 : 1;
            AudioListener.pause = isPaused;
            // Show or hide the pause menu
            UIManager.Instance.TogglePauseMenu();
        }
    }
}
