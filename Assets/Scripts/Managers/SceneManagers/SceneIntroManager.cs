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
        
        /// <value>Property <c>objectsToActivate</c> represents the objects to be activated.</value>
        public GameObject[] objectsToActivate;
    }
    
    /// <summary>
    /// Class <c>SceneIntroManager</c> contains the logic for the notice scene.
    /// </summary>
    public class SceneIntroManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SceneIntroManager Instance;
        
        /// <value>Property <c>_inputReceived</c> represents whether the input was received.</value>
        private bool _inputReceived;
        
        #region General
        
            /// <value>Property <c>nextLevel</c> represents the next level to be loaded.</value>
            [Header("General")]
            public Level nextLevel;

            /// <value>Property <c>imageContainer</c> represents the image container.</value>
            public Image imageContainer;
            
            /// <value>Property <c>textContainer</c> represents the text container.</value>
            public TextMeshProUGUI textContainer;
            
            /// <value>Property <c>waitAfterLastFrame</c> represents the wait time after the last frame.</value>
            public float waitAfterLastFrame = 5.0f;
        
        #endregion
        
        #region Frames
        
            /// <value>Property <c>introFrames</c> represents the frames in the intro sequence.</value>
            [Header("Frames")]
            [SerializeField]
            public IntroFrame[] introFrames;
        
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
            if (_inputReceived)
                return; 
            if (!UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame)
                return;
            _inputReceived = true;
            CustomSceneManager.Instance.LoadLevel(nextLevel.name);
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private IEnumerator Start()
        {
            _inputReceived = false;
            foreach (var frame in introFrames)
            {
                // If the frame has an image, fade it in
                if (frame.image != null)
                    FadeImage(frame.image, imageContainer, 1.0f, 1.5f);
                // Wait for a second
                yield return new WaitForSeconds(1.0f);
                // If the frame has a text, fade it out
                if (frame.image != null)
                    FadeImage(frame.image, imageContainer, 0.0f, 1.5f);
                // If the frame has a text, display it
                if (frame.text != "")
                    FadeText(frame.text, textContainer, 1.0f, 0.5f);
                // If the frame has objects to activate, activate them
                foreach (var obj in frame.objectsToActivate)
                    obj.SetActive(true);
                // If the frame has an audio clip, play it
                if (frame.audioClip != null)
                {
                    SoundManager.Instance.PlaySound(frame.audioClip);
                    yield return new WaitForSeconds(frame.audioClip.length);
                }
                // If the frame is the last one, fade the music and load the next level
                if (frame.Equals(introFrames[^1]))
                    SoundManager.Instance.FadeOutMusic(waitAfterLastFrame, LoadNextLevel);
                else
                    FadeText(frame.text, textContainer, 0.0f, 0.5f);
            }
        }
        
        /// <summary>
        /// Method <c>FadeImage</c> fades the image.
        /// </summary>
        /// <param name="image">The image to be displayed.</param>
        /// <param name="container">The container of the image.</param>
        /// <param name="targetAlpha">The target alpha of the image.</param>
        /// <param name="duration">The duration of the fade.</param>
        private void FadeImage(Sprite image, Image container, float targetAlpha, float duration)
        {
            if (targetAlpha > 0)
                container.canvasRenderer.SetAlpha(0.0f);
            container.sprite = image;
            container.CrossFadeAlpha(targetAlpha, duration, false);
        }
        
        /// <summary>
        /// Method <c>FadeText</c> fades the text.
        /// </summary>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="container">The container of the text.</param>
        /// <param name="targetAlpha">The target alpha of the text.</param>
        /// <param name="duration">The duration of the fade.</param>
        private void FadeText(string text, TextMeshProUGUI container, float targetAlpha, float duration)
        {
            if (targetAlpha > 0)
                container.canvasRenderer.SetAlpha(0.0f);
            container.text = text;
            container.CrossFadeAlpha(targetAlpha, duration, false);
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
