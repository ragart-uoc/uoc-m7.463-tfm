using TMPro;
using UnityEngine;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>SceneIntroManager</c> contains the logic for the notice scene.
    /// </summary>
    public class SceneIntroManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneIntroManager Instance;
        
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
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame
                    || UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
                CustomSceneManager.Instance.LoadScene("Level_Room");
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            InvokeRepeating(nameof(BlinkText), 0, 0.5f);
        }
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void BlinkText()
        {
            blinkingText.enabled = !blinkingText.enabled;
        }
    }
}
