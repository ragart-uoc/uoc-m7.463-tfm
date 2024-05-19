using TFM.Debug.Scripts.Interaction.Managers;

namespace TFM.Debug.Scripts.Interaction.Actions
{
    /// <summary>
    /// Class <c>HideDialogueAction</c> represents a dialogue action.
    /// </summary>
    [System.Serializable]
    public class HideDialogueAction : ActionBase
    {
        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            UIManager.Instance.HideDialogue();
        }
    }
}