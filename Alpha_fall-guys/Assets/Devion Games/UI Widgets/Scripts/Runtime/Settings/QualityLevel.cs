using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
    public class QualityLevel : MonoBehaviour
    {
		private const string QUALITY_LEVEL_KEY = "QualityLevel";

        private void Start()
        {
			int qualityIndex = PlayerPrefs.GetInt(QUALITY_LEVEL_KEY, QualitySettings.GetQualityLevel());
			SetQualityLevel(qualityIndex);
			Slider slider = GetComponent<Slider>();
			if (slider != null) {
				slider.minValue = 0;
				slider.maxValue = QualitySettings.names.Length;
				slider.wholeNumbers = true;
				slider.value = qualityIndex;
				slider.onValueChanged.AddListener(SetQualityLevel);
			}

		}

		public void SetQualityLevel(float index)
		{
			SetQualityLevel(Mathf.RoundToInt(index));
		}

		public void SetQualityLevel(int index)
		{
			if (QualitySettings.GetQualityLevel() != index)
			{
				QualitySettings.SetQualityLevel(index, true);
				PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, index);
			}
		}

		public void IncreaseQualityLevel()
		{
			QualitySettings.IncreaseLevel();
			PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, QualitySettings.GetQualityLevel());
		}

		public void DecreaseQualityLevel()
		{
			QualitySettings.DecreaseLevel();
			PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, QualitySettings.GetQualityLevel());
		}
	}
}