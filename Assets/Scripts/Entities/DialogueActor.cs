using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// ScriptableObject <c>DialogueActor</c> represents a dialogue actor.
    /// </summary>
    [CreateAssetMenu(fileName="DialogueActor", menuName = "Custom/DialogueActor")]
    public class DialogueActor : ScriptableObject
    {
        /// <value>Property <c>actorImagePrefab</c> represents the actor image.</value>
        public GameObject actorImagePrefab;
        
        /// <value>Property <c>actorName</c> represents the actor name.</value>
        public string actorName;
    }
}