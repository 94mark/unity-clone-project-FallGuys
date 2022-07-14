using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DevionGames
{
    public class PlayAudioClip : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_AudioClip=null;
        [SerializeField]
        private AudioMixerGroup m_AudioMixerGroup = null;
        [SerializeField]
        private float m_Volume = 1f;
        [SerializeField]
        private float m_Delay = 0f;


        private IEnumerator Start()
        {
            yield return new WaitForSeconds(this.m_Delay);
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null) {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.outputAudioMixerGroup = this.m_AudioMixerGroup;
            audioSource.volume = this.m_Volume;
            audioSource.PlayOneShot(this.m_AudioClip);
        }
    }
}