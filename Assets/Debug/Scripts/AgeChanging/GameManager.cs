using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TFM.Debug.Scripts.AgeChanging
{
    /// <summary>
    /// Class <c>GameManager</c> contains the logic for managing the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static GameManager Instance;

        /// <value>Property <c>ageGroups</c> represents the age groups.</value>
        public AgeGroup[] ageGroups;
        
        /// <value>Property <c>_currentAgeGroup</c> represents the current age group.</value>
        private AgeProperties.Groups _currentAgeGroup;
        
        /// <value>Property <c>ageButtons</c> represents the age buttons.</value>
        public Button[] ageButtons;
        
        /// <value>Property <c>ageIsChanging</c> represents whether the age is currently changing.</value>
        private bool _ageIsChanging;

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

            // Set the first age group as the current age group
            _currentAgeGroup = ageGroups[0].group;
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        public void Start()
        {
            // Loop through all age groups
            foreach (var ageGroup in ageGroups)
            {
                // Do not fade the current age group
                if (ageGroup.group == _currentAgeGroup)
                    continue;

                // Loop through all characters
                foreach (var character in ageGroup.characters)
                {
                    // Get character mesh renderer
                    var characterMeshRenderer = character.GetComponent<Renderer>();
                    
                    // Loop through all materials
                    foreach (var material in characterMeshRenderer.materials)
                    {
                        // Set material alpha to 0
                        material.color = new Color(material.color.r, material.color.g, material.color.b, 0.0f);
                    }
                }
            }
            
            // Enable all age buttons except the current age group
            EnableAgeButtons(true);
            EnableAgeButton(_currentAgeGroup, false);
        }

        /// <summary>
        /// Method <c>EnableAgeButtons</c> enables or disables all age buttons.
        /// </summary>
        /// <param name="enableButtons">The flag to enable or disable the buttons.</param>
        private void EnableAgeButtons(bool enableButtons)
        {
            foreach (var button in ageButtons)
            {
                button.interactable = enableButtons;
            }
        }
        
        /// <summary>
        /// Method <c>EnableAgeButton</c> enables or disables an age button.
        /// </summary>
        /// <param name="ageGroup">The age group.</param>
        /// <param name="enableButton">The flag to enable or disable the button.</param>
        private void EnableAgeButton(AgeProperties.Groups ageGroup, bool enableButton)
        {
            foreach (var button in ageButtons)
            {
                var buttonChangeAge = button.GetComponent<ButtonChangeAge>();
                if (buttonChangeAge.ageGroup == ageGroup)
                    button.interactable = enableButton;
            }
        }
        
        /// <summary>
        /// Method <c>ChangeAgeGroup</c> changes the age group.
        /// </summary>
        /// <param name="newAgeGroup">The new age group.</param>
        public void ChangeAgeGroup(AgeProperties.Groups newAgeGroup)
        {
            // Do not change age if the age is currently changing
            if (_ageIsChanging)
                return;
            
            // Set the age as changing
            _ageIsChanging = true;
            
            // Disable all age buttons
            EnableAgeButtons(false);
            
            // Switch the age group
            StartCoroutine(SwitchAgeGroup(_currentAgeGroup, newAgeGroup));
        }
        
        /// <summary>
        /// Method <c>SwitchAgeGroup</c> switches the age group.
        /// </summary>
        /// <param name="oldAgeGroup">The old age group.</param>
        /// <param name="newAgeGroup">The new age group.</param>
        private IEnumerator SwitchAgeGroup(AgeProperties.Groups oldAgeGroup, AgeProperties.Groups newAgeGroup)
        {
            // Fade out the old age group
            yield return StartCoroutine(FadeAgeGroup(oldAgeGroup, 0.0f, 1.0f));
            
            // Wait for the duration
            yield return new WaitForSeconds(1.0f);
            
            // Fade in the new age group
            yield return StartCoroutine(FadeAgeGroup(newAgeGroup, 1.0f, 1.0f));
            
            // Set the new age group as the current age group
            _currentAgeGroup = newAgeGroup;
            
            // Set the age as not changing
            _ageIsChanging = false;
            
            // Enable all age buttons except the current age group
            EnableAgeButtons(true);
            EnableAgeButton(_currentAgeGroup, false);
        }
        
        /// <summary>
        /// Method <c>FadeAgeGroup</c> fades an age group.
        /// </summary>
        /// <param name="ageGroup">The age group.</param>
        /// <param name="targetAlpha">The target alpha value.</param>
        /// <param name="duration">The duration of the fade.</param>
        private IEnumerator FadeAgeGroup(AgeProperties.Groups ageGroup, float targetAlpha, float duration)
        {
            // Get the struct of the age group
            foreach (var ageGroupObject in ageGroups)
            {
                if (ageGroupObject.group != ageGroup)
                    continue;

                // Loop through all characters
                foreach (var character in ageGroupObject.characters)
                {
                    // Get character mesh renderer
                    var characterMeshRenderer = character.GetComponent<Renderer>();
                        
                    // Start fade coroutine
                    StartCoroutine(FadeAllMaterials(characterMeshRenderer, targetAlpha, duration));
                }
            }
                
            // Wait for the duration
            yield return new WaitForSeconds(duration);
        }

        /// <summary>
        /// Method <c>FadeAllMaterials</c> fades all materials of the character.
        /// <param name="targetAlpha">The target alpha value.</param>
        /// <param name="duration">The duration of the fade.</param>
        /// </summary>
        private IEnumerator FadeAllMaterials(Renderer characterMeshRenderer, float targetAlpha, float duration)
        {
            // Loop through all materials
            foreach (var material in characterMeshRenderer.materials)
            {
                // Start fade coroutine
                yield return StartCoroutine(FadeMaterial(material, targetAlpha, duration));
            }
            
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
