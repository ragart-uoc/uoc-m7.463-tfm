using UnityEngine;
using UnityEngine.UI;

namespace TFM.Managers.SceneManagers
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
            continueButton.interactable = GameManager.Instance.ExistsSaveData();
        }
    }
}