using System;
using System.Collections;
using UnityEngine;

namespace TFM.Handlers
{
    public class CoroutineHandler : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the coroutine handler.</value>
        public static CoroutineHandler Instance;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }
        
        /// <summary>
        /// Method <c>StartStaticCoroutine</c> starts a coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to start.</param>
        /// <param name="callback">The callback to invoke after the coroutine finishes.</param>
        public static void StartStaticCoroutine(IEnumerator coroutine, Action callback = null)
        {
            Instance.StartCoroutine(Instance.StartCoroutineWithCallback(coroutine, callback));
        }
        
        /// <summary>
        /// Method <c>StartCoroutineWithCallback</c> starts a coroutine with a callback.
        /// </summary>
        /// <param name="coroutine">The coroutine to start.</param>
        /// <param name="callback">The callback to invoke after the coroutine finishes.</param>
        private IEnumerator StartCoroutineWithCallback(IEnumerator coroutine, Action callback = null)
        {
            yield return StartCoroutine(coroutine);
            callback?.Invoke();
        }
    }
}
