using System;
using System.Collections.Generic;
using UnityEngine;
using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// ScriptableObject <c>ActionSequence</c> represents an action sequence.
    /// </summary>
    [CreateAssetMenu(fileName = "ActionSequence", menuName = "Custom/ActionSequence")]
    public class ActionSequence : ScriptableObject
    {
        /// <value>Property <c>actions</c> represents the list of actions.</value>
        [SerializeReference] public List<ActionBase> sequenceActions = new List<ActionBase>();

        /// <summary>
        /// Method <c>ExecuteSequence</c> executes the sequence.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        public void ExecuteSequence(Action callback = null)
        {
            ActionManager.Instance.ExecuteSequence(sequenceActions, callback);
        }
    }
}
