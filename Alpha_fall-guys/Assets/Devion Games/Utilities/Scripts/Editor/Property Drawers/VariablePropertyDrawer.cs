using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    [CustomPropertyDrawer(typeof(Variable),true)]
    public class VariablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty name = property.FindPropertyRelative("m_Name");
            SerializedProperty value = property.FindPropertyRelative("m_Value");
            SerializedProperty isShared = property.FindPropertyRelative("m_IsShared");
            bool sharedOnly = fieldInfo.HasAttribute<SharedAttribute>();
            if (sharedOnly)
            {
                isShared.boolValue = true;
                EditorGUI.PropertyField(position, name, label);
            }
            else
            {

                position.width = position.width - 21f;
                EditorGUI.PropertyField(position, isShared.boolValue ? name : value, label);
                position.x += position.width + 2f;
                position.width = 17f;
                DrawSharedToggle(position, isShared);
            }
        }

        public virtual bool DrawSharedToggle(Rect position, SerializedProperty isShared)
        {
            EditorGUI.BeginChangeCheck();
            bool value = EditorGUI.Toggle(position, isShared.boolValue, EditorStyles.radioButton);
            if (EditorGUI.EndChangeCheck())
            {
                isShared.boolValue = value;
            }
            return value;
        }
    }
}