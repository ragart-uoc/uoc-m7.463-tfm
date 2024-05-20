using System.Collections.Generic;
using UnityEngine;
using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// ScriptableObject <c>ActionSequence</c> represents an action sequence.
    /// </summary>
    [CreateAssetMenu(fileName = "ActionSequence", menuName = "Actions/ActionSequence")]
    public class ActionSequence : ScriptableObject
    {
        /// <value>Property <c>actions</c> represents the list of actions.</value>
        [SerializeReference] public List<ActionBase> sequenceActions = new List<ActionBase>();

        /// <summary>
        /// Method <c>ExecuteSequence</c> executes the sequence.
        /// </summary>
        public void ExecuteSequence()
        {
            ActionManager.Instance.ExecuteSequence(sequenceActions);
        }
    }
}
