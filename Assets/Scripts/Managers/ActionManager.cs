using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TFM.Entities;

namespace TFM.Managers
{
    /// <summary>
    /// Class <c>ActionManager</c> contains the logic for the action manager.
    /// </summary>
    public class ActionManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static ActionManager Instance;
        
        /// <value>Property <c>waitBetweenActions</c> represents the wait between actions.</value>
        public float waitBetweenActions = 0.5f;
        
        /// <value>Property <c>_isExecutingSequence</c> represents if the sequence is executing.</value>
        private bool _isExecutingSequence;
        
        private Coroutine _currentSequenceCoroutine;
        
        private readonly Dictionary<List<ActionBase>, Action> _pendingSequences = new Dictionary<List<ActionBase>, Action>();

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
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            _isExecutingSequence = _currentSequenceCoroutine != null;
            if (_pendingSequences.Count == 0 || _isExecutingSequence)
                return;
            var sequence = _pendingSequences.First();
            _pendingSequences.Remove(sequence.Key);
            StartCoroutine(ExecuteSequenceCoroutine(sequence.Key, sequence.Value));
        }
        
        /// <summary>
        /// Method <c>ExecuteSequence</c> executes the sequence.
        /// </summary>
        /// <param name="sequenceActions">The list of actions to execute.</param>
        /// <param name="callback">The callback action.</param>
        public void ExecuteSequence(List<ActionBase> sequenceActions, Action callback = null)
        {
            if (_currentSequenceCoroutine != null)
            {
                _pendingSequences.Add(sequenceActions, callback);
                return;
            }
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
            UIManager.Instance?.EnableInteractions(false);
            UIManager.Instance?.SetStatusBarText("");
            foreach (var action in sequenceActions)
            {
                action.Execute();
                yield return new WaitForSeconds(waitBetweenActions);
                if (action.waitForInput == 1)
                    yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
            }
            yield return waitBetweenActions;
            UIManager.Instance?.EnableInteractions();
            callback?.Invoke();
        }
        
        /// <summary>
        /// Method <c>IsExecutingSequence</c> checks if the a is executing.
        /// </summary>
        public bool IsExecutingSequence()
        {
            return _isExecutingSequence;
        }
    }
}
