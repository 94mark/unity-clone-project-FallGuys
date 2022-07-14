using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    [CustomDrawer(typeof(NamedVariable),true)]
    public class NamedVariableLayoutDrawer : CustomDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            NamedVariable variable = value as NamedVariable;
            EditorGUILayout.TextField("Name", variable.Name);
            variable.VariableType = (NamedVariableType)EditorGUILayout.Popup((int)variable.VariableType, variable.VariableTypeNames);
            variable.SetValue(EditorTools.DrawFields(variable.GetValue()));

        }

    }
}