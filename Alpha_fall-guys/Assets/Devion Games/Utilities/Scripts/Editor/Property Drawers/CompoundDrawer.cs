using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    [CustomPropertyDrawer(typeof(CompoundAttribute))]
    public class CompoundDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUIUtility.wideMode = true;
            string propertyPath = (attribute as CompoundAttribute).propertyPath;
            SerializedProperty compoundProperty = property.serializedObject.FindProperty(propertyPath);
            if (compoundProperty.boolValue) {
                position.x += 15f;
                position.width -= 15f;
                EditorGUI.PropertyField(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string propertyPath = (attribute as CompoundAttribute).propertyPath;
            SerializedProperty compoundProperty = property.serializedObject.FindProperty(propertyPath);
            return compoundProperty.boolValue?base.GetPropertyHeight(property, label):0f;
        }
    }
}