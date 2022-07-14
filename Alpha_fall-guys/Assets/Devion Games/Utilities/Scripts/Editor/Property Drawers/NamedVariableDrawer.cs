using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    [CustomPropertyDrawer(typeof(NamedVariable),true)]
    public class NamedVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position,property.FindPropertyRelative("m_Name"));


            position.y += EditorGUIUtility.singleLineHeight+EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_Description"));
            EditorGUI.PropertyField(position,property.FindPropertyRelative("m_Description"));


            position.y += EditorGUIUtility.standardVerticalSpacing + position.height;
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position,property.FindPropertyRelative("m_VariableType"), new GUIContent("Type"));
            position.y += EditorGUIUtility.singleLineHeight+EditorGUIUtility.standardVerticalSpacing; ;
            if ((property.GetValue() as NamedVariable) != null)
            {
                SerializedProperty value = property.FindPropertyRelative((property.GetValue() as NamedVariable).PropertyPath);
                position.height = EditorGUI.GetPropertyHeight(value);
                EditorGUI.PropertyField(position, value, new GUIContent("Value"));
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 2;
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_Description"));

            if ((property.GetValue() as NamedVariable) != null){
                SerializedProperty value = property.FindPropertyRelative((property.GetValue() as NamedVariable).PropertyPath);
                height += EditorGUI.GetPropertyHeight(value);
            }
            return height;
        }
    }
}