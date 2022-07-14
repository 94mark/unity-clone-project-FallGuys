using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

namespace DevionGames
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            EditorGUI.EndProperty();
        }
    }
}