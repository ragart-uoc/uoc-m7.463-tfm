using System.Collections;
using TFM.Persistence;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>CustomSceneManager</c> contains the logic for the custom scene manager.
    /// </summary>
    public class CustomSceneManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static CustomSceneManager Instance;
        
        /// <value>Property <c>currentLevel</c> represents the current level.</value>
        public Level currentLevel;

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
        }

        /// <summary>
        /// Method <c>LoadScene</c> loads the scene.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        /// <summary>
        /// Method <c>LoadSceneCoroutine</c> loads the scene coroutine.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        /// <returns>Returns the coroutine.</returns>
        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
