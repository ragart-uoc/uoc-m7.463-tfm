using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TFM.Entities;
using TFM.Managers;
using Event = TFM.Entities.Event;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>IceCreamMinigameManager</c> contains the logic for the ice cream minigame manager.
    /// </summary>
    public class IceCreamMinigameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static IceCreamMinigameManager Instance;
        
        #region Minigame settings
            
            /// <value>Property <c>minigameDuration</c> represents the minigame duration.</value>
            [Header("Minigame settings")]
            public float minigameDuration = 60f;
            
            /// <value>Property <c>_minigameTimer</c> represents the minigame timer.</value>
            private float _minigameTimer;
        
            /// <value>Property <c>maxRoundTime</c> represents the max time limit for the round.</value>
            public float maxRoundTime = 10f;

            /// <value>Property <c>minRoundTime</c> represents the min time limit for the round.</value>
            public float minRoundTime = 3f;

            /// <value>Property <c>_roundTimer</c> represents the timer for the round.</value>
            private float _roundTimer;
        
            /// <value>Property <c>completionEvent</c> represents the completion event.</value>
            public Event completionEvent;
            
            /// <value>Property <c>nextLevel</c> represents the next level.</value>
            public Level nextLevel;
        
            /// <value>Property <c>_gameActive</c> represents if the game is active.</value>
            private bool _gameActive;
        
            /// <value>Property <c>_victoryCount</c> represents the victory count.</value>
            private int _score;
            
        #endregion
        
        #region Camera
        
            /// <value>Property <c>camera</c> represents the camera.</value>
            [Header("Camera")]
            public ScreenShake mainCamera;
            
        #endregion
        
        #region UI

            /// <value>Property <c>customerSelectionPanel</c> represents the customer selection panel.</value>
            [Header("UI")]
            public Transform customerSelectionPanel;
            
            /// <value>Property <c>timerText</c> represents the timer text.</value>
            public TextMeshProUGUI timerText;
            
            /// <value>Property <c>playerSelectionPanel</c> represents the player selection panel.</value>
            public Transform playerSelectionPanel;

            /// <value>Property <c>playerSelectedPanel</c> represents the player selected panel.</value>
            public Transform playerSelectedPanel;
            
            /// <value>Property <c>playerSelectionButtonPrefab</c> represents the player selection button prefab.</value>
            public GameObject playerSelectionButtonPrefab;
        
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
        
        #region Color requests

            /// <value>Property <c>_colors</c> represents the colors.</value>
            private readonly Color[] _colors =
            {
                Color.black,
                Color.yellow,
                Color.green,
                Color.blue,
                Color.red,
                Color.magenta,
                Color.cyan,
                Color.white
            };
            
            /// <value>Property <c>_currentRequest</c> represents the current request.</value>
            private List<Color> _currentRequest;
            
            /// <value>Property <c>_playerSelection</c> represents the player selection.</value>
            private List<Color> _playerSelection;
        
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
            _roundTimer = maxRoundTime;

            // Set the colors for the player selection button
            foreach (Transform child in playerSelectionPanel)
                Destroy(child.gameObject);
            foreach (var color in _colors)
            {
                var button = Instantiate(playerSelectionButtonPrefab, playerSelectionPanel).GetComponent<Button>();
                button.GetComponent<Image>().color = color;
                button.onClick.AddListener(() => AddColor(color));
            }
            
            // Set the player selection
            foreach (var playerSelectecColor in playerSelectedPanel.GetComponentsInChildren<Image>())
                playerSelectecColor.color = Color.clear;
            _playerSelection = new List<Color>();
            
            // Set the initial dialogue
            UIManager.Instance.ShowDialogueTimed(successActor, "Come on, brother. Let's play! Serve me the ice cream I want.");
            yield return new WaitForSeconds(1f);

            // Start the minigame
            StartNewOrder();
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
            _roundTimer -= Time.deltaTime;
            // Print the round time
            var seconds = Mathf.FloorToInt(_roundTimer % 60);
            var milliseconds = Mathf.FloorToInt((_roundTimer * 100) % 100);
            timerText.text = $"{seconds:00}:{milliseconds:00}";
            // Check if the round is over
            if (_roundTimer > 0)
                return;
            _gameActive = false;
            CheckOrder();
        }

        /// <summary>
        /// Method <c>StartNewOrder</c> starts a new order.
        /// </summary>
        private void StartNewOrder()
        {
            // Check if the minigame is over
            if (_minigameTimer <= 0)
            {
                StartCoroutine(EndMinigame());
                return;
            }
            // Reset the selections
            _currentRequest = new List<Color>();
            for (var i = 0; i < 3; i++)
            {
                _currentRequest.Add(_colors[Random.Range(0, _colors.Length)]);
                customerSelectionPanel.GetChild(i).GetComponent<Image>().color = _currentRequest[i];
                playerSelectedPanel.GetChild(i).GetComponent<Image>().color = Color.clear;
            }
            _playerSelection.Clear();
            // Set the round timer
            var elapsedTime = minigameDuration - _minigameTimer;
            var timeRatio = Mathf.Clamp01(elapsedTime / (minigameDuration * 0.75f));
            _roundTimer = Mathf.Lerp(maxRoundTime, minRoundTime, timeRatio);
            // Set the game as active
            _gameActive = true;
        }

        /// <summary>
        /// Method <c>CheckOrder</c> checks the order.
        /// </summary>
        private void CheckOrder()
        {
            if (_currentRequest.Count == _playerSelection.Count)
            {
                if (_currentRequest.Where((t, i) => t != _playerSelection[i]).Any())
                {
                    ShowFailDialogue(StartNewOrder);
                    return;
                }
                _score++;
                ShowSuccessDialogue(StartNewOrder);
            }
            else
            {
                ShowFailDialogue(StartNewOrder);
            }
        }
        
        /// <summary>
        /// Method <c>EndMinigame</c> ends the minigame.
        /// </summary>
        private IEnumerator EndMinigame()
        {
            UIManager.Instance.ShowDialogueTimed(successActor, "It's over now, little brother...");
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
        /// Method <c>AddColor</c> adds a color to the player selection.
        /// </summary>
        /// <param name="color">The color to add.</param>
        private void AddColor(Color color)
        {
            if (!_gameActive)
                return;
            _playerSelection.Add(color);
            playerSelectedPanel.GetChild(_playerSelection.Count - 1).GetComponent<Image>().color = color;
            if (_playerSelection.Count != 3)
                return;
            _gameActive = false;
            CheckOrder();
        }
    }
}
