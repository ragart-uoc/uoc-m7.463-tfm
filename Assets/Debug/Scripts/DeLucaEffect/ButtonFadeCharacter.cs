using System.Collections;
using UnityEngine;

namespace TFM.Debug.Scripts.DeLucaEffect
{
    /// <summary>
    /// Class <c>ButtonFadeCharacter</c> contains the logic for fading a character in and out.
    /// </summary>
    public class ButtonFadeCharacter : MonoBehaviour
    {
        /// <value>Property <c>character</c> represents the character to fade in and out.</value>
        public GameObject character;
        
        /// <value>Property <c>_characterMeshRenderer</c> represents the character mesh renderer.</value>
        private Renderer _characterMeshRenderer;

        /// <value>Property <c>_currentlyFading</c> represents if the character is currently fading.</value>
        private bool _currentlyFading;
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            // Get character mesh renderer
            _characterMeshRenderer = character.GetComponent<MeshRenderer>();
            
            // Loop through all materials
            foreach (var material in _characterMeshRenderer.materials)
            {
                // Set material alpha to 0
                material.color = new Color(material.color.r, material.color.g, material.color.b, 0.0f);
            }
        }

        /// <summary>
        /// Method <c>FadeCharacterIn</c> fades the character in.
        /// </summary>
        public void FadeCharacterIn()
        {
            // Check if already fading
            if (_currentlyFading)
                return;
            _currentlyFading = true;
            UnityEngine.Debug.Log("Fade character in");
            // Start fade coroutine for all materials
            StartCoroutine(FadeAllMaterials(1.0f, 1.0f));
        }

        /// <summary>
        /// Method <c>FadeCharacterOut</c> fades the character out.
        /// </summary>
        public void FadeCharacterOut()
        {
            // Check if already fading
            if (_currentlyFading)
                return;
            UnityEngine.Debug.Log("Fade character out");
            _currentlyFading = true;
            // Start fade coroutine for all materials
            StartCoroutine(FadeAllMaterials(0.0f, 1.0f));
        }
        
        /// <summary>
        /// Method <c>ToggleFadeCharacter</c> toggles the fade of the character.
        /// </summary>
        public void ToggleFadeCharacter()
        {
            // Check if already fading
            if (_currentlyFading)
                return;
            UnityEngine.Debug.Log("Toggle fade character");
            _currentlyFading = true;
            // Start fade coroutine for all materials
            var targetAlpha = _characterMeshRenderer.materials[0].color.a > 0.01f ? 0.0f : 1.0f;
            UnityEngine.Debug.Log("Current alpha: " + _characterMeshRenderer.materials[0].color.a);
            UnityEngine.Debug.Log("Target alpha: " + targetAlpha);
            StartCoroutine(FadeAllMaterials(targetAlpha, 1.0f));
        }

        /// <summary>
        /// Method <c>FadeAllMaterials</c> fades all materials of the character.
        /// <param name="targetAlpha">The target alpha value.</param>
        /// <param name="duration">The duration of the fade.</param>
        /// </summary>
        private IEnumerator FadeAllMaterials(float targetAlpha, float duration)
        {
            // Loop through all materials
            foreach (var material in _characterMeshRenderer.materials)
            {
                // Start fade coroutine
                yield return StartCoroutine(FadeMaterial(material, targetAlpha, duration));
            }
            
            // Set currently fading to false
            _currentlyFading = false;
        }
        
        /// <summary>
        /// Method <c>FadeMaterial</c> fades a material.
        /// </summary>
        /// <param name="material">The material to fade.</param>
        /// <param name="targetAlpha">The target alpha value.</param>
        /// <param name="duration">The duration of the fade.</param>
        /// <returns></returns>
        private static IEnumerator FadeMaterial(Material material, float targetAlpha, float duration)
        {
            // Get current alpha
            var startAlpha = material.color.a;
            
            // Loop through time
            for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
            {
                // Get current alpha
                var alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                
                // Set alpha
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
                
                // Wait for next frame
                yield return null;
            }
        }
    }
}
