using System.Collections.Generic;
using System.Linq;
using TFM.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Event = TFM.Persistence.Event;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>IceCreamMinigameManager</c> contains the logic for the ice cream minigame manager.
    /// </summary>
    public class IceCreamMinigameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static IceCreamMinigameManager Instance;

        /// <value>Property <c>customerSelectionPanel</c> represents the customer selection panel.</value>
        public Transform customerSelectionPanel;
        
        /// <value>Property <c>timerText</c> represents the timer text.</value>
        public TextMeshProUGUI timerText;
        
        /// <value>Property <c>timeLimit</c> represents the time limit.</value>
        public float timeLimit = 5f;
        
        /// <value>Property <c>playerSelectionPanel</c> represents the player selection panel.</value>
        public Transform playerSelectionPanel;

        /// <value>Property <c>playerSelectedPanel</c> represents the player selected panel.</value>
        public Transform playerSelectedPanel;
        
        /// <value>Property <c>playerSelectionButtonPrefab</c> represents the player selection button prefab.</value>
        public GameObject playerSelectionButtonPrefab;

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
        
        /// <value>Property <c>_timer</c> represents the timer.</value>
        private float _timer;
        
        /// <value>Property <c>_gameActive</c> represents if the game is active.</value>
        private bool _gameActive;
        
        /// <value>Property <c>completionEvent</c> represents the completion event.</value>
        public Event completionEvent;
        
        /// <value>Property <c>_victoryCount</c> represents the victory count.</value>
        private int _victoryCount;

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
            foreach (Transform child in playerSelectionPanel)
                Destroy(child.gameObject);
            foreach (var color in _colors)
            {
                var button = Instantiate(playerSelectionButtonPrefab, playerSelectionPanel).GetComponent<Button>();
                button.GetComponent<Image>().color = color;
                button.onClick.AddListener(() => AddColor(color));
            }
            _playerSelection = new List<Color>();
            StartNewOrder();
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (!_gameActive)
                return;
            _timer -= Time.deltaTime;
            var seconds = Mathf.FloorToInt(_timer % 60);
            var milliseconds = Mathf.FloorToInt((_timer * 100) % 100);
            timerText.text = $"{seconds:00}:{milliseconds:00}";
            if (_timer > 0)
                return;
            _gameActive = false;
            CheckOrder();
        }

        /// <summary>
        /// Method <c>StartNewOrder</c> starts a new order.
        /// </summary>
        private void StartNewOrder()
        {
            if (_victoryCount >= 5)
            {
                EventManager.Instance.UpsertEventState(completionEvent, true);
                CustomSceneManager.Instance.LoadNewScene("Level_IceCreamParlor");
                return;
            }
            _currentRequest = new List<Color>();
            for (var i = 0; i < 3; i++)
            {
                _currentRequest.Add(_colors[Random.Range(0, _colors.Length)]);
                customerSelectionPanel.GetChild(i).GetComponent<Image>().color = _currentRequest[i];
                playerSelectedPanel.GetChild(i).GetComponent<Image>().color = Color.clear;
            }
            _playerSelection.Clear();
            _timer = timeLimit;
            _gameActive = true;
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

        /// <summary>
        /// Method <c>CheckOrder</c> checks the order.
        /// </summary>
        private void CheckOrder()
        {
            if (_currentRequest.Count == _playerSelection.Count)
            {
                if (_currentRequest.Where((t, i) => t != _playerSelection[i]).Any())
                {
                    StartCoroutine(UIManager.Instance.ShowMessage("Wrong!", 2f));
                    StartNewOrder();
                    return;
                }
                StartCoroutine(UIManager.Instance.ShowMessage("Correct!", 2f));
                _victoryCount++;
                StartNewOrder();
            }
            else
            {
                StartCoroutine(UIManager.Instance.ShowMessage("Incomplete!", 2f));
                StartNewOrder();
            }
        }
    }
}
