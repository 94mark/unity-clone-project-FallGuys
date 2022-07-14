using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
    public class ScreenResolution : MonoBehaviour
    {
        private const string RESOLUTION_KEY = "ScreenResolutionWidth";

        private void Start()
        {
            int resolution = PlayerPrefs.GetInt(RESOLUTION_KEY, Array.IndexOf(Screen.resolutions,Screen.currentResolution));
            SetResolution(resolution);

			Dropdown dropdown = GetComponent<Dropdown>();
			if (dropdown != null) {
				dropdown.value = resolution;
				dropdown.onValueChanged.AddListener(SetResolution);
			}
        }

		public void SetResolution(int index)
		{
			Resolution resolution = Screen.resolutions[index];
			SetResolution(resolution.width, resolution.height);
			PlayerPrefs.SetInt(RESOLUTION_KEY,index);
		}

		public void SetResolution(int width, int height)
		{
			if (IsStandalone && Screen.fullScreen)
			{
				Screen.SetResolution(width, height, Screen.fullScreen);
			}
		}

		public void IncreaseResolution()
		{
			for (int i = 0; i < Screen.resolutions.Length; i++)
			{
				Resolution resolution = Screen.resolutions[i];
				if (resolution.width > Screen.currentResolution.width)
				{
					SetResolution(i);
					break;
				}
			}
		}

		public void DecreaseResolution()
		{
			for (int i = 0; i < Screen.resolutions.Length; i++)
			{
				Resolution resolution = Screen.resolutions[i];
				if (resolution.width == Screen.currentResolution.width && i > 0)
				{
					SetResolution(i - 1);
					break;
				}
			}
		}

		public bool IsStandalone
		{
			get
			{
#if UNITY_STANDALONE
				return true;
#else
				return false;
#endif
			}
		}
	}
}