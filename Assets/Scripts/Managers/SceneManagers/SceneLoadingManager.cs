using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TFM.Managers.SceneManagers
{
    /// <summary>
    /// Class <c>SceneLoading</c> contains the methods and properties needed for the loading sequence.
    /// </summary>
    public class SceneLoadingManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneLoadingManager Instance;

        /// <value>Property <c>companyLogo</c> represents the UI element containing the company logo.</value>
        public Image gameLogo;

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
            InvokeRepeating(nameof(RotateLogo), 0, 0.01f);
            yield return new WaitForSeconds(2.0f);
            GameManager.Instance.LoadGameState(true);
        }
        
        /// <summary>
        /// Method <c>RotateLogo</c> rotates the logo.
        /// </summary>
        private void RotateLogo()
        {
            gameLogo.transform.Rotate(-Vector3.forward, 1.0f);
        }
    }
}