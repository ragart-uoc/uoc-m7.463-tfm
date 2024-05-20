using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>ShowDialogueAction</c> represents a dialogue action.
    /// </summary>
    [System.Serializable]
    public class ShowDialogueAction : ActionBase
    {
        /// <value>Property <c>actor</c> represents the dialogue actor.</value>
        public DialogueActor actor;
        
        /// <value>Property <c>dialogueLine</c> represents the dialogue text.</value>
        public string dialogueLine;

        /// <value>Property <c>showLeft</c> represents if the actor is shown on the left.</value>
        public int position;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            UIManager.Instance.ShowDialogue(actor, dialogueLine, position);
        }
    }
}
