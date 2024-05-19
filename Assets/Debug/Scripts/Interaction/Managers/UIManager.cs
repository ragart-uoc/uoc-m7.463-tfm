using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TFM.Debug.Scripts.Interaction.Managers
{
    /// <summary>
    /// Class <c>UIManager</c> contains the logic for the UI.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static UIManager Instance;
        
        /// <value>Property <c>radialMenu</c> represents the radial menu.</value>
        public RadialMenu radialMenu;
        
        /// <value>Property <c>statusBarText</c> represents the status bar text.</value>
        public TextMeshProUGUI statusBarText;

        /// <value>Property <c>itemNotice</c> represents the item notice.</value>
        public Transform itemNotice;
        
        /// <value>Property <c>itemNoticeIcon</c> represents the item notice icon.</value>
        public Image itemNoticeIcon;
        
        /// <value>Property <c>itemNoticeActionText</c> represents the item notice action text.</value>
        public TextMeshProUGUI itemNoticeActionText;
        
        /// <value>Property <c>itemNoticeNameText</c> represents the item notice name text.</value>
        public TextMeshProUGUI itemNoticeNameText;

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
        /// Method <c>SetStatusBarText</c> sets the status bar text.
        /// </summary>
        /// <param name="text">The text to set.</param>
        public void SetStatusBarText(string text)
        {
            statusBarText.text = text;
        }
        
        /// <summary>
        /// Method <c>GetStatusBarText</c> gets the status bar text.
        /// </summary>
        /// <returns>The status bar text.</returns>
        public string GetStatusBarText()
        {
            return statusBarText.text;
        }

        /// <summary>
        /// Method <c>ShowMessage</c> shows a message for a duration.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="duration">The duration to show the message.</param>
        /// <param name="afterMessage">The message to show after the duration.</param>
        /// <returns>An IEnumerator.</returns>
        public IEnumerator ShowMessage(string message, float duration = 3f, string afterMessage = "")
        {
            statusBarText.text = message;
            yield return new WaitForSeconds(duration);
            statusBarText.text = afterMessage;
        }
        
        /// <summary>
        /// Method <c>ShowItemNotice</c> shows an item notice for a duration.
        /// </summary>
        /// <param name="icon">The icon to show.</param>
        /// <param name="actionText">The action text to show.</param>
        /// <param name="nameText">The name text to show.</param>
        /// <param name="duration">The duration to show the item notice.</param>
        public IEnumerator ShowItemNotice(Sprite icon, string actionText, string nameText, float duration = 3f)
        {
            itemNoticeIcon.sprite = icon;
            itemNoticeActionText.text = actionText;
            itemNoticeNameText.text = nameText;
            itemNotice.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            itemNotice.gameObject.SetActive(false);
        }
    }
}
