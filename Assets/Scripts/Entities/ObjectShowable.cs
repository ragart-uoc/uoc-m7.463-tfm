using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>ObjectShowable</c> represents an object that can be shown.
    /// </summary>
    [Serializable]
    public class ObjectShowable : MonoBehaviour
    {
        /// <value>Property <c>showEvents</c> represents the events required for showing the object.</value>
        public List<Event> showEvents;
        
        /// <value>Property <c>hideEvents</c> represents the events required for hiding the object.</value>
        public List<Event> hideEvents;
        
        /// <value>Property <c>requiredAgeGroups</c> represents the required age groups.</value>
        public List<AgeGroupProperties.Groups> availableAgeGroups;
        
        /// <value>Property <c>_originalMode</c> represents the original mode.</value>
        private float _originalMode;
        
        /// <value>Property <c>_originalSrcBlend</c> represents the original source blend.</value>
        private int _originalSrcBlend;
        
        /// <value>Property <c>_originalDstBlend</c> represents the original destination blend.</value>
        private int _originalDstBlend;
        
        /// <value>Property <c>_originalZWrite</c> represents the original z write.</value>
        private int _originalZWrite;

        /// <value>Property <c>Mode</c> represents the mode.</value>
        private static readonly int Mode = Shader.PropertyToID("_Mode");

        /// <value>Property <c>SrcBlend</c> represents the source blend.</value>
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        
        /// <value>Property <c>DstBlend</c> represents the destination blend.</value>
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        
        /// <value>Property <c>ZWrite</c> represents the z write.</value>
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

        /// <summary>
        /// Method <c>CheckConditions</c> checks the conditions.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <param name="currentAgeGroup">The current age group.</param>
        public bool CheckConditions(Dictionary<Event, bool> events, AgeGroupProperties.Groups currentAgeGroup)
        {
            var eventsShown = showEvents == null
                              || showEvents.Count == 0
                              || showEvents.All(e => events.ContainsKey(e) && events[e]);
            var eventsHidden = hideEvents == null
                               || hideEvents.Count == 0
                               || hideEvents.All(e => events.ContainsKey(e) && !events[e]);
            var ageGroupAvailable = availableAgeGroups.Contains(currentAgeGroup);
            return eventsShown && eventsHidden && ageGroupAvailable;
        }
        
        /// <summary>
        /// Method <c>Fade</c> fades the object.
        /// </summary>
        public void Fade(float targetAlpha, float duration)
        {
            // Get all the mesh renderers
            var parentRenderers = GetComponents<Renderer>() ?? Array.Empty<Renderer>();
            var childRenderers = GetComponentsInChildren<Renderer>() ?? Array.Empty<Renderer>();
            var meshRenderers = parentRenderers.Concat(childRenderers).ToArray();
            
            // Loop through all mesh renderers
            foreach (var meshRenderer in meshRenderers)
            {
                // Fail-safe
                if (meshRenderer == null || meshRenderer.materials == null)
                    continue;

                // Loop through all materials
                foreach (var material in meshRenderer.materials)
                {
                    // Store the current blend mode
                    _originalMode = material.GetFloat(Mode);
                    _originalSrcBlend = material.GetInt(SrcBlend);
                    _originalDstBlend = material.GetInt(DstBlend);
                    _originalZWrite = material.GetInt(ZWrite);

                    // Change blend mode to fade
                    material.SetFloat(Mode, 3.0f);
                    material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt(ZWrite, 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    
                    // Set the alpha to the contrary of the target alpha
                    material.color = new Color(material.color.r, material.color.g, material.color.b, targetAlpha == 0 ? 1 : 0);
                }
                
                // Enable the game object
                gameObject.SetActive(true);
                
                // Start fade coroutine
                StartCoroutine(FadeAllMaterials(meshRenderer, targetAlpha, duration));
            }
        }
        
        /// <summary>
        /// Method <c>FadeAllMaterials</c> fades all materials of the character.
        /// <param name="targetAlpha">The target alpha value.</param>
        /// <param name="duration">The duration of the fade.</param>
        /// </summary>
        private IEnumerator FadeAllMaterials(Renderer meshRenderer, float targetAlpha, float duration)
        {
            // Loop through all materials
            return meshRenderer.materials.Select(material => StartCoroutine(FadeMaterial(material, targetAlpha, duration))).GetEnumerator();
        }
        
        /// <summary>
        /// Method <c>FadeMaterial</c> fades a material.
        /// </summary>
        /// <param name="material">The material to fade.</param>
        /// <param name="targetAlpha">The target alpha value.</param>
        /// <param name="duration">The duration of the fade.</param>
        /// <returns></returns>
        private IEnumerator FadeMaterial(Material material, float targetAlpha, float duration)
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
            
            // If target alpha is greater than 0, restore the original blend mode
            if (!(targetAlpha > 0))
                yield break;
            material.SetFloat(Mode, material.GetFloat(Mode) == 0 ? _originalMode : material.GetFloat(Mode));
            material.SetInt(SrcBlend, material.GetInt(SrcBlend) == 0 ? _originalSrcBlend : material.GetInt(SrcBlend));
            material.SetInt(DstBlend, material.GetInt(DstBlend) == 0 ? _originalDstBlend : material.GetInt(DstBlend));
            material.SetInt(ZWrite, material.GetInt(ZWrite) == 0 ? _originalZWrite : material.GetInt(ZWrite));
        }
    }
}
