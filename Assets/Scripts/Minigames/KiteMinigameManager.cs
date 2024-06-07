using System;
using System.Collections;
using UnityEngine;
using TMPro;
using TFM.Entities;
using TFM.Managers;
using Event = TFM.Entities.Event;
using Random = UnityEngine.Random;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>KiteMinigameManager</c> contains the logic for the kite minigame manager.
    /// </summary>
    public class KiteMinigameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static KiteMinigameManager Instance;
        
        #region Minigame settings
            
            /// <value>Property <c>minigameDuration</c> represents the minigame duration.</value>
            [Header("Minigame settings")]
            public float minigameDuration = 60f;
                
            /// <value>Property <c>_minigameTimer</c> represents the minigame timer.</value>
            private float _minigameTimer;
            
            /// <value>Property <c>obstacleHeightRange</c> represents the obstacle height range.</value>
            public float obstacleHeightRange = 1.5f;
            
            /// <value>Property <c>maxObstacleSpawnRate</c> represents the max obstacle spawn rate.</value>
            public float maxObstacleSpawnRate = 3f;
            
            /// <value>Property <c>minObstacleSpawnRate</c> represents the min obstacle spawn rate.</value>
            public float minObstacleSpawnRate = 1f;
            
            /// <value>Property <c>_obstacleSpawnRate</c> represents the obstacle spawn rate.</value>
            private float _obstacleSpawnRate;
            
            /// <value>Property <c>maxObstacleMoveSpeed</c> represents the max obstacle move speed.</value>
            public float maxObstacleMoveSpeed = 5f;
            
            /// <value>Property <c>minObstacleMoveSpeed</c> represents the min obstacle move speed.</value>
            public float minObstacleMoveSpeed = 2f;
            
            /// <value>Property <c>kiteController</c> represents the kite controller.</value>
            public KiteMinigameController kiteController;
            
            /// <value>Property <c>kiteMoveSpeed</c> represents the kite move speed.</value>
            public float kiteMoveSpeed = 3f;
            
            /// <value>Property <c>obstacleMoveSpeed</c> represents the obstacle move speed.</value>
            [HideInInspector]
            public float obstacleMoveSpeed;
            
            /// <value>Property <c>obstaclePrefab</c> represents the obstacle prefab.</value>
            public GameObject obstaclePrefab;
            
            /// <value>Property <c>obstacleContainer</c> represents the obstacle container.</value>
            public GameObject obstacleContainer;
            
            /// <value>Property <c>obstacleSpawnPoint</c> represents the obstacle spawn point.</value>
            public Transform obstacleSpawnPoint;
            
            /// <value>Property <c>obstacleDespawnPoint</c> represents the obstacle despawn point.</value>
            public Transform obstacleDespawnPoint;

            /// <value>Property <c>_obstacleSpawning</c> represents the obstacle spawning coroutine.</value>
            private Coroutine _obstacleSpawning;
        
            /// <value>Property <c>completionEvent</c> represents the completion event.</value>
            public Event completionEvent;

            /// <value>Property <c>nextLevel</c> represents the next level.</value>
            public Level nextLevel;
        
            /// <value>Property <c>_gameActive</c> represents if the game is active.</value>
            private bool _gameActive;

            /// <value>Property <c>_score</c> represents the score.</value>
            private int _score;
            
        #endregion
        
        #region Camera
        
            /// <value>Property <c>camera</c> represents the camera.</value>
            [Header("Camera")]
            public ScreenShake mainCamera;
            
        #endregion
        
        #region UI

            /// <value>Property <c>scoreText</c> represents the score text.</value>
            [Header("UI")]
            public TextMeshProUGUI scoreText;
            
        #endregion
        
        #region Dialogue
        
            /// <value>Property <c>successActor</c> represents the success actor.</value>
            [Header("Dialogue")]
            public DialogueActor successActor;
                
            /// <value>Property <c>failActor</c> represents the fail actor.</value>
            public DialogueActor failActor;
                
            /// <value>Property <c>successDialogues</c> represents the success dialogues.</value>
            public string[] successDialogues;
                
            /// <value>Property <c>_successDialogueIndex</c> represents the success dialogue index.</value>
            private int _successDialogueIndex;
                
            /// <value>Property <c>failDialogues</c> represents the fail dialogues.</value>
            public string[] failDialogues;
                
            /// <value>Property <c>_failDialogueIndex</c> represents the fail dialogue index.</value>
            private int _failDialogueIndex;
            
        #endregion

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
        private IEnumerator Start()
        {
            // Set the times
            _minigameTimer = minigameDuration;
            
            // Set the obstacle spawn rate
            _obstacleSpawnRate = maxObstacleSpawnRate;
            
            // Set the obstacle move speed
            obstacleMoveSpeed = minObstacleMoveSpeed;
            
            // Set the score text
            _score = 0;
            UpdateScoreText();
            
            // Show the dialogue
            UIManager.Instance.ShowDialogueTimed(successActor, "Come on, dad. Fly the kite with me! Remember, up and down, up and down! And be careful with the seagulls!");
            yield return new WaitForSeconds(3f);
            
            // Start the minigame
            StartNewGame();
        }
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (!_gameActive)
                return;
            // Update the timers
            _minigameTimer -= Time.deltaTime;
            // Set the obstacle spawn rate and the obstacle move speed
            var elapsedTime = minigameDuration - _minigameTimer;
            var timeRatio = Mathf.Clamp01(elapsedTime / (minigameDuration * 0.75f));
            _obstacleSpawnRate = Mathf.Lerp(maxObstacleSpawnRate, minObstacleSpawnRate, timeRatio);
            obstacleMoveSpeed = Mathf.Lerp(minObstacleMoveSpeed, maxObstacleMoveSpeed, timeRatio);
        }
        
        /// <summary>
        /// Method <c>StartNewGame</c> starts a new game.
        /// </summary>
        private void StartNewGame()
        {
            // Check if the minigame is over
            if (_minigameTimer <= 0)
            {
                StartCoroutine(EndMinigame());
                return;
            }
            // Reset the minigame
            ResetMiniGame();
            // Set the kite controller as active
            kiteController.gameObject.SetActive(true);
            // Start the obstacle spawning coroutine
            _obstacleSpawning = StartCoroutine(SpawnObstacles());
            // Set the game as active
            _gameActive = true;
        }
        
        /// <summary>
        /// Method <c>ResetMiniGame</c> resets the minigame.
        /// </summary>
        private void ResetMiniGame()
        {
            // Stop the obstacle spawning coroutine
            if (_obstacleSpawning != null)
                StopCoroutine(_obstacleSpawning);
            _obstacleSpawning = null;
            // Destroy all the obstacles
            foreach (var child in obstacleContainer.GetComponentsInChildren<KiteMinigameObstacle>())
                Destroy(child.gameObject);
            // Reset the score
            _score = 0;
            UpdateScoreText();
        }
        
        /// <summary>
        /// Method <c>EndMinigame</c> ends the minigame.
        /// </summary>
        private IEnumerator EndMinigame()
        {
            UIManager.Instance.ShowDialogueTimed(successActor, "Dad! There's an ambulance! Let's go see what happened!");
            yield return new WaitForSeconds(3f);
            EventManager.Instance.UpsertEventState(completionEvent, true);
            CustomSceneManager.Instance.LoadLevel(nextLevel.name);
        }
        
        /// <summary>
        /// Method <c>ShowSuccessDialogue</c> shows the dialogue.
        /// </summary>
        private void ShowSuccessDialogue(Action callback = null)
        {
            var message = successDialogues[_successDialogueIndex];
            _successDialogueIndex = (_successDialogueIndex + 1) % successDialogues.Length;
            UIManager.Instance.ShowDialogueTimed(successActor, message);
            callback?.Invoke();
        }

        /// <summary>
        /// Method <c>ShowFailDialogue</c> shows the dialogue.
        /// </summary>
        private void ShowFailDialogue(Action callback = null)
        {
            mainCamera.ShakeScreen();
            var message = failDialogues[_failDialogueIndex];
            _failDialogueIndex = Mathf.Clamp(_failDialogueIndex + 1, 0, failDialogues.Length - 1);
            UIManager.Instance.ShowDialogueTimed(failActor, message);
            callback?.Invoke();
        }

        /// <summary>
        /// Method <c>IncreaseScore</c> increases the score.
        /// </summary>
        public void IncreaseScore()
        {
            _score++;
            UpdateScoreText();
            if (_score % 5 == 0)
                ShowSuccessDialogue();
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
            _gameActive = false;
            kiteController.gameObject.SetActive(false);
            ShowFailDialogue(StartNewGame);
        }

        /// <summary>
        /// Method <c>SpawnObstacles</c> spawns the obstacles.
        /// </summary>
        private IEnumerator SpawnObstacles()
        {
            while (true)
            {
                var obstacleHeight = Random.Range(-obstacleHeightRange, obstacleHeightRange) + obstacleSpawnPoint.position.y;
                var spawnPosition = new Vector3(obstacleSpawnPoint.position.x, obstacleHeight, obstacleSpawnPoint.position.z);
                var obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
                obstacle.transform.SetParent(obstacleContainer.transform);
                yield return new WaitForSeconds(_obstacleSpawnRate);
            }
        }
    }
}