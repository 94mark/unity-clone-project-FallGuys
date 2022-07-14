using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DevionGames
{
	[CustomPropertyDrawer (typeof(InspectorLabelAttribute))]
	public class InspectorLabelDrawer : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
            InspectorLabelAttribute attr = attribute as InspectorLabelAttribute;
			EditorGUI.PropertyField (position, property, new GUIContent (attr.label, attr.tooltip));
		}

	}
}