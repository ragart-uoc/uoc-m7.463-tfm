using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = TFM.Persistence.Event;

namespace TFM
{
    /// <summary>
    /// Class <c>ObjectShowable</c> represents an object that can be shown.
    /// </summary>
    [System.Serializable]
    public class ObjectShowable : MonoBehaviour
    {
        /// <value>Property <c>showEvents</c> represents the events required for showing the object.</value>
        public List<Event> showEvents;
        
        /// <value>Property <c>hideEvents</c> represents the events required for hiding the object.</value>
        public List<Event> hideEvents;
        
        /// <value>Property <c>requiredAgeGroups</c> represents the required age groups.</value>
        public List<AgeGroupProperties.Groups> availableAgeGroups;

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
            var meshRenderers = GetComponentsInChildren<Renderer>();
            
            // Loop through all mesh renderers
            foreach (var meshRenderer in meshRenderers)
            {
                // Loop through all materials
                foreach (var material in meshRenderer.materials)
                {
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
