using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TFM.Entities;

namespace TFM.Managers.SceneManagers
{
    /// <summary>
    /// Struct <c>IntroFrame</c> represents a frame in the intro sequence.
    /// </summary>
    [Serializable]
    public struct IntroFrame
    {
        /// <value>Property <c>text</c> represents the text to be displayed.</value>
        public string text;

        /// <value>Property <c>audio</c> represents the audio to be played.</value>
        public AudioClip audioClip;

        /// <value>Property <c>image</c> represents the image to be displayed.</value>
        public Sprite image;
    }
    
    /// <summary>
    /// Class <c>SceneIntroManager</c> contains the logic for the notice scene.
    /// </summary>
    public class SceneIntroManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneIntroManager Instance;
        
        /// <value>Property <c>clickedScreen</c> represents if the screen was clicked.</value>
        private bool _clickedScreen;
        
        #region General
        
            /// <value>Property <c>nextLevel</c> represents the next level to be loaded.</value>
            [Header("General")]
            public Level nextLevel;

            /// <value>Property <c>imageContainer</c> represents the image container.</value>
            public Image imageContainer;
            
            /// <value>Property <c>textContainer</c> represents the text container.</value>
            public TextMeshProUGUI textContainer;
            
            /// <value>Property <c>clipSource</c> represents the clip source.</value>
            public AudioSource clipSource;
            
            /// <value>Property <c>musicSource</c> represents the music source.</value>
            public AudioSource musicSource;
        
        #endregion
        
        #region Frames
        
            /// <value>Property <c>introFrames</c> represents the frames in the intro sequence.</value>
            [Header("Frames")]
            [SerializeField]
            public IntroFrame[] introFrames;
        
        #endregion
        
        #region Last Frame
        
            /// <value>Property <c>lastFrame</c> represents the last frame to be displayed.</value>
            [Header("Last Frame")]
            public AudioClip lastFrameAudio;
            
            /// <value>Property <c>lastFrameText</c> represents the text to be displayed in the last frame.</value>
            public Transform lastFrameText;
        
        #endregion

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
        }
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (_clickedScreen
                || (!UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame
                    && !UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame))
                return;
            _clickedScreen = true;
            CustomSceneManager.Instance.LoadLevel(nextLevel.name);
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private IEnumerator Start()
        {
            _clickedScreen = false;
            foreach (var frame in introFrames)
            {
                imageContainer.sprite = frame.image;
                imageContainer.CrossFadeAlpha(1.0f, 1.5f, false);
                yield return new WaitForSeconds(1.0f);
                imageContainer.CrossFadeAlpha(0.0f, 1.5f, false);
                textContainer.text = frame.text;
                clipSource.clip = frame.audioClip;
                clipSource.Play();
                yield return new WaitForSeconds(frame.audioClip.length);
                textContainer.text = "";
            }
            yield return new WaitForSeconds(1.0f);
            lastFrameText.gameObject.SetActive(true);
            clipSource.clip = lastFrameAudio;
            clipSource.Play();
            yield return new WaitForSeconds(lastFrameAudio.length);
            StartCoroutine(FadeMusic(5.0f, LoadNextLevel));
        }

        /// <summary>
        /// Method <c>FadeMusic</c> fades the music.
        /// </summary>
        /// <param name="duration">The duration of the fade.</param>
        /// <param name="callback">The callback to be executed after the fade.</param>
        /// <returns></returns>
        private IEnumerator FadeMusic(float duration, Action callback = null)
        {
            var startVolume = musicSource.volume;
            var startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                musicSource.volume = startVolume * (1 - ((Time.time - startTime) / duration));
                yield return null;
            }
            musicSource.Stop();
            callback?.Invoke();
        }

        /// <summary>
        /// Method <c>LoadNextLevel</c> loads the next level.
        /// </summary>
        private void LoadNextLevel()
        {
            CustomSceneManager.Instance.LoadLevel(nextLevel.name);
        }
    }
}
