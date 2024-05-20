using UnityEngine;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>GameManager</c> contains the logic for the game manager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public struct GameEvent
        {
            public string name;
            public bool isCompleted;
        }

        /// <value>Property <c>Instance</c> represents the instance of the game manager.</value>
        public static GameManager Instance;

        /// <value>Property <c>gameEvents</c> represents the game events.</value>
        public GameEvent[] gameEvents;
        

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
    }
}
