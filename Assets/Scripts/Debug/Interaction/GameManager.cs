using UnityEngine;
using TMPro;

namespace TFM.Debug.Interaction
{
    /// <summary>
    /// Class <c>GameManager</c> contains the logic for managing the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static GameManager Instance;

        /// <value>Property <c>highlightMaterial</c> represents the highlight material.</value>
        public Material highlightMaterial;
        
        /// <value>Property <c>statusBarText</c> represents the status bar text.</value>
        public TextMeshProUGUI statusBarText;

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
        /// Method <c>SetStatusBarText</c> sets the status bar text.
        /// <param name="text">The text to set.</param>
        /// </summary>
        public void SetStatusBarText(string text)
        {
            statusBarText.text = text;
        }
    }
}