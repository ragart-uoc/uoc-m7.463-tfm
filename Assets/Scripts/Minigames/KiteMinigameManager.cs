using TFM.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Event = TFM.Entities.Event;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>KiteMinigameManager</c> contains the logic for the kite minigame manager.
    /// </summary>
    public class KiteMinigameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static KiteMinigameManager Instance;

        /// <value>Property <c>scoreText</c> represents the score text.</value>
        public TextMeshProUGUI scoreText;

        /// <value>Property <c>_score</c> represents the score.</value>
        private int _score;

        public Event completionEvent;

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
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _score = 0;
            UpdateScoreText();
        }

        /// <summary>
        /// Method <c>IncreaseScore</c> increases the score.
        /// </summary>
        public void IncreaseScore()
        {
            _score++;
            UpdateScoreText();
            if (_score < 10)
                return;
            EventManager.Instance.UpsertEventState(completionEvent, true);
            CustomSceneManager.Instance.LoadLevel("Park");
        }

        /// <summary>
        /// Method <c>UpdateScore</c> updates the score text
        /// </summary>
        private void UpdateScoreText()
        {
            scoreText.text = "Score: " + _score;
        }

        /// <summary>
        /// Method <c>GameOver</c> shows the game over message.
        /// </summary>
        public void GameOver()
        {
            //StartCoroutine(UIManager.Instance.ShowMessage("Game Over!"));
            RestartGame();
        }
        
        /// <summary>
        /// Method <c>RestartGame</c> restarts the game.
        /// </summary>
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}