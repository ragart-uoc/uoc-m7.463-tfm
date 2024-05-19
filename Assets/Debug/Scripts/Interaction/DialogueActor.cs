using UnityEngine;

namespace TFM.Debug.Scripts.Interaction
{
    /// <summary>
    /// ScriptableObject <c>DialogueActor</c> represents a dialogue actor.
    /// </summary>
    [CreateAssetMenu(fileName="DialogueActor", menuName = "Dialogue/DialogueActor")]
    public class DialogueActor : ScriptableObject
    {
        /// <value>Property <c>actorImage</c> represents the actor image.</value>
        public Sprite actorImage;
        
        /// <value>Property <c>actorName</c> represents the actor name.</value>
        public string actorName;
    }
}
