using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TFM.Actions;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>ActionManager</c> contains the logic for the action manager.
    /// </summary>
    public class ActionManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static ActionManager Instance;

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
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Method <c>ExecuteSequence</c> executes the sequence.
        /// </summary>
        /// <param name="sequenceActions">The list of actions to execute.</param>
        /// <param name="callback">The callback action.</param>
        public void ExecuteSequence(List<ActionBase> sequenceActions, Action callback = null)
        {
            StartCoroutine(ExecuteSequenceCoroutine(sequenceActions, callback));
        }

        /// <summary>
        /// Method <c>ExecuteSequenceCoroutine</c> executes the sequence coroutine.
        /// </summary>
        /// <param name="sequenceActions">The list of actions to execute.</param>
        /// <param name="callback">The callback action.</param>
        /// <returns>Returns the coroutine.</returns>
        private IEnumerator ExecuteSequenceCoroutine(List<ActionBase> sequenceActions, Action callback = null)
        {
            UIManager.Instance.EnableInteractions(false);
            UIManager.Instance.SetStatusBarText("");
            foreach (var action in sequenceActions)
            {
                action.Execute();
                yield return new WaitForSeconds(1f);
                if (action.waitForInput == 1)
                    yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
            }
            callback?.Invoke();
        }
    }
}
