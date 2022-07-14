using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

namespace DevionGames
{
    public class AudioEventListener : MonoBehaviour
    {

        [SerializeField]
        private List<AudioGroup> m_AudioGroups = new List<AudioGroup>();

        private void Awake()
        {
            for (int i = 0; i < this.m_AudioGroups.Count; i++) {
                AudioGroup group = this.m_AudioGroups[i];
                group.audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void PlayAudio(AnimationEvent evt) {
            AudioGroup group = this.m_AudioGroups.First(x => x.name == evt.stringParameter);
            group.PlayOneShot(evt.objectReferenceParameter as AudioClip, evt.floatParameter);
        }



        [System.Serializable]
        public class AudioGroup {
            public string name = "SFX";
            [SerializeField]
            private AudioMixerGroup m_AudioMixerGroup=null;

            private AudioSource m_AudioSource;

            public AudioSource audioSource {
                get { return this.m_AudioSource; }
                set { this.m_AudioSource = value;
                    this.m_AudioSource.outputAudioMixerGroup = this.m_AudioMixerGroup;
                    this.m_AudioSource.spatialBlend = 1f;
                }
            }

            public void PlayOneShot(AudioClip clip, float volumeScale) {
                audioSource.PlayOneShot(clip,volumeScale);
            }
        }
    }
}