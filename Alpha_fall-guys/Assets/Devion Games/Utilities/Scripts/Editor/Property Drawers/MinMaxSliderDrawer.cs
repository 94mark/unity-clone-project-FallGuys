using UnityEngine;
using UnityEditor;

namespace DevionGames
{
	[CustomPropertyDrawer (typeof(MinMaxSliderAttribute))]
	class MinMaxSliderDrawer : PropertyDrawer
	{

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{

			if (property.propertyType == SerializedPropertyType.Vector2) {
				Vector2 range = property.vector2Value;
				float min = range.x;
				float max = range.y;
				MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;
				EditorGUI.BeginChangeCheck ();
		
				EditorGUI.LabelField (position, label);

				Rect sliderRect = new Rect (position.x + EditorGUIUtility.labelWidth + 47f, position.y, position.width - EditorGUIUtility.labelWidth - 99f, position.height);
				EditorGUI.MinMaxSlider (sliderRect, ref min, ref max, attr.min, attr.max);
				Rect rect = new Rect (sliderRect.x - 47f, position.y, 45f, position.height);
				min = EditorGUI.FloatField (rect, min);

				rect.x += sliderRect.width + 49f;
				max = EditorGUI.FloatField (rect, max);

				if (EditorGUI.EndChangeCheck ()) {
					range.x = Round (min, 2);
					range.y = Round (max, 2);
					property.vector2Value = range;
				}
			} else {
				EditorGUI.LabelField (position, label, "Use only with Vector2");
			}
		}

		public static float Round (float value, int digits)
		{
			float mult = Mathf.Pow (10.0f, (float)digits);
			return Mathf.Round (value * mult) / mult;
		}

	}
}