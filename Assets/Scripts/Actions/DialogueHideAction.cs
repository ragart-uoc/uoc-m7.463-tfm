using TFM.Entities;
using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>DialogueHideAction</c> represents the hide dialogue action.
    /// </summary>
    [System.Serializable]
    public class DialogueHideAction : ActionBase
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