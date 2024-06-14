using System;
using System.Collections;
using UnityEngine;

namespace TFM.Managers
{
    public class SoundManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static SoundManager Instance;
        
        /// <value>Property <c>musicSource</c> represents the music source.</value>
        public AudioSource musicSource;
        
        /// <value>Property <c>musicVolume</c> represents the music volume.</value>
        [HideInInspector]
        public float musicVolume = 1.0f;
        
        /// <value>Property <c>sfxSource</c> represents the sound effects source.</value>
        public AudioSource sfxSource;
        
        /// <value>Property <c>sfxVolume</c> represents the effects volume.</value>
        [HideInInspector]
        public float sfxVolume = 1.0f;

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
            
            // Get data from player preferences
            if (!PlayerPrefs.HasKey("MusicVolume"))
                PlayerPrefs.SetFloat("MusicVolume", 1.0f);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicSource.outputAudioMixerGroup.audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20);
            if (!PlayerPrefs.HasKey("SfxVolume"))
                PlayerPrefs.SetFloat("SfxVolume", 1.0f);
            sfxVolume = PlayerPrefs.GetFloat("SfxVolume");
            sfxSource.outputAudioMixerGroup.audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolume) * 20);
        }
        
        /// <summary>
        /// Method <c>PlayMusic</c> changes the background music.
        /// </summary>
        /// <param name="clip">The music clip</param>
        /// <param name="isLooping">Whether the music should loop or not</param>
        public void PlayMusic(AudioClip clip, bool isLooping = true)
        {
            musicSource.clip = clip;
            musicSource.loop = isLooping;
            musicSource.Play();
        }
        
        /// <summary>
        /// Method <c>PlaySound</c> plays a sound.
        /// </summary>
        /// <param name="clip">The sound clip</param>
        public void PlaySound(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }
        
        /// <summary>
        /// Method <c>FadeOutMusic</c> fades out the music.
        /// </summary>
        /// <param name="duration">The duration of the fade</param>
        /// <param name="callback">The callback function</param>
        public void FadeOutMusic(float duration, Action callback = null)
        {
            StartCoroutine(FadeOutMusicCoroutine(duration, callback));
        }
        
        /// <summary>
        /// Method <c>FadeOutMusicCoroutine</c> is the coroutine for fading out the music.
        /// </summary>
        /// <param name="duration">The duration of the fade</param>
        /// <param name="callback">The callback function</param>
        private IEnumerator FadeOutMusicCoroutine(float duration, Action callback = null)
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
        /// Method <c>SetMusicVolume</c> sets the music volume.
        /// </summary>
        /// <param name="newVolume">The new volume</param>
        public void SetMusicVolume(float newVolume)
        {
            musicSource.outputAudioMixerGroup.audioMixer.SetFloat("musicVolume", Mathf.Log10(newVolume) * 20);
        }

        /// <summary>
        /// Method <c>SetSfxVolume</c> sets the sound effects volume.
        /// </summary>
        /// <param name="newVolume">The new volume</param>
        public void SetSfxVolume(float newVolume)
        {
            sfxSource.outputAudioMixerGroup.audioMixer.SetFloat("sfxVolume", Mathf.Log10(newVolume) * 20);
        }
    }
}
