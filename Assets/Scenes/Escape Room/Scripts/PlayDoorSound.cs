using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayDoorSound : MonoBehaviour
{
        [Header("Sound Data")]
        [Tooltip("The sound that is played when the code is correct")]
        public AudioClip m_SoundCorrect = null;
        [Tooltip("The sound that is played when the code is incorrect")]
        public AudioClip m_SoundWrong = null;

        [SerializeField]
        [Tooltip("The volume of the sound")]
        public float m_Volume = 1.0f;

        [SerializeField]
        [Tooltip("The range of pitch the sound is played at (-pitch, pitch)")]
        [Range(0, 1)]
        public float m_RandomPitchVariance = 0.0f;

        AudioSource m_AudioSource = null;

        float m_DefaultPitch = 1.0f;

        void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void PlayCorrect()
        {
            float randomVariance = Random.Range(-m_RandomPitchVariance, m_RandomPitchVariance);
            randomVariance += m_DefaultPitch;

            m_AudioSource.pitch = randomVariance;
            m_AudioSource.PlayOneShot(m_SoundCorrect, m_Volume);
            m_AudioSource.pitch = m_DefaultPitch;
        }

    public void PlayWrong()
    {
        float randomVariance = Random.Range(-m_RandomPitchVariance, m_RandomPitchVariance);
        randomVariance += m_DefaultPitch;

        m_AudioSource.pitch = randomVariance;
        m_AudioSource.PlayOneShot(m_SoundWrong, m_Volume);
        m_AudioSource.pitch = m_DefaultPitch;
    }

    void OnValidate()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
}

