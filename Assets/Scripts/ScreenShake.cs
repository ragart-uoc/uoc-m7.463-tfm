using System.Collections;
using UnityEngine;

namespace TFM
{
    public class ScreenShake : MonoBehaviour
    {
        public float duration = 1.0f;
        public AnimationCurve curve;
        
        public void ShakeScreen()
        {
            StartCoroutine(ShakeScreenCoroutine());
        }

        private IEnumerator ShakeScreenCoroutine()
        {
            var startPosition = transform.position;
            var elapsedTime = 0.0f;
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
