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
        
        #region Radial menu
        
            /// <value>Property <c>radialMenu</c> represents the radial menu.</value>
            [Header("Radial Menu")]
            public RadialMenu radialMenu;
        
        #endregion
        
        #region Status bar

            /// <value>Property <c>statusBar</c> represents the status bar text.</value>
            [Header("Status Bar")]
            public TextMeshProUGUI statusBar;

        #endregion

        #region Notice

            /// <value>Property <c>notice</c> represents the item notice.</value>
            [Header("Notice")]
            public Transform notice;
            
            /// <value>Property <c>noticeIcon</c> represents the item notice icon.</value>
            public Image noticeIcon;
            
            /// <value>Property <c>noticeAction</c> represents the item notice action text.</value>
            public TextMeshProUGUI noticeAction;
            
            /// <value>Property <c>noticeText</c> represents the item notice name text.</value>
            public TextMeshProUGUI noticeText;

        #endregion
        
        #region Dialogue
        
            /// <value>Property <c>dialogue</c> represents the dialogue.</value>
            [Header("Dialogue")]
            public Transform dialogue;
            
            /// <value>Property <c>dialogueActorContainer</c> represents the dialogue actor container.</value>
            public Transform dialogueActorContainer;
            
            /// <value>Property <c>dialogueActorName</c> represents the dialogue actor name.</value>
            public TextMeshProUGUI dialogueActorName;
            
            /// <value>Property <c>dialogueText</c> represents the dialogue text.</value>
            public TextMeshProUGUI dialogueText;
        
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
        
        #region Status bar
        
            /// <summary>
            /// Method <c>SetStatusBarText</c> sets the status bar text.
            /// </summary>
            /// <param name="text">The text to set.</param>
            public void SetStatusBarText(string text)
            {
                statusBar.text = text;
            }
            
            /// <summary>
            /// Method <c>GetStatusBarText</c> gets the status bar text.
            /// </summary>
            /// <returns>The status bar text.</returns>
            public string GetStatusBarText()
            {
                return statusBar.text;
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
                statusBar.text = message;
                yield return new WaitForSeconds(duration);
                statusBar.text = afterMessage;
            }
        
        #endregion
        
        #region Notice
        
            /// <summary>
            /// Method <c>ShowItemNotice</c> shows an item notice for a duration.
            /// </summary>
            /// <param name="icon">The icon to show.</param>
            /// <param name="actionText">The action text to show.</param>
            /// <param name="nameText">The name text to show.</param>
            /// <param name="duration">The duration to show the item notice.</param>
            public IEnumerator ShowItemNotice(Sprite icon, string actionText, string nameText, float duration = 3f)
            {
                noticeIcon.sprite = icon;
                noticeAction.text = actionText;
                noticeText.text = nameText;
                notice.gameObject.SetActive(true);
                yield return new WaitForSeconds(duration);
                notice.gameObject.SetActive(false);
            }
            
        #endregion
        
        #region Dialogue
        
            /// <summary>
            /// Method <c>ShowDialogue</c> shows a dialogue.
            /// </summary>
            /// <param name="actor">The actor object.</param>
            /// <param name="text">The text.</param>
            /// <param name="position">The position of the actor. 0 is left, 1 is right.</param>
            public void ShowDialogue(DialogueActor actor, string text, int position = 0)
            {
                // Remove all childs of the dialogue actor container
                foreach (Transform child in dialogueActorContainer)
                    Destroy(child.gameObject);
                
                // Instantiate the actor image prefab
                var actorImage = Instantiate(actor.actorImagePrefab, dialogueActorContainer);
                var actorPosition = actorImage.transform.localPosition;
                var actorScale = actorImage.transform.localScale;
                if (position == 1) {
                    actorImage.transform.localPosition = new Vector3(-actorPosition.x, actorPosition.y, actorPosition.z);
                    actorImage.transform.localScale = new Vector3(-actorScale.x, actorScale.y, actorScale.z);
                }
                
                // Set the actor name
                dialogueActorName.text = actor.actorName;
                
                // Set the dialogue text
                dialogueText.text = text;
                
                // Show the dialogue
                dialogue.gameObject.SetActive(true);
            }

            /// <summary>
            /// Method <c>HideDialogue</c> hides the dialogue.
            /// </summary>
            public void HideDialogue()
            {
                dialogue.gameObject.SetActive(false);
            }

            #endregion
    }
}
