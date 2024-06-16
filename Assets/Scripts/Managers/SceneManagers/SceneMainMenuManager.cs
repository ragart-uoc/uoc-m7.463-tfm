using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TFM.Managers.SceneManagers
{
    /// <summary>
    /// Class <c>SceneMainMenuManager</c> contains the logic for the main menu scene.
    /// </summary>
    public class SceneMainMenuManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneMainMenuManager Instance;
        
        /// <value>Property <c>continueButton</c> represents the continue button in the scene.</value>
        public Button continueButton;
        
        /// <value>Property <c>optionsPanel</c> represents the options panel in the scene.</value>
        public GameObject optionsPanel;
        
        /// <value>Property <c>creditsPanel</c> represents the credits panel in the scene.</value>
        public GameObject creditsPanel;
        
        /// <value>Property <c>musicVolumeSlider</c> represents the music volume slider.</value>
        public Slider musicVolumeSlider;
        
        /// <value>Property <c>musicVolumeText</c> represents the music volume text.</value>
        public TextMeshProUGUI musicVolumeText;
        
        /// <value>Property <c>sfxVolumeSlider</c> represents the effects volume slider.</value>
        public Slider sfxVolumeSlider;
        
        /// <value>Property <c>sfxVolumeText</c> represents the effects volume text.</value>
        public TextMeshProUGUI sfxVolumeText;
        
        /// <value>Property <c>Difficulty</c> represents the available difficulties.</value>
        private enum Difficulty
        {
            Easy,
            Normal,
            Hard
        }
        
        /// <value>Property <c>difficulty</c> represents the difficulty.</value>
        [HideInInspector]
        public float difficulty = 1;
        
        /// <value>Property <c>difficultySlider</c> represents the difficulty slider.</value>
        public Slider difficultySlider;
        
        /// <value>Property <c>difficultyText</c> represents the difficulty text.</value>
        public TextMeshProUGUI difficultyText;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            // Enable the time scale and audio listener in case it comes from the pause menu
            Time.timeScale = 1;
            AudioListener.pause = false;
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        public void Start()
        {
            // Load the data
            LoadData();
            // Check if the contine button should be interactable
            continueButton.interactable = GameManager.Instance.ExistsSaveData();
        }

        /// <summary>
        /// Method <c>LoadData</c> is used to load the data in PlayerPrefs.
        /// </summary>
        private void LoadData()
        {
            // Music volume
            var musicVolume = SoundManager.Instance?.musicVolume ?? 1;
            musicVolumeSlider.value = musicVolume; 
            musicVolumeText.text = Mathf.Round(musicVolume * 100).ToString(CultureInfo.InvariantCulture);
        
            // Effects volume
            var sfxVolume = SoundManager.Instance?.sfxVolume ?? 1;
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeText.text = Mathf.Round(sfxVolume * 100).ToString(CultureInfo.InvariantCulture);
        
            // Difficulty
            if (!PlayerPrefs.HasKey("Difficulty"))
                PlayerPrefs.SetFloat("Difficulty", 1);
            difficulty = PlayerPrefs.GetFloat("Difficulty");
            difficultySlider.value = difficulty;
            difficultyText.text = ((Difficulty) difficulty).ToString();
        }
        
        /// <summary>
        /// Method <c>SetMusicVolume</c> is used to set the music volume.
        /// </summary>
        /// <param name="newVolume">The new volume.</param>
        public void SetMusicVolume(float newVolume)
        {
            SoundManager.Instance.SetMusicVolume(newVolume);
            musicVolumeText.text = Mathf.Round(newVolume * 100).ToString(CultureInfo.InvariantCulture);
            PlayerPrefs.SetFloat("MusicVolume", newVolume);
        }
        
        /// <summary>
        /// Method <c>SetSfxVolume</c> is used to set the effects volume.
        /// </summary>
        /// <param name="newVolume">The new volume.</param>
        public void SetSfxVolume(float newVolume)
        {
            SoundManager.Instance.SetSfxVolume(newVolume);
            sfxVolumeText.text = Mathf.Round(newVolume * 100).ToString(CultureInfo.InvariantCulture);
            PlayerPrefs.SetFloat("SfxVolume", newVolume);
        }
        
        /// <summary>
        /// Method <c>SetDifficulty</c> is used to set the difficulty.
        /// </summary>
        /// <param name="newDifficulty">The new difficulty.</param>
        public void SetDifficulty(float newDifficulty)
        {
            difficulty = newDifficulty;
            difficultyText.text = ((Difficulty) difficulty).ToString();
            PlayerPrefs.SetFloat("Difficulty", difficulty);
        }

        /// <summary>
        /// Method <c>ToggleOptions</c> toggles the options panel.
        /// </summary>
        public void ToggleOptions()
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
        
        /// <summary>
        /// Method <c>ToggleCredits</c> toggles the credits panel.
        /// </summary>
        public void ToggleCredits()
        {
            creditsPanel.SetActive(!optionsPanel.activeSelf);
        }
        
        /// <summary>
        /// Method <c>MainMenu</c> loads the main menu.
        /// </summary>
        public void LetMeTellYouAStory()
        {
            CustomSceneManager.Instance.LoadLevel("scene_0_level", true);
        }
    }
}