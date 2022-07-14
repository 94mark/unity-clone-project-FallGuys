using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DevionGames.UIWidgets
{
    [CustomEditor(typeof(UIContainer<>), true)]
    public class UIContainerInspector : UIWidgetInspector
    {
        private SerializedProperty m_DynamicContainer;
        private SerializedProperty m_SlotPrefab;
        private SerializedProperty m_SlotParent;
        private AnimBool m_ShowDynamicContainer;

        private string[] m_PropertiesToExcludeForDefaultInspector;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.m_DynamicContainer = base.serializedObject.FindProperty("m_DynamicContainer");
            this.m_SlotParent = serializedObject.FindProperty("m_SlotParent");
            this.m_SlotPrefab = serializedObject.FindProperty("m_SlotPrefab");

            if (this.m_SlotParent.objectReferenceValue == null)
            {
                GridLayoutGroup group = ((MonoBehaviour)target).gameObject.GetComponentInChildren<GridLayoutGroup>();
                if (group != null)
                {
                    serializedObject.Update();
                    this.m_SlotParent.objectReferenceValue = group.transform;
                    serializedObject.ApplyModifiedProperties();
                }
            }

            this.m_ShowDynamicContainer = new AnimBool(this.m_DynamicContainer.boolValue);
            this.m_ShowDynamicContainer.valueChanged.AddListener(new UnityAction(this.Repaint));
            this.m_PropertiesToExcludeForDefaultInspector = new[] {
                this.m_DynamicContainer.propertyPath,
                this.m_SlotParent.propertyPath,
                this.m_SlotPrefab.propertyPath,
            };
        }

        private void DrawInspector()
        {
            EditorGUILayout.PropertyField(this.m_DynamicContainer);
            this.m_ShowDynamicContainer.target = this.m_DynamicContainer.boolValue;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowDynamicContainer.faded))
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(this.m_SlotParent);
                EditorGUILayout.PropertyField(this.m_SlotPrefab);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            EditorGUILayout.EndFadeGroup();
            DrawClassPropertiesExcluding(this.m_PropertiesToExcludeForDefaultInspector);
        }

    }
}