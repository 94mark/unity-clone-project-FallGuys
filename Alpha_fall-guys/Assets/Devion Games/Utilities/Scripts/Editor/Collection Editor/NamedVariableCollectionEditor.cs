using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    [System.Serializable]
    public class NamedVariableCollectionEditor : ObjectCollectionEditor<NamedVariable>
    {
        public override string ToolbarName
        {
            get
            {
                return "Variables";
            }
        }


        public NamedVariableCollectionEditor(SerializedObject serializedObject, SerializedProperty serializedProperty) : base(serializedObject, serializedProperty)
        {

        }
    }

}