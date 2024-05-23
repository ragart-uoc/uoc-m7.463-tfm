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

        /// <summary>
        /// Delegate <c>SceneChange</c> represents the scene change.
        /// </summary>
        public delegate void CustomSceneManagerEvents(string sceneName);

        /// <value>Event <c>LoadScene</c> represents the load scene event.</value>
        public event CustomSceneManagerEvents LoadScene;
        
        /// <value>Event <c>UnloadScene</c> represents the unload scene event.</value>
        public event CustomSceneManagerEvents UnloadScene;

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
        }

        /// <summary>
        /// Method <c>LoadNewScene</c> loads the new scene.
        /// </summary>
        /// <param name="sceneName">The scene name.</param>
        public void LoadNewScene(string sceneName)
        {
            StartCoroutine(LoadLevel(sceneName));
        }
        
        /// <summary>
        /// Method <c>UnloadCurrentScene</c> unloads the current scene.
        /// </summary>
        public void UnloadCurrentScene()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            OnUnloadScene(sceneName);
        }

        /// <summary>
        /// Method <c>LoadLevel</c> loads the level.
        /// </summary>
        /// <param name="sceneName">The scene name.</param>
        private IEnumerator LoadLevel(string sceneName)
        {
            OnUnloadScene(SceneManager.GetActiveScene().name);
            var asyncLevelLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncLevelLoad!.isDone)
                yield return null;
            LoadScene?.Invoke(sceneName);
        }

        /// <summary>
        /// Method <c>OnUnloadScene</c> unloads the scene.
        /// </summary>
        /// <param name="sceneName">The scene name.</param>
        private void OnUnloadScene(string sceneName)
        {
            UnloadScene?.Invoke(sceneName);
        }
    }
}
