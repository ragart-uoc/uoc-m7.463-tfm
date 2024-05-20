using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>SceneOpeningManager</c> contains the methods and properties needed for the opening sequence.
    /// </summary>
    public class SceneOpeningManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneOpeningManager Instance;

        /// <value>Property <c>companyLogo</c> represents the UI element containing the company logo.</value>
        public TextMeshProUGUI companyLogo;
        
        /// <value>Property <c>companyMotto</c> represents the UI element containing the company motto.</value>
        public TextMeshProUGUI companyMotto;
        
        /// <value>Property <c>blinkingText</c> represents the blinking text in the scene.</value>
        public TextMeshProUGUI blinkingText;

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
        /// Method <c>Start</c> is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private IEnumerator Start()
        {
            companyLogo.canvasRenderer.SetAlpha(0.0f);
            companyMotto.canvasRenderer.SetAlpha(0.0f);
            companyLogo.CrossFadeAlpha(1.0f, 1.5f, false);
            companyMotto.CrossFadeAlpha(1.0f, 1.5f, false);
            yield return new WaitForSeconds(2.5f);
            companyLogo.CrossFadeAlpha(0.0f, 1.5f, false);
            companyMotto.CrossFadeAlpha(0.0f, 1.5f, false);
            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene("Notice");
        }
    }
}
