using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using TFM.Entities;
using TFM.Managers;
using Event = TFM.Entities.Event;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>GraffitiMinigameManager</c> contains the logic for the graffiti minigame manager.
    /// </summary>
    public class GraffitiMinigameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static GraffitiMinigameManager Instance;

        #region Minigame settings

        /// <value>Property <c>minigameDuration</c> represents the minigame duration.</value>
        [Header("Minigame settings")] public float minigameDuration = 60f;

            /// <value>Property <c>_minigameTimer</c> represents the minigame timer.</value>
            private float _minigameTimer;

            /// <value>Property <c>policeTurnDegrees</c> represents the police turn degrees.</value>
            public float policeTurnDegrees = 180f;

            /// <value>Property <c>policeTurnSpeed</c> represents the police turn speed.</value>
            public float policeTurnSpeed = 1.0f;

            /// <value>Property <c>basePoliceTurnRate</c> represents the base police turn rate.</value>
            public float basePoliceTurnRate = 1.0f;

            /// <value>Property <c>minPoliceTurnRate</c> represents the min police turn rate.</value>
            public float minPoliceTurnRate = 1.0f;

            /// <value>Property <c>maxPoliceTurnRate</c> represents the max police turn rate.</value>
            public float maxPoliceTurnRate = 3.0f;

            /// <value>Property <c>_topPoliceTurnRate</c> represents the top police turn rate.</value>
            private float _topPoliceTurnRate;

            /// <value>Property <c>minGraffitiPaintingSpeed</c> represents the min graffiti painting speed.</value>
            public float minGraffitiPaintingSpeed = 3.0f;

            /// <value>Property <c>maxGraffitiPaintingSpeed</c> represents the max graffiti painting speed.</value>
            public float maxGraffitiPaintingSpeed = 5.0f;

            /// <value>Property <c>_graffitiPaintingSpeed</c> represents the graffiti painting speed.</value>
            private float _graffitiPaintingSpeed;

            /// <value>Property <c>_graffitiTimer</c> represents the graffiti timer.</value>
            private float _graffitiTimer;

            /// <value>Property <c>_isPainting</c> represents if the player is painting.</value>
            private bool _isPainting;

            /// <value>Property <c>police</c> represents the police.</value>
            public GameObject police;

            /// <value>Property <c>_policeTurnCoroutine</c> represents the police turn coroutine.</value>
            private Coroutine _policeTurnCoroutine;
            
            /// <value>Property <c>_policeRotationCoroutine</c> represents the police rotation coroutine.</value>
            private Coroutine _policeRotationCoroutine;

            /// <value>Property <c>_policeOriginalRotation</c> represents the police original rotation.</value>
            private Quaternion _policeOriginalRotation;

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
            [Header("Camera")] public ScreenShake mainCamera;

        #endregion

        #region UI

            /// <value>Property <c>scoreText</c> represents the score text.</value>
            [Header("UI")] public TextMeshProUGUI scoreText;

            /// <value>Property <c>graffitiImage</c> represents the graffiti image.</value>
            public Image graffitiImage;

        #endregion

        #region Dialogue

            /// <value>Property <c>successActor</c> represents the success actor.</value>
            [Header("Dialogue")] public DialogueActor successActor;

            /// <value>Property <c>failActor</c> represents the fail actor.</value>
            public DialogueActor failActor;
            
            /// <value>Property <c>dialogueDuration</c> represents the dialogue duration.</value>
            public float dialogueDuration = 3f;

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

            // Set the police turn rate
            _topPoliceTurnRate = maxPoliceTurnRate;

            // Set the graffiti painting speed
            _graffitiPaintingSpeed = maxGraffitiPaintingSpeed;

            // Save the police original transform
            _policeOriginalRotation = police.transform.rotation;

            // Set the score text
            _score = 0;
            UpdateScoreText();

            // Show the dialogue
            UIManager.Instance.ShowDialogueTimed(successActor,
                "That police woman thinks she's smarter than me! I'll show her my graffiti skills! I'll hold the button while she's not looking!");
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
            // Set the police turn rate and the graffiti painting speed
            var elapsedTime = minigameDuration - _minigameTimer;
            var timeRatio = Mathf.Clamp01(elapsedTime / (minigameDuration * 0.75f));
            _topPoliceTurnRate = Mathf.Lerp(maxPoliceTurnRate, minPoliceTurnRate, timeRatio);
            _graffitiPaintingSpeed = Mathf.Lerp(minGraffitiPaintingSpeed, maxGraffitiPaintingSpeed, timeRatio);
            // Check four mouse input
            if (Mouse.current.leftButton.wasPressedThisFrame)
                _isPainting = true;
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                _isPainting = false;
            // Check if the player is painting
            if (_isPainting)
                PaintGraffiti();
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
            // Set the game as active
            _gameActive = true;
            // Start the police turn coroutine
            _policeTurnCoroutine = StartCoroutine(PoliceTurnCoroutine());
        }

        /// <summary>
        /// Method <c>ResetMiniGame</c> resets the minigame.
        /// </summary>
        private void ResetMiniGame()
        {
            // Stop the police turn coroutines and turn the police to the right
            if (_policeTurnCoroutine != null)
                StopCoroutine(_policeTurnCoroutine);
            _policeTurnCoroutine = null;
            if (_policeRotationCoroutine != null)
                StopCoroutine(_policeRotationCoroutine);
            _policeRotationCoroutine = null;
            police.transform.rotation = _policeOriginalRotation;
            // Reset the graffiti
            graffitiImage.fillAmount = 0;
            _graffitiTimer = 0;
        }

        /// <summary>
        /// Method <c>EndMinigame</c> ends the minigame.
        /// </summary>
        private IEnumerator EndMinigame()
        {
            UIManager.Instance.ShowDialogueTimed(successActor, "Damn, she caught me! I'll have to run away!");
            yield return new WaitForSeconds(3f);
            EventManager.Instance.UpsertEventState(completionEvent, true);
            CustomSceneManager.Instance.LoadLevel(nextLevel.name);
        }

        /// <summary>
        /// Method <c>ShowSuccessDialogue</c> shows the dialogue.
        /// </summary>
        private IEnumerator ShowSuccessDialogue(Action callback = null)
        {
            var message = successDialogues[_successDialogueIndex];
            _successDialogueIndex = (_successDialogueIndex + 1) % successDialogues.Length;
            UIManager.Instance.ShowDialogueTimed(successActor, message, dialogueDuration);
            yield return new WaitForSeconds(dialogueDuration);
            callback?.Invoke();
        }

        /// <summary>
        /// Method <c>ShowFailDialogue</c> shows the dialogue.
        /// </summary>
        private IEnumerator ShowFailDialogue(Action callback = null)
        {
            mainCamera.ShakeScreen();
            var message = failDialogues[_failDialogueIndex];
            _failDialogueIndex = Mathf.Clamp(_failDialogueIndex + 1, 0, failDialogues.Length - 1);
            UIManager.Instance.ShowDialogueTimed(failActor, message, dialogueDuration);
            yield return new WaitForSeconds(dialogueDuration);
            callback?.Invoke();
        }

        /// <summary>
        /// Method <c>IncreaseScore</c> increases the score.
        /// </summary>
        public void IncreaseScore()
        {
            _score++;
            UpdateScoreText();
            StartCoroutine(ShowSuccessDialogue(StartNewGame));
        }

        /// <summary>
        /// Method <c>UpdateScore</c> updates the score text
        /// </summary>
        private void UpdateScoreText()
        {
            scoreText.text = "Score: " + _score;
        }

        /// <summary>
        /// Coroutine <c>PoliceTurnCoroutine</c> rotates the police.
        /// </summary>
        private IEnumerator PoliceTurnCoroutine()
        {
            while (_gameActive)
            {
                _policeRotationCoroutine = StartCoroutine(RotatePolice(policeTurnDegrees, policeTurnSpeed));
                yield return new WaitForSeconds(policeTurnSpeed);
                if (_isPainting)
                {
                    _isPainting = false;
                    _gameActive = false;
                    StartCoroutine(ShowFailDialogue(StartNewGame));
                    yield break;
                }
                yield return new WaitForSeconds(0.5f);
                _policeRotationCoroutine = StartCoroutine(RotatePolice(-policeTurnDegrees, policeTurnSpeed));
                yield return new WaitForSeconds(UnityEngine.Random.Range(basePoliceTurnRate, _topPoliceTurnRate));
            }
        }

        /// <summary>
        /// Coroutine <c>RotatePolice</c> rotates the police.
        /// </summary>
        /// <param name="degrees">The degrees.</param>
        /// <param name="time">The time.</param>
        private IEnumerator RotatePolice(float degrees, float time = 1.0f)
        {
            var startRotation = police.transform.rotation;
            var endRotation = startRotation * Quaternion.Euler(0, degrees, 0);
            var turnTime = 0.0f;
            while (turnTime < time)
            {
                turnTime += Time.deltaTime;
                police.transform.rotation = Quaternion.Slerp(startRotation, endRotation, turnTime / time);
                yield return null;
            }
        }

        /// <summary>
        /// Method <c>PaintGraffiti</c> paints the graffiti.
        /// </summary>
        private void PaintGraffiti()
        {
            _graffitiTimer += Time.deltaTime;
            graffitiImage.fillAmount = Mathf.Clamp01(_graffitiTimer / _graffitiPaintingSpeed);
            if (_graffitiTimer < _graffitiPaintingSpeed)
                return;
            _isPainting = false;
            _gameActive = false;
            IncreaseScore();
        }
    }
}
