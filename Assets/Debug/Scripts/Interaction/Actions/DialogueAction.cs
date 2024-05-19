namespace TFM.Debug.Scripts.Interaction.Actions
{
    /// <summary>
    /// Class <c>DialogueAction</c> represents a dialogue action.
    /// </summary>
    [System.Serializable]
    public class DialogueAction : ActionBase
    {
        /// <value>Property <c>actor</c> represents the dialogue actor.</value>
        public DialogueActor actor;
        
        /// <value>Property <c>dialogueLine</c> represents the dialogue text.</value>
        public string dialogueLine;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            UnityEngine.Debug.Log($"{actor.actorName}: {dialogueLine}");
        }
    }
}
