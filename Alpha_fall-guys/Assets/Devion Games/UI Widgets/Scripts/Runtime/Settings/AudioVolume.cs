using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
    [RequireComponent(typeof(Slider))]
    public class AudioVolume : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer m_MixerGroup = null;
        [SerializeField]
        private string m_ExposedParameter = "MusicVolume";

        private Slider m_Slider;

        private void Start()
        {
            this.m_Slider = GetComponent<Slider>();
            this.m_Slider.minValue = 0.0001f;
            this.m_Slider.maxValue = 1.0f;

            float defaultValue;
            this.m_MixerGroup.GetFloat(this.m_ExposedParameter, out defaultValue);
          
            float volume = PlayerPrefs.GetFloat(this.m_ExposedParameter, Mathf.Pow(10, defaultValue / 20));
            this.m_Slider.value = volume;
            SetVolume(volume);
            this.m_Slider.onValueChanged.AddListener(SetVolume);
        }

        public void SetVolume(float volume) {
            this.m_MixerGroup.SetFloat(this.m_ExposedParameter, Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(this.m_ExposedParameter, volume);
        }
    }
}