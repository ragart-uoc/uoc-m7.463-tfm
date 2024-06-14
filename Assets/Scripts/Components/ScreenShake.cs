using System.Collections;
using UnityEngine;
using TFM.Managers;

namespace TFM.Components
{
    /// <summary>
    /// Class that shakes the screen for a given duration.
    /// </summary>
    public class ScreenShake : MonoBehaviour
    {
        /// <value>Property <c>duration</c> represents the duration of the screen shake.</value>
        public float duration = 1.0f;

        /// <value>Property <c>clip</c> represents the audio clip of the screen shake.</value>
        public AudioClip clip;
        
        /// <value>Property <c>curve</c> represents the curve of the screen shake.</value>
        public AnimationCurve curve;
        
        /// <summary>
        /// Method <c>ShakeScreen</c> shakes the screen for a given duration.
        /// </summary>
        public void ShakeScreen()
        {
            StartCoroutine(ShakeScreenCoroutine());
        }

        /// <summary>
        /// Method <c>ShakeScreenCoroutine</c> shakes the screen for a given duration.
        /// </summary>
        private IEnumerator ShakeScreenCoroutine()
        {
            var startPosition = transform.position;
            var elapsedTime = 0.0f;
            if (clip != null)
                SoundManager.Instance.PlaySound(clip);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var strength = curve.Evaluate(elapsedTime / duration);
                transform.position = startPosition + Random.insideUnitSphere * strength;
                yield return null;
            }
            transform.position = startPosition;
        }
    }
}
