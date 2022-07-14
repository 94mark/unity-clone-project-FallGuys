using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevionGames
{
    public class SaveUIComponent : MonoBehaviour
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private UIBehaviour target;

        private void Awake()
        {
            if (target == null) {
                return;
            }

            if (target is Slider) {
                Slider slider = target as Slider;
                slider.value = LoadFloat(slider.value);
                slider.onValueChanged.AddListener(SaveFloat);
            }else if (target is Dropdown){
                Dropdown dropdown = target as Dropdown;
                dropdown.value = LoadInt(dropdown.value);
                dropdown.onValueChanged.AddListener(SaveInt);
            }else if (target is InputField){
                InputField inputField = target as InputField;
                inputField.text = LoadString(inputField.text);
                inputField.onValueChanged.AddListener(SaveString);
            }else if (target is Dropdown){
                Dropdown dropdown = target as Dropdown;
                dropdown.value = LoadInt(dropdown.value);
                dropdown.onValueChanged.AddListener(SaveInt);
            }else if (target is Toggle){
                Toggle toggle = target as Toggle;
                toggle.isOn = LoadBool(toggle.isOn);
                toggle.onValueChanged.AddListener(SaveBool);
            }
        }

        private void SaveFloat(float value) {
            PlayerPrefs.SetFloat(key, value);
        }

        private float LoadFloat(float defaultValue) {
            return PlayerPrefs.GetFloat(key,defaultValue);
        }

        private void SaveInt(int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        private int LoadInt(int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        private void SaveString(string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        private string LoadString(string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        private void SaveBool(bool value)
        {
            PlayerPrefs.SetInt(key, value?1:0);
        }

        private bool LoadBool(bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0)==0?false:true;
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(key)) {
                key = System.Guid.NewGuid().ToString();
            }
            if (target == null)
            {
                target = GetComponent<UIBehaviour>();
            }else if (!(typeof(Slider).IsAssignableFrom(target.GetType()) || 
                typeof(Dropdown).IsAssignableFrom(target.GetType()) || 
                typeof(InputField).IsAssignableFrom(target.GetType()) || 
                typeof(Toggle).IsAssignableFrom(target.GetType()))){

                Debug.LogWarning("SaveUIComponent does not support target type ("+ target.GetType().Name+"). Supported types are Slider, Dropdown, InputField, Toggle.");
            }

        }
    }
}