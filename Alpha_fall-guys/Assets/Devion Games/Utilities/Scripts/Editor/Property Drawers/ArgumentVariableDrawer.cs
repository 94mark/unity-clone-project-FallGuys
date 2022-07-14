using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    [CustomPropertyDrawer(typeof(ArgumentVariable))]
    public class ArgumentVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty argumentType = property.FindPropertyRelative("m_ArgumentType");
            EditorGUI.PropertyField(position,argumentType, new GUIContent("Type"));
            if (argumentType.enumValueIndex > 0)
            {
                position.y += position.height;
                SerializedProperty valueProperty = property.FindPropertyRelative(ArgumentVariable.GetPropertyValuePath((ArgumentType)argumentType.enumValueIndex));
                EditorGUI.PropertyField(position, valueProperty, new GUIContent("Value"));
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty m_ArgumentType = property.FindPropertyRelative("m_ArgumentType");
            return EditorGUIUtility.singleLineHeight * (m_ArgumentType.enumValueIndex == 0 ? 1f : 2f);
        }
    }
}